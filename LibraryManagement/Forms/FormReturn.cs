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
    /// Form trả sách
    /// </summary>
    public partial class FormReturn : Form
    {
        private BorrowRecordDAO borrowDAO = new BorrowRecordDAO();
        private MemberDAO memberDAO = new MemberDAO();
        private BorrowRecord? selectedRecord;
        private BarcodeService barcodeService = new BarcodeService();
        private ReceiptService receiptService = new ReceiptService();
        private EmailService emailService = new EmailService();
        private ReservationDAO reservationDAO = new ReservationDAO();
        private Panel? heroPanel;
        private Label? lblHeroSubtitle;

        public FormReturn()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaReturnLayout();
                }
                catch
                {
                }
            }

            this.Load += FormReturn_Load;
            this.Resize += FormReturn_Resize;
        }

        private async void FormReturn_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                ApplyFigmaReturnLayout();
                cboStatus.SelectedIndex = 0;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void FormReturn_Resize(object? sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            ApplyFigmaReturnLayout();
        }

        private void ApplyFigmaReturnLayout()
        {
            BackColor = Color.FromArgb(241, 245, 249);

            if (heroPanel == null)
            {
                heroPanel = new Panel
                {
                    Name = "heroReturnPanel",
                    BackColor = Color.FromArgb(30, 64, 175)
                };
                heroPanel.Paint += (_, e) =>
                {
                    using var brush = new LinearGradientBrush(heroPanel.ClientRectangle,
                        Color.FromArgb(30, 64, 175), Color.FromArgb(15, 23, 42), 18f);
                    e.Graphics.FillRectangle(brush, heroPanel.ClientRectangle);
                };
                Controls.Add(heroPanel);

                lblHeroSubtitle = new Label
                {
                    Text = "Quét mã, kiểm tra trạng thái và trả sách nhanh",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(191, 219, 254),
                    AutoSize = true
                };
                heroPanel.Controls.Add(lblHeroSubtitle);
            }

            int margin = 16;
            int heroHeight = 108;
            int gap = 12;

            heroPanel.Bounds = new Rectangle(margin, margin, ClientSize.Width - margin * 2, heroHeight);

            lblTitle.Parent = heroPanel;
            lblTitle.Text = "Trung tâm trả sách";
            lblTitle.Location = new Point(20, 14);
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            if (lblHeroSubtitle != null)
            {
                lblHeroSubtitle.Location = new Point(22, 64);
            }

            panelSearch.BackColor = Color.White;
            panelSearch.BorderStyle = BorderStyle.FixedSingle;
            panelSearch.Bounds = new Rectangle(margin, heroPanel.Bottom + gap, ClientSize.Width - margin * 2, 64);

            lblSearch.Location = new Point(14, 23);
            txtSearch.Location = new Point(76, 19);
            txtSearch.Size = new Size(290, 25);

            lblStatus.Location = new Point(376, 23);
            cboStatus.Location = new Point(444, 19);
            cboStatus.Size = new Size(150, 23);

            lblBarcode.Location = new Point(606, 23);
            txtBarcode.Location = new Point(664, 19);
            txtBarcode.Size = new Size(214, 25);

            btnScanBarcode.Location = new Point(panelSearch.Width - 226, 16);
            btnScanBarcode.Size = new Size(108, 32);
            btnScanBarcode.Text = "Quét barcode";
            btnScanBarcode.BackColor = Color.FromArgb(59, 130, 246);
            btnScanBarcode.ForeColor = Color.White;
            btnScanBarcode.FlatStyle = FlatStyle.Flat;
            btnScanBarcode.FlatAppearance.BorderSize = 0;
            btnScanBarcode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            btnRefresh.Location = new Point(panelSearch.Width - 108, 16);
            btnRefresh.Size = new Size(92, 32);
            btnRefresh.BackColor = Color.FromArgb(37, 99, 235);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            int gridTop = panelSearch.Bottom + gap;
            int bottomRowTop = ClientSize.Height - margin - 62;

            dgvBorrowRecords.Bounds = new Rectangle(margin, gridTop, ClientSize.Width - margin * 2, bottomRowTop - gridTop - gap);
            dgvBorrowRecords.EnableHeadersVisualStyles = false;
            dgvBorrowRecords.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(5)
            };

            panelInfo.BackColor = Color.White;
            panelInfo.BorderStyle = BorderStyle.FixedSingle;
            panelInfo.Bounds = new Rectangle(margin, bottomRowTop, ClientSize.Width - margin * 2 - 258, 62);

            lblSelectedInfo.Location = new Point(8, 6);
            lblSelectedInfo.Size = new Size(panelInfo.Width - 16, 50);

            btnReturn.Bounds = new Rectangle(panelInfo.Right + 10, bottomRowTop, 126, 62);
            btnReturn.BackColor = Color.FromArgb(37, 99, 235);
            btnReturn.ForeColor = Color.White;
            btnReturn.FlatStyle = FlatStyle.Flat;
            btnReturn.FlatAppearance.BorderSize = 0;

            btnRenew.Bounds = new Rectangle(btnReturn.Right + 8, bottomRowTop, 114, 62);
            btnRenew.BackColor = Color.FromArgb(99, 102, 241);
            btnRenew.ForeColor = Color.White;
            btnRenew.FlatStyle = FlatStyle.Flat;
            btnRenew.FlatAppearance.BorderSize = 0;
        }

        private async void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            await SearchRecordsAsync();
        }

        private void TxtBarcode_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _ = ApplyBarcodeSearchAsync(txtBarcode.Text);
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
                await ApplyBarcodeSearchAsync(barcode);
            }
            catch { }
        }

        private async Task ApplyBarcodeSearchAsync(string? barcode)
        {
            barcode = string.IsNullOrWhiteSpace(barcode) ? null : barcode.Trim();
            if (barcode == null) return;

            txtSearch.Text = barcode;
            await SearchRecordsAsync();
        }

        private async void CboStatus_SelectedIndexChanged(object? sender, EventArgs e)
        {
            await SearchRecordsAsync();
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void DgvBorrowRecords_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            ReturnBook();
        }

        private void BtnReturnBook_Click(object? sender, EventArgs e)
        {
            ReturnBook();
        }

        private void BtnRenewBook_Click(object? sender, EventArgs e)
        {
            RenewBook();
        }

        private async Task LoadDataAsync()
        {
            // Update overdue status first
            await borrowDAO.UpdateOverdueStatusAsync();
            await SearchRecordsAsync();
        }

        private async Task SearchRecordsAsync()
        {
            try
            {
                string? keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
                string? status = cboStatus.SelectedIndex > 0 ? cboStatus.SelectedItem?.ToString() : null;

                // Only show borrowing/overdue records (not returned)
                var records = await borrowDAO.SearchAsync(keyword, status);
                records = records.Where(r => r.Status == BorrowRecord.STATUS_BORROWING || r.Status == BorrowRecord.STATUS_OVERDUE).ToList();

                dgvBorrowRecords.Rows.Clear();
                foreach (var record in records)
                {
                    string daysText;
                    if (record.IsOverdue)
                    {
                        daysText = $"Quá {record.DaysOverdue} ngày";
                    }
                    else
                    {
                        daysText = $"Còn {record.DaysRemaining} ngày";
                    }

                    var row = dgvBorrowRecords.Rows.Add(
                        record.BorrowID,
                        record.BorrowCode,
                        record.MemberCode,
                        record.MemberName,
                        record.BookTitle,
                        record.BorrowDate.ToString("dd/MM/yyyy"),
                        record.DueDate.ToString("dd/MM/yyyy"),
                        daysText,
                        record.Status
                    );

                    // Highlight overdue
                    if (record.IsOverdue)
                    {
                        dgvBorrowRecords.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200);
                        dgvBorrowRecords.Rows[row].DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DgvBorrowRecords_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvBorrowRecords.CurrentRow == null)
            {
                selectedRecord = null;
                lblSelectedInfo.Text = "";
                return;
            }

            int borrowId = Convert.ToInt32(dgvBorrowRecords.CurrentRow.Cells["BorrowID"].Value);
            selectedRecord = await borrowDAO.GetByIdAsync(borrowId);

            if (selectedRecord != null)
            {
                var settingDAO = new SystemSettingDAO();
                decimal finePerDay = settingDAO.GetDecimalValue(SystemSetting.KEY_FINE_PER_DAY, 10000);
                decimal estimatedFine = 0;
                decimal currentDebt = 0;

                var member = memberDAO.GetById(selectedRecord.MemberID);
                if (member != null)
                {
                    currentDebt = member.TotalFine;
                }

                if (selectedRecord.IsOverdue)
                {
                    estimatedFine = selectedRecord.DaysOverdue * finePerDay;
                }

                lblSelectedInfo.Text = $"📌 Đã chọn: {selectedRecord.BookTitle}\n" +
                    $"   Độc giả: {selectedRecord.MemberName} | " +
                    $"Mượn: {selectedRecord.BorrowDate:dd/MM/yyyy} | " +
                    $"Hạn: {selectedRecord.DueDate:dd/MM/yyyy} | " +
                    (estimatedFine > 0 ? $"Tiền phạt dự kiến: {estimatedFine:N0} VNĐ" : "Không phạt") +
                    $" | Nợ hiện tại: {currentDebt:N0} VNĐ";

                lblSelectedInfo.ForeColor = selectedRecord.IsOverdue ? Color.DarkRed : Color.Black;
            }
        }

        private async void ReturnBook()
        {
            if (selectedRecord == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu mượn cần trả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate fine preview
            var settingDAO = new SystemSettingDAO();
            decimal finePerDay = settingDAO.GetDecimalValue(SystemSetting.KEY_FINE_PER_DAY, 10000);
            decimal estimatedFine = selectedRecord.IsOverdue ? selectedRecord.DaysOverdue * finePerDay : 0;

            string message = $"Xác nhận trả sách:\n\n" +
                $"Mã phiếu: {selectedRecord.BorrowCode}\n" +
                $"Độc giả: {selectedRecord.MemberName}\n" +
                $"Sách: {selectedRecord.BookTitle}\n" +
                $"Ngày mượn: {selectedRecord.BorrowDate:dd/MM/yyyy}\n" +
                $"Hạn trả: {selectedRecord.DueDate:dd/MM/yyyy}\n";

            if (estimatedFine > 0)
            {
                message += $"\n⚠️ Quá hạn {selectedRecord.DaysOverdue} ngày\n" +
                    $"Tiền phạt: {estimatedFine:N0} VNĐ";
            }

            var result = MessageBox.Show(message, "Xác nhận trả sách",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var (success, msg, fineAmount) = await borrowDAO.ReturnBookAsync(selectedRecord.BorrowID, CurrentUser.User?.UserID ?? 0);

                if (success)
                {
                    // Log
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log($"Trả sách: {selectedRecord.BookTitle}", "BorrowRecords", selectedRecord.BorrowID);

                    string receiptPath = receiptService.PrintReturnReceipt(new ReturnReceiptData
                    {
                        LibraryName = ReceiptService.GetLibraryName(),
                        BorrowCode = selectedRecord.BorrowCode,
                        MemberName = selectedRecord.MemberName ?? "",
                        MemberCode = selectedRecord.MemberCode ?? "",
                        BookTitle = selectedRecord.BookTitle ?? "",
                        BookIsbn = selectedRecord.ISBN,
                        BookBarcode = selectedRecord.BookBarcode,
                        BorrowDate = selectedRecord.BorrowDate,
                        DueDate = selectedRecord.DueDate,
                        ReturnDate = DateTime.Now,
                        FineAmount = fineAmount,
                        StaffName = CurrentUser.User?.FullName
                    });
                    MessageBox.Show($"Đã xuất phiếu trả PDF:\n{receiptPath}", "Phiếu trả", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string returnEmail = await ResolveMemberEmailAsync(selectedRecord.MemberID, selectedRecord.MemberEmail);
                    if (!string.IsNullOrWhiteSpace(returnEmail))
                    {
                        selectedRecord.MemberEmail = returnEmail;
                        bool sent = await emailService.SendReturnSuccessAsync(selectedRecord, fineAmount);
                        if (!sent)
                        {
                            MessageBox.Show(
                                $"Đã trả sách thành công nhưng chưa gửi được email thông báo.\n\nChi tiết: {emailService.LastError}",
                                "Cảnh báo gửi email",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "Đã trả sách thành công nhưng chưa gửi email vì độc giả chưa có địa chỉ email hợp lệ.",
                            "Cảnh báo gửi email",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    _ = NotifyReservationsWhenBookAvailableAsync(selectedRecord.BookID);

                    if (fineAmount > 0)
                    {
                        MessageBox.Show($"Trả sách thành công!\n\nTiền phạt: {fineAmount:N0} VNĐ\n" +
                            "Tiền phạt đã được cộng vào tài khoản độc giả.",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var member = memberDAO.GetById(selectedRecord.MemberID);
                        if (member != null && member.TotalFine > 0)
                        {
                            var payNow = MessageBox.Show(
                                $"Độc giả hiện còn nợ {member.TotalFine:N0} VNĐ. Bạn có muốn thanh toán ngay không?",
                                "Thanh toán tiền phạt",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (payNow == DialogResult.Yes)
                            {
                                using var payForm = new FormPayFine(member);
                                payForm.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Trả sách thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    await LoadDataAsync();
                    lblSelectedInfo.Text = "";
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenewBook()
        {
            if (selectedRecord == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu mượn cần gia hạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedRecord.IsOverdue)
            {
                MessageBox.Show("Không thể gia hạn sách đã quá hạn!\nVui lòng trả sách và mượn lại.",
                    "Không thể gia hạn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var renewForm = new FormRenewBook(selectedRecord))
            {
                if (renewForm.ShowDialog() == DialogResult.OK)
                {
                    _ = LoadDataAsync();
                }
            }
        }

        private async Task NotifyReservationsWhenBookAvailableAsync(int bookId)
        {
            try
            {
                var list = reservationDAO.GetPendingReservationsForBook(bookId, 20);
                int notifiedCount = 0;
                int failedCount = 0;
                string firstError = string.Empty;
                foreach (var item in list)
                {
                    string targetEmail = await ResolveMemberEmailAsync(item.MemberID, item.MemberEmail);
                    if (string.IsNullOrWhiteSpace(targetEmail))
                    {
                        failedCount++;
                        if (string.IsNullOrWhiteSpace(firstError))
                        {
                            firstError = $"Độc giả {item.MemberName} chưa có email hợp lệ.";
                        }
                        continue;
                    }

                    bool sent = await emailService.SendReservationAvailableAsync(targetEmail, item.MemberName, item.BookTitle);
                    if (!sent)
                    {
                        failedCount++;
                        if (string.IsNullOrWhiteSpace(firstError))
                        {
                            firstError = emailService.LastError;
                        }
                        continue;
                    }

                    reservationDAO.MarkAsNotified(item.ReservationID);
                    notifiedCount++;
                }

                if (notifiedCount > 0)
                {
                    MessageBox.Show($"Đã gửi {notifiedCount} email thông báo sách đặt trước đã có.",
                        "Thông báo đặt trước",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                if (failedCount > 0)
                {
                    MessageBox.Show(
                        $"Có {failedCount} email đặt trước chưa gửi được.\nChi tiết: {firstError}",
                        "Cảnh báo gửi email",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch
            {
            }
        }

        private async Task<string> ResolveMemberEmailAsync(int memberId, string? currentEmail)
        {
            if (!string.IsNullOrWhiteSpace(currentEmail)) return currentEmail.Trim();

            Member? reloaded = await Task.Run(() => memberDAO.GetById(memberId));
            return reloaded?.Email?.Trim() ?? string.Empty;
        }
    }
}
