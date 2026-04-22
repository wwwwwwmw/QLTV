using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form công khai - Cho phép xem sách không cần đăng nhập
    /// </summary>
    public partial class FormPublic : Form
    {
        private readonly BookDAO bookDAO = new BookDAO();
        private List<Book> allBooks = new List<Book>();
        private readonly GeminiBookChatService chatService = new GeminiBookChatService();
        private readonly List<(string Role, string Text)> chatHistory = new List<(string Role, string Text)>();

        private Panel? panelChatWidget;
        private RichTextBox? rtbChat;
        private TextBox? txtChatInput;
        private Button? btnChatSend;
        private Button? btnChatToggle;
        private Button? btnChatClose;
        private bool chatSending;

        public FormPublic()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaPublicLayout();
                    StyleTopButtons();
                    AdjustHighlightsLayout();
                }
                catch
                {
                }
            }

            Load += FormPublic_Load;
        }

        private void FormPublic_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            ApplyFigmaPublicLayout();
            StyleTopButtons();
            AdjustHighlightsLayout();
            SetupChatWidget();
            LoadCategories();
            LoadBooks();
        }

        private void ApplyFigmaPublicLayout()
        {
            BackColor = Color.FromArgb(243, 246, 251);

            panelHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(226, 232, 240));
                e.Graphics.DrawLine(pen, 0, panelHeader.Height - 1, panelHeader.Width, panelHeader.Height - 1);
            };

            panelSearch.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(226, 232, 240));
                e.Graphics.DrawLine(pen, 0, panelSearch.Height - 1, panelSearch.Width, panelSearch.Height - 1);
            };

            panelHighlights.Paint += (s, e) =>
            {
                using var brush = new LinearGradientBrush(panelHighlights.ClientRectangle,
                    Color.FromArgb(244, 248, 255),
                    Color.FromArgb(239, 246, 255),
                    12f);
                e.Graphics.FillRectangle(brush, panelHighlights.ClientRectangle);

                using var accent = new SolidBrush(Color.FromArgb(18, 59, 130, 246));
                e.Graphics.FillEllipse(accent, panelHighlights.Width - 340, -110, 360, 220);
                e.Graphics.FillEllipse(accent, -120, 180, 260, 180);
            };

            lblHeaderTitle.Text = "THƯ VIỆN SÁCH · Tra cứu thông minh";
            lblNewBooks.Text = "SÁCH NỔI BẬT";
            lblCategories.Text = "THỂ LOẠI";
        }

        private void FormPublic_Resize(object? sender, EventArgs e)
        {
            AdjustHighlightsLayout();
            ResizeAndLayoutChatWidget();
            PositionChatWidget();
        }

        private void SetupChatWidget()
        {
            if (panelChatWidget != null && btnChatToggle != null)
            {
                ResizeAndLayoutChatWidget();
                PositionChatWidget();
                return;
            }

            panelChatWidget = new Panel
            {
                Size = new Size(420, 500),
                BackColor = Color.White,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.None
            };

            Label lblChatTitle = new Label
            {
                Text = "Trợ lý thư viện AI",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 130, 246),
                Dock = DockStyle.Top,
                Height = 38,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            btnChatClose = new Button
            {
                Text = "-",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 130, 246),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(38, 30),
                Location = new Point(panelChatWidget.Width - 42, 4)
            };
            btnChatClose.FlatAppearance.BorderSize = 0;
            btnChatClose.Click += BtnChatClose_Click;

            rtbChat = new RichTextBox
            {
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(10, 48),
                Size = new Size(398, 345),
                BackColor = Color.White
            };

            txtChatInput = new TextBox
            {
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(10, 408),
                Size = new Size(318, 70),
                Multiline = true,
                PlaceholderText = "Mô tả sách bạn cần tìm..."
            };
            txtChatInput.KeyDown += TxtChatInput_KeyDown;

            btnChatSend = new Button
            {
                Text = "Gửi",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 130, 246),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(338, 408),
                Size = new Size(72, 70)
            };
            btnChatSend.FlatAppearance.BorderSize = 0;
            btnChatSend.Click += BtnChatSend_Click;

            panelChatWidget.Controls.Add(lblChatTitle);
            panelChatWidget.Controls.Add(btnChatClose);
            panelChatWidget.Controls.Add(rtbChat);
            panelChatWidget.Controls.Add(txtChatInput);
            panelChatWidget.Controls.Add(btnChatSend);

            btnChatToggle = new Button
            {
                Text = "Chatbot",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 130, 246),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 42),
                Anchor = AnchorStyles.None
            };
            btnChatToggle.FlatAppearance.BorderSize = 0;
            btnChatToggle.Click += BtnChatToggle_Click;

            Controls.Add(panelChatWidget);
            Controls.Add(btnChatToggle);
            panelChatWidget.BringToFront();
            btnChatToggle.BringToFront();
            ResizeAndLayoutChatWidget();
            PositionChatWidget();

            AddChatLine("Trợ lý", "Xin chào! Tôi có thể gợi ý sách và tìm sách theo mô tả của bạn.");
        }

        private void ResizeAndLayoutChatWidget()
        {
            if (panelChatWidget == null)
                return;

            int desiredWidth = Math.Max(400, Math.Min(560, (int)(ClientSize.Width * 0.35)));
            int desiredHeight = Math.Max(460, Math.Min(680, (int)(ClientSize.Height * 0.65)));
            panelChatWidget.Size = new Size(desiredWidth, desiredHeight);

            if (rtbChat == null || txtChatInput == null || btnChatSend == null || btnChatClose == null)
                return;

            const int padding = 10;
            const int titleHeight = 38;
            const int gap = 8;
            int inputHeight = 70;
            int sendWidth = 72;

            int inputTop = panelChatWidget.Height - padding - inputHeight;
            int inputWidth = panelChatWidget.Width - (padding * 3) - sendWidth;

            if (inputWidth < 220)
            {
                sendWidth = 66;
                inputWidth = panelChatWidget.Width - (padding * 3) - sendWidth;
            }

            rtbChat.Location = new Point(padding, titleHeight + padding);
            rtbChat.Size = new Size(
                panelChatWidget.Width - (padding * 2),
                Math.Max(140, inputTop - (titleHeight + padding) - gap));

            txtChatInput.Location = new Point(padding, inputTop);
            txtChatInput.Size = new Size(inputWidth, inputHeight);

            btnChatSend.Location = new Point(txtChatInput.Right + padding, inputTop);
            btnChatSend.Size = new Size(sendWidth, inputHeight);

            btnChatClose.Location = new Point(panelChatWidget.Width - btnChatClose.Width - 4, 4);
        }

        private void PositionChatWidget()
        {
            if (panelChatWidget == null || btnChatToggle == null) return;

            int margin = 16;
            int gap = 10;

            btnChatToggle.Location = new Point(ClientSize.Width - btnChatToggle.Width - margin, ClientSize.Height - btnChatToggle.Height - margin);

            int chatY = btnChatToggle.Top - panelChatWidget.Height - gap;
            if (chatY < margin) chatY = margin;

            panelChatWidget.Location = new Point(ClientSize.Width - panelChatWidget.Width - margin, chatY);

            panelChatWidget.BringToFront();
            btnChatToggle.BringToFront();
        }

        private void BtnChatToggle_Click(object? sender, EventArgs e)
        {
            if (panelChatWidget == null || btnChatToggle == null) return;

            panelChatWidget.Visible = !panelChatWidget.Visible;
            btnChatToggle.Text = panelChatWidget.Visible ? "Ẩn chat" : "Chatbot";
            PositionChatWidget();
        }

        private void BtnChatClose_Click(object? sender, EventArgs e)
        {
            if (panelChatWidget == null || btnChatToggle == null) return;

            panelChatWidget.Visible = false;
            btnChatToggle.Text = "Chatbot";
            PositionChatWidget();
        }

        private async void BtnChatSend_Click(object? sender, EventArgs e)
        {
            await SendChatMessageAsync();
        }

        private async void TxtChatInput_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                await SendChatMessageAsync();
            }
        }

        private async System.Threading.Tasks.Task SendChatMessageAsync()
        {
            if (chatSending || txtChatInput == null || btnChatSend == null) return;

            string message = txtChatInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(message)) return;

            chatSending = true;
            btnChatSend.Enabled = false;

            try
            {
                AddChatLine("Bạn", message);
                chatHistory.Add(("user", message));
                txtChatInput.Clear();

                string reply = await chatService.ChatAsync(message, chatHistory);
                AddChatLine("Trợ lý", reply);
                chatHistory.Add(("assistant", reply));
            }
            catch (Exception ex)
            {
                AddChatLine("Trợ lý", "Xin lỗi, chatbot đang bận: " + ex.Message);
            }
            finally
            {
                chatSending = false;
                btnChatSend.Enabled = true;
                txtChatInput.Focus();
            }
        }

        private void AddChatLine(string role, string text)
        {
            if (rtbChat == null) return;

            if (rtbChat.TextLength > 0)
                rtbChat.AppendText("\n");

            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionColor = role == "Bạn" ? Color.FromArgb(59, 130, 246) : Color.FromArgb(16, 185, 129);
            rtbChat.SelectionFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            rtbChat.AppendText(role + ": ");

            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionColor = Color.Black;
            rtbChat.SelectionFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            rtbChat.AppendText(text + "\n");

            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.ScrollToCaret();
        }

        private void StyleTopButtons()
        {
            StyleButton(btnRegister, Color.FromArgb(124, 58, 237), 14);
            StyleButton(btnLogin, Color.FromArgb(59, 130, 246), 14);
            StyleButton(btnRefresh, Color.FromArgb(59, 130, 246), 12);
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Để đăng ký thẻ thư viện, vui lòng:\n\n" +
                "1. Đến trực tiếp thư viện với CMND/CCCD\n" +
                "2. Điền đơn đăng ký\n" +
                "3. Nhận thẻ và tài khoản\n\n" +
                "📞 Liên hệ: 0123-456-789\n" +
                "📍 Địa chỉ: 123 Đường ABC, Quận XYZ",
                "Hướng dẫn đăng ký thẻ thư viện",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadBooks();
        }

        private void LoadCategories()
        {
            try
            {
                cboCategory.Items.Clear();
                cboCategory.Items.Add(new ComboBoxItem { Value = 0, Text = "-- Tất cả thể loại --" });

                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1 ORDER BY CategoryName";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cboCategory.Items.Add(new ComboBoxItem
                                {
                                    Value = reader.GetInt32(0),
                                    Text = reader.GetString(1)
                                });
                            }
                        }
                    }
                }

                cboCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thể loại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBooks()
        {
            try
            {
                allBooks = bookDAO.GetAll() ?? new List<Book>();
                DisplayBooks(allBooks);
                LoadHighlights();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách sách: " + ex.Message + "\n\nVui lòng kiểm tra kết nối database.",
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void DisplayBooks(List<Book> books)
        {
            flowBooks.Controls.Clear();
            lblTotalBooks.Text = $"Tổng: {books.Count} sách";

            foreach (var book in books)
            {
                flowBooks.Controls.Add(CreateBookCard(book));
            }
        }

        private void LoadHighlights()
        {
            flowNewBooks.Controls.Clear();

            var latestBooks = allBooks
                .OrderByDescending(b => b.CreatedDate)
                .Take(3)
                .ToList();

            foreach (var book in latestBooks)
            {
                flowNewBooks.Controls.Add(CreateSmallBookCard(book));
            }

            flowCategories.Controls.Clear();
            flowCategories.Controls.Add(CreateAllCategoryChip());

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1 ORDER BY CategoryName";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var category = new Category
                                {
                                    CategoryID = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1)
                                };
                                flowCategories.Controls.Add(CreateCategoryChip(category));
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            AdjustHighlightsLayout();
        }

        private Control CreateSmallBookCard(Book book)
        {
            Panel card = new Panel
            {
                Size = new Size(300, 166),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 16, 0),
                Cursor = Cursors.Hand
            };

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(
                    e.Graphics,
                    card.ClientRectangle,
                    Color.FromArgb(226, 232, 240),
                    ButtonBorderStyle.Solid
                );
            };

            PictureBox pic = new PictureBox
            {
                Size = new Size(92, 132),
                Location = new Point(16, 16),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            LoadBookImage(pic, book.ImagePath);

            if (pic.Image == null)
            {
                Label ico = new Label
                {
                    Text = "📖",
                    Font = new Font("Segoe UI", 28),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pic.Controls.Add(ico);
            }

            Label title = new Label
            {
                Text = book.Title ?? "",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 35, 35),
                Location = new Point(122, 20),
                Size = new Size(160, 48)
            };

            Label info = new Label
            {
                Text = (book.AuthorName ?? "") + (string.IsNullOrWhiteSpace(book.CategoryName) ? "" : $" • {book.CategoryName}"),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(122, 74),
                Size = new Size(165, 44)
            };

            Label status = new Label
            {
                Text = book.AvailableCopies > 0 ? $"Còn {book.AvailableCopies} cuốn" : "Hết sách",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = book.AvailableCopies > 0 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60),
                Location = new Point(122, 124),
                AutoSize = true
            };

            card.Controls.Add(pic);
            card.Controls.Add(title);
            card.Controls.Add(info);
            card.Controls.Add(status);

            card.Click += (s, e) => ShowBookDetail(book);
            foreach (Control c in card.Controls)
                c.Click += (s, e) => ShowBookDetail(book);

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(246, 250, 255);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            return card;
        }

        private Control CreateCategoryChip(Category category)
        {
            var btn = new Button
            {
                Text = category.CategoryName,
                AutoSize = true,
                Padding = new Padding(14, 8, 14, 8),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 10, 8),
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9)
            };

            btn.FlatAppearance.BorderSize = 0;
            StyleButton(btn, Color.FromArgb(59, 130, 246), 12);

            btn.Click += (s, e) =>
            {
                for (int i = 0; i < cboCategory.Items.Count; i++)
                {
                    if (cboCategory.Items[i] is ComboBoxItem item && item.Value == category.CategoryID)
                    {
                        cboCategory.SelectedIndex = i;
                        break;
                    }
                }
            };

            return btn;
        }

        private Control CreateAllCategoryChip()
        {
            var btn = new Button
            {
                Text = "Tất cả",
                AutoSize = true,
                Padding = new Padding(14, 8, 14, 8),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 10, 8),
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9)
            };

            btn.FlatAppearance.BorderSize = 0;
            StyleButton(btn, Color.FromArgb(59, 130, 246), 12);

            btn.Click += (s, e) => cboCategory.SelectedIndex = 0;

            return btn;
        }

        private void AdjustHighlightsLayout()
        {
            if (flowNewBooks == null || flowCategories == null)
                return;

            int width = ClientSize.Width - 40;
            if (width < 600) width = 600;

            flowNewBooks.Size = new Size(width, 196);
            flowCategories.Size = new Size(width, 82);
        }

        private Panel CreateBookCard(Book book)
        {
            Panel card = new Panel
            {
                Size = new Size(230, 332),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 16, 16),
                Cursor = Cursors.Hand,
                Tag = book
            };

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(
                    e.Graphics,
                    card.ClientRectangle,
                    Color.FromArgb(226, 232, 240),
                    ButtonBorderStyle.Solid
                );
            };

            PictureBox picBook = new PictureBox
            {
                Size = new Size(200, 192),
                Location = new Point(15, 14),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 250, 252)
            };

            LoadBookImage(picBook, book.ImagePath);

            if (picBook.Image == null)
            {
                Label lblNoImage = new Label
                {
                    Text = "📖",
                    Font = new Font("Segoe UI", 42),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                picBook.Controls.Add(lblNoImage);
            }

            Label lblTitle = new Label
            {
                Text = book.Title ?? "",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(14, 212),
                Size = new Size(200, 34)
            };

            Label lblAuthor = new Label
            {
                Text = (book.AuthorName ?? "Chưa rõ"),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(14, 248),
                Size = new Size(200, 18)
            };

            Label lblCategory = new Label
            {
                Text = (book.CategoryName ?? "Chưa phân loại"),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(14, 268),
                Size = new Size(200, 18)
            };

            Label lblStatus = new Label
            {
                Text = book.AvailableCopies > 0 ? $"Còn {book.AvailableCopies} cuốn" : "Hết sách",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = book.AvailableCopies > 0 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60),
                Location = new Point(14, 292),
                Size = new Size(200, 22)
            };

            card.Controls.Add(picBook);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblAuthor);
            card.Controls.Add(lblCategory);
            card.Controls.Add(lblStatus);

            card.Click += (s, e) => ShowBookDetail(book);
            foreach (Control c in card.Controls)
                c.Click += (s, e) => ShowBookDetail(book);

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(248, 252, 255);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            return card;
        }

        private void LoadBookImage(PictureBox pictureBox, string? relativeImagePath)
        {
            if (string.IsNullOrWhiteSpace(relativeImagePath))
                return;

            try
            {
                string imagePath = Path.Combine(Application.StartupPath, "Images", relativeImagePath);
                if (!File.Exists(imagePath))
                    return;

                using (var img = Image.FromFile(imagePath))
                {
                    pictureBox.Image = new Bitmap(img);
                }
            }
            catch
            {
            }
        }

        private void ShowBookDetail(Book book)
        {
            using (var detailForm = new FormBookDetailPublic(book))
            {
                if (detailForm.ShowDialog() == DialogResult.Yes)
                {
                    BtnLogin_Click(null, null);
                }
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            FilterBooks();
        }

        private void CboCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            FilterBooks();
        }

        private void FilterBooks()
        {
            string searchText = (txtSearch.Text ?? string.Empty).ToLower().Trim();
            int categoryId = (cboCategory.SelectedItem as ComboBoxItem)?.Value ?? 0;

            var filtered = allBooks.FindAll(b =>
            {
                string title = (b.Title ?? string.Empty).ToLower();
                string author = (b.AuthorName ?? string.Empty).ToLower();
                string isbn = (b.ISBN ?? string.Empty).ToLower();

                bool matchSearch =
                    string.IsNullOrEmpty(searchText) ||
                    title.Contains(searchText) ||
                    author.Contains(searchText) ||
                    isbn.Contains(searchText);

                bool matchCategory = categoryId == 0 || b.CategoryID == categoryId;

                return matchSearch && matchCategory;
            });

            DisplayBooks(filtered);
        }

        private void BtnLogin_Click(object? sender, EventArgs? e)
        {
            using (var loginForm = new FormLogin())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    var mainForm = new FormMain();
                    mainForm.FormClosed += (s2, e2) =>
                    {
                        if (CurrentUser.User == null)
                        {
                            Show();
                            LoadBooks();
                        }
                        else
                        {
                            CurrentUser.Logout();
                            Show();
                        }
                    };

                    Hide();
                    mainForm.Show();
                }
            }
        }

        private void StyleButton(Button btn, Color backColor, int radius)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = Lighten(backColor, 18);
            btn.FlatAppearance.MouseDownBackColor = Darken(backColor, 12);
            btn.Resize -= Button_ResizeApplyRegion;
            btn.Resize += Button_ResizeApplyRegion;
            ApplyRoundedCorners(btn, radius);
            btn.Tag = radius;
        }

        private void Button_ResizeApplyRegion(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is int radius)
            {
                ApplyRoundedCorners(btn, radius);
            }
        }

        private void ApplyRoundedCorners(Control c, int radius)
        {
            if (c.Width == 0 || c.Height == 0)
                return;

            using (var path = new GraphicsPath())
            {
                int r = radius;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(c.Width - r, 0, r, r, 270, 90);
                path.AddArc(c.Width - r, c.Height - r, r, r, 0, 90);
                path.AddArc(0, c.Height - r, r, r, 90, 90);
                path.CloseAllFigures();
                c.Region = new Region(path);
            }
        }

        private Color Lighten(Color color, int amount)
        {
            int r = Math.Min(255, color.R + amount);
            int g = Math.Min(255, color.G + amount);
            int b = Math.Min(255, color.B + amount);
            return Color.FromArgb(r, g, b);
        }

        private Color Darken(Color color, int amount)
        {
            int r = Math.Max(0, color.R - amount);
            int g = Math.Max(0, color.G - amount);
            int b = Math.Max(0, color.B - amount);
            return Color.FromArgb(r, g, b);
        }

        private class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; } = "";
            public override string ToString() => Text;
        }
    }
}
