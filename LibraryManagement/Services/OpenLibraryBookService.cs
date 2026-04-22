using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Services
{
    public class OpenLibraryBookService
    {
        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://openlibrary.org/")
        };

        private static readonly string[] DefaultIsbns =
        {
            "9780140328721",
            "9780439554930",
            "9780061120084",
            "9780307277671",
            "9780553380163",
            "9780140449136",
            "9780307743657",
            "9780261103573",
            "9780451524935",
            "9780141439518"
        };

        private readonly BookDAO _bookDao = new BookDAO();

        public async Task<int> SeedSampleBooksAsync(int maxBooks = 10)
        {
            int count = Math.Min(Math.Max(1, maxBooks), DefaultIsbns.Length);
            var docs = await FetchByIsbnsAsync(DefaultIsbns.Take(count));
            int imported = 0;

            foreach (var doc in docs)
            {
                if (await TryImportDocumentAsync(doc))
                {
                    imported++;
                }
            }

            return imported;
        }

        public async Task<Book?> ImportBookByBarcodeOrIsbnAsync(string rawBarcode)
        {
            string isbn = NormalizeIsbn(rawBarcode);
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return null;
            }

            var existing = await _bookDao.GetByBarcodeAsync(isbn);
            if (existing != null)
            {
                return existing;
            }

            var docs = await FetchByIsbnsAsync(new[] { isbn });
            var first = docs.FirstOrDefault();
            if (first == null)
            {
                return null;
            }

            bool inserted = await TryImportDocumentAsync(first);
            if (!inserted)
            {
                return await _bookDao.GetByBarcodeAsync(isbn);
            }

            return await _bookDao.GetByBarcodeAsync(isbn);
        }

        private static async Task<List<OpenLibraryDocument>> FetchByIsbnsAsync(IEnumerable<string> isbns)
        {
            string[] normalized = isbns
                .Select(NormalizeIsbn)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct(StringComparer.Ordinal)
                .ToArray();

            if (normalized.Length == 0)
            {
                return new List<OpenLibraryDocument>();
            }

            string bibkeys = string.Join(",", normalized.Select(isbn => $"ISBN:{isbn}"));
            string endpoint = $"api/books?bibkeys={Uri.EscapeDataString(bibkeys)}&format=json&jscmd=data";

            using var response = await HttpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                return new List<OpenLibraryDocument>();
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var json = await JsonDocument.ParseAsync(stream);

            var result = new List<OpenLibraryDocument>();
            foreach (var property in json.RootElement.EnumerateObject())
            {
                var item = ParseOpenLibraryItem(property.Value);
                if (item != null)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private async Task<bool> TryImportDocumentAsync(OpenLibraryDocument doc)
        {
            if (string.IsNullOrWhiteSpace(doc.Isbn13) || string.IsNullOrWhiteSpace(doc.Title))
            {
                return false;
            }

            string isbn = NormalizeIsbn(doc.Isbn13);
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return false;
            }

            if (await _bookDao.GetByBarcodeAsync(isbn) != null)
            {
                return false;
            }

            int categoryId = await EnsureCategoryAsync("Sách API");
            int authorId = await EnsureAuthorAsync(doc.Author ?? "Unknown Author");
            int publisherId = await EnsurePublisherAsync(doc.Publisher ?? "OpenLibrary");
            int publishYear = ParsePublishYear(doc.PublishDate);

            var book = new Book
            {
                ISBN = isbn,
                Barcode = isbn,
                Title = doc.Title.Trim(),
                CategoryID = categoryId,
                AuthorID = authorId,
                PublisherID = publisherId,
                PublishYear = publishYear > 0 ? publishYear : DateTime.Now.Year,
                Price = 0,
                TotalCopies = 1,
                AvailableCopies = 1,
                Description = "Imported from OpenLibrary API",
                Location = "API-01",
                IsActive = true
            };

            await _bookDao.InsertAsync(book);
            return true;
        }

        private static OpenLibraryDocument? ParseOpenLibraryItem(JsonElement element)
        {
            try
            {
                string title = GetString(element, "title");
                string publishDate = GetString(element, "publish_date");
                string author = GetNestedArrayName(element, "authors");
                string publisher = GetNestedArrayName(element, "publishers");
                string isbn13 = GetArrayFirstString(element, "identifiers", "isbn_13");

                if (string.IsNullOrWhiteSpace(isbn13))
                {
                    isbn13 = GetArrayFirstString(element, "identifiers", "isbn_10");
                }

                if (string.IsNullOrWhiteSpace(isbn13))
                {
                    return null;
                }

                return new OpenLibraryDocument
                {
                    Isbn13 = isbn13,
                    Title = title,
                    PublishDate = publishDate,
                    Author = author,
                    Publisher = publisher
                };
            }
            catch
            {
                return null;
            }
        }

        private static string GetString(JsonElement element, string name)
        {
            if (!element.TryGetProperty(name, out var prop) || prop.ValueKind != JsonValueKind.String)
            {
                return string.Empty;
            }

            return prop.GetString() ?? string.Empty;
        }

        private static string GetNestedArrayName(JsonElement element, string name)
        {
            if (!element.TryGetProperty(name, out var prop) || prop.ValueKind != JsonValueKind.Array)
            {
                return string.Empty;
            }

            foreach (var item in prop.EnumerateArray())
            {
                if (item.TryGetProperty("name", out var nameProp) && nameProp.ValueKind == JsonValueKind.String)
                {
                    return nameProp.GetString() ?? string.Empty;
                }
            }

            return string.Empty;
        }

        private static string GetArrayFirstString(JsonElement element, string parentName, string arrayName)
        {
            if (!element.TryGetProperty(parentName, out var parent) || parent.ValueKind != JsonValueKind.Object)
            {
                return string.Empty;
            }

            if (!parent.TryGetProperty(arrayName, out var arr) || arr.ValueKind != JsonValueKind.Array)
            {
                return string.Empty;
            }

            foreach (var item in arr.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String)
                {
                    return item.GetString() ?? string.Empty;
                }
            }

            return string.Empty;
        }

        private static int ParsePublishYear(string publishDate)
        {
            if (string.IsNullOrWhiteSpace(publishDate))
            {
                return 0;
            }

            string digits = new string(publishDate.Where(char.IsDigit).ToArray());
            if (digits.Length >= 4 && int.TryParse(digits[..4], NumberStyles.Integer, CultureInfo.InvariantCulture, out int year))
            {
                return year;
            }

            return 0;
        }

        private static string NormalizeIsbn(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var chars = input.Where(char.IsDigit).ToArray();
            if (chars.Length == 10 || chars.Length == 13)
            {
                return new string(chars);
            }

            return string.Empty;
        }

        private static async Task<int> EnsureCategoryAsync(string categoryName)
        {
            using var conn = DatabaseConnection.GetConnection();
            int? id = await conn.ExecuteScalarAsync<int?>(
                "SELECT TOP 1 CategoryID FROM Categories WHERE CategoryName = @CategoryName",
                new { CategoryName = categoryName });
            if (id.HasValue)
            {
                return id.Value;
            }

            return await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO Categories (CategoryName, Description, IsActive)
                  VALUES (@CategoryName, N'Tạo tự động từ OpenLibrary', 1);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new { CategoryName = categoryName });
        }

        private static async Task<int> EnsureAuthorAsync(string authorName)
        {
            using var conn = DatabaseConnection.GetConnection();
            int? id = await conn.ExecuteScalarAsync<int?>(
                "SELECT TOP 1 AuthorID FROM Authors WHERE AuthorName = @AuthorName",
                new { AuthorName = authorName });
            if (id.HasValue)
            {
                return id.Value;
            }

            return await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO Authors (AuthorName, IsActive)
                  VALUES (@AuthorName, 1);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new { AuthorName = authorName });
        }

        private static async Task<int> EnsurePublisherAsync(string publisherName)
        {
            using var conn = DatabaseConnection.GetConnection();
            int? id = await conn.ExecuteScalarAsync<int?>(
                "SELECT TOP 1 PublisherID FROM Publishers WHERE PublisherName = @PublisherName",
                new { PublisherName = publisherName });
            if (id.HasValue)
            {
                return id.Value;
            }

            return await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO Publishers (PublisherName, IsActive)
                  VALUES (@PublisherName, 1);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new { PublisherName = publisherName });
        }

        private sealed class OpenLibraryDocument
        {
            public string Isbn13 { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string PublishDate { get; set; } = string.Empty;
            public string Author { get; set; } = string.Empty;
            public string Publisher { get; set; } = string.Empty;
        }
    }
}
