using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form hiển thị lịch sử mượn sách của độc giả
    /// </summary>
    public partial class FormBorrowHistory : Form
    {
        private Member member;

        public FormBorrowHistory()
            : this(new Member())
        {
        }

        public FormBorrowHistory(Member member)
        {
            this.member = member ?? new Member();
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaHistoryLayout();
                }
                catch
                {
                }
            }

            this.Load += FormBorrowHistory_Load;
            this.Resize += FormBorrowHistory_Resize;
        }

        private void FormBorrowHistory_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            Text = $"Lịch sử mượn sách - {member.FullName}";
            ApplyFigmaHistoryLayout();
            LoadHistory();
        }

        private void FormBorrowHistory_Resize(object? sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            ApplyFigmaHistoryLayout();
        }

        private void ApplyFigmaHistoryLayout()
        {
            BackColor = Color.FromArgb(241, 245, 249);
            dgvHistory.Location = new Point(16, 16);
            dgvHistory.Size = new Size(ClientSize.Width - 32, ClientSize.Height - 72);
            dgvHistory.EnableHeadersVisualStyles = false;
            dgvHistory.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(4)
            };
            dgvHistory.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(3)
            };

            btnCloseHistory.Location = new Point(ClientSize.Width - 100, ClientSize.Height - 46);
            btnCloseHistory.Size = new Size(84, 32);
            btnCloseHistory.BackColor = Color.FromArgb(107, 114, 128);
            btnCloseHistory.ForeColor = Color.White;
            btnCloseHistory.FlatStyle = FlatStyle.Flat;
            btnCloseHistory.FlatAppearance.BorderSize = 0;
            btnCloseHistory.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void LoadHistory()
        {
            var borrowDAO = new BorrowRecordDAO();
            var history = borrowDAO.GetMemberHistory(member.MemberID);

            dgvHistory.Rows.Clear();

            foreach (var record in history)
            {
                dgvHistory.Rows.Add(
                    record.BorrowCode,
                    record.BookTitle,
                    record.BorrowDate.ToString("dd/MM/yyyy"),
                    record.DueDate.ToString("dd/MM/yyyy"),
                    record.ReturnDate?.ToString("dd/MM/yyyy"),
                    record.Status,
                    record.FineAmount.ToString("N0") + " đ"
                );
            }
        }

        private void BtnCloseHistory_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
