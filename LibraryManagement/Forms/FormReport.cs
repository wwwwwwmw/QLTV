using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form thống kê báo cáo
    /// </summary>
    public partial class FormReport : Form
    {
        private Panel? heroPanel;
        private Label? lblHeroSubtitle;
        private readonly ExcelReportService excelReportService = new();
        private bool isUpdatingSheetSelection;

        private sealed class ExportSheetOption
        {
            public ExportSheetOption(string key, string label)
            {
                Key = key;
                Label = label;
            }

            public string Key { get; }
            public string Label { get; }

            public override string ToString() => Label;
        }

        public FormReport()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaReportLayout();
                }
                catch
                {
                }
            }

            this.Load += FormReport_Load;
            this.Resize += FormReport_Resize;
        }

        private void FormReport_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                ApplyFigmaReportLayout();
                dtpFrom.Value = DateTime.Today.AddMonths(-1);
                dtpTo.Value = DateTime.Today;
                cboReportType.SelectedIndex = 0;
                InitializeExportSheetOptions();
                LoadDashboardStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void FormReport_Resize(object? sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            ApplyFigmaReportLayout();
        }

        private void ApplyFigmaReportLayout()
        {
            BackColor = Color.FromArgb(241, 245, 249);

            if (heroPanel == null)
            {
                heroPanel = new Panel
                {
                    Name = "heroReportPanel",
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
                    Text = "Phân tích dữ liệu mượn trả và xuất báo cáo nhanh",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(191, 219, 254),
                    AutoSize = true
                };
                heroPanel.Controls.Add(lblHeroSubtitle);
            }

            int margin = 16;
            int heroHeight = 110;
            int gap = 12;

            heroPanel.Bounds = new Rectangle(margin, margin, ClientSize.Width - margin * 2, heroHeight);

            lblTitle.Parent = heroPanel;
            lblTitle.Text = "Trung tâm báo cáo";
            lblTitle.Location = new Point(20, 14);
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            if (lblHeroSubtitle != null)
            {
                lblHeroSubtitle.Location = new Point(22, 64);
            }

            panelFilter.BackColor = Color.White;
            panelFilter.BorderStyle = BorderStyle.FixedSingle;
            panelFilter.Location = new Point(margin, heroPanel.Bottom + gap);
            panelFilter.Size = new Size(ClientSize.Width - margin * 2, 146);

            lblType.Location = new Point(18, 20);
            cboReportType.Location = new Point(76, 16);
            cboReportType.Size = new Size(230, 23);

            lblFrom.Location = new Point(320, 20);
            dtpFrom.Location = new Point(348, 16);
            dtpFrom.Size = new Size(120, 23);

            lblTo.Location = new Point(478, 20);
            dtpTo.Location = new Point(512, 16);
            dtpTo.Size = new Size(120, 23);

            StyleActionButton(btnGenerate, Color.FromArgb(37, 99, 235));
            StyleActionButton(btnExport, Color.FromArgb(16, 185, 129));
            StyleActionButton(btnImport, Color.FromArgb(14, 116, 144));
            StyleActionButton(btnPrint, Color.FromArgb(99, 102, 241));
            StyleActionButton(btnBorrowReturnDetails, Color.FromArgb(59, 130, 246));

            btnGenerate.Location = new Point(panelFilter.Width - 450, 14);
            btnGenerate.Size = new Size(108, 34);
            btnExport.Location = new Point(panelFilter.Width - 334, 14);
            btnExport.Size = new Size(100, 34);
            btnImport.Location = new Point(panelFilter.Width - 226, 14);
            btnImport.Size = new Size(100, 34);
            btnPrint.Location = new Point(panelFilter.Width - 118, 14);
            btnPrint.Size = new Size(56, 34);

            lblQuickActions.Location = new Point(18, 70);
            btnBorrowReturnDetails.Location = new Point(102, 62);
            btnBorrowReturnDetails.Size = new Size(118, 34);

            lblExportData.Location = new Point(240, 70);
            chkSelectAllSheets.Location = new Point(334, 68);
            clbExportSheets.Location = new Point(430, 58);
            clbExportSheets.Size = new Size(Math.Max(300, panelFilter.Width - 500), 72);

            panelContent.BackColor = Color.White;
            panelContent.BorderStyle = BorderStyle.FixedSingle;
            panelContent.Location = new Point(margin, panelFilter.Bottom + gap);
            panelContent.Size = new Size(ClientSize.Width - margin * 2, ClientSize.Height - (panelFilter.Bottom + gap) - 52);

            lblSummary.Location = new Point(margin, panelContent.Bottom + 8);
            lblSummary.Size = new Size(ClientSize.Width - margin * 2, 30);
            lblSummary.ForeColor = Color.FromArgb(51, 65, 85);
            lblSummary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }

        private static void StyleActionButton(Button button, Color backColor)
        {
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void CboReportType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            GenerateReport();
        }

        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            GenerateReport();
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void BtnImport_Click(object? sender, EventArgs e)
        {
            ImportFromExcel();
        }

        private void ChkSelectAllSheets_CheckedChanged(object? sender, EventArgs e)
        {
            if (isUpdatingSheetSelection)
                return;

            bool isChecked = chkSelectAllSheets.Checked;
            isUpdatingSheetSelection = true;
            for (int i = 0; i < clbExportSheets.Items.Count; i++)
            {
                clbExportSheets.SetItemChecked(i, isChecked);
            }
            isUpdatingSheetSelection = false;
        }

        private void ClbExportSheets_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (isUpdatingSheetSelection)
                return;

            BeginInvoke(new Action(UpdateSelectAllCheckboxState));
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            PrintReport();
        }

        private void BtnBorrowReturnDetails_Click(object? sender, EventArgs e)
        {
            OpenBorrowReturnDetailsForm();
        }

        private void LoadDashboardStats()
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            panelContent.Controls.Clear();
            lblSummary.Text = "";

            switch (cboReportType.SelectedIndex)
            {
                case 0: // Tổng quan
                    ShowDashboardOverview();
                    break;
                case 1: // Sách mượn nhiều nhất
                    ShowMostBorrowedBooks();
                    break;
                case 2: // Độc giả mượn nhiều nhất
                    ShowTopBorrowers();
                    break;
                case 3: // Sách quá hạn
                    ShowOverdueBooks();
                    break;
                case 4: // Thống kê theo ngày
                    ShowDailyStats();
                    break;
                case 5: // Phạt chưa thu
                    ShowUnpaidFines();
                    break;
                case 6: // Sách hết
                    ShowOutOfStockBooks();
                    break;
            }
        }

        private void ShowDashboardOverview()
        {
            try
            {
                var borrowDAO = new BorrowRecordDAO();
                var stats = borrowDAO.GetDashboardStats();
                var memberDAO = new MemberDAO();
                var totalMembers = memberDAO.GetAll().Count;

                // Stats cards
                CreateStatCard("📚 Tổng sách", stats.TotalBooks.ToString("N0"), Color.FromArgb(52, 152, 219), 30, 30);
                CreateStatCard("🔄 Đang mượn", stats.BorrowingBooks.ToString("N0"), Color.FromArgb(46, 204, 113), 300, 30);
                CreateStatCard("⚠️ Quá hạn", stats.OverdueBooks.ToString("N0"), Color.FromArgb(231, 76, 60), 570, 30);
                CreateStatCard("👥 Độc giả", totalMembers.ToString("N0"), Color.FromArgb(155, 89, 182), 840, 30);

                // Recent activities - get today's stats
                var lblRecent = new Label
                {
                    Text = $"📅 Thống kê hôm nay ({DateTime.Today:dd/MM/yyyy}):",
                    Location = new Point(30, 170),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };
                panelContent.Controls.Add(lblRecent);

                var todayBorrowed = borrowDAO.CountByDate(DateTime.Today, "Borrow");
                var todayReturned = borrowDAO.CountByDate(DateTime.Today, "Return");

                var lblToday = new Label
                {
                    Text = $"   📖 Mượn: {todayBorrowed} phiếu    |    📥 Trả: {todayReturned} phiếu",
                    Location = new Point(30, 200),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 11)
                };
                panelContent.Controls.Add(lblToday);

                // Available books
                var bookDAO = new BookDAO();
                var allBooks = bookDAO.Search(null, null, null);
                int available = 0;
                int outOfStock = 0;
                foreach (var book in allBooks)
                {
                    if (book.AvailableQuantity > 0) available++;
                    else outOfStock++;
                }

                var lblAvail = new Label
                {
                    Text = $"   ✅ Sách còn: {available}    |    ❌ Sách hết: {outOfStock}",
                    Location = new Point(30, 230),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 11)
                };
                panelContent.Controls.Add(lblAvail);

                lblSummary.Text = $"📊 Báo cáo tổng quan - Cập nhật lúc {DateTime.Now:HH:mm dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateStatCard(string title, string value, Color color, int x, int y)
        {
            var card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(240, 110),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var accent = new Panel
            {
                Dock = DockStyle.Top,
                Height = 5,
                BackColor = color
            };

            var lblTitle = new Label
            {
                Text = title,
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(71, 85, 105)
            };

            var lblValue = new Label
            {
                Text = value,
                Location = new Point(15, 50),
                AutoSize = true,
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42)
            };

            card.Controls.Add(accent);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            panelContent.Controls.Add(card);
        }

        private void ShowMostBorrowedBooks()
        {
            var dgv = CreateReportGrid();
            dgv.Columns.Add("STT", "STT");
            dgv.Columns.Add("BookTitle", "Tên sách");
            dgv.Columns.Add("Author", "Tác giả");
            dgv.Columns.Add("BorrowCount", "Lượt mượn");
            dgv.Columns.Add("Quantity", "Tồn kho");

            dgv.Columns["STT"]!.Width = 50;
            dgv.Columns["BookTitle"]!.Width = 400;
            dgv.Columns["Author"]!.Width = 200;
            dgv.Columns["BorrowCount"]!.Width = 100;
            dgv.Columns["Quantity"]!.Width = 100;

            try
            {
                var borrowDAO = new BorrowRecordDAO();
                var topBooks = borrowDAO.GetMostBorrowedBooks(dtpFrom.Value, dtpTo.Value, 20);

                int stt = 1;
                foreach (var item in topBooks)
                {
                    dgv.Rows.Add(stt++, item.BookTitle, item.AuthorName, item.BorrowCount, item.Quantity);
                }

                lblSummary.Text = $"📊 Top {topBooks.Count} sách mượn nhiều nhất từ {dtpFrom.Value:dd/MM/yyyy} đến {dtpTo.Value:dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowTopBorrowers()
        {
            var dgv = CreateReportGrid();
            dgv.Columns.Add("STT", "STT");
            dgv.Columns.Add("MemberCode", "Mã thẻ");
            dgv.Columns.Add("MemberName", "Tên độc giả");
            dgv.Columns.Add("BorrowCount", "Số lần mượn");
            dgv.Columns.Add("Phone", "SĐT");

            dgv.Columns["STT"]!.Width = 50;
            dgv.Columns["MemberCode"]!.Width = 100;
            dgv.Columns["MemberName"]!.Width = 300;
            dgv.Columns["BorrowCount"]!.Width = 120;
            dgv.Columns["Phone"]!.Width = 150;

            try
            {
                var borrowDAO = new BorrowRecordDAO();
                var topMembers = borrowDAO.GetTopBorrowers(dtpFrom.Value, dtpTo.Value, 20);

                int stt = 1;
                foreach (var item in topMembers)
                {
                    dgv.Rows.Add(stt++, item.MemberCode, item.MemberName, item.BorrowCount, item.Phone);
                }

                lblSummary.Text = $"📊 Top {topMembers.Count} độc giả mượn nhiều nhất từ {dtpFrom.Value:dd/MM/yyyy} đến {dtpTo.Value:dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowOverdueBooks()
        {
            var dgv = CreateReportGrid();
            dgv.Columns.Add("BorrowCode", "Mã phiếu");
            dgv.Columns.Add("MemberName", "Độc giả");
            dgv.Columns.Add("BookTitle", "Tên sách");
            dgv.Columns.Add("DueDate", "Hạn trả");
            dgv.Columns.Add("DaysOverdue", "Số ngày quá hạn");
            dgv.Columns.Add("EstFine", "Tiền phạt dự kiến");

            dgv.Columns["BorrowCode"]!.Width = 110;
            dgv.Columns["MemberName"]!.Width = 200;
            dgv.Columns["BookTitle"]!.Width = 300;
            dgv.Columns["DueDate"]!.Width = 100;
            dgv.Columns["DaysOverdue"]!.Width = 120;
            dgv.Columns["EstFine"]!.Width = 130;

            try
            {
                var borrowDAO = new BorrowRecordDAO();
                var overdueList = borrowDAO.Search(null, BorrowRecord.STATUS_OVERDUE);

                var settingDAO = new SystemSettingDAO();
                decimal finePerDay = settingDAO.GetDecimalValue(SystemSetting.KEY_FINE_PER_DAY, 5000);
                decimal totalFine = 0;

                foreach (var record in overdueList)
                {
                    decimal fine = record.DaysOverdue * finePerDay;
                    totalFine += fine;
                    dgv.Rows.Add(record.BorrowCode, record.MemberName, record.BookTitle,
                        record.DueDate.ToString("dd/MM/yyyy"), record.DaysOverdue, fine.ToString("N0"));
                }

                lblSummary.Text = $"📊 Tổng: {overdueList.Count} phiếu quá hạn | Tổng tiền phạt dự kiến: {totalFine:N0} VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDailyStats()
        {
            var dgv = CreateReportGrid();
            dgv.Columns.Add("Date", "Ngày");
            dgv.Columns.Add("BorrowCount", "Số lượt mượn");
            dgv.Columns.Add("ReturnCount", "Số lượt trả");
            dgv.Columns.Add("NewMembers", "ĐG mới");

            dgv.Columns["Date"]!.Width = 150;
            dgv.Columns["BorrowCount"]!.Width = 150;
            dgv.Columns["ReturnCount"]!.Width = 150;
            dgv.Columns["NewMembers"]!.Width = 150;

            try
            {
                var borrowDAO = new BorrowRecordDAO();
                var dailyStats = borrowDAO.GetDailyStats(dtpFrom.Value, dtpTo.Value);

                int totalBorrow = 0, totalReturn = 0;
                foreach (var stat in dailyStats)
                {
                    dgv.Rows.Add(stat.Date.ToString("dd/MM/yyyy"), stat.BorrowCount, stat.ReturnCount, stat.NewMembers);
                    totalBorrow += stat.BorrowCount;
                    totalReturn += stat.ReturnCount;
                }

                lblSummary.Text = $"📊 Từ {dtpFrom.Value:dd/MM/yyyy} đến {dtpTo.Value:dd/MM/yyyy}: {totalBorrow} lượt mượn, {totalReturn} lượt trả";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowUnpaidFines()
        {
            var dgv = CreateReportGrid();
            dgv.Columns.Add("MemberCode", "Mã thẻ");
            dgv.Columns.Add("MemberName", "Tên độc giả");
            dgv.Columns.Add("Phone", "SĐT");
            dgv.Columns.Add("TotalFine", "Tổng nợ phạt");

            dgv.Columns["MemberCode"]!.Width = 120;
            dgv.Columns["MemberName"]!.Width = 300;
            dgv.Columns["Phone"]!.Width = 150;
            dgv.Columns["TotalFine"]!.Width = 150;

            try
            {
                var memberDAO = new MemberDAO();
                var members = memberDAO.GetAll();
                decimal totalUnpaid = 0;
                int count = 0;

                foreach (var member in members)
                {
                    if (member.TotalFine > 0)
                    {
                        dgv.Rows.Add(member.MemberCode, member.FullName, member.Phone, member.TotalFine.ToString("N0"));
                        totalUnpaid += member.TotalFine;
                        count++;
                    }
                }

                lblSummary.Text = $"📊 Có {count} độc giả còn nợ phạt | Tổng tiền phạt chưa thu: {totalUnpaid:N0} VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowOutOfStockBooks()
        {
            var dgv = CreateReportGrid();
            dgv.Columns.Add("BookCode", "Mã sách");
            dgv.Columns.Add("BookTitle", "Tên sách");
            dgv.Columns.Add("Author", "Tác giả");
            dgv.Columns.Add("Category", "Thể loại");
            dgv.Columns.Add("TotalQty", "Tổng SL");
            dgv.Columns.Add("BorrowedQty", "Đang mượn");

            dgv.Columns["BookCode"]!.Width = 100;
            dgv.Columns["BookTitle"]!.Width = 350;
            dgv.Columns["Author"]!.Width = 180;
            dgv.Columns["Category"]!.Width = 120;
            dgv.Columns["TotalQty"]!.Width = 80;
            dgv.Columns["BorrowedQty"]!.Width = 100;

            try
            {
                var bookDAO = new BookDAO();
                var allBooks = bookDAO.Search(null, null, null);
                int count = 0;

                foreach (var book in allBooks)
                {
                    if (book.AvailableQuantity <= 0)
                    {
                        dgv.Rows.Add(book.BookCode, book.Title, book.AuthorName, book.CategoryName,
                            book.Quantity, book.BorrowedQuantity);
                        count++;
                    }
                }

                lblSummary.Text = $"📊 Có {count} đầu sách đã hết (đang được mượn hết)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataGridView CreateReportGrid()
        {
            dgvReport = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(Math.Max(200, panelContent.ClientSize.Width - 20), Math.Max(120, panelContent.ClientSize.Height - 20)),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(5)
            };

            dgvReport.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(30, 41, 59),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(15, 23, 42),
                Padding = new Padding(4)
            };

            dgvReport.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            panelContent.Controls.Add(dgvReport);
            return dgvReport;
        }

        private void ExportToExcel()
        {
            var selectedSheets = GetSelectedSheetKeys();
            if (selectedSheets.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một loại dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                saveDialog.FileName = $"BaoCao_ThuVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (dtpFrom.Value.Date > dtpTo.Value.Date)
                        {
                            MessageBox.Show("Khoảng thời gian không hợp lệ. 'Từ ngày' phải nhỏ hơn hoặc bằng 'Đến ngày'.",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        excelReportService.ExportWorkbook(saveDialog.FileName, dtpFrom.Value, dtpTo.Value, selectedSheets);

                        MessageBox.Show("Xuất file Excel thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveDialog.FileName}\"");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void PrintReport()
        {
            MessageBox.Show("Tính năng in báo cáo đang được phát triển.\n" +
                "Bạn có thể xuất ra file XLSX và in từ Excel.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ImportFromExcel()
        {
            using var openDialog = new OpenFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                Title = "Chọn file Excel để nhập"
            };

            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                var importResult = excelReportService.ImportWorkbook(openDialog.FileName);

                string message = $"Nhập dữ liệu hoàn tất.\n- Thêm mới: {importResult.InsertedCount}\n- Cập nhật: {importResult.UpdatedCount}";

                if (importResult.HasErrors)
                {
                    message += "\n\nCảnh báo:";
                    int maxWarnings = Math.Min(importResult.Warnings.Count, 20);
                    for (int i = 0; i < maxWarnings; i++)
                    {
                        message += $"\n• {importResult.Warnings[i]}";
                    }

                    if (importResult.Warnings.Count > maxWarnings)
                    {
                        message += $"\n... và {importResult.Warnings.Count - maxWarnings} cảnh báo khác.";
                    }

                    MessageBox.Show(message, "Nhập Excel có cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(message, "Nhập Excel thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                GenerateReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nhập file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeExportSheetOptions()
        {
            isUpdatingSheetSelection = true;
            clbExportSheets.Items.Clear();
            foreach (var item in ExcelReportService.GetAvailableSheetOptions())
            {
                clbExportSheets.Items.Add(new ExportSheetOption(item.Key, item.Value), true);
            }
            chkSelectAllSheets.Checked = true;
            isUpdatingSheetSelection = false;
            UpdateSelectAllCheckboxState();
        }

        private List<string> GetSelectedSheetKeys()
        {
            return clbExportSheets.CheckedItems
                .OfType<ExportSheetOption>()
                .Select(x => x.Key)
                .ToList();
        }

        private void UpdateSelectAllCheckboxState()
        {
            if (clbExportSheets.Items.Count == 0)
            {
                chkSelectAllSheets.Checked = false;
                return;
            }

            bool allChecked = true;
            for (int i = 0; i < clbExportSheets.Items.Count; i++)
            {
                if (!clbExportSheets.GetItemChecked(i))
                {
                    allChecked = false;
                    break;
                }
            }

            if (chkSelectAllSheets.Checked != allChecked)
            {
                isUpdatingSheetSelection = true;
                chkSelectAllSheets.Checked = allChecked;
                isUpdatingSheetSelection = false;
            }
        }

        private void OpenBorrowReturnDetailsForm()
        {
            using (var form = new FormBorrowReturnDetails())
            {
                form.ShowDialog(this);
            }
        }

        private void ShowBorrowRecordDetails()
        {
            var dgv = CreateReportGrid();
            dgv.Columns.Add("BorrowCode", "Mã phiếu");
            dgv.Columns.Add("MemberName", "Độc giả");
            dgv.Columns.Add("BookTitle", "Tên sách");
            dgv.Columns.Add("BorrowDate", "Ngày mượn");
            dgv.Columns.Add("DueDate", "Hạn trả");
            dgv.Columns.Add("Status", "Trạng thái");
            dgv.Columns.Add("StaffName", "Nhân viên");

            dgv.Columns["BorrowCode"]!.Width = 130;
            dgv.Columns["MemberName"]!.Width = 180;
            dgv.Columns["BookTitle"]!.Width = 280;
            dgv.Columns["BorrowDate"]!.Width = 100;
            dgv.Columns["DueDate"]!.Width = 100;
            dgv.Columns["Status"]!.Width = 100;
            dgv.Columns["StaffName"]!.Width = 120;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT br.BorrowCode, m.FullName AS MemberName, b.Title AS BookTitle,
                                   br.BorrowDate, br.DueDate, br.Status, u.FullName AS StaffName
                            FROM BorrowRecords br
                            INNER JOIN Members m ON br.MemberID = m.MemberID
                            INNER JOIN Books b ON br.BookID = b.BookID
                            LEFT JOIN Users u ON br.StaffID = u.UserID
                            WHERE br.BorrowDate BETWEEN @FromDate AND @ToDate
                            ORDER BY br.BorrowDate DESC";
                        cmd.Parameters.AddWithValue("@FromDate", dtpFrom.Value.Date);
                        cmd.Parameters.AddWithValue("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1));

                        using (var reader = cmd.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                string status = reader["Status"].ToString() ?? "";
                                int rowIndex = dgv.Rows.Add(
                                    reader["BorrowCode"],
                                    reader["MemberName"],
                                    reader["BookTitle"],
                                    ((DateTime)reader["BorrowDate"]).ToString("dd/MM/yyyy"),
                                    ((DateTime)reader["DueDate"]).ToString("dd/MM/yyyy"),
                                    status,
                                    reader["StaffName"]
                                );

                                // Đổi màu theo trạng thái
                                if (status == "Quá hạn")
                                    dgv.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                else if (status == "Đã trả")
                                    dgv.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;

                                count++;
                            }
                            lblSummary.Text = $"📋 Danh sách {count} phiếu mượn từ {dtpFrom.Value:dd/MM/yyyy} đến {dtpTo.Value:dd/MM/yyyy}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowReturnRecordDetails()
        {
            var dgv = CreateReportGrid();
            dgv.Columns.Add("BorrowCode", "Mã phiếu");
            dgv.Columns.Add("MemberName", "Độc giả");
            dgv.Columns.Add("BookTitle", "Tên sách");
            dgv.Columns.Add("BorrowDate", "Ngày mượn");
            dgv.Columns.Add("ReturnDate", "Ngày trả");
            dgv.Columns.Add("FineAmount", "Tiền phạt");
            dgv.Columns.Add("StaffName", "Nhân viên");

            dgv.Columns["BorrowCode"]!.Width = 130;
            dgv.Columns["MemberName"]!.Width = 180;
            dgv.Columns["BookTitle"]!.Width = 280;
            dgv.Columns["BorrowDate"]!.Width = 100;
            dgv.Columns["ReturnDate"]!.Width = 100;
            dgv.Columns["FineAmount"]!.Width = 100;
            dgv.Columns["StaffName"]!.Width = 120;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT br.BorrowCode, m.FullName AS MemberName, b.Title AS BookTitle,
                                   br.BorrowDate, br.ReturnDate, br.FineAmount, u.FullName AS StaffName
                            FROM BorrowRecords br
                            INNER JOIN Members m ON br.MemberID = m.MemberID
                            INNER JOIN Books b ON br.BookID = b.BookID
                            LEFT JOIN Users u ON br.StaffID = u.UserID
                            WHERE br.Status = N'Đã trả' 
                              AND br.ReturnDate BETWEEN @FromDate AND @ToDate
                            ORDER BY br.ReturnDate DESC";
                        cmd.Parameters.AddWithValue("@FromDate", dtpFrom.Value.Date);
                        cmd.Parameters.AddWithValue("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1));

                        decimal totalFine = 0;
                        using (var reader = cmd.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                decimal fine = reader["FineAmount"] != DBNull.Value ? (decimal)reader["FineAmount"] : 0;
                                totalFine += fine;

                                int rowIndex = dgv.Rows.Add(
                                    reader["BorrowCode"],
                                    reader["MemberName"],
                                    reader["BookTitle"],
                                    ((DateTime)reader["BorrowDate"]).ToString("dd/MM/yyyy"),
                                    reader["ReturnDate"] != DBNull.Value ? ((DateTime)reader["ReturnDate"]).ToString("dd/MM/yyyy") : "-",
                                    fine > 0 ? fine.ToString("N0") + " đ" : "-",
                                    reader["StaffName"]
                                );

                                // Đổi màu nếu có phạt
                                if (fine > 0)
                                    dgv.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.OrangeRed;

                                count++;
                            }
                            lblSummary.Text = $"📋 Danh sách {count} phiếu trả từ {dtpFrom.Value:dd/MM/yyyy} đến {dtpTo.Value:dd/MM/yyyy} | 💰 Tổng tiền phạt: {totalFine:N0} đ";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
