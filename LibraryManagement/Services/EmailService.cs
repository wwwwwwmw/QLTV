using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Services
{
    public class EmailService
    {
        private const string KeyLastReminderDate = "LastOverdueReminderDate";
        private static readonly HttpClient HttpClient = new HttpClient();
        public string LastError { get; private set; } = string.Empty;

        public async Task<int> SendDailyOverdueRemindersAsync()
        {
            var settingsDao = new SystemSettingDAO();
            string last = settingsDao.GetValue(KeyLastReminderDate, "");
            if (DateTime.TryParse(last, out var lastDate) && lastDate.Date == DateTime.Today)
                return 0;

            var borrowDao = new BorrowRecordDAO();
            var overdue = borrowDao.GetOverdueRecords();

            decimal finePerDay = settingsDao.GetDecimalValue(SystemSetting.KEY_FINE_PER_DAY, 5000);

            int sent = 0;
            foreach (var r in overdue)
            {
                if (string.IsNullOrWhiteSpace(r.MemberEmail))
                    continue;

                int daysOverdue = Math.Max(0, (int)(DateTime.Now.Date - r.DueDate.Date).TotalDays);
                decimal fine = daysOverdue * finePerDay;

                bool ok = await SendOverdueReminderAsync(r, daysOverdue, fine).ConfigureAwait(false);
                if (ok) sent++;
            }

            settingsDao.UpdateValue(KeyLastReminderDate, DateTime.Today.ToString("yyyy-MM-dd"));
            return sent;
        }

        public Task<bool> SendMemberRegistrationSuccessAsync(Member member)
        {
            if (string.IsNullOrWhiteSpace(member.Email)) return Task.FromResult(false);

            string subject = "Đăng ký thành viên thư viện thành công";
            string html = WrapHtml(
                "Đăng ký thành công",
                $"<p>Xin chào <b>{Html(member.FullName)}</b>,</p>" +
                $"<p>Bạn đã đăng ký thành công thành viên thư viện.</p>" +
                $"<p>Mã thẻ: <b>{Html(member.MemberCode)}</b></p>" +
                $"<p>Loại thẻ: <b>{Html(member.MemberType)}</b></p>" +
                $"<p>Hạn thẻ: <b>{member.ExpiryDate:dd/MM/yyyy}</b></p>");

            return SendAsync(member.Email.Trim(), subject, html, true);
        }

        public Task<bool> SendBorrowSuccessAsync(Member member, Book book, DateTime dueDate, string borrowCode)
        {
            if (string.IsNullOrWhiteSpace(member.Email)) return Task.FromResult(false);

            string subject = "Thông báo mượn sách thành công";
            string html = WrapHtml(
                "Mượn sách thành công",
                $"<p>Xin chào <b>{Html(member.FullName)}</b>,</p>" +
                $"<p>Bạn đã mượn thành công sách <b>{Html(book.Title)}</b>.</p>" +
                $"<p>Mã phiếu: <b>{Html(borrowCode)}</b></p>" +
                $"<p>Hạn trả: <b>{dueDate:dd/MM/yyyy}</b></p>");

            return SendAsync(member.Email.Trim(), subject, html, true);
        }

        public Task<bool> SendOverdueReminderAsync(BorrowRecord record, int daysOverdue, decimal fine)
        {
            if (string.IsNullOrWhiteSpace(record.MemberEmail)) return Task.FromResult(false);

            string subject = $"Nhắc trả sách quá hạn - {record.BookTitle}";
            string html = WrapHtml(
                "Nhắc trả sách quá hạn",
                $"<p>Xin chào <b>{Html(record.MemberName ?? "Quý độc giả")}</b>,</p>" +
                $"<p>Bạn đang có sách quá hạn cần trả.</p>" +
                $"<p>Tên sách: <b>{Html(record.BookTitle ?? "")}</b></p>" +
                $"<p>Mã phiếu: <b>{Html(record.BorrowCode)}</b></p>" +
                $"<p>Ngày mượn: <b>{record.BorrowDate:dd/MM/yyyy}</b></p>" +
                $"<p>Hạn trả: <b>{record.DueDate:dd/MM/yyyy}</b></p>" +
                $"<p>Số ngày trễ: <b>{daysOverdue}</b></p>" +
                $"<p>Tiền phạt tạm tính: <b>{fine:N0} VNĐ</b></p>");

            return SendAsync(record.MemberEmail.Trim(), subject, html, true);
        }

        public Task<bool> SendReturnSuccessAsync(BorrowRecord record, decimal fineAmount)
        {
            if (string.IsNullOrWhiteSpace(record.MemberEmail)) return Task.FromResult(false);

            string subject = "Thông báo trả sách thành công";
            string fineText = fineAmount > 0 ? $"<p>Tiền phạt phát sinh: <b>{fineAmount:N0} VNĐ</b></p>" : "<p>Không phát sinh tiền phạt.</p>";
            string html = WrapHtml(
                "Trả sách thành công",
                $"<p>Xin chào <b>{Html(record.MemberName ?? "Quý độc giả")}</b>,</p>" +
                $"<p>Thư viện đã ghi nhận bạn trả sách thành công.</p>" +
                $"<p>Tên sách: <b>{Html(record.BookTitle ?? "")}</b></p>" +
                $"<p>Mã phiếu: <b>{Html(record.BorrowCode)}</b></p>" +
                $"<p>Ngày trả: <b>{DateTime.Now:dd/MM/yyyy HH:mm}</b></p>" + fineText);

            return SendAsync(record.MemberEmail.Trim(), subject, html, true);
        }

        public Task<bool> SendAccountTemporarilyLockedAsync(string toEmail, string username, DateTime unlockAt)
        {
            if (string.IsNullOrWhiteSpace(toEmail)) return Task.FromResult(false);

            string subject = "Cảnh báo: tài khoản tạm khóa do đăng nhập sai";
            string html = WrapHtml(
                "Tạm khóa tài khoản",
                $"<p>Tài khoản <b>{Html(username)}</b> vừa bị tạm khóa do đăng nhập sai quá số lần cho phép.</p>" +
                $"<p>Bạn có thể thử đăng nhập lại từ: <b>{unlockAt:dd/MM/yyyy HH:mm:ss}</b></p>" +
                "<p>Nếu không phải bạn thực hiện, vui lòng liên hệ quản trị viên ngay.</p>");

            return SendAsync(toEmail.Trim(), subject, html, true);
        }

        public Task<bool> SendReservationAvailableAsync(string toEmail, string memberName, string bookTitle)
        {
            if (string.IsNullOrWhiteSpace(toEmail)) return Task.FromResult(false);

            string subject = "Thông báo: sách đặt trước đã có";
            string html = WrapHtml(
                "Sách đặt trước đã có",
                $"<p>Xin chào <b>{Html(memberName)}</b>,</p>" +
                $"<p>Sách <b>{Html(bookTitle)}</b> bạn đặt trước hiện đã có sẵn tại thư viện.</p>" +
                "<p>Vui lòng đến thư viện sớm để làm thủ tục mượn.</p>");

            return SendAsync(toEmail.Trim(), subject, html, true);
        }

        public Task<bool> SendCustomTemplateAsync(string toEmail, string subject, string htmlContent)
        {
            return SendAsync(toEmail, subject, htmlContent, true);
        }

        public async Task<bool> SendAsync(string toEmail, string subject, string body, bool isBodyHtml = false)
        {
            LastError = string.Empty;

            if (string.IsNullOrWhiteSpace(toEmail) || string.IsNullOrWhiteSpace(subject))
            {
                LastError = "Thiếu email người nhận hoặc tiêu đề email.";
                return false;
            }

            bool sentByMailKit = await TrySendByMailKitAsync(toEmail, subject, body, isBodyHtml).ConfigureAwait(false);
            string mailKitError = LastError;
            if (sentByMailKit) return true;

            if (string.IsNullOrWhiteSpace(mailKitError))
            {
                LastError = "Gửi email bằng MailKit thất bại nhưng không có thông tin lỗi chi tiết.";
                return false;
            }

            if (!HasMailerSendConfig())
            {
                LastError = mailKitError;
                return false;
            }

            bool sentByMailerSend = await TrySendByMailerSendAsync(toEmail, subject, body, isBodyHtml).ConfigureAwait(false);
            string mailerSendError = LastError;
            if (sentByMailerSend) return true;

            if (!string.IsNullOrWhiteSpace(mailKitError) && !string.IsNullOrWhiteSpace(mailerSendError))
            {
                LastError = $"MailKit: {mailKitError}\nMailerSend: {mailerSendError}";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(mailKitError))
            {
                LastError = mailKitError;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(mailerSendError))
            {
                LastError = mailerSendError;
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastError))
            {
                LastError = "Không thể gửi email do cấu hình MailKit/MailerSend chưa đầy đủ hoặc dịch vụ không phản hồi.";
            }

            return false;
        }

        private static bool HasMailerSendConfig()
        {
            string apiKey = ConfigurationManager.AppSettings["MailerSendApiKey"] ?? "";
            string fromEmail = ConfigurationManager.AppSettings["MailerSendFromEmail"] ?? "";
            return !string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(fromEmail);
        }

        private static bool IsApiAuthorizationError(string? error)
        {
            if (string.IsNullOrWhiteSpace(error)) return false;

            string lower = error.ToLowerInvariant();
            return lower.Contains("401")
                || lower.Contains("403")
                || lower.Contains("unauthorized")
                || lower.Contains("forbidden")
                || lower.Contains("token không hợp lệ")
                || lower.Contains("token da het han")
                || (lower.Contains("token") && lower.Contains("invalid"));
        }

        private async Task<bool> TrySendByMailerSendAsync(string toEmail, string subject, string body, bool isBodyHtml)
        {
            string apiKey = ConfigurationManager.AppSettings["MailerSendApiKey"]
                ?? ConfigurationManager.AppSettings["MailjetApiKey"]
                ?? "";
            string fromEmail = ConfigurationManager.AppSettings["MailerSendFromEmail"]
                ?? ConfigurationManager.AppSettings["MailjetFromEmail"]
                ?? "";
            if (string.IsNullOrWhiteSpace(fromEmail))
            {
                fromEmail = ConfigurationManager.AppSettings["SmtpFrom"] ?? "";
            }
            if (string.IsNullOrWhiteSpace(fromEmail))
            {
                fromEmail = ConfigurationManager.AppSettings["SmtpUser"] ?? "";
            }
            string fromName = ConfigurationManager.AppSettings["MailerSendFromName"]
                ?? ConfigurationManager.AppSettings["MailjetFromName"]
                ?? (ConfigurationManager.AppSettings["AppName"] ?? "Library");

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(fromEmail))
            {
                LastError = "MailerSend chưa đủ cấu hình (thiếu MailerSendApiKey hoặc MailerSendFromEmail).";
                return false;
            }

            try
            {
                string contentText = isBodyHtml ? StripHtml(body) : body;
                string contentHtml = isBodyHtml ? body : $"<pre>{Html(body)}</pre>";

                var payload = new
                {
                    from = new { email = fromEmail, name = fromName },
                    to = new[] { new { email = toEmail } },
                    subject,
                    text = contentText,
                    html = contentHtml
                };

                string json = JsonSerializer.Serialize(payload);
                using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.mailersend.com/v1/email");
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");

                using var resp = await HttpClient.SendAsync(req).ConfigureAwait(false);
                if (resp.IsSuccessStatusCode)
                {
                    return true;
                }

                string reason = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                LastError = BuildMailerSendErrorMessage(resp.StatusCode, reason);
                return false;
            }
            catch (Exception ex)
            {
                LastError = $"Lỗi MailerSend: {ex.Message}";
                return false;
            }
        }

        private static string BuildMailerSendErrorMessage(HttpStatusCode statusCode, string? responseBody)
        {
            string compactBody = CompactErrorJson(responseBody);
            int status = (int)statusCode;

            if (statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.Forbidden)
            {
                return $"MailerSend từ chối xác thực (HTTP {status}). Kiểm tra MailerSendApiKey và quyền gửi domain. Chi tiết: {compactBody}";
            }

            if (statusCode == HttpStatusCode.UnprocessableEntity)
            {
                return $"MailerSend không chấp nhận dữ liệu gửi (HTTP {status}). Thường do địa chỉ gửi chưa verify domain hoặc sai định dạng email. Chi tiết: {compactBody}";
            }

            if (statusCode == HttpStatusCode.TooManyRequests)
            {
                return $"MailerSend bị giới hạn tần suất (HTTP {status}). Vui lòng thử lại sau. Chi tiết: {compactBody}";
            }

            if (statusCode == HttpStatusCode.BadRequest)
            {
                return $"MailerSend báo yêu cầu không hợp lệ (HTTP {status}). Kiểm tra subject, from/to email và nội dung. Chi tiết: {compactBody}";
            }

            return $"MailerSend trả về lỗi HTTP {status}. Chi tiết: {compactBody}";
        }

        private static string CompactErrorJson(string? content)
        {
            if (string.IsNullOrWhiteSpace(content)) return "(không có nội dung phản hồi)";

            string compact = Regex.Replace(content, "\\s+", " ").Trim();
            return compact.Length <= 500 ? compact : compact[..500] + "...";
        }

        private async Task<bool> TrySendByMailKitAsync(string toEmail, string subject, string body, bool isBodyHtml)
        {
            string host = ConfigurationManager.AppSettings["SmtpHost"] ?? "";
            int port = int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out int p) ? p : 587;
            bool enableSsl = bool.TryParse(ConfigurationManager.AppSettings["SmtpEnableSsl"], out bool ssl) ? ssl : true;
            string user = ConfigurationManager.AppSettings["SmtpUser"] ?? "";
            string pass = ConfigurationManager.AppSettings["SmtpPassword"] ?? "";
            string from = ConfigurationManager.AppSettings["SmtpFrom"] ?? "";
            if (string.IsNullOrWhiteSpace(from))
            {
                from = user;
            }
            string fromName = ConfigurationManager.AppSettings["SmtpFromName"] ?? (ConfigurationManager.AppSettings["AppName"] ?? "Library");

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(from))
            {
                LastError = "MailKit chưa đủ cấu hình (thiếu SmtpHost hoặc SmtpFrom).";
                return false;
            }

            if (string.IsNullOrWhiteSpace(user))
            {
                LastError = "MailKit thiếu tài khoản SMTP (SmtpUser).";
                return false;
            }

            if (string.IsNullOrWhiteSpace(pass))
            {
                LastError = "MailKit thiếu mật khẩu SMTP (SmtpPassword). Với Gmail, dùng App Password 16 ký tự.";
                return false;
            }

            var message = new MimeMessage();
            try
            {
                message.From.Add(new MailboxAddress(fromName, from));
                message.To.Add(new MailboxAddress(toEmail, toEmail));
                message.Subject = subject;

                var builder = new BodyBuilder();
                if (isBodyHtml)
                {
                    builder.HtmlBody = body;
                    builder.TextBody = StripHtml(body);
                }
                else
                {
                    builder.TextBody = body;
                    builder.HtmlBody = $"<pre>{Html(body)}</pre>";
                }

                message.Body = builder.ToMessageBody();
            }
            catch (Exception ex)
            {
                LastError = $"MailKit không thể tạo nội dung email: {ex.Message}";
                return false;
            }

            try
            {
                using var client = new SmtpClient();
                SecureSocketOptions secureOption = ResolveSocketOption(enableSsl, port);

                await client.ConnectAsync(host, port, secureOption).ConfigureAwait(false);
                await client.AuthenticateAsync(user, pass).ConfigureAwait(false);
                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
                return true;
            }
            catch (AuthenticationException ex)
            {
                LastError = $"MailKit xác thực thất bại. Kiểm tra SmtpUser/SmtpPassword (Gmail cần App Password). Chi tiết: {ex.Message}";
                return false;
            }
            catch (Exception ex)
            {
                LastError = $"Lỗi MailKit SMTP: {ex.Message}";
                return false;
            }
        }

        private static SecureSocketOptions ResolveSocketOption(bool enableSsl, int port)
        {
            if (!enableSsl) return SecureSocketOptions.None;
            return port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
        }

        private static string Html(string text)
        {
            return System.Net.WebUtility.HtmlEncode(text ?? string.Empty);
        }

        private static string StripHtml(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var sb = new StringBuilder(input.Length);
            bool insideTag = false;
            foreach (char c in input)
            {
                if (c == '<') { insideTag = true; continue; }
                if (c == '>') { insideTag = false; continue; }
                if (!insideTag) sb.Append(c);
            }
            return sb.ToString();
        }

        private static string WrapHtml(string title, string bodyHtml)
        {
            return $@"<html><body style='font-family:Segoe UI,Arial,sans-serif;background:#f4f7fb;padding:24px;'>
<div style='max-width:680px;margin:0 auto;background:#ffffff;border-radius:10px;padding:24px;border:1px solid #e8eef7;'>
<div style='font-size:20px;font-weight:700;color:#1e3a8a;margin-bottom:16px;'>HỆ THỐNG QUẢN LÝ THƯ VIỆN</div>
<div style='font-size:18px;font-weight:600;color:#0f172a;margin-bottom:14px;'>{Html(title)}</div>
{bodyHtml}
<hr style='border:none;border-top:1px solid #e5e7eb;margin:20px 0;' />
<div style='font-size:12px;color:#6b7280;'>Email tự động từ hệ thống. Vui lòng không trả lời trực tiếp email này.</div>
</div></body></html>";
        }
    }
}
