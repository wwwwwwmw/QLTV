using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormReturn
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle = null!;
        private Panel panelSearch = null!;
        private Label lblSearch = null!;
        private Label lblStatus = null!;
        private Label lblBarcode = null!;
        private Button btnRefresh = null!;
        private Panel panelInfo = null!;
        private Button btnReturn = null!;
        private Button btnRenew = null!;
        private DataGridViewTextBoxColumn colBorrowId = null!;
        private DataGridViewTextBoxColumn colBorrowCode = null!;
        private DataGridViewTextBoxColumn colMemberCode = null!;
        private DataGridViewTextBoxColumn colMemberName = null!;
        private DataGridViewTextBoxColumn colBookTitle = null!;
        private DataGridViewTextBoxColumn colBorrowDate = null!;
        private DataGridViewTextBoxColumn colDueDate = null!;
        private DataGridViewTextBoxColumn colDaysLeft = null!;
        private DataGridViewTextBoxColumn colStatus = null!;

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
            this.lblTitle = new Label();
            this.panelSearch = new Panel();
            this.btnRefresh = new Button();
            this.lblStatus = new Label();
            this.lblBarcode = new Label();
            this.txtBarcode = new TextBox();
            this.btnScanBarcode = new Button();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();
            this.cboStatus = new ComboBox();
            this.dgvBorrowRecords = new DataGridView();
            this.colBorrowId = new DataGridViewTextBoxColumn();
            this.colBorrowCode = new DataGridViewTextBoxColumn();
            this.colMemberCode = new DataGridViewTextBoxColumn();
            this.colMemberName = new DataGridViewTextBoxColumn();
            this.colBookTitle = new DataGridViewTextBoxColumn();
            this.colBorrowDate = new DataGridViewTextBoxColumn();
            this.colDueDate = new DataGridViewTextBoxColumn();
            this.colDaysLeft = new DataGridViewTextBoxColumn();
            this.colStatus = new DataGridViewTextBoxColumn();
            this.panelInfo = new Panel();
            this.lblSelectedInfo = new Label();
            this.btnReturn = new Button();
            this.btnRenew = new Button();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvBorrowRecords).BeginInit();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            this.lblTitle.Location = new Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(156, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Trả sách";
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = Color.White;
            this.panelSearch.Controls.Add(this.btnRefresh);
            this.panelSearch.Controls.Add(this.lblStatus);
            this.panelSearch.Controls.Add(this.lblBarcode);
            this.panelSearch.Controls.Add(this.txtBarcode);
            this.panelSearch.Controls.Add(this.btnScanBarcode);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.cboStatus);
            this.panelSearch.Location = new Point(20, 66);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new Size(1180, 64);
            this.panelSearch.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = Color.FromArgb(37, 99, 235);
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.Location = new Point(1076, 16);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(90, 32);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += this.BtnRefresh_Click;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new Point(360, 23);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(62, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // lblBarcode
            // 
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Location = new Point(682, 23);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new Size(55, 15);
            this.lblBarcode.TabIndex = 5;
            this.lblBarcode.Text = "Barcode:";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Font = new Font("Segoe UI", 10F);
            this.txtBarcode.Location = new Point(742, 19);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.PlaceholderText = "Quét/nhập...";
            this.txtBarcode.Size = new Size(200, 25);
            this.txtBarcode.TabIndex = 6;
            this.txtBarcode.KeyDown += this.TxtBarcode_KeyDown;
            // 
            // btnScanBarcode
            // 
            this.btnScanBarcode.BackColor = Color.FromArgb(59, 130, 246);
            this.btnScanBarcode.FlatAppearance.BorderSize = 0;
            this.btnScanBarcode.FlatStyle = FlatStyle.Flat;
            this.btnScanBarcode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnScanBarcode.ForeColor = Color.White;
            this.btnScanBarcode.Location = new Point(950, 16);
            this.btnScanBarcode.Name = "btnScanBarcode";
            this.btnScanBarcode.Size = new Size(112, 32);
            this.btnScanBarcode.TabIndex = 7;
            this.btnScanBarcode.Text = "Quét barcode";
            this.btnScanBarcode.UseVisualStyleBackColor = false;
            this.btnScanBarcode.Click += this.BtnScanBarcode_Click;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new Point(16, 23);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(57, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tìm kiếm:";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new Font("Segoe UI", 10F);
            this.txtSearch.Location = new Point(80, 19);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Mã phiếu, mã thẻ, tên độc giả, tên sách...";
            this.txtSearch.Size = new Size(250, 25);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += this.TxtSearch_TextChanged;
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "-- Tất cả --",
            "Đang mượn",
            "Quá hạn"});
            this.cboStatus.Location = new Point(428, 19);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new Size(150, 23);
            this.cboStatus.TabIndex = 3;
            this.cboStatus.SelectedIndexChanged += this.CboStatus_SelectedIndexChanged;
            // 
            // dgvBorrowRecords
            // 
            this.dgvBorrowRecords.AllowUserToAddRows = false;
            this.dgvBorrowRecords.BackgroundColor = Color.White;
            this.dgvBorrowRecords.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(37, 99, 235);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dgvBorrowRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBorrowRecords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBorrowRecords.Columns.AddRange(new DataGridViewColumn[] {
            this.colBorrowId,
            this.colBorrowCode,
            this.colMemberCode,
            this.colMemberName,
            this.colBookTitle,
            this.colBorrowDate,
            this.colDueDate,
            this.colDaysLeft,
            this.colStatus});
            this.dgvBorrowRecords.Location = new Point(20, 142);
            this.dgvBorrowRecords.Name = "dgvBorrowRecords";
            this.dgvBorrowRecords.ReadOnly = true;
            this.dgvBorrowRecords.RowHeadersVisible = false;
            this.dgvBorrowRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvBorrowRecords.Size = new Size(1180, 332);
            this.dgvBorrowRecords.TabIndex = 2;
            this.dgvBorrowRecords.CellDoubleClick += this.DgvBorrowRecords_CellDoubleClick;
            this.dgvBorrowRecords.SelectionChanged += this.DgvBorrowRecords_SelectionChanged;
            // 
            // colBorrowId
            // 
            this.colBorrowId.HeaderText = "ID";
            this.colBorrowId.Name = "BorrowID";
            this.colBorrowId.ReadOnly = true;
            this.colBorrowId.Visible = false;
            // 
            // colBorrowCode
            // 
            this.colBorrowCode.HeaderText = "Mã phiếu";
            this.colBorrowCode.Name = "BorrowCode";
            this.colBorrowCode.ReadOnly = true;
            this.colBorrowCode.Width = 110;
            // 
            // colMemberCode
            // 
            this.colMemberCode.HeaderText = "Mã thẻ";
            this.colMemberCode.Name = "MemberCode";
            this.colMemberCode.ReadOnly = true;
            this.colMemberCode.Width = 80;
            // 
            // colMemberName
            // 
            this.colMemberName.HeaderText = "Tên độc giả";
            this.colMemberName.Name = "MemberName";
            this.colMemberName.ReadOnly = true;
            this.colMemberName.Width = 150;
            // 
            // colBookTitle
            // 
            this.colBookTitle.HeaderText = "Tên sách";
            this.colBookTitle.Name = "BookTitle";
            this.colBookTitle.ReadOnly = true;
            this.colBookTitle.Width = 280;
            // 
            // colBorrowDate
            // 
            this.colBorrowDate.HeaderText = "Ngày mượn";
            this.colBorrowDate.Name = "BorrowDate";
            this.colBorrowDate.ReadOnly = true;
            this.colBorrowDate.Width = 100;
            // 
            // colDueDate
            // 
            this.colDueDate.HeaderText = "Hạn trả";
            this.colDueDate.Name = "DueDate";
            this.colDueDate.ReadOnly = true;
            this.colDueDate.Width = 100;
            // 
            // colDaysLeft
            // 
            this.colDaysLeft.HeaderText = "Còn/Quá hạn";
            this.colDaysLeft.Name = "DaysLeft";
            this.colDaysLeft.ReadOnly = true;
            this.colDaysLeft.Width = 100;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Trạng thái";
            this.colStatus.Name = "Status";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 100;
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = Color.White;
            this.panelInfo.Controls.Add(this.lblSelectedInfo);
            this.panelInfo.Location = new Point(20, 484);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new Size(800, 62);
            this.panelInfo.TabIndex = 3;
            // 
            // lblSelectedInfo
            // 
            this.lblSelectedInfo.Font = new Font("Segoe UI", 11F);
            this.lblSelectedInfo.Location = new Point(0, 5);
            this.lblSelectedInfo.Name = "lblSelectedInfo";
            this.lblSelectedInfo.Size = new Size(800, 50);
            this.lblSelectedInfo.TabIndex = 0;
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = Color.FromArgb(37, 99, 235);
            this.btnReturn.FlatAppearance.BorderSize = 0;
            this.btnReturn.FlatStyle = FlatStyle.Flat;
            this.btnReturn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnReturn.ForeColor = Color.White;
            this.btnReturn.Location = new Point(840, 490);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new Size(130, 50);
            this.btnReturn.TabIndex = 4;
            this.btnReturn.Text = "Trả sách";
            this.btnReturn.UseVisualStyleBackColor = false;
            this.btnReturn.Click += this.BtnReturnBook_Click;
            // 
            // btnRenew
            // 
            this.btnRenew.BackColor = Color.FromArgb(99, 102, 241);
            this.btnRenew.FlatAppearance.BorderSize = 0;
            this.btnRenew.FlatStyle = FlatStyle.Flat;
            this.btnRenew.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnRenew.ForeColor = Color.White;
            this.btnRenew.Location = new Point(980, 490);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new Size(110, 50);
            this.btnRenew.TabIndex = 5;
            this.btnRenew.Text = "Gia hạn";
            this.btnRenew.UseVisualStyleBackColor = false;
            this.btnRenew.Click += this.BtnRenewBook_Click;
            // 
            // FormReturn
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(246, 248, 252);
            this.ClientSize = new Size(1220, 560);
            this.Controls.Add(this.btnRenew);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.dgvBorrowRecords);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.Name = "FormReturn";
            this.Text = "Trả sách";
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvBorrowRecords).EndInit();
            this.panelInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtSearch;
        private TextBox txtBarcode;
        private Button btnScanBarcode;
        private ComboBox cboStatus;
        private DataGridView dgvBorrowRecords;
        private Label lblSelectedInfo;
    }
}
