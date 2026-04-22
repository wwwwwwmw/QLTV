using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormBorrowHistory
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvHistory = null!;
        private Button btnCloseHistory = null!;
        private DataGridViewTextBoxColumn colBorrowCode = null!;
        private DataGridViewTextBoxColumn colBookTitle = null!;
        private DataGridViewTextBoxColumn colBorrowDate = null!;
        private DataGridViewTextBoxColumn colDueDate = null!;
        private DataGridViewTextBoxColumn colReturnDate = null!;
        private DataGridViewTextBoxColumn colStatus = null!;
        private DataGridViewTextBoxColumn colFineAmount = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvHistory = new DataGridView();
            this.colBorrowCode = new DataGridViewTextBoxColumn();
            this.colBookTitle = new DataGridViewTextBoxColumn();
            this.colBorrowDate = new DataGridViewTextBoxColumn();
            this.colDueDate = new DataGridViewTextBoxColumn();
            this.colReturnDate = new DataGridViewTextBoxColumn();
            this.colStatus = new DataGridViewTextBoxColumn();
            this.colFineAmount = new DataGridViewTextBoxColumn();
            this.btnCloseHistory = new Button();
            ((System.ComponentModel.ISupportInitialize)this.dgvHistory).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.BackgroundColor = Color.White;
            this.dgvHistory.BorderStyle = BorderStyle.None;
            this.dgvHistory.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            this.dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Columns.AddRange(new DataGridViewColumn[] {
            this.colBorrowCode,
            this.colBookTitle,
            this.colBorrowDate,
            this.colDueDate,
            this.colReturnDate,
            this.colStatus,
            this.colFineAmount});
            this.dgvHistory.Location = new Point(20, 20);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.RowHeadersVisible = false;
            this.dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistory.Size = new Size(740, 400);
            this.dgvHistory.TabIndex = 0;
            this.dgvHistory.EnableHeadersVisualStyles = false;
            // 
            // colBorrowCode
            // 
            this.colBorrowCode.HeaderText = "Mã phiếu";
            this.colBorrowCode.Name = "BorrowCode";
            this.colBorrowCode.ReadOnly = true;
            this.colBorrowCode.Width = 100;
            // 
            // colBookTitle
            // 
            this.colBookTitle.HeaderText = "Tên sách";
            this.colBookTitle.Name = "BookTitle";
            this.colBookTitle.ReadOnly = true;
            this.colBookTitle.Width = 250;
            // 
            // colBorrowDate
            // 
            this.colBorrowDate.HeaderText = "Ngày mượn";
            this.colBorrowDate.Name = "BorrowDate";
            this.colBorrowDate.ReadOnly = true;
            this.colBorrowDate.Width = 90;
            // 
            // colDueDate
            // 
            this.colDueDate.HeaderText = "Hạn trả";
            this.colDueDate.Name = "DueDate";
            this.colDueDate.ReadOnly = true;
            this.colDueDate.Width = 90;
            // 
            // colReturnDate
            // 
            this.colReturnDate.HeaderText = "Ngày trả";
            this.colReturnDate.Name = "ReturnDate";
            this.colReturnDate.ReadOnly = true;
            this.colReturnDate.Width = 90;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Trạng thái";
            this.colStatus.Name = "Status";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 90;
            // 
            // colFineAmount
            // 
            this.colFineAmount.HeaderText = "Tiền phạt";
            this.colFineAmount.Name = "FineAmount";
            this.colFineAmount.ReadOnly = true;
            this.colFineAmount.Width = 90;
            // 
            // btnCloseHistory
            // 
            this.btnCloseHistory.BackColor = Color.FromArgb(107, 114, 128);
            this.btnCloseHistory.FlatAppearance.BorderSize = 0;
            this.btnCloseHistory.FlatStyle = FlatStyle.Flat;
            this.btnCloseHistory.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnCloseHistory.ForeColor = Color.White;
            this.btnCloseHistory.Location = new Point(680, 430);
            this.btnCloseHistory.Name = "btnCloseHistory";
            this.btnCloseHistory.Size = new Size(80, 30);
            this.btnCloseHistory.TabIndex = 1;
            this.btnCloseHistory.Text = "Đóng";
            this.btnCloseHistory.UseVisualStyleBackColor = true;
            this.btnCloseHistory.Click += this.BtnCloseHistory_Click;
            // 
            // FormBorrowHistory
            // 
            this.BackColor = Color.FromArgb(246, 248, 252);
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.btnCloseHistory);
            this.Controls.Add(this.dgvHistory);
            this.Name = "FormBorrowHistory";
            this.Text = "Lịch sử mượn sách";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            ((System.ComponentModel.ISupportInitialize)this.dgvHistory).EndInit();
            this.ResumeLayout(false);
        }
    }
}
