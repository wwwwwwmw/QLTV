using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form gia hạn sách
    /// </summary>
    public partial class FormRenewBook : Form
    {
        private BorrowRecord record;

        public FormRenewBook()
            : this(new BorrowRecord())
        {
        }

        public FormRenewBook(BorrowRecord record)
        {
            this.record = record ?? new BorrowRecord();
            InitializeComponent();
            
            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);
            this.Load += FormRenewBook_Load;
        }

        private void FormRenewBook_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            lblBook.Text = $"Sách: {record.BookTitle}";
            lblMember.Text = $"Độc giả: {record.MemberName}";
            lblCurrentDue.Text = $"Hạn trả hiện tại: {record.DueDate:dd/MM/yyyy}";
            numDays.Value = 7;
            UpdateNewDueDate();
        }

        private void NumDays_ValueChanged(object? sender, EventArgs e)
        {
            UpdateNewDueDate();
        }

        private void UpdateNewDueDate()
        {
            DateTime newDue = record.DueDate.AddDays((int)numDays.Value);
            lblNewDue.Text = $"Hạn trả mới: {newDue:dd/MM/yyyy}";
        }

        private void BtnCancelRenew_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void BtnRenew_Click(object? sender, EventArgs e)
        {
            try
            {
                var borrowDAO = new BorrowRecordDAO();
                var (success, message) = borrowDAO.RenewBook(record.BorrowID, (int)numDays.Value);

                if (success)
                {
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log($"Gia hạn sách: {record.BookTitle} thêm {numDays.Value} ngày", "BorrowRecords", record.BorrowID);

                    MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
