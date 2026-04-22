using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.Services;

namespace LibraryManagement.Forms
{
    public partial class FormBookChatbot : Form
    {
        private readonly GeminiBookChatService _chatService = new GeminiBookChatService();
        private readonly List<(string Role, string Text)> _conversation = new List<(string Role, string Text)>();
        private bool _isSending;
        private Panel? _heroPanel;
        private Label? _heroSubtitle;

        public FormBookChatbot()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaChatLayout();
                }
                catch
                {
                }
            }

            Load += FormBookChatbot_Load;
            Resize += FormBookChatbot_Resize;
        }

        private void FormBookChatbot_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            ApplyFigmaChatLayout();
            AddChatLine("Trợ lý", "Xin chào! Mình là chatbot thư viện. Bạn muốn tìm sách theo mô tả hay cần gợi ý chủ đề nào?");
        }

        private void FormBookChatbot_Resize(object? sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            ApplyFigmaChatLayout();
        }

        private void ApplyFigmaChatLayout()
        {
            BackColor = Color.FromArgb(241, 245, 249);

            if (_heroPanel == null)
            {
                _heroPanel = new Panel
                {
                    Name = "chatHeroPanel",
                    BackColor = Color.FromArgb(30, 64, 175)
                };
                _heroPanel.Paint += (_, e) =>
                {
                    using var brush = new LinearGradientBrush(_heroPanel.ClientRectangle,
                        Color.FromArgb(30, 64, 175), Color.FromArgb(15, 23, 42), 18f);
                    e.Graphics.FillRectangle(brush, _heroPanel.ClientRectangle);
                };
                Controls.Add(_heroPanel);

                _heroSubtitle = new Label
                {
                    Text = "Mô tả nhu cầu đọc và nhận gợi ý sách ngay",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(191, 219, 254),
                    AutoSize = true
                };
                _heroPanel.Controls.Add(_heroSubtitle);
            }

            int margin = 16;
            int heroHeight = 108;
            int gap = 12;

            _heroPanel.Bounds = new Rectangle(margin, margin, ClientSize.Width - margin * 2, heroHeight);

            lblTitle.Parent = _heroPanel;
            lblTitle.Text = "Trợ lý sách AI";
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 14);

            if (_heroSubtitle != null)
            {
                _heroSubtitle.Location = new Point(22, 64);
            }

            lblHint.Parent = this;
            lblHint.Text = "Gợi ý: \"Sách phát triển bản thân\", \"Tiểu thuyết trinh thám dễ đọc\"";
            lblHint.ForeColor = Color.FromArgb(71, 85, 105);
            lblHint.Font = new Font("Segoe UI", 9F);
            lblHint.Location = new Point(margin + 4, _heroPanel.Bottom + 6);

            int bodyTop = lblHint.Bottom + 8;
            int inputHeight = 78;
            int buttonWidth = 142;

            rtbChat.Location = new Point(margin, bodyTop);
            rtbChat.Size = new Size(ClientSize.Width - margin * 2, ClientSize.Height - bodyTop - inputHeight - gap - margin);
            rtbChat.BorderStyle = BorderStyle.FixedSingle;
            rtbChat.BackColor = Color.White;

            txtMessage.Location = new Point(margin, rtbChat.Bottom + gap);
            txtMessage.Size = new Size(ClientSize.Width - margin * 2 - buttonWidth - 10, inputHeight);
            txtMessage.PlaceholderText = "Nhập mô tả sách bạn muốn tìm hoặc nhu cầu để nhận đề xuất...";

            btnSend.Location = new Point(txtMessage.Right + 10, txtMessage.Top);
            btnSend.Size = new Size(buttonWidth, 36);
            btnSend.Text = "Gửi yêu cầu";
            btnSend.BackColor = Color.FromArgb(37, 99, 235);
            btnSend.ForeColor = Color.White;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.FlatAppearance.BorderSize = 0;

            btnClear.Location = new Point(txtMessage.Right + 10, txtMessage.Top + 42);
            btnClear.Size = new Size(buttonWidth, 36);
            btnClear.Text = "Xóa hội thoại";
            btnClear.BackColor = Color.FromArgb(107, 114, 128);
            btnClear.ForeColor = Color.White;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.FlatAppearance.BorderSize = 0;
        }

        private async void BtnSend_Click(object? sender, EventArgs e)
        {
            await SendMessageAsync();
        }

        private async void TxtMessage_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                await SendMessageAsync();
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            _conversation.Clear();
            rtbChat.Clear();
            AddChatLine("Trợ lý", "Đã xóa lịch sử hội thoại. Bạn có thể mô tả sách bạn cần tìm.");
        }

        private async Task SendMessageAsync()
        {
            if (_isSending) return;

            string message = txtMessage.Text.Trim();
            if (string.IsNullOrWhiteSpace(message))
                return;

            _isSending = true;
            btnSend.Enabled = false;

            try
            {
                AddChatLine("Ban", message);
                _conversation.Add(("user", message));
                txtMessage.Clear();

                AddChatLine("Trợ lý", "Đang phân tích yêu cầu và tìm sách phù hợp...");
                string reply = await _chatService.ChatAsync(message, _conversation);

                RemoveLastLineIfLoading();
                AddChatLine("Trợ lý", reply);
                _conversation.Add(("assistant", reply));
            }
            catch (Exception ex)
            {
                RemoveLastLineIfLoading();
                AddChatLine("Trợ lý", "Xin lỗi, chatbot đang gặp lỗi: " + ex.Message);
            }
            finally
            {
                _isSending = false;
                btnSend.Enabled = true;
                txtMessage.Focus();
            }
        }

        private void AddChatLine(string speaker, string text)
        {
            if (rtbChat.TextLength > 0)
            {
                rtbChat.AppendText("\n");
            }

            Color color = speaker == "Bạn" ? Color.FromArgb(41, 128, 185) : Color.FromArgb(39, 174, 96);
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = color;
            rtbChat.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            rtbChat.AppendText($"{speaker}: ");

            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = Color.Black;
            rtbChat.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Regular);
            rtbChat.AppendText(text + "\n");

            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.ScrollToCaret();
        }

        private void RemoveLastLineIfLoading()
        {
            const string marker = "Trợ lý: Đang phân tích yêu cầu và tìm sách phù hợp...";
            if (!rtbChat.Text.Contains(marker)) return;

            int idx = rtbChat.Text.LastIndexOf(marker, StringComparison.Ordinal);
            if (idx < 0) return;

            rtbChat.Select(idx, rtbChat.TextLength - idx);
            rtbChat.SelectedText = string.Empty;
            rtbChat.Text = rtbChat.Text.TrimEnd();
        }
    }
}
