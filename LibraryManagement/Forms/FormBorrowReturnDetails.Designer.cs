using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormBorrowReturnDetails
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle = null!;
        private Panel panelFilter = null!;
        private Label lblFrom = null!;
        private Label lblTo = null!;
        private Button btnFilter = null!;
        private Button btnExport = null!;
        private Button btnClose = null!;
        private TabPage tabBorrow = null!;
        private TabPage tabReturn = null!;
        private DataGridViewTextBoxColumn colBorrowCodeBorrow = null!;
        private DataGridViewTextBoxColumn colMemberNameBorrow = null!;
        private DataGridViewTextBoxColumn colBookTitleBorrow = null!;
        private DataGridViewTextBoxColumn colBorrowDateBorrow = null!;
        private DataGridViewTextBoxColumn colDueDateBorrow = null!;
        private DataGridViewTextBoxColumn colStatusBorrow = null!;
        private DataGridViewTextBoxColumn colStaffNameBorrow = null!;
        private DataGridViewTextBoxColumn colBorrowCodeReturn = null!;
        private DataGridViewTextBoxColumn colMemberNameReturn = null!;
        private DataGridViewTextBoxColumn colBookTitleReturn = null!;
        private DataGridViewTextBoxColumn colBorrowDateReturn = null!;
        private DataGridViewTextBoxColumn colReturnDateReturn = null!;
        private DataGridViewTextBoxColumn colFineAmountReturn = null!;
        private DataGridViewTextBoxColumn colStaffNameReturn = null!;

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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            this.lblTitle = new Label();
            this.panelFilter = new Panel();
            this.btnExport = new Button();
            this.btnFilter = new Button();
            this.lblTo = new Label();
            this.lblFrom = new Label();
            this.dtpFrom = new DateTimePicker();
            this.dtpTo = new DateTimePicker();
            this.tabControl = new TabControl();
            this.tabBorrow = new TabPage();
            this.lblBorrowSummary = new Label();
            this.dgvBorrow = new DataGridView();
            this.colBorrowCodeBorrow = new DataGridViewTextBoxColumn();
            this.colMemberNameBorrow = new DataGridViewTextBoxColumn();
            this.colBookTitleBorrow = new DataGridViewTextBoxColumn();
            this.colBorrowDateBorrow = new DataGridViewTextBoxColumn();
            this.colDueDateBorrow = new DataGridViewTextBoxColumn();
            this.colStatusBorrow = new DataGridViewTextBoxColumn();
            this.colStaffNameBorrow = new DataGridViewTextBoxColumn();
            this.tabReturn = new TabPage();
            this.lblReturnSummary = new Label();
            this.dgvReturn = new DataGridView();
            this.colBorrowCodeReturn = new DataGridViewTextBoxColumn();
            this.colMemberNameReturn = new DataGridViewTextBoxColumn();
            this.colBookTitleReturn = new DataGridViewTextBoxColumn();
            this.colBorrowDateReturn = new DataGridViewTextBoxColumn();
            this.colReturnDateReturn = new DataGridViewTextBoxColumn();
            this.colFineAmountReturn = new DataGridViewTextBoxColumn();
            this.colStaffNameReturn = new DataGridViewTextBoxColumn();
            this.btnClose = new Button();
            this.panelFilter.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabBorrow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvBorrow).BeginInit();
            this.tabReturn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvReturn).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            this.lblTitle.Location = new Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(403, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết phiếu mượn / trả sách";
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = Color.White;
            this.panelFilter.Controls.Add(this.btnExport);
            this.panelFilter.Controls.Add(this.btnFilter);
            this.panelFilter.Controls.Add(this.lblTo);
            this.panelFilter.Controls.Add(this.lblFrom);
            this.panelFilter.Controls.Add(this.dtpFrom);
            this.panelFilter.Controls.Add(this.dtpTo);
            this.panelFilter.Location = new Point(20, 62);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new Size(945, 56);
            this.panelFilter.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = Color.FromArgb(16, 185, 129);
            this.btnExport.Cursor = Cursors.Hand;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.Location = new Point(520, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(90, 32);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Xuất Excel";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += this.BtnExport_Click;
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = Color.FromArgb(37, 99, 235);
            this.btnFilter.Cursor = Cursors.Hand;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatStyle = FlatStyle.Flat;
            this.btnFilter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnFilter.ForeColor = Color.White;
            this.btnFilter.Location = new Point(410, 12);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new Size(100, 32);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "Lọc dữ liệu";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += this.BtnFilter_Click;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new Point(225, 18);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new Size(31, 15);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "Đến:";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new Point(15, 18);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new Size(50, 15);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Từ ngày:";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = DateTimePickerFormat.Short;
            this.dtpFrom.Location = new Point(80, 15);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new Size(130, 23);
            this.dtpFrom.TabIndex = 1;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = DateTimePickerFormat.Short;
            this.dtpTo.Location = new Point(265, 15);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new Size(130, 23);
            this.dtpTo.TabIndex = 3;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabBorrow);
            this.tabControl.Controls.Add(this.tabReturn);
            this.tabControl.Font = new Font("Segoe UI", 10F);
            this.tabControl.Location = new Point(20, 126);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(945, 428);
            this.tabControl.TabIndex = 2;
            // 
            // tabBorrow
            // 
            this.tabBorrow.BackColor = Color.White;
            this.tabBorrow.Controls.Add(this.lblBorrowSummary);
            this.tabBorrow.Controls.Add(this.dgvBorrow);
            this.tabBorrow.Location = new Point(4, 26);
            this.tabBorrow.Name = "tabBorrow";
            this.tabBorrow.Padding = new Padding(10);
            this.tabBorrow.Size = new Size(937, 410);
            this.tabBorrow.TabIndex = 0;
            this.tabBorrow.Text = "Phiếu mượn";
            // 
            // lblBorrowSummary
            // 
            this.lblBorrowSummary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblBorrowSummary.ForeColor = Color.FromArgb(41, 128, 185);
            this.lblBorrowSummary.Location = new Point(10, 370);
            this.lblBorrowSummary.Name = "lblBorrowSummary";
            this.lblBorrowSummary.Size = new Size(900, 25);
            this.lblBorrowSummary.TabIndex = 1;
            // 
            // dgvBorrow
            // 
            this.dgvBorrow.AllowUserToAddRows = false;
            this.dgvBorrow.AllowUserToDeleteRows = false;
            this.dgvBorrow.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvBorrow.BackgroundColor = Color.White;
            this.dgvBorrow.BorderStyle = BorderStyle.FixedSingle;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(37, 99, 235);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dgvBorrow.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBorrow.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBorrow.Columns.AddRange(new DataGridViewColumn[] {
            this.colBorrowCodeBorrow,
            this.colMemberNameBorrow,
            this.colBookTitleBorrow,
            this.colBorrowDateBorrow,
            this.colDueDateBorrow,
            this.colStatusBorrow,
            this.colStaffNameBorrow});
            this.dgvBorrow.Location = new Point(10, 10);
            this.dgvBorrow.Name = "dgvBorrow";
            this.dgvBorrow.ReadOnly = true;
            this.dgvBorrow.RowHeadersVisible = false;
            this.dgvBorrow.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvBorrow.Size = new Size(905, 355);
            this.dgvBorrow.TabIndex = 0;
            // 
            // colBorrowCodeBorrow
            // 
            this.colBorrowCodeBorrow.HeaderText = "Mã phiếu";
            this.colBorrowCodeBorrow.Name = "BorrowCode";
            this.colBorrowCodeBorrow.ReadOnly = true;
            this.colBorrowCodeBorrow.Width = 120;
            // 
            // colMemberNameBorrow
            // 
            this.colMemberNameBorrow.HeaderText = "Độc giả";
            this.colMemberNameBorrow.Name = "MemberName";
            this.colMemberNameBorrow.ReadOnly = true;
            this.colMemberNameBorrow.Width = 150;
            // 
            // colBookTitleBorrow
            // 
            this.colBookTitleBorrow.HeaderText = "Tên sách";
            this.colBookTitleBorrow.Name = "BookTitle";
            this.colBookTitleBorrow.ReadOnly = true;
            this.colBookTitleBorrow.Width = 220;
            // 
            // colBorrowDateBorrow
            // 
            this.colBorrowDateBorrow.HeaderText = "Ngày mượn";
            this.colBorrowDateBorrow.Name = "BorrowDate";
            this.colBorrowDateBorrow.ReadOnly = true;
            this.colBorrowDateBorrow.Width = 90;
            // 
            // colDueDateBorrow
            // 
            this.colDueDateBorrow.HeaderText = "Hạn trả";
            this.colDueDateBorrow.Name = "DueDate";
            this.colDueDateBorrow.ReadOnly = true;
            this.colDueDateBorrow.Width = 90;
            // 
            // colStatusBorrow
            // 
            this.colStatusBorrow.HeaderText = "Trạng thái";
            this.colStatusBorrow.Name = "Status";
            this.colStatusBorrow.ReadOnly = true;
            this.colStatusBorrow.Width = 90;
            // 
            // colStaffNameBorrow
            // 
            this.colStaffNameBorrow.HeaderText = "Nhân viên";
            this.colStaffNameBorrow.Name = "StaffName";
            this.colStaffNameBorrow.ReadOnly = true;
            this.colStaffNameBorrow.Width = 120;
            // 
            // tabReturn
            // 
            this.tabReturn.BackColor = Color.White;
            this.tabReturn.Controls.Add(this.lblReturnSummary);
            this.tabReturn.Controls.Add(this.dgvReturn);
            this.tabReturn.Location = new Point(4, 26);
            this.tabReturn.Name = "tabReturn";
            this.tabReturn.Padding = new Padding(10);
            this.tabReturn.Size = new Size(937, 410);
            this.tabReturn.TabIndex = 1;
            this.tabReturn.Text = "Phiếu trả";
            // 
            // lblReturnSummary
            // 
            this.lblReturnSummary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblReturnSummary.ForeColor = Color.FromArgb(39, 174, 96);
            this.lblReturnSummary.Location = new Point(10, 370);
            this.lblReturnSummary.Name = "lblReturnSummary";
            this.lblReturnSummary.Size = new Size(900, 25);
            this.lblReturnSummary.TabIndex = 1;
            // 
            // dgvReturn
            // 
            this.dgvReturn.AllowUserToAddRows = false;
            this.dgvReturn.AllowUserToDeleteRows = false;
            this.dgvReturn.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvReturn.BackgroundColor = Color.White;
            this.dgvReturn.BorderStyle = BorderStyle.FixedSingle;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(37, 99, 235);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            this.dgvReturn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReturn.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReturn.Columns.AddRange(new DataGridViewColumn[] {
            this.colBorrowCodeReturn,
            this.colMemberNameReturn,
            this.colBookTitleReturn,
            this.colBorrowDateReturn,
            this.colReturnDateReturn,
            this.colFineAmountReturn,
            this.colStaffNameReturn});
            this.dgvReturn.Location = new Point(10, 10);
            this.dgvReturn.Name = "dgvReturn";
            this.dgvReturn.ReadOnly = true;
            this.dgvReturn.RowHeadersVisible = false;
            this.dgvReturn.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReturn.Size = new Size(905, 355);
            this.dgvReturn.TabIndex = 0;
            // 
            // colBorrowCodeReturn
            // 
            this.colBorrowCodeReturn.HeaderText = "Mã phiếu";
            this.colBorrowCodeReturn.Name = "BorrowCode";
            this.colBorrowCodeReturn.ReadOnly = true;
            this.colBorrowCodeReturn.Width = 120;
            // 
            // colMemberNameReturn
            // 
            this.colMemberNameReturn.HeaderText = "Độc giả";
            this.colMemberNameReturn.Name = "MemberName";
            this.colMemberNameReturn.ReadOnly = true;
            this.colMemberNameReturn.Width = 150;
            // 
            // colBookTitleReturn
            // 
            this.colBookTitleReturn.HeaderText = "Tên sách";
            this.colBookTitleReturn.Name = "BookTitle";
            this.colBookTitleReturn.ReadOnly = true;
            this.colBookTitleReturn.Width = 220;
            // 
            // colBorrowDateReturn
            // 
            this.colBorrowDateReturn.HeaderText = "Ngày mượn";
            this.colBorrowDateReturn.Name = "BorrowDate";
            this.colBorrowDateReturn.ReadOnly = true;
            this.colBorrowDateReturn.Width = 90;
            // 
            // colReturnDateReturn
            // 
            this.colReturnDateReturn.HeaderText = "Ngày trả";
            this.colReturnDateReturn.Name = "ReturnDate";
            this.colReturnDateReturn.ReadOnly = true;
            this.colReturnDateReturn.Width = 90;
            // 
            // colFineAmountReturn
            // 
            this.colFineAmountReturn.HeaderText = "Tiền phạt";
            this.colFineAmountReturn.Name = "FineAmount";
            this.colFineAmountReturn.ReadOnly = true;
            this.colFineAmountReturn.Width = 90;
            // 
            // colStaffNameReturn
            // 
            this.colStaffNameReturn.HeaderText = "Nhân viên";
            this.colStaffNameReturn.Name = "StaffName";
            this.colStaffNameReturn.ReadOnly = true;
            this.colStaffNameReturn.Width = 120;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = Color.FromArgb(107, 114, 128);
            this.btnClose.Cursor = Cursors.Hand;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(865, 562);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(100, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += this.BtnClose_Click;
            // 
            // FormBorrowReturnDetails
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(246, 248, 252);
            this.ClientSize = new Size(984, 611);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelFilter);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormBorrowReturnDetails";
            this.StartPosition = FormStartPosition.CenterParent;
            this.DoubleBuffered = true;
            this.Text = "Chi tiết phiếu mượn / trả sách";
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabBorrow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.dgvBorrow).EndInit();
            this.tabReturn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.dgvReturn).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private TabControl tabControl;
        private DataGridView dgvBorrow;
        private DataGridView dgvReturn;
        private Label lblBorrowSummary;
        private Label lblReturnSummary;
    }
}
