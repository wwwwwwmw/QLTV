using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form đóng tiền phạt
    /// </summary>
    public partial class FormPayFine : Form
    {
        private Member member;

        public FormPayFine()
            : this(new Member())
        {
        }

        public FormPayFine(Member member)
        {
            this.member = member ?? new Member();
            InitializeComponent();
            
            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);
            this.Load += FormPayFine_Load;
        }

        private void FormPayFine_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            lblMember.Text = $"Độc giả: {member.FullName} ({member.MemberCode})";
            lblCurrentFine.Text = $"Số tiền nợ: {member.TotalFine:N0} VNĐ";
            numAmount.Maximum = member.TotalFine;
            numAmount.Value = member.TotalFine;
        }

        private void BtnCancelPayFine_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void BtnPay_Click(object? sender, EventArgs e)
        {
            if (numAmount.Value <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var payment = new FinePayment
                {
                    MemberID = member.MemberID,
                    Amount = numAmount.Value,
                    PaymentMethod = cboMethod.SelectedItem?.ToString() ?? FinePayment.METHOD_CASH,
                    Notes = txtNotes.Text.Trim(),
                    StaffID = CurrentUser.User?.UserID
                };

                var paymentDAO = new FinePaymentDAO();
                paymentDAO.Insert(payment);

                MessageBox.Show($"Đã thanh toán {numAmount.Value:N0} VNĐ thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
