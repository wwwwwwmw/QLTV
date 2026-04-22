using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form mượn sách
    /// </summary>
    public partial class FormBorrow : Form
    {
        private MemberDAO memberDAO = new MemberDAO();
        private BookDAO bookDAO = new BookDAO();
        private BorrowRecordDAO borrowDAO = new BorrowRecordDAO();
        private SystemSettingDAO settingDAO = new SystemSettingDAO();
        private BarcodeService barcodeService = new BarcodeService();
        private OpenLibraryBookService openLibraryBookService = new OpenLibraryBookService();
        private RecommendationService recommendationService = new RecommendationService();
        private ReceiptService receiptService = new ReceiptService();
        private EmailService emailService = new EmailService();
        private ReservationDAO reservationDAO = new ReservationDAO();
        private Button? btnReserve;
        private Panel? heroPanel;
        private Label? lblHeroSubtitle;

        private Member? currentMember;
        private Book? selectedBook;
        private RecommendationItem? selectedRecommendation;
        private int allowedBorrowDays = 14;
        private bool suppressBookSearchTextChanged;

        public FormBorrow()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);
            this.Load += FormBorrow_Load;
            this.Resize += FormBorrow_Resize;
            InitializeReservationButton();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaBorrowLayout();
                }
                catch
                {
                }
            }
        }

        private void InitializeReservationButton()
        {
            if (btnReserve != null) return;
            btnReserve = new Button
            {
                Name = "btnReserve",
                Text = "Đặt trước",
                Width = 120,
                Height = 40,
                Left = btnBorrow.Right + 12,
                Top = btnBorrow.Top,
                Enabled = false
            };
            btnReserve.Click += BtnReserve_Click;
            btnBorrow.Parent?.Controls.Add(btnReserve);
            btnReserve.BringToFront();
        }

        private void TxtMemberCode_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnFindMember_Click(sender, e);
                e.Handled = true;
            }
        }

        private void BtnFindMember_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMemberCode.Text))
            {
                MessageBox.Show("Vui lòng nhập mã thẻ độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            currentMember = memberDAO.GetByCode(txtMemberCode.Text.Trim());

            if (currentMember == null)
            {
                lblMemberInfo.Text = "Không tìm thấy độc giả!";
                lblMemberInfo.ForeColor = Color.Red;
                lblMemberStatus.Text = "";
                dgvBorrowing.Rows.Clear();
                return;
            }

            // Display member info
            allowedBorrowDays = Member.GetMaxBorrowDaysByType(currentMember.MemberType);
            numDays.Maximum = allowedBorrowDays;
            if (numDays.Value > allowedBorrowDays)
            {
                numDays.Value = allowedBorrowDays;
            }

            lblMemberInfo.ForeColor = Color.Black;
            lblMemberInfo.Text = $"Họ tên: {currentMember.FullName}\n" +
                                 $"Loại thẻ: {currentMember.MemberType}    Hạn thẻ: {currentMember.ExpiryDate?.ToString("dd/MM/yyyy")}\n" +
                                 $"Nợ phạt: {currentMember.TotalFine:N0} VNĐ    Tối đa: {allowedBorrowDays} ngày/lần mượn";

            // Check if can borrow
            var (canBorrow, message) = memberDAO.CanBorrow(currentMember!.MemberID);
            if (canBorrow)
            {
                lblMemberStatus.Text = "✓ Có thể mượn sách";
                lblMemberStatus.ForeColor = Color.Green;
            }
            else
            {
                lblMemberStatus.Text = $"✗ {message}";
                lblMemberStatus.ForeColor = Color.Red;
            }

            // Load current borrowings
            _ = LoadMemberBorrowingsAsync();
            LoadRecommendations();
            if (btnReserve != null)
            {
                btnReserve.Enabled = selectedBook != null && selectedBook.AvailableCopies <= 0;
            }
        }

        private void LoadRecommendations()
        {
            lstRecommendations.Items.Clear();
            selectedRecommendation = null;

            if (currentMember == null) return;

            try
            {
                var books = recommendationService.GetRecommendationsForMember(currentMember.MemberID, 5);
                foreach (var b in books)
                {
                    string author = string.IsNullOrWhiteSpace(b.AuthorName) ? "" : $" - {b.AuthorName}";
                    lstRecommendations.Items.Add(new RecommendationItem { BookID = b.BookID, Title = b.Title, Display = $"{b.Title}{author}" });
                }
            }
            catch { }
        }

        private void LstRecommendations_DoubleClick(object? sender, EventArgs e)
        {
            if (lstRecommendations.SelectedItem is not RecommendationItem item) return;
            selectedRecommendation = item;
            txtBookSearch.Text = item.Title;
            _ = LoadBooksAsync(item.Title);
            SelectBookRow(item.BookID);
        }

        private async Task LoadMemberBorrowingsAsync()
        {
            dgvBorrowing.Rows.Clear();
            if (currentMember == null) return;

            var borrowings = await borrowDAO.GetMemberBorrowingsAsync(currentMember.MemberID);
            foreach (var borrow in borrowings)
            {
                var row = dgvBorrowing.Rows.Add(
                    borrow.BookTitle,
                    borrow.BorrowDate.ToString("dd/MM/yyyy"),
                    borrow.DueDate.ToString("dd/MM/yyyy"),
                    borrow.StatusDisplay
                );

                if (borrow.IsOverdue)
                {
                    dgvBorrowing.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                }
            }
        }

        private async Task LoadBooksAsync(string? keyword = null)
        {
            try
            {
                var books = await bookDAO.SearchAsync(keyword, availableOnly: false);
                PopulateBooksGrid(books);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateBooksGrid(System.Collections.Generic.IEnumerable<Book> books)
        {
            dgvBooks.Rows.Clear();
            foreach (var book in books)
            {
                dgvBooks.Rows.Add(
                    book.BookID,
                    book.ISBN,
                    book.Title,
                    book.AuthorName,
                    book.AvailableCopies,
                    book.Location
                );
            }
        }

        private async Task LoadBooksByExactCodeAsync(string code, Book? fallbackBook = null)
        {
            var books = await bookDAO.SearchAsync(code, availableOnly: false);
            var exactBooks = books.Where(b =>
                    string.Equals(b.Barcode, code, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(b.ISBN, code, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (exactBooks.Count == 0)
            {
                var exactByBarcode = await bookDAO.GetByBarcodeAsync(code);
                if (exactByBarcode != null)
                {
                    exactBooks.Add(exactByBarcode);
                }
            }

            if (exactBooks.Count == 0 && fallbackBook != null)
            {
                exactBooks.Add(fallbackBook);
            }

            PopulateBooksGrid(exactBooks);
        }

        private async void TxtBookSearch_TextChanged(object? sender, EventArgs e)
        {
            if (suppressBookSearchTextChanged) return;

            try
            {
                string? keyword = string.IsNullOrWhiteSpace(txtBookSearch.Text) ? null : txtBookSearch.Text.Trim();
                await LoadBooksAsync(keyword);
            }
            catch { }
        }

        private void TxtBarcode_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _ = FindAndSelectBookByBarcodeAsync(txtBarcode.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private async void BtnScanBarcode_Click(object? sender, EventArgs e)
        {
            try
            {
                string? barcode = await barcodeService.ScanWithWebcamAsync(this, CancellationToken.None);
                if (string.IsNullOrWhiteSpace(barcode)) return;
                txtBarcode.Text = barcode;
                await FindAndSelectBookByBarcodeAsync(barcode);
            }
            catch { }
        }

        private async Task FindAndSelectBookByBarcodeAsync(string? barcode)
        {
            barcode = string.IsNullOrWhiteSpace(barcode) ? null : barcode.Trim();
            if (barcode == null) return;

            var book = await bookDAO.GetByBarcodeAsync(barcode);
            if (book == null)
            {
                book = await openLibraryBookService.ImportBookByBarcodeOrIsbnAsync(barcode);
            }

            if (book == null)
            {
                MessageBox.Show("Không tìm thấy sách theo barcode!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            suppressBookSearchTextChanged = true;
            txtBookSearch.Text = barcode;
            suppressBookSearchTextChanged = false;

            await LoadBooksByExactCodeAsync(barcode, book);
            SelectBookRow(book.BookID);
        }

        private void SelectBookRow(int bookId)
        {
            foreach (DataGridViewRow row in dgvBooks.Rows)
            {
                if (row.Cells["BookID"].Value == null) continue;
                if (Convert.ToInt32(row.Cells["BookID"].Value) == bookId)
                {
                    row.Selected = true;
                    dgvBooks.CurrentCell = row.Cells["Title"];
                    break;
                }
            }
        }

        private async void DgvBooks_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow == null)
            {
                selectedBook = null;
                return;
            }

            int bookId = Convert.ToInt32(dgvBooks.CurrentRow.Cells["BookID"].Value);
            selectedBook = await bookDAO.GetByIdAsync(bookId);
            if (btnReserve != null)
            {
                btnReserve.Enabled = selectedBook != null && selectedBook.AvailableCopies <= 0 && currentMember != null;
            }
        }

        private async void BtnBorrow_Click(object? sender, EventArgs e)
        {
            // Validate
            if (currentMember == null)
            {
                MessageBox.Show("Vui lòng tìm và chọn độc giả trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMemberCode.Focus();
                return;
            }

            if (selectedBook == null)
            {
                MessageBox.Show("Vui lòng chọn sách cần mượn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var member = currentMember;
            var bookToBorrow = selectedBook;

            int selectedDays = (int)numDays.Value;
            int maxDays = Member.GetMaxBorrowDaysByType(member.MemberType);
            if (selectedDays > maxDays)
            {
                MessageBox.Show($"Loại thẻ {member.MemberType} chỉ được mượn tối đa {maxDays} ngày.",
                    "Cảnh báo số ngày mượn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numDays.Value = maxDays;
                return;
            }

            // Check if member can borrow
            var (canBorrow, message) = memberDAO.CanBorrow(member.MemberID);
            if (!canBorrow)
            {
                MessageBox.Show(message, "Không thể mượn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm
            var result = MessageBox.Show(
                $"Xác nhận mượn sách:\n\n" +
                $"Độc giả: {member.FullName} ({member.MemberCode})\n" +
                $"Sách: {bookToBorrow.Title}\n" +
                $"Số ngày mượn: {numDays.Value} ngày\n" +
                $"Hạn trả: {DateTime.Now.AddDays((int)numDays.Value):dd/MM/yyyy}",
                "Xác nhận mượn sách",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var (success, msg) = await borrowDAO.BorrowBookAsync(
                    member.MemberID,
                    bookToBorrow.BookID,
                    CurrentUser.User?.UserID ?? 0,
                    (int)numDays.Value
                );

                if (success)
                {
                    MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string borrowCode = ExtractBorrowCode(msg) ?? "";
                    string receiptPath = receiptService.PrintBorrowReceipt(new BorrowReceiptData
                    {
                        LibraryName = ReceiptService.GetLibraryName(),
                        BorrowCode = borrowCode,
                        MemberName = member.FullName,
                        MemberCode = member.MemberCode,
                        BookTitle = bookToBorrow.Title,
                        BookIsbn = bookToBorrow.ISBN,
                        BookBarcode = bookToBorrow.Barcode,
                        BorrowDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays((int)numDays.Value),
                        StaffName = CurrentUser.User?.FullName
                    });
                    MessageBox.Show($"Đã xuất phiếu mượn PDF:\n{receiptPath}", "Phiếu mượn", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string targetEmail = await ResolveMemberEmailAsync(member.MemberID, member.Email);
                    if (!string.IsNullOrWhiteSpace(targetEmail))
                    {
                        member.Email = targetEmail;
                        bool sent = await emailService.SendBorrowSuccessAsync(member, bookToBorrow, DateTime.Now.AddDays((int)numDays.Value), borrowCode);
                        if (!sent)
                        {
                            MessageBox.Show(
                                $"Đã tạo phiếu mượn nhưng chưa gửi được email thông báo.\n\nChi tiết: {emailService.LastError}",
                                "Cảnh báo gửi email",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "Đã tạo phiếu mượn nhưng chưa gửi email vì độc giả chưa có địa chỉ email hợp lệ.",
                            "Cảnh báo gửi email",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                    // Refresh
                    await LoadBooksAsync();
                    await LoadMemberBorrowingsAsync();
                    BtnFindMember_Click(null, EventArgs.Empty); // Refresh member status
                    LoadRecommendations();

                    // Log
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log($"Mượn sách: {bookToBorrow.Title}", "BorrowRecords", bookToBorrow.BookID);
                }
                else
                {
                    if (msg.Contains("Sách đã hết", StringComparison.OrdinalIgnoreCase))
                    {
                        var reserve = MessageBox.Show(
                            "Sách đã hết. Bạn có muốn đặt trước cho độc giả này không?",
                            "Đặt trước sách",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (reserve == DialogResult.Yes)
                        {
                            ReserveCurrentBook();
                        }
                    }
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void FormBorrow_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            ApplyFigmaBorrowLayout();

            try
            {
                int maxDays = settingDAO.GetIntValue(SystemSetting.KEY_MAX_BORROW_DAYS, 14);
                maxDays = Math.Max((int)numDays.Minimum, Math.Min((int)numDays.Maximum, maxDays));
                numDays.Value = maxDays;
                allowedBorrowDays = maxDays;
            }
            catch
            {
                numDays.Value = 14;
                allowedBorrowDays = 14;
            }

            // Load available books
            await LoadBooksAsync();
        }

        private void FormBorrow_Resize(object? sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            ApplyFigmaBorrowLayout();
        }

        private void ApplyFigmaBorrowLayout()
        {
            BackColor = Color.FromArgb(241, 245, 249);

            if (heroPanel == null)
            {
                heroPanel = new Panel
                {
                    Name = "heroBorrowPanel",
                    BackColor = Color.FromArgb(30, 64, 175)
                };
                heroPanel.Paint += (_, e) =>
                {
                    using var brush = new LinearGradientBrush(heroPanel.ClientRectangle,
                        Color.FromArgb(30, 64, 175), Color.FromArgb(15, 23, 42), 15f);
                    e.Graphics.FillRectangle(brush, heroPanel.ClientRectangle);
                };
                Controls.Add(heroPanel);

                lblHeroSubtitle = new Label
                {
                    Text = "Quét mã, tìm sách và xử lý phiếu mượn trong một màn hình",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(191, 219, 254),
                    AutoSize = true
                };
                heroPanel.Controls.Add(lblHeroSubtitle);
            }

            int margin = 16;
            int heroHeight = 110;
            int gap = 14;
            int bodyTop = margin + heroHeight + gap;
            int bodyHeight = ClientSize.Height - bodyTop - margin;
            int leftWidth = Math.Max(520, (ClientSize.Width - margin * 2 - gap) * 42 / 100);
            int rightWidth = ClientSize.Width - margin * 2 - gap - leftWidth;

            heroPanel.Bounds = new Rectangle(margin, margin, ClientSize.Width - margin * 2, heroHeight);

            lblTitle.Parent = heroPanel;
            lblTitle.Text = "Trạm mượn sách";
            lblTitle.Location = new Point(20, 14);
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            if (lblHeroSubtitle != null)
            {
                lblHeroSubtitle.Location = new Point(22, 64);
            }

            panelMember.BackColor = Color.White;
            panelMember.BorderStyle = BorderStyle.FixedSingle;
            panelMember.Location = new Point(margin, bodyTop);
            panelMember.Size = new Size(leftWidth, 220);

            lblMemberTitle.Text = "Thông tin độc giả";
            lblMemberTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblMemberTitle.ForeColor = Color.FromArgb(15, 23, 42);

            btnFindMember.BackColor = Color.FromArgb(37, 99, 235);
            btnFindMember.FlatStyle = FlatStyle.Flat;
            btnFindMember.FlatAppearance.BorderSize = 0;
            btnFindMember.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            lblCurrentBorrow.Location = new Point(margin, panelMember.Bottom + 12);
            lblCurrentBorrow.Text = "Sách đang mượn";
            lblCurrentBorrow.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCurrentBorrow.ForeColor = Color.FromArgb(30, 41, 59);

            dgvBorrowing.Location = new Point(margin, lblCurrentBorrow.Bottom + 8);
            dgvBorrowing.Size = new Size(leftWidth, bodyTop + bodyHeight - (lblCurrentBorrow.Bottom + 8));
            dgvBorrowing.EnableHeadersVisualStyles = false;
            dgvBorrowing.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            panelBook.BackColor = Color.White;
            panelBook.BorderStyle = BorderStyle.FixedSingle;
            panelBook.Location = new Point(panelMember.Right + gap, bodyTop);
            panelBook.Size = new Size(rightWidth, bodyHeight);

            lblBookTitle.Text = "Chọn đầu sách mượn";
            lblBookTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            btnScanBarcode.BackColor = Color.FromArgb(59, 130, 246);
            btnScanBarcode.FlatStyle = FlatStyle.Flat;
            btnScanBarcode.FlatAppearance.BorderSize = 0;
            btnScanBarcode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            dgvBooks.EnableHeadersVisualStyles = false;
            dgvBooks.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            btnBorrow.BackColor = Color.FromArgb(37, 99, 235);
            btnBorrow.FlatStyle = FlatStyle.Flat;
            btnBorrow.FlatAppearance.BorderSize = 0;

            if (btnReserve != null)
            {
                btnReserve.BackColor = Color.FromArgb(99, 102, 241);
                btnReserve.FlatStyle = FlatStyle.Flat;
                btnReserve.FlatAppearance.BorderSize = 0;
                btnReserve.ForeColor = Color.White;
                btnReserve.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }
        }

        private async Task<string> ResolveMemberEmailAsync(int memberId, string? currentEmail)
        {
            if (!string.IsNullOrWhiteSpace(currentEmail)) return currentEmail.Trim();

            Member? reloaded = await Task.Run(() => memberDAO.GetById(memberId));
            return reloaded?.Email?.Trim() ?? string.Empty;
        }

        private void BtnReserve_Click(object? sender, EventArgs e)
        {
            ReserveCurrentBook();
        }

        private void ReserveCurrentBook()
        {
            if (currentMember == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả trước khi đặt trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedBook == null)
            {
                MessageBox.Show("Vui lòng chọn sách cần đặt trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedBook.AvailableCopies > 0)
            {
                MessageBox.Show("Sách đang có sẵn, vui lòng mượn trực tiếp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var (success, message) = reservationDAO.ReserveBook(currentMember.MemberID, selectedBook.BookID);
            MessageBox.Show(message, success ? "Thành công" : "Thông báo",
                MessageBoxButtons.OK,
                success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private void NumDays_ValueChanged(object? sender, EventArgs e)
        {
            if (currentMember == null) return;

            int maxDays = Member.GetMaxBorrowDaysByType(currentMember.MemberType);
            if (numDays.Value > maxDays)
            {
                MessageBox.Show($"Loại thẻ {currentMember.MemberType} chỉ được mượn tối đa {maxDays} ngày.",
                    "Cảnh báo số ngày mượn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numDays.Value = maxDays;
            }
        }

        private static string? ExtractBorrowCode(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return null;
            int idx = message.IndexOf("PM", StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return null;
            int end = idx;
            while (end < message.Length && !char.IsWhiteSpace(message[end]) && message[end] != '.')
                end++;
            return message.Substring(idx, end - idx).Trim();
        }

        private sealed class RecommendationItem
        {
            public int BookID { get; set; }
            public string Title { get; set; } = "";
            public string Display { get; set; } = "";
            public override string ToString() => Display;
        }
    }
}
