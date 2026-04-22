using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LibraryManagement.Services;

namespace LibraryManagement.Forms
{
    public partial class FormMailCenter : Form
    {
        private readonly EmailService emailService = new EmailService();

        public FormMailCenter()
        {
            InitializeComponent();
            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);
            if (cboTemplate.Items.Count > 0)
            {
                cboTemplate.SelectedIndex = 0;
            }
            BtnGenerate_Click(this, EventArgs.Empty);
        }

        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            var (subject, html) = BuildTemplate(cboTemplate.SelectedIndex + 1);
            txtSubject.Text = subject;
            rtbPreview.Text = html;
            lblStatus.Text = "Đã tạo nội dung mẫu.";
        }

        private async void BtnSend_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTo.Text))
            {
                MessageBox.Show("Vui lòng nhập email người nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSend.Enabled = false;
            lblStatus.Text = "Đang gửi email...";
            try
            {
                bool ok = await emailService.SendCustomTemplateAsync(txtTo.Text.Trim(), txtSubject.Text.Trim(), rtbPreview.Text);
                lblStatus.Text = ok
                    ? "Gửi email thành công."
                    : $"Gửi email thất bại: {emailService.LastError}";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi gửi email: " + ex.Message;
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }

        private (string Subject, string Html) BuildTemplate(int templateType)
        {
            string name = string.IsNullOrWhiteSpace(txtName.Text) ? "Quý độc giả" : txtName.Text.Trim();
            string bookTitle = string.IsNullOrWhiteSpace(txtBookTitle.Text) ? "[Tên sách]" : txtBookTitle.Text.Trim();
            string borrowCode = string.IsNullOrWhiteSpace(txtBorrowCode.Text) ? "[Mã phiếu]" : txtBorrowCode.Text.Trim();
            string due = dtpDueDate.Value.ToString("dd/MM/yyyy HH:mm");

            return templateType switch
            {
                1 => (
                    "Đăng ký thành viên thành công",
                    WrapHtml($"<h2>Đăng ký thành viên thành công</h2><p>Xin chào <b>{name}</b>,</p><p>Bạn đã đăng ký thành công thành viên thư viện.</p><p>Mời bạn đến quầy để nhận thẻ và bắt đầu sử dụng dịch vụ.</p>")
                ),
                2 => (
                    "Mượn sách thành công",
                    WrapHtml($"<h2>Mượn sách thành công</h2><p>Xin chào <b>{name}</b>,</p><p>Bạn đã mượn thành công sách <b>{bookTitle}</b>.</p><p>Mã phiếu: <b>{borrowCode}</b></p><p>Hạn trả: <b>{due}</b></p>")
                ),
                3 => (
                    "Nhắc trả sách quá hạn",
                    WrapHtml($"<h2>Nhắc trả sách quá hạn</h2><p>Xin chào <b>{name}</b>,</p><p>Sách <b>{bookTitle}</b> đang quá hạn trả.</p><p>Mã phiếu: <b>{borrowCode}</b></p><p>Vui lòng hoàn trả sớm để tránh phát sinh thêm phí phạt.</p>")
                ),
                4 => (
                    "Trả sách thành công",
                    WrapHtml($"<h2>Trả sách thành công</h2><p>Xin chào <b>{name}</b>,</p><p>Thư viện xác nhận bạn đã trả thành công sách <b>{bookTitle}</b>.</p><p>Mã phiếu: <b>{borrowCode}</b></p><p>Thời gian xác nhận: <b>{DateTime.Now:dd/MM/yyyy HH:mm}</b></p>")
                ),
                5 => (
                    "Tạm khóa tài khoản do đăng nhập sai nhiều lần",
                    WrapHtml($"<h2>Tạm khóa tài khoản</h2><p>Xin chào <b>{name}</b>,</p><p>Tài khoản của bạn vừa bị tạm khóa 60 giây do đăng nhập sai quá số lần cho phép.</p><p>Bạn có thể thử lại từ: <b>{due}</b></p>")
                ),
                6 => (
                    "Sách đặt trước đã có sẵn",
                    WrapHtml($"<h2>Sách đặt trước đã có sẵn</h2><p>Xin chào <b>{name}</b>,</p><p>Sách <b>{bookTitle}</b> bạn đặt trước hiện đã có sẵn tại thư viện.</p><p>Vui lòng đến thư viện để làm thủ tục mượn sớm.</p>")
                ),
                _ => ("Thông báo từ thư viện", WrapHtml("<p>Thông báo.</p>"))
            };
        }

        private static string WrapHtml(string content)
        {
            var sb = new StringBuilder();
            sb.Append("<html><body style='font-family:Segoe UI,Arial,sans-serif;background:#f4f7fb;padding:24px;'>");
            sb.Append("<div style='max-width:680px;margin:0 auto;background:#ffffff;border-radius:10px;padding:24px;border:1px solid #e8eef7;'>");
            sb.Append("<div style='font-size:20px;font-weight:700;color:#1e3a8a;margin-bottom:16px;'>HỆ THỐNG QUẢN LÝ THƯ VIỆN</div>");
            sb.Append(content);
            sb.Append("<hr style='border:none;border-top:1px solid #e5e7eb;margin:20px 0;' />");
            sb.Append("<div style='font-size:12px;color:#6b7280;'>Email tự động từ hệ thống. Vui lòng không trả lời trực tiếp email này.</div>");
            sb.Append("</div></body></html>");
            return sb.ToString();
        }
    }
}
