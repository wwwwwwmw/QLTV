using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LibraryManagement.Data;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form xem chi tiết phiếu mượn/trả
    /// </summary>
    public partial class FormBorrowReturnDetails : Form
    {
        private Panel? heroPanel;
        private Label? heroSubtitle;

        public FormBorrowReturnDetails()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaLayout();
                }
                catch
                {
                }
            }

            this.Load += FormBorrowReturnDetails_Load;
            this.Resize += FormBorrowReturnDetails_Resize;
        }

        private void FormBorrowReturnDetails_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                ApplyFigmaLayout();
                dtpFrom.Value = DateTime.Today.AddMonths(-1);
                dtpTo.Value = DateTime.Today;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void FormBorrowReturnDetails_Resize(object? sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            ApplyFigmaLayout();
        }

        private void ApplyFigmaLayout()
        {
            BackColor = Color.FromArgb(241, 245, 249);

            if (heroPanel == null)
            {
                heroPanel = new Panel
                {
                    Name = "heroBorrowReturnPanel",
                    BackColor = Color.FromArgb(30, 64, 175)
                };
                heroPanel.Paint += (_, e) =>
                {
                    using var brush = new LinearGradientBrush(heroPanel.ClientRectangle,
                        Color.FromArgb(30, 64, 175), Color.FromArgb(15, 23, 42), 18f);
                    e.Graphics.FillRectangle(brush, heroPanel.ClientRectangle);
                };
                Controls.Add(heroPanel);

                heroSubtitle = new Label
                {
                    Text = "Theo dõi chi tiết toàn bộ phiếu mượn và phiếu trả",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(191, 219, 254),
                    AutoSize = true
                };
                heroPanel.Controls.Add(heroSubtitle);
            }

            int margin = 14;
            int heroHeight = 96;
            int gap = 10;

            heroPanel.Bounds = new Rectangle(margin, margin, ClientSize.Width - margin * 2, heroHeight);
            lblTitle.Parent = heroPanel;
            lblTitle.Location = new Point(18, 10);
            lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            if (heroSubtitle != null)
            {
                heroSubtitle.Location = new Point(20, 56);
            }

            panelFilter.Location = new Point(margin, heroPanel.Bottom + gap);
            panelFilter.Size = new Size(ClientSize.Width - margin * 2, 56);
            panelFilter.BackColor = Color.White;
            panelFilter.BorderStyle = BorderStyle.FixedSingle;

            lblFrom.Location = new Point(12, 18);
            dtpFrom.Location = new Point(80, 15);
            lblTo.Location = new Point(222, 18);
            dtpTo.Location = new Point(265, 15);

            btnFilter.Location = new Point(412, 12);
            btnExport.Location = new Point(520, 12);
            btnFilter.BackColor = Color.FromArgb(37, 99, 235);
            btnExport.BackColor = Color.FromArgb(16, 185, 129);
            btnFilter.ForeColor = btnExport.ForeColor = Color.White;
            btnFilter.FlatStyle = btnExport.FlatStyle = FlatStyle.Flat;
            btnFilter.FlatAppearance.BorderSize = btnExport.FlatAppearance.BorderSize = 0;

            tabControl.Location = new Point(margin, panelFilter.Bottom + gap);
            tabControl.Size = new Size(ClientSize.Width - margin * 2, ClientSize.Height - (panelFilter.Bottom + gap) - 54);

            btnClose.Location = new Point(ClientSize.Width - margin - 102, tabControl.Bottom + 8);
            btnClose.BackColor = Color.FromArgb(107, 114, 128);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;

            StyleGrid(dgvBorrow);
            StyleGrid(dgvReturn);
        }

        private static void StyleGrid(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(4)
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(3)
            };
        }

        private void LoadData()
        {
            LoadBorrowRecords();
            LoadReturnRecords();
        }

        private void LoadBorrowRecords()
        {
            dgvBorrow.Rows.Clear();

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

                        int count = 0;
                        int borrowing = 0;
                        int overdue = 0;
                        int returned = 0;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string status = reader["Status"]?.ToString() ?? "";
                                int rowIndex = dgvBorrow.Rows.Add(
                                    reader["BorrowCode"],
                                    reader["MemberName"],
                                    reader["BookTitle"],
                                    ((DateTime)reader["BorrowDate"]).ToString("dd/MM/yyyy"),
                                    ((DateTime)reader["DueDate"]).ToString("dd/MM/yyyy"),
                                    status,
                                    reader["StaffName"]
                                );

                                // Color by status
                                if (status == "Quá hạn")
                                {
                                    dgvBorrow.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    overdue++;
                                }
                                else if (status == "Đã trả")
                                {
                                    dgvBorrow.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                                    returned++;
                                }
                                else if (status == "Đang mượn")
                                {
                                    dgvBorrow.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(41, 128, 185);
                                    borrowing++;
                                }

                                count++;
                            }
                        }

                        lblBorrowSummary.Text = $"📊 Tổng: {count} phiếu | 📖 Đang mượn: {borrowing} | ⚠️ Quá hạn: {overdue} | ✅ Đã trả: {returned}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu phiếu mượn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReturnRecords()
        {
            dgvReturn.Rows.Clear();

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

                        decimal totalFineGenerated = 0;
                        int count = 0;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                decimal fine = reader["FineAmount"] != DBNull.Value ? (decimal)reader["FineAmount"] : 0;
                                totalFineGenerated += fine;

                                int rowIndex = dgvReturn.Rows.Add(
                                    reader["BorrowCode"],
                                    reader["MemberName"],
                                    reader["BookTitle"],
                                    ((DateTime)reader["BorrowDate"]).ToString("dd/MM/yyyy"),
                                    reader["ReturnDate"] != DBNull.Value ? ((DateTime)reader["ReturnDate"]).ToString("dd/MM/yyyy") : "-",
                                    fine > 0 ? fine.ToString("N0") + " đ" : "-",
                                    reader["StaffName"]
                                );

                                if (fine > 0)
                                    dgvReturn.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.OrangeRed;

                                count++;
                            }
                        }

                        decimal totalFinePaid = 0;
                        using (var cmdPaid = conn.CreateCommand())
                        {
                            cmdPaid.CommandText = @"
                                SELECT ISNULL(SUM(fp.Amount), 0)
                                FROM FinePayments fp
                                WHERE fp.PaymentDate BETWEEN @FromDate AND @ToDate";
                            cmdPaid.Parameters.AddWithValue("@FromDate", dtpFrom.Value.Date);
                            cmdPaid.Parameters.AddWithValue("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1));
                            totalFinePaid = Convert.ToDecimal(cmdPaid.ExecuteScalar());
                        }

                        decimal totalDebt = 0;
                        using (var cmdDebt = conn.CreateCommand())
                        {
                            cmdDebt.CommandText = "SELECT ISNULL(SUM(TotalFine), 0) FROM Members WHERE IsActive = 1";
                            totalDebt = Convert.ToDecimal(cmdDebt.ExecuteScalar());
                        }

                        lblReturnSummary.Text = $"📊 Tổng: {count} phiếu trả | 💸 Phạt phát sinh: {totalFineGenerated:N0} đ | 💰 Đã nộp: {totalFinePaid:N0} đ | 📌 Còn nợ: {totalDebt:N0} đ";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu phiếu trả: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToExcel()
        {
            DataGridView currentDgv = tabControl.SelectedIndex == 0 ? dgvBorrow : dgvReturn;
            string type = tabControl.SelectedIndex == 0 ? "PhieuMuon" : "PhieuTra";

            if (currentDgv.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                saveDialog.FileName = $"{type}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var writer = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
                        {
                            // Headers
                            var headers = new System.Collections.Generic.List<string>();
                            foreach (DataGridViewColumn col in currentDgv.Columns)
                            {
                                headers.Add(col.HeaderText);
                            }
                            writer.WriteLine(string.Join(",", headers));

                            // Data
                            foreach (DataGridViewRow row in currentDgv.Rows)
                            {
                                var values = new System.Collections.Generic.List<string>();
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    values.Add($"\"{cell.Value?.ToString() ?? ""}\"");
                                }
                                writer.WriteLine(string.Join(",", values));
                            }
                        }

                        MessageBox.Show("Xuất file thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveDialog.FileName}\"");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnFilter_Click(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
