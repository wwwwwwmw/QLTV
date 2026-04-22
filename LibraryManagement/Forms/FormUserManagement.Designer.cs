using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormUserManagement
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle = null!;
        private Panel panelSearch = null!;
        private Label lblSearch = null!;
        private Label lblRole = null!;
        private Button btnAdd = null!;
        private Button btnRefresh = null!;
        private Label lblDetail = null!;
        private Label lblUsername = null!;
        private Label lblFullName = null!;
        private Label lblEmail2 = null!;
        private Label lblPhone2 = null!;
        private Label lblUserRole = null!;
        private Label lblPassword = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Button btnDelete = null!;
        private Button btnResetPwd = null!;
        private DataGridViewTextBoxColumn colUserId = null!;
        private DataGridViewTextBoxColumn colUsername = null!;
        private DataGridViewTextBoxColumn colFullName = null!;
        private DataGridViewTextBoxColumn colEmail = null!;
        private DataGridViewTextBoxColumn colRole = null!;
        private DataGridViewTextBoxColumn colStatus = null!;
        private DataGridViewTextBoxColumn colLastLogin = null!;

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
            this.btnAdd = new Button();
            this.lblRole = new Label();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();
            this.cboRole = new ComboBox();
            this.dgvUsers = new DataGridView();
            this.colUserId = new DataGridViewTextBoxColumn();
            this.colUsername = new DataGridViewTextBoxColumn();
            this.colFullName = new DataGridViewTextBoxColumn();
            this.colEmail = new DataGridViewTextBoxColumn();
            this.colRole = new DataGridViewTextBoxColumn();
            this.colStatus = new DataGridViewTextBoxColumn();
            this.colLastLogin = new DataGridViewTextBoxColumn();
            this.panelDetail = new Panel();
            this.btnResetPwd = new Button();
            this.btnDelete = new Button();
            this.btnCancel = new Button();
            this.btnSave = new Button();
            this.lblPassword = new Label();
            this.lblUserRole = new Label();
            this.lblPhone2 = new Label();
            this.lblEmail2 = new Label();
            this.lblFullName = new Label();
            this.lblUsername = new Label();
            this.lblDetail = new Label();
            this.txtUsername = new TextBox();
            this.txtFullName = new TextBox();
            this.txtEmail = new TextBox();
            this.txtPhone = new TextBox();
            this.cboUserRole = new ComboBox();
            this.chkActive = new CheckBox();
            this.txtNewPassword = new TextBox();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
            this.panelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            this.lblTitle.Location = new Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(264, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý tài khoản";
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = Color.White;
            this.panelSearch.Controls.Add(this.btnRefresh);
            this.panelSearch.Controls.Add(this.btnAdd);
            this.panelSearch.Controls.Add(this.lblRole);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.cboRole);
            this.panelSearch.Location = new Point(20, 66);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new Size(750, 64);
            this.panelSearch.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = Color.FromArgb(59, 130, 246);
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.Location = new Point(600, 16);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(48, 32);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "R";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += this.BtnRefresh_Click;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = Color.FromArgb(37, 99, 235);
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(486, 16);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(108, 32);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Thêm mới";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += this.BtnAdd_Click;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new Point(308, 23);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new Size(46, 15);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "Vai trò:";
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
            this.txtSearch.Location = new Point(82, 19);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tên đăng nhập, họ tên...";
            this.txtSearch.Size = new Size(200, 25);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += this.TxtSearch_TextChanged;
            // 
            // cboRole
            // 
            this.cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboRole.FormattingEnabled = true;
            this.cboRole.Items.AddRange(new object[] {
            "-- Tất cả --",
            "Admin",
            "Manager",
            "Staff"});
            this.cboRole.Location = new Point(360, 19);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new Size(120, 23);
            this.cboRole.TabIndex = 3;
            this.cboRole.SelectedIndexChanged += this.CboRole_SelectedIndexChanged;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.BackgroundColor = Color.White;
            this.dgvUsers.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(37, 99, 235);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Columns.AddRange(new DataGridViewColumn[] {
            this.colUserId,
            this.colUsername,
            this.colFullName,
            this.colEmail,
            this.colRole,
            this.colStatus,
            this.colLastLogin});
            this.dgvUsers.Location = new Point(20, 142);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new Size(750, 388);
            this.dgvUsers.TabIndex = 2;
            this.dgvUsers.CellDoubleClick += this.DgvUsers_CellDoubleClick;
            this.dgvUsers.SelectionChanged += this.DgvUsers_SelectionChanged;
            // 
            // colUserId
            // 
            this.colUserId.HeaderText = "ID";
            this.colUserId.Name = "UserID";
            this.colUserId.ReadOnly = true;
            this.colUserId.Visible = false;
            // 
            // colUsername
            // 
            this.colUsername.HeaderText = "Tên đăng nhập";
            this.colUsername.Name = "Username";
            this.colUsername.ReadOnly = true;
            this.colUsername.Width = 120;
            // 
            // colFullName
            // 
            this.colFullName.HeaderText = "Họ tên";
            this.colFullName.Name = "FullName";
            this.colFullName.ReadOnly = true;
            this.colFullName.Width = 180;
            // 
            // colEmail
            // 
            this.colEmail.HeaderText = "Email";
            this.colEmail.Name = "Email";
            this.colEmail.ReadOnly = true;
            this.colEmail.Width = 180;
            // 
            // colRole
            // 
            this.colRole.HeaderText = "Vai trò";
            this.colRole.Name = "Role";
            this.colRole.ReadOnly = true;
            this.colRole.Width = 80;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Trạng thái";
            this.colStatus.Name = "Status";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 80;
            // 
            // colLastLogin
            // 
            this.colLastLogin.HeaderText = "Đăng nhập cuối";
            this.colLastLogin.Name = "LastLogin";
            this.colLastLogin.ReadOnly = true;
            this.colLastLogin.Width = 130;
            // 
            // panelDetail
            // 
            this.panelDetail.BackColor = Color.White;
            this.panelDetail.Controls.Add(this.btnResetPwd);
            this.panelDetail.Controls.Add(this.btnDelete);
            this.panelDetail.Controls.Add(this.btnCancel);
            this.panelDetail.Controls.Add(this.btnSave);
            this.panelDetail.Controls.Add(this.lblPassword);
            this.panelDetail.Controls.Add(this.lblUserRole);
            this.panelDetail.Controls.Add(this.lblPhone2);
            this.panelDetail.Controls.Add(this.lblEmail2);
            this.panelDetail.Controls.Add(this.lblFullName);
            this.panelDetail.Controls.Add(this.lblUsername);
            this.panelDetail.Controls.Add(this.lblDetail);
            this.panelDetail.Controls.Add(this.txtUsername);
            this.panelDetail.Controls.Add(this.txtFullName);
            this.panelDetail.Controls.Add(this.txtEmail);
            this.panelDetail.Controls.Add(this.txtPhone);
            this.panelDetail.Controls.Add(this.cboUserRole);
            this.panelDetail.Controls.Add(this.chkActive);
            this.panelDetail.Controls.Add(this.txtNewPassword);
            this.panelDetail.Location = new Point(790, 66);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new Size(400, 470);
            this.panelDetail.TabIndex = 3;
            // 
            // btnResetPwd
            // 
            this.btnResetPwd.BackColor = Color.FromArgb(245, 158, 11);
            this.btnResetPwd.FlatAppearance.BorderSize = 0;
            this.btnResetPwd.FlatStyle = FlatStyle.Flat;
            this.btnResetPwd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnResetPwd.ForeColor = Color.White;
            this.btnResetPwd.Location = new Point(20, 355);
            this.btnResetPwd.Name = "btnResetPwd";
            this.btnResetPwd.Size = new Size(130, 32);
            this.btnResetPwd.TabIndex = 17;
            this.btnResetPwd.Text = "Reset mật khẩu";
            this.btnResetPwd.UseVisualStyleBackColor = false;
            this.btnResetPwd.Click += this.BtnResetPassword_Click;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(190, 310);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(70, 35);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += this.BtnDelete_Click;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = Color.FromArgb(107, 114, 128);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(110, 310);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(70, 35);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += this.BtnCancel_Click;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = Color.FromArgb(37, 99, 235);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(20, 310);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(80, 35);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += this.BtnSave_Click;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new Point(20, 265);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(80, 15);
            this.lblPassword.TabIndex = 12;
            this.lblPassword.Text = "Mật khẩu mới:";
            // 
            // lblUserRole
            // 
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.Location = new Point(20, 195);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new Size(46, 15);
            this.lblUserRole.TabIndex = 8;
            this.lblUserRole.Text = "Vai trò:";
            // 
            // lblPhone2
            // 
            this.lblPhone2.AutoSize = true;
            this.lblPhone2.Location = new Point(20, 160);
            this.lblPhone2.Name = "lblPhone2";
            this.lblPhone2.Size = new Size(60, 15);
            this.lblPhone2.TabIndex = 6;
            this.lblPhone2.Text = "Điện thoại:";
            // 
            // lblEmail2
            // 
            this.lblEmail2.AutoSize = true;
            this.lblEmail2.Location = new Point(20, 125);
            this.lblEmail2.Name = "lblEmail2";
            this.lblEmail2.Size = new Size(39, 15);
            this.lblEmail2.TabIndex = 4;
            this.lblEmail2.Text = "Email:";
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new Point(20, 90);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new Size(47, 15);
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "Họ tên:";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new Point(20, 55);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new Size(84, 15);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Tên đăng nhập:";
            // 
            // lblDetail
            // 
            this.lblDetail.AutoSize = true;
            this.lblDetail.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblDetail.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblDetail.Location = new Point(15, 15);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Size = new Size(144, 20);
            this.lblDetail.TabIndex = 18;
            this.lblDetail.Text = "Chi tiết tài khoản";
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new Font("Segoe UI", 10F);
            this.txtUsername.Location = new Point(140, 52);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(230, 25);
            this.txtUsername.TabIndex = 1;
            // 
            // txtFullName
            // 
            this.txtFullName.Font = new Font("Segoe UI", 10F);
            this.txtFullName.Location = new Point(140, 87);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new Size(230, 25);
            this.txtFullName.TabIndex = 3;
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new Font("Segoe UI", 10F);
            this.txtEmail.Location = new Point(140, 122);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new Size(230, 25);
            this.txtEmail.TabIndex = 5;
            // 
            // txtPhone
            // 
            this.txtPhone.Font = new Font("Segoe UI", 10F);
            this.txtPhone.Location = new Point(140, 157);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new Size(230, 25);
            this.txtPhone.TabIndex = 7;
            // 
            // cboUserRole
            // 
            this.cboUserRole.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboUserRole.FormattingEnabled = true;
            this.cboUserRole.Items.AddRange(new object[] {
            "Admin",
            "Manager",
            "Staff"});
            this.cboUserRole.Location = new Point(140, 192);
            this.cboUserRole.Name = "cboUserRole";
            this.cboUserRole.Size = new Size(230, 23);
            this.cboUserRole.TabIndex = 9;
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = CheckState.Checked;
            this.chkActive.Location = new Point(140, 230);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new Size(122, 19);
            this.chkActive.TabIndex = 10;
            this.chkActive.Text = "Kích hoạt tài khoản";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Font = new Font("Segoe UI", 10F);
            this.txtNewPassword.Location = new Point(140, 262);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PlaceholderText = "Để trống nếu không đổi";
            this.txtNewPassword.Size = new Size(230, 25);
            this.txtNewPassword.TabIndex = 13;
            this.txtNewPassword.UseSystemPasswordChar = true;
            // 
            // FormUserManagement
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(246, 248, 252);
            this.ClientSize = new Size(1210, 540);
            this.Controls.Add(this.panelDetail);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.Name = "FormUserManagement";
            this.Text = "Quản lý người dùng";
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtSearch;
        private ComboBox cboRole;
        private DataGridView dgvUsers;
        private Panel panelDetail;
        private TextBox txtUsername;
        private TextBox txtFullName;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private ComboBox cboUserRole;
        private CheckBox chkActive;
        private TextBox txtNewPassword;
    }
}
