using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormSettings
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle = null!;
        private Panel panelSettings = null!;
        private Label lblLibInfo = null!;
        private Label lblName = null!;
        private Label lblAddress = null!;
        private Label lblPhone = null!;
        private Label lblEmail = null!;
        private Label lblRules = null!;
        private Label lblBorrowDays = null!;
        private Label lblDaysUnit = null!;
        private Label lblMaxBooks = null!;
        private Label lblBooksUnit = null!;
        private Label lblFine = null!;
        private Label lblFineUnit = null!;
        private Label lblConnection = null!;
        private Button btnTestConn = null!;
        private Button btnSave = null!;
        private Panel panelLogs = null!;
        private Label lblLogs = null!;
        private Button btnClearLogs = null!;
        private Button btnRefreshLogs = null!;
        private DataGridViewTextBoxColumn colLogTime = null!;
        private DataGridViewTextBoxColumn colUsername = null!;
        private DataGridViewTextBoxColumn colAction = null!;

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
            this.panelSettings = new Panel();
            this.btnSave = new Button();
            this.btnTestConn = new Button();
            this.lblConnInfo = new Label();
            this.lblConnection = new Label();
            this.lblFineUnit = new Label();
            this.lblFine = new Label();
            this.lblBooksUnit = new Label();
            this.lblMaxBooks = new Label();
            this.lblDaysUnit = new Label();
            this.lblBorrowDays = new Label();
            this.lblRules = new Label();
            this.lblEmail = new Label();
            this.lblPhone = new Label();
            this.lblAddress = new Label();
            this.lblName = new Label();
            this.lblLibInfo = new Label();
            this.txtLibraryName = new TextBox();
            this.txtAddress = new TextBox();
            this.txtPhone = new TextBox();
            this.txtEmail = new TextBox();
            this.numBorrowDays = new NumericUpDown();
            this.numMaxBooks = new NumericUpDown();
            this.numFinePerDay = new NumericUpDown();
            this.panelLogs = new Panel();
            this.btnRefreshLogs = new Button();
            this.btnClearLogs = new Button();
            this.lblLogs = new Label();
            this.dgvLogs = new DataGridView();
            this.colLogTime = new DataGridViewTextBoxColumn();
            this.colUsername = new DataGridViewTextBoxColumn();
            this.colAction = new DataGridViewTextBoxColumn();
            this.panelSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.numBorrowDays).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.numMaxBooks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.numFinePerDay).BeginInit();
            this.panelLogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvLogs).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            this.lblTitle.Location = new Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(252, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Cấu hình hệ thống";
            // 
            // panelSettings
            // 
            this.panelSettings.BackColor = Color.White;
            this.panelSettings.Controls.Add(this.btnSave);
            this.panelSettings.Controls.Add(this.btnTestConn);
            this.panelSettings.Controls.Add(this.lblConnInfo);
            this.panelSettings.Controls.Add(this.lblConnection);
            this.panelSettings.Controls.Add(this.lblFineUnit);
            this.panelSettings.Controls.Add(this.lblFine);
            this.panelSettings.Controls.Add(this.lblBooksUnit);
            this.panelSettings.Controls.Add(this.lblMaxBooks);
            this.panelSettings.Controls.Add(this.lblDaysUnit);
            this.panelSettings.Controls.Add(this.lblBorrowDays);
            this.panelSettings.Controls.Add(this.lblRules);
            this.panelSettings.Controls.Add(this.lblEmail);
            this.panelSettings.Controls.Add(this.lblPhone);
            this.panelSettings.Controls.Add(this.lblAddress);
            this.panelSettings.Controls.Add(this.lblName);
            this.panelSettings.Controls.Add(this.lblLibInfo);
            this.panelSettings.Controls.Add(this.txtLibraryName);
            this.panelSettings.Controls.Add(this.txtAddress);
            this.panelSettings.Controls.Add(this.txtPhone);
            this.panelSettings.Controls.Add(this.txtEmail);
            this.panelSettings.Controls.Add(this.numBorrowDays);
            this.panelSettings.Controls.Add(this.numMaxBooks);
            this.panelSettings.Controls.Add(this.numFinePerDay);
            this.panelSettings.Location = new Point(20, 66);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new Size(550, 480);
            this.panelSettings.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = Color.FromArgb(37, 99, 235);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(20, 440);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(130, 35);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Lưu cài đặt";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += this.BtnSave_Click;
            // 
            // btnTestConn
            // 
            this.btnTestConn.BackColor = Color.FromArgb(59, 130, 246);
            this.btnTestConn.FlatAppearance.BorderSize = 0;
            this.btnTestConn.FlatStyle = FlatStyle.Flat;
            this.btnTestConn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnTestConn.ForeColor = Color.White;
            this.btnTestConn.Location = new Point(20, 405);
            this.btnTestConn.Name = "btnTestConn";
            this.btnTestConn.Size = new Size(110, 32);
            this.btnTestConn.TabIndex = 21;
            this.btnTestConn.Text = "Test kết nối";
            this.btnTestConn.UseVisualStyleBackColor = false;
            this.btnTestConn.Click += this.BtnTestConn_Click;
            // 
            // lblConnInfo
            // 
            this.lblConnInfo.Font = new Font("Segoe UI", 9F);
            this.lblConnInfo.Location = new Point(20, 360);
            this.lblConnInfo.Name = "lblConnInfo";
            this.lblConnInfo.Size = new Size(500, 40);
            this.lblConnInfo.TabIndex = 20;
            this.lblConnInfo.Text = "Connection String:";
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblConnection.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblConnection.Location = new Point(20, 330);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new Size(148, 20);
            this.lblConnection.TabIndex = 19;
            this.lblConnection.Text = "Kết nối mạng LAN";
            // 
            // lblFineUnit
            // 
            this.lblFineUnit.AutoSize = true;
            this.lblFineUnit.Location = new Point(290, 275);
            this.lblFineUnit.Name = "lblFineUnit";
            this.lblFineUnit.Size = new Size(66, 15);
            this.lblFineUnit.TabIndex = 18;
            this.lblFineUnit.Text = "VNĐ/ngày";
            // 
            // lblFine
            // 
            this.lblFine.AutoSize = true;
            this.lblFine.Location = new Point(20, 275);
            this.lblFine.Name = "lblFine";
            this.lblFine.Size = new Size(96, 15);
            this.lblFine.TabIndex = 16;
            this.lblFine.Text = "Tiền phạt quá hạn:";
            // 
            // lblBooksUnit
            // 
            this.lblBooksUnit.AutoSize = true;
            this.lblBooksUnit.Location = new Point(270, 240);
            this.lblBooksUnit.Name = "lblBooksUnit";
            this.lblBooksUnit.Size = new Size(66, 15);
            this.lblBooksUnit.TabIndex = 15;
            this.lblBooksUnit.Text = "quyển/lần";
            // 
            // lblMaxBooks
            // 
            this.lblMaxBooks.AutoSize = true;
            this.lblMaxBooks.Location = new Point(20, 240);
            this.lblMaxBooks.Name = "lblMaxBooks";
            this.lblMaxBooks.Size = new Size(105, 15);
            this.lblMaxBooks.TabIndex = 13;
            this.lblMaxBooks.Text = "Số sách mượn tối đa:";
            // 
            // lblDaysUnit
            // 
            this.lblDaysUnit.AutoSize = true;
            this.lblDaysUnit.Location = new Point(270, 205);
            this.lblDaysUnit.Name = "lblDaysUnit";
            this.lblDaysUnit.Size = new Size(30, 15);
            this.lblDaysUnit.TabIndex = 12;
            this.lblDaysUnit.Text = "ngày";
            // 
            // lblBorrowDays
            // 
            this.lblBorrowDays.AutoSize = true;
            this.lblBorrowDays.Location = new Point(20, 205);
            this.lblBorrowDays.Name = "lblBorrowDays";
            this.lblBorrowDays.Size = new Size(112, 15);
            this.lblBorrowDays.TabIndex = 10;
            this.lblBorrowDays.Text = "Số ngày mượn tối đa:";
            // 
            // lblRules
            // 
            this.lblRules.AutoSize = true;
            this.lblRules.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblRules.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblRules.Location = new Point(20, 170);
            this.lblRules.Name = "lblRules";
            this.lblRules.Size = new Size(157, 20);
            this.lblRules.TabIndex = 9;
            this.lblRules.Text = "Quy định mượn trả";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new Point(340, 120);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new Size(39, 15);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "Email:";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new Point(20, 120);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new Size(60, 15);
            this.lblPhone.TabIndex = 5;
            this.lblPhone.Text = "Điện thoại:";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new Point(20, 85);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new Size(46, 15);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Địa chỉ:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new Point(20, 50);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(72, 15);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Tên thư viện:";
            // 
            // lblLibInfo
            // 
            this.lblLibInfo.AutoSize = true;
            this.lblLibInfo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblLibInfo.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblLibInfo.Location = new Point(20, 15);
            this.lblLibInfo.Name = "lblLibInfo";
            this.lblLibInfo.Size = new Size(167, 20);
            this.lblLibInfo.TabIndex = 0;
            this.lblLibInfo.Text = "Thông tin thư viện";
            // 
            // txtLibraryName
            // 
            this.txtLibraryName.Font = new Font("Segoe UI", 10F);
            this.txtLibraryName.Location = new Point(140, 47);
            this.txtLibraryName.Name = "txtLibraryName";
            this.txtLibraryName.Size = new Size(380, 25);
            this.txtLibraryName.TabIndex = 2;
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new Font("Segoe UI", 10F);
            this.txtAddress.Location = new Point(140, 82);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new Size(380, 25);
            this.txtAddress.TabIndex = 4;
            // 
            // txtPhone
            // 
            this.txtPhone.Font = new Font("Segoe UI", 10F);
            this.txtPhone.Location = new Point(140, 117);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new Size(180, 25);
            this.txtPhone.TabIndex = 6;
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new Font("Segoe UI", 10F);
            this.txtEmail.Location = new Point(390, 117);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new Size(130, 25);
            this.txtEmail.TabIndex = 8;
            // 
            // numBorrowDays
            // 
            this.numBorrowDays.Location = new Point(180, 202);
            this.numBorrowDays.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numBorrowDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBorrowDays.Name = "numBorrowDays";
            this.numBorrowDays.Size = new Size(80, 23);
            this.numBorrowDays.TabIndex = 11;
            this.numBorrowDays.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // numMaxBooks
            // 
            this.numMaxBooks.Location = new Point(180, 237);
            this.numMaxBooks.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numMaxBooks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxBooks.Name = "numMaxBooks";
            this.numMaxBooks.Size = new Size(80, 23);
            this.numMaxBooks.TabIndex = 14;
            this.numMaxBooks.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numFinePerDay
            // 
            this.numFinePerDay.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numFinePerDay.Location = new Point(180, 272);
            this.numFinePerDay.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numFinePerDay.Name = "numFinePerDay";
            this.numFinePerDay.Size = new Size(100, 23);
            this.numFinePerDay.TabIndex = 17;
            this.numFinePerDay.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // panelLogs
            // 
            this.panelLogs.BackColor = Color.White;
            this.panelLogs.Controls.Add(this.btnRefreshLogs);
            this.panelLogs.Controls.Add(this.btnClearLogs);
            this.panelLogs.Controls.Add(this.lblLogs);
            this.panelLogs.Controls.Add(this.dgvLogs);
            this.panelLogs.Location = new Point(590, 66);
            this.panelLogs.Name = "panelLogs";
            this.panelLogs.Size = new Size(600, 480);
            this.panelLogs.TabIndex = 2;
            // 
            // btnRefreshLogs
            // 
            this.btnRefreshLogs.BackColor = Color.FromArgb(59, 130, 246);
            this.btnRefreshLogs.FlatAppearance.BorderSize = 0;
            this.btnRefreshLogs.FlatStyle = FlatStyle.Flat;
            this.btnRefreshLogs.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnRefreshLogs.ForeColor = Color.White;
            this.btnRefreshLogs.Location = new Point(155, 440);
            this.btnRefreshLogs.Name = "btnRefreshLogs";
            this.btnRefreshLogs.Size = new Size(100, 30);
            this.btnRefreshLogs.TabIndex = 3;
            this.btnRefreshLogs.Text = "Làm mới";
            this.btnRefreshLogs.UseVisualStyleBackColor = false;
            this.btnRefreshLogs.Click += this.BtnRefreshLogs_Click;
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.BackColor = Color.FromArgb(239, 68, 68);
            this.btnClearLogs.FlatAppearance.BorderSize = 0;
            this.btnClearLogs.FlatStyle = FlatStyle.Flat;
            this.btnClearLogs.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnClearLogs.ForeColor = Color.White;
            this.btnClearLogs.Location = new Point(15, 440);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new Size(130, 30);
            this.btnClearLogs.TabIndex = 2;
            this.btnClearLogs.Text = "Xóa nhật ký cũ";
            this.btnClearLogs.UseVisualStyleBackColor = false;
            this.btnClearLogs.Click += this.BtnClearLogs_Click;
            // 
            // lblLogs
            // 
            this.lblLogs.AutoSize = true;
            this.lblLogs.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblLogs.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblLogs.Location = new Point(15, 15);
            this.lblLogs.Name = "lblLogs";
            this.lblLogs.Size = new Size(154, 20);
            this.lblLogs.TabIndex = 0;
            this.lblLogs.Text = "Nhật ký hoạt động";
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.BackgroundColor = Color.White;
            this.dgvLogs.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(37, 99, 235);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dgvLogs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogs.Columns.AddRange(new DataGridViewColumn[] {
            this.colLogTime,
            this.colUsername,
            this.colAction});
            this.dgvLogs.Location = new Point(15, 50);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.Size = new Size(570, 380);
            this.dgvLogs.TabIndex = 1;
            // 
            // colLogTime
            // 
            this.colLogTime.HeaderText = "Thời gian";
            this.colLogTime.Name = "LogTime";
            this.colLogTime.ReadOnly = true;
            this.colLogTime.Width = 130;
            // 
            // colUsername
            // 
            this.colUsername.HeaderText = "Người dùng";
            this.colUsername.Name = "Username";
            this.colUsername.ReadOnly = true;
            this.colUsername.Width = 100;
            // 
            // colAction
            // 
            this.colAction.HeaderText = "Hoạt động";
            this.colAction.Name = "Action";
            this.colAction.ReadOnly = true;
            this.colAction.Width = 320;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(246, 248, 252);
            this.ClientSize = new Size(1210, 550);
            this.Controls.Add(this.panelLogs);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.Name = "FormSettings";
            this.Text = "Cài đặt hệ thống";
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.numBorrowDays).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.numMaxBooks).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.numFinePerDay).EndInit();
            this.panelLogs.ResumeLayout(false);
            this.panelLogs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvLogs).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtLibraryName;
        private TextBox txtAddress;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private NumericUpDown numBorrowDays;
        private NumericUpDown numMaxBooks;
        private NumericUpDown numFinePerDay;
        private DataGridView dgvLogs;
        private Label lblConnInfo;
    }
}
