using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Services
{
    public sealed class GeminiBookChatService
    {
        private static readonly HttpClient Http = new HttpClient();
        private readonly BookDAO _bookDAO = new BookDAO();

        private sealed class BookMatch
        {
            public Book Book { get; set; } = null!;
            public int Score { get; set; }
            public string Reason { get; set; } = string.Empty;
            public string Snippet { get; set; } = string.Empty;
        }

        private static readonly string[] AnimalIntentTokens =
        {
            "dong vat", "con vat", "nhan vat dong vat", "thu cung", "thu vat",
            "meo", "cho", "ho", "su tu", "gau", "tho", "chim", "ca", "de men"
        };

        private static readonly string[] AnimalBookIndicators =
        {
            "de men", "dong vat", "con vat", "ngu ngon", "thieu nhi", "phieu luu",
            "truyen dong vat", "nhan vat la", "fable", "animal"
        };

        public async Task<string> ChatAsync(string userMessage, IReadOnlyList<(string Role, string Text)> conversation)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return "Bạn hãy mô tả sách bạn cần tìm, ví dụ: 'sách kinh tế cơ bản cho người mới'.";
            }

            var localMatches = FindBooksByDescription(userMessage, 8);
            string localReply = BuildLocalReply(userMessage, localMatches);
            string? deterministicReply = TryBuildDeterministicReply(userMessage, localMatches);

            string apiKey = ConfigurationManager.AppSettings["GeminiApiKey"] ?? string.Empty;
            string primaryModel = ConfigurationManager.AppSettings["GeminiModel"] ?? "gemini-2.5-flash";
            var modelCandidates = BuildModelCandidates(primaryModel);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return deterministicReply ?? localReply;
            }

            List<Book> matchedBooks = localMatches
                .Select(x => x.Book)
                .ToList();

            string booksContext = BuildBookContext(matchedBooks);
            string historyContext = BuildHistoryContext(conversation);
            string systemPrompt = BuildSystemPrompt(booksContext, historyContext);

            string combinedPrompt =
                $"{systemPrompt}\n\n" +
                $"Tin nhắn người dùng: {userMessage}\n\n" +
                "Yêu cầu: Trả lời tự nhiên như người thật, thân thiện, ngắn gọn, ưu tiên đề xuất sách và tìm sách theo mô tả. " +
                "Nếu không có sách phù hợp trong dữ liệu, hãy nói rõ và đưa gợi ý tìm kiếm tiếp. " +
                "BẮT BUỘC dùng tiếng Việt có dấu, không viết kiểu không dấu.";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = combinedPrompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.75,
                    topP = 0.9,
                    maxOutputTokens = 700
                }
            };

            string? geminiReply = await TryGetGeminiReplyAsync(apiKey, modelCandidates, payload);
            if (!string.IsNullOrWhiteSpace(geminiReply) && !LooksLikeUnaccentedVietnamese(geminiReply))
            {
                return geminiReply;
            }

            return deterministicReply ?? localReply;
        }

        private static List<string> BuildModelCandidates(string primaryModel)
        {
            var models = new List<string>();

            if (!string.IsNullOrWhiteSpace(primaryModel))
            {
                models.Add(primaryModel.Trim());
            }

            foreach (var fallback in new[] { "gemini-2.5-flash", "gemini-2.0-flash", "gemini-2.0-flash-lite" })
            {
                if (!models.Contains(fallback, StringComparer.OrdinalIgnoreCase))
                {
                    models.Add(fallback);
                }
            }

            return models;
        }

        private static string? TryBuildDeterministicReply(string userMessage, IReadOnlyList<BookMatch> matches)
        {
            string normalized = NormalizeText(userMessage);
            bool asksCount = normalized.Contains("con may cuon", StringComparison.Ordinal)
                || normalized.Contains("may cuon", StringComparison.Ordinal)
                || normalized.Contains("ton kho", StringComparison.Ordinal)
                || normalized.Contains("con khong", StringComparison.Ordinal);

            if (!asksCount || matches.Count == 0)
            {
                return null;
            }

            var best = matches.OrderByDescending(m => m.Score).FirstOrDefault();
            if (best == null)
            {
                return null;
            }

            Book b = best.Book;
            string count = b.AvailableCopies > 0
                ? $"còn {b.AvailableCopies} cuốn"
                : "hiện đang hết sách";

            return $"Sách '{b.Title}' của tác giả {b.AuthorName ?? "N/A"} hiện {count}.";
        }

        private static async Task<string?> TryGetGeminiReplyAsync(string apiKey, IReadOnlyList<string> models, object payload)
        {
            string json = JsonSerializer.Serialize(payload);

            foreach (string model in models)
            {
                int attempts = 2;
                for (int i = 0; i < attempts; i++)
                {
                    string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

                    try
                    {
                        using var content = new StringContent(json, Encoding.UTF8, "application/json");
                        using HttpResponseMessage response = await Http.PostAsync(endpoint, content);
                        string body = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            string? parsed = ParseGeminiText(body);
                            if (!string.IsNullOrWhiteSpace(parsed))
                            {
                                return parsed;
                            }
                            break;
                        }

                        if (response.StatusCode == HttpStatusCode.TooManyRequests ||
                            response.StatusCode == HttpStatusCode.ServiceUnavailable)
                        {
                            await Task.Delay(600 * (i + 1));
                            continue;
                        }

                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        await Task.Delay(400 * (i + 1));
                    }
                }
            }

            return null;
        }

        private static string? ParseGeminiText(string responseBody)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(responseBody);
                JsonElement root = doc.RootElement;

                if (!root.TryGetProperty("candidates", out JsonElement candidates) || candidates.GetArrayLength() == 0)
                {
                    return null;
                }

                JsonElement first = candidates[0];
                if (!first.TryGetProperty("content", out JsonElement contentObj) ||
                    !contentObj.TryGetProperty("parts", out JsonElement parts) ||
                    parts.GetArrayLength() == 0)
                {
                    return null;
                }

                foreach (JsonElement part in parts.EnumerateArray())
                {
                    if (part.TryGetProperty("text", out JsonElement textNode))
                    {
                        string? text = textNode.GetString();
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            return text.Trim();
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private List<BookMatch> FindBooksByDescription(string userMessage, int take)
        {
            var books = _bookDAO.GetAll();
            if (books.Count == 0)
                return new List<BookMatch>();

            string normalizedQuery = NormalizeText(userMessage);
            var queryTokens = Tokenize(normalizedQuery);
            var queryTokenSet = new HashSet<string>(queryTokens, StringComparer.Ordinal);

            if (queryTokenSet.Count == 0)
            {
                return _bookDAO.Search(userMessage, availableOnly: false)
                    .Take(take)
                    .Select(b => new BookMatch
                    {
                        Book = b,
                        Score = 1,
                        Reason = "khop tim kiem co ban"
                    })
                    .ToList();
            }

            var idf = BuildIdfMap(books);
            var keyPhrases = ExtractKeyPhrases(normalizedQuery);
            bool animalIntent = IsAnimalIntent(normalizedQuery);

            var matches = new List<BookMatch>();
            foreach (var book in books)
            {
                double score = 0;
                var reasons = new List<string>();

                string title = NormalizeText(book.Title);
                string author = NormalizeText(book.AuthorName);
                string category = NormalizeText(book.CategoryName);
                string description = NormalizeText(book.Description);
                string isbn = NormalizeText(book.ISBN);

                string fullText = $"{title} {author} {category} {description} {isbn}".Trim();

                // 1) Token overlap co trong cac truong, co trong so va IDF.
                double tokenWeightedScore = 0;
                int matchedTokenCount = 0;

                foreach (var token in queryTokenSet)
                {
                    double tokenIdf = idf.TryGetValue(token, out double idfValue) ? idfValue : 1.0;

                    if (title.Contains(token, StringComparison.Ordinal))
                    {
                        tokenWeightedScore += 4.2 * tokenIdf;
                        matchedTokenCount++;
                        reasons.Add("trùng từ khóa tên sách");
                        continue;
                    }

                    if (description.Contains(token, StringComparison.Ordinal))
                    {
                        tokenWeightedScore += 3.2 * tokenIdf;
                        matchedTokenCount++;
                        reasons.Add("trùng từ khóa nội dung");
                        continue;
                    }

                    if (author.Contains(token, StringComparison.Ordinal))
                    {
                        tokenWeightedScore += 2.8 * tokenIdf;
                        matchedTokenCount++;
                        reasons.Add("trùng từ khóa tác giả");
                        continue;
                    }

                    if (category.Contains(token, StringComparison.Ordinal))
                    {
                        tokenWeightedScore += 2.5 * tokenIdf;
                        matchedTokenCount++;
                        reasons.Add("trùng từ khóa thể loại");
                        continue;
                    }

                    if (isbn.Contains(token, StringComparison.Ordinal))
                    {
                        tokenWeightedScore += 5.0;
                        matchedTokenCount++;
                        reasons.Add("trùng ISBN");
                    }
                }

                score += tokenWeightedScore * 10;

                // 2) Thuong neu do phu token cao.
                double coverage = queryTokenSet.Count == 0 ? 0 : (double)matchedTokenCount / queryTokenSet.Count;
                if (coverage >= 0.7)
                {
                    score += 30;
                    reasons.Add("độ phủ mô tả cao");
                }
                else if (coverage >= 0.45)
                {
                    score += 14;
                }

                // 3) Khop cum tu dai (rất quan trọng cho mo ta truyen).
                foreach (var phrase in keyPhrases)
                {
                    if (phrase.Length < 5) continue;
                    if (title.Contains(phrase, StringComparison.Ordinal))
                    {
                        score += 45;
                        reasons.Add("khớp cụm từ trong tên");
                    }
                    else if (description.Contains(phrase, StringComparison.Ordinal))
                    {
                        score += 28;
                        reasons.Add("khớp cụm từ trong mô tả");
                    }
                }

                if (animalIntent)
                {
                    int animalHit = AnimalBookIndicators.Count(indicator =>
                        title.Contains(indicator, StringComparison.Ordinal) ||
                        category.Contains(indicator, StringComparison.Ordinal) ||
                        description.Contains(indicator, StringComparison.Ordinal));

                    if (animalHit > 0)
                    {
                        score += 95 + (animalHit * 18);
                        reasons.Add("chủ đề động vật/ngụ ngôn");
                    }

                    if (title.Contains("de men", StringComparison.Ordinal))
                    {
                        score += 260;
                        reasons.Add("khớp rất mạnh với 'Dế Mèn'");
                    }
                }

                // 4) Do tuong dong semantic gan dung bang 3-gram (hieu ca cau mo ta dai).
                double trigramTitle = TrigramDice(normalizedQuery, title);
                double trigramDesc = TrigramDice(normalizedQuery, description);
                double trigramFull = TrigramDice(normalizedQuery, fullText);
                double semantic = Math.Max(trigramFull, Math.Max(trigramTitle, trigramDesc));
                score += semantic * 120;
                if (semantic >= 0.20)
                {
                    reasons.Add("mô tả giống ngữ nghĩa");
                }

                if (!string.IsNullOrWhiteSpace(normalizedQuery) && fullText.Contains(normalizedQuery, StringComparison.Ordinal))
                {
                    score += 80;
                    reasons.Add("khớp gần như đầy đủ mô tả");
                }

                if (book.AvailableCopies > 0)
                {
                    score += 6;
                }
                else
                {
                    score -= 2;
                }

                if (score >= 18)
                {
                    matches.Add(new BookMatch
                    {
                        Book = book,
                        Score = (int)Math.Round(score),
                        Reason = string.Join(", ", reasons.Distinct().Take(2)),
                        Snippet = ExtractSnippet(book.Description, queryTokenSet)
                    });
                }
            }

            if (matches.Count == 0)
            {
                // Fallback tim theo search SQL co san de tranh miss do tokenization.
                var fallback = _bookDAO.Search(userMessage, availableOnly: false)
                    .Take(take)
                    .Select(b => new BookMatch
                    {
                        Book = b,
                        Score = 1,
                        Reason = "khớp tìm kiếm cơ bản",
                        Snippet = string.Empty
                    })
                    .ToList();

                return fallback;
            }

            return matches
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.Book.AvailableCopies)
                .ThenBy(x => x.Book.Title)
                .Take(take)
                .ToList();
        }

        private static string BuildLocalReply(string userMessage, IReadOnlyList<BookMatch> matches)
        {
            if (matches.Count == 0)
            {
                return "Mình chưa tìm thấy sách phù hợp với mô tả này. Bạn có thể bổ sung chủ đề, tác giả hoặc thể loại (ví dụ: kinh tế, kỹ năng sống, lập trình C#).";
            }

            var sb = new StringBuilder();
            sb.AppendLine("Mình tìm thấy một số sách phù hợp với mô tả của bạn:");

            int idx = 1;
            foreach (var match in matches.Take(5))
            {
                var b = match.Book;
                string stock = b.AvailableCopies > 0 ? $"Còn {b.AvailableCopies} cuốn" : "Đang hết sách";
                string reason = string.IsNullOrWhiteSpace(match.Reason) ? "phù hợp theo nội dung" : match.Reason;

                sb.AppendLine($"{idx}. {b.Title} - Tác giả: {b.AuthorName ?? "N/A"} - Thể loại: {b.CategoryName ?? "N/A"}");
                sb.AppendLine($"   {stock}. Lý do gợi ý: {reason}.");
                if (!string.IsNullOrWhiteSpace(match.Snippet))
                {
                    sb.AppendLine($"   Trích mô tả: \"{match.Snippet}\"");
                }
                idx++;
            }

            sb.Append("Nếu bạn muốn, hãy nói thêm mức độ dễ/khó, số trang mong muốn, hoặc sách cho đối tượng nào để mình lọc sát hơn.");
            return sb.ToString();
        }

        private static string NormalizeText(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            string normalized = text.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);
            foreach (char c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString()
                .Normalize(NormalizationForm.FormC)
                .Replace('đ', 'd')
                .Replace('Đ', 'd');
        }

        private static List<string> Tokenize(string normalizedText)
        {
            if (string.IsNullOrWhiteSpace(normalizedText))
                return new List<string>();

            return Regex.Split(normalizedText, "[^a-z0-9]+")
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 2)
                .Distinct(StringComparer.Ordinal)
                .ToList();
        }

        private static Dictionary<string, double> BuildIdfMap(IReadOnlyList<Book> books)
        {
            int totalDocs = Math.Max(books.Count, 1);
            var docFreq = new Dictionary<string, int>(StringComparer.Ordinal);

            foreach (var book in books)
            {
                string doc = NormalizeText($"{book.Title} {book.AuthorName} {book.CategoryName} {book.Description} {book.ISBN}");
                var tokens = Tokenize(doc).Distinct(StringComparer.Ordinal);
                foreach (var token in tokens)
                {
                    docFreq[token] = docFreq.TryGetValue(token, out int c) ? c + 1 : 1;
                }
            }

            var idf = new Dictionary<string, double>(StringComparer.Ordinal);
            foreach (var kv in docFreq)
            {
                // Smoothed IDF.
                idf[kv.Key] = Math.Log((1.0 + totalDocs) / (1.0 + kv.Value)) + 1.0;
            }

            return idf;
        }

        private static List<string> ExtractKeyPhrases(string normalizedQuery)
        {
            var phrases = new List<string>();

            string compact = Regex.Replace(normalizedQuery, "\\s+", " ").Trim();
            if (!string.IsNullOrWhiteSpace(compact))
            {
                phrases.Add(compact);
            }

            var tokens = Tokenize(normalizedQuery);
            for (int i = 0; i < tokens.Count - 1; i++)
            {
                phrases.Add(tokens[i] + " " + tokens[i + 1]);
            }
            for (int i = 0; i < tokens.Count - 2; i++)
            {
                phrases.Add(tokens[i] + " " + tokens[i + 1] + " " + tokens[i + 2]);
            }

            return phrases
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .OrderByDescending(x => x.Length)
                .ToList();
        }

        private static double TrigramDice(string source, string target)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(target))
                return 0;

            var a = BuildNgrams(source, 3);
            var b = BuildNgrams(target, 3);

            if (a.Count == 0 || b.Count == 0)
                return 0;

            int intersection = 0;
            foreach (var g in a)
            {
                if (b.Contains(g)) intersection++;
            }

            return (2.0 * intersection) / (a.Count + b.Count);
        }

        private static HashSet<string> BuildNgrams(string text, int n)
        {
            string compact = Regex.Replace(text, "\\s+", " ").Trim();
            var grams = new HashSet<string>(StringComparer.Ordinal);
            if (compact.Length < n)
                return grams;

            for (int i = 0; i <= compact.Length - n; i++)
            {
                grams.Add(compact.Substring(i, n));
            }

            return grams;
        }

        private static string ExtractSnippet(string? originalDescription, HashSet<string> queryTokens)
        {
            if (string.IsNullOrWhiteSpace(originalDescription) || queryTokens.Count == 0)
                return string.Empty;

            var sentences = Regex.Split(originalDescription, "(?<=[.!?;])\\s+")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            string best = string.Empty;
            int bestScore = 0;

            foreach (var sentence in sentences)
            {
                string norm = NormalizeText(sentence);
                int score = 0;
                foreach (var token in queryTokens)
                {
                    if (norm.Contains(token, StringComparison.Ordinal))
                        score++;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    best = sentence.Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(best))
                return string.Empty;

            if (best.Length > 140)
                return best.Substring(0, 140).Trim() + "...";

            return best;
        }

        private static string BuildBookContext(IReadOnlyList<Book> books)
        {
            if (books.Count == 0)
            {
                return "Không tìm thấy sách tương ứng trong CSDL nội bộ.";
            }

            var lines = new List<string>
            {
                "Danh sách sách liên quan trong CSDL:"
            };

            foreach (Book b in books)
            {
                lines.Add(
                    $"- {b.Title} | Tác giả: {b.AuthorName ?? "N/A"} | Thể loại: {b.CategoryName ?? "N/A"} | ISBN: {b.ISBN ?? "N/A"} | Còn lại: {b.AvailableCopies} | Vị trí: {b.Location ?? "N/A"}");
            }

            return string.Join("\n", lines);
        }

        private static string BuildHistoryContext(IReadOnlyList<(string Role, string Text)> conversation)
        {
            if (conversation.Count == 0) return "";

            var lines = conversation
                .TakeLast(8)
                .Select(x => $"{x.Role}: {x.Text}");

            return "Lịch sử hội thoại gần đây:\n" + string.Join("\n", lines);
        }

        private static string BuildSystemPrompt(string booksContext, string historyContext)
        {
            return
                "Bạn là trợ lý thư viện giao tiếp như người thật, lịch sự, dễ hiểu. " +
                "Nhiệm vụ chính: đề xuất sách theo nhu cầu và tìm sách theo mô tả người dùng.\n" +
                "Nguyên tắc: \n" +
                "1) Ưu tiên dữ liệu sách trong CSDL nội bộ khi đề xuất.\n" +
                "2) Nếu người dùng nói mô tả mơ hồ, hỏi tối đa 1-2 câu để làm rõ rồi mới đề xuất.\n" +
                "3) Khi đề xuất, nên đưa 3-5 lựa chọn, mỗi lựa chọn có lý do ngắn.\n" +
                "4) Nếu sách hết, thông báo trạng thái và đề xuất sách thay thế.\n" +
                "5) Không bịa thông tin tồn kho nếu CSDL không có.\n" +
                "6) Bắt buộc dùng tiếng Việt có dấu, không dùng kiểu viết không dấu.\n\n" +
                booksContext + "\n\n" +
                historyContext;
        }

        private static bool IsAnimalIntent(string normalizedQuery)
        {
            if (string.IsNullOrWhiteSpace(normalizedQuery))
                return false;

            return AnimalIntentTokens.Any(t => normalizedQuery.Contains(t, StringComparison.Ordinal));
        }

        private static bool LooksLikeUnaccentedVietnamese(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            bool hasVietnameseDiacritic = Regex.IsMatch(text, "[ăâêôơưđáàảãạắằẳẵặấầẩẫậéèẻẽẹếềểễệíìỉĩịóòỏõọốồổỗộớờởỡợúùủũụứừửữựýỳỷỹỵĂÂÊÔƠƯĐÁÀẢÃẠẮẰẲẴẶẤẦẨẪẬÉÈẺẼẸẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌỐỒỔỖỘỚỜỞỠỢÚÙỦŨỤỨỪỬỮỰÝỲỶỸỴ]");
            if (hasVietnameseDiacritic)
                return false;

            int markerCount = 0;
            string lower = text.ToLowerInvariant();
            string[] markers = { " goi y ", " khong ", " sach ", " the loai ", " tac gia ", " neu ban muon", " mo ta " };
            foreach (var marker in markers)
            {
                if (lower.Contains(marker, StringComparison.Ordinal))
                {
                    markerCount++;
                }
            }

            return markerCount >= 2;
        }
    }
}
