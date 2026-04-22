using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormConnectionConfig
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblServer = null!;
        private Label lblDatabase = null!;
        private Label lblAuthType = null!;
        private Label lblUserId = null!;
        private Label lblPassword = null!;
        private Button btnCancel = null!;
        private Label lblInfo = null!;

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
            this.lblServer = new Label();
            this.txtServer = new TextBox();
            this.lblDatabase = new Label();
            this.txtDatabase = new TextBox();
            this.lblAuthType = new Label();
            this.rbWindowsAuth = new RadioButton();
            this.rbSqlAuth = new RadioButton();
            this.lblUserId = new Label();
            this.txtUserId = new TextBox();
            this.lblPassword = new Label();
            this.txtPassword = new TextBox();
            this.lblStatus = new Label();
            this.btnTest = new Button();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.lblInfo = new Label();
            this.SuspendLayout();
            // 
            // lblServer
            // 
            this.lblServer.Font = new Font("Segoe UI", 10F);
            this.lblServer.Location = new Point(20, 23);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new Size(150, 25);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Máy chủ SQL Server:";
            // 
            // txtServer
            // 
            this.txtServer.Font = new Font("Segoe UI", 10F);
            this.txtServer.Location = new Point(170, 20);
            this.txtServer.Name = "txtServer";
            this.txtServer.PlaceholderText = "Ví dụ: 192.168.1.100\\SQLEXPRESS";
            this.txtServer.Size = new Size(250, 25);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = ".\\SQLEXPRESS";
            // 
            // lblDatabase
            // 
            this.lblDatabase.Font = new Font("Segoe UI", 10F);
            this.lblDatabase.Location = new Point(20, 63);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new Size(150, 25);
            this.lblDatabase.TabIndex = 2;
            this.lblDatabase.Text = "Tên Database:";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Font = new Font("Segoe UI", 10F);
            this.txtDatabase.Location = new Point(170, 60);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new Size(250, 25);
            this.txtDatabase.TabIndex = 3;
            this.txtDatabase.Text = "LibraryManagement";
            // 
            // lblAuthType
            // 
            this.lblAuthType.Font = new Font("Segoe UI", 10F);
            this.lblAuthType.Location = new Point(20, 103);
            this.lblAuthType.Name = "lblAuthType";
            this.lblAuthType.Size = new Size(150, 25);
            this.lblAuthType.TabIndex = 4;
            this.lblAuthType.Text = "Phương thức xác thực:";
            // 
            // rbWindowsAuth
            // 
            this.rbWindowsAuth.Checked = true;
            this.rbWindowsAuth.Location = new Point(40, 128);
            this.rbWindowsAuth.Name = "rbWindowsAuth";
            this.rbWindowsAuth.Size = new Size(400, 25);
            this.rbWindowsAuth.TabIndex = 5;
            this.rbWindowsAuth.TabStop = true;
            this.rbWindowsAuth.Text = "Windows Authentication (Dùng tài khoản Windows)";
            this.rbWindowsAuth.UseVisualStyleBackColor = true;
            this.rbWindowsAuth.CheckedChanged += this.AuthType_Changed;
            // 
            // rbSqlAuth
            // 
            this.rbSqlAuth.Location = new Point(40, 158);
            this.rbSqlAuth.Name = "rbSqlAuth";
            this.rbSqlAuth.Size = new Size(400, 25);
            this.rbSqlAuth.TabIndex = 6;
            this.rbSqlAuth.Text = "SQL Server Authentication (Dùng User/Password)";
            this.rbSqlAuth.UseVisualStyleBackColor = true;
            this.rbSqlAuth.CheckedChanged += this.AuthType_Changed;
            // 
            // lblUserId
            // 
            this.lblUserId.Font = new Font("Segoe UI", 10F);
            this.lblUserId.Location = new Point(20, 203);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new Size(150, 25);
            this.lblUserId.TabIndex = 7;
            this.lblUserId.Text = "User ID:";
            // 
            // txtUserId
            // 
            this.txtUserId.Enabled = false;
            this.txtUserId.Font = new Font("Segoe UI", 10F);
            this.txtUserId.Location = new Point(170, 200);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new Size(250, 25);
            this.txtUserId.TabIndex = 8;
            this.txtUserId.Text = "sa";
            // 
            // lblPassword
            // 
            this.lblPassword.Font = new Font("Segoe UI", 10F);
            this.lblPassword.Location = new Point(20, 243);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(150, 25);
            this.lblPassword.TabIndex = 9;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Font = new Font("Segoe UI", 10F);
            this.txtPassword.Location = new Point(170, 240);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new Size(250, 25);
            this.txtPassword.TabIndex = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = Color.Gray;
            this.lblStatus.Location = new Point(20, 290);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(400, 25);
            this.lblStatus.TabIndex = 11;
            // 
            // btnTest
            // 
            this.btnTest.BackColor = Color.FromArgb(52, 152, 219);
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = FlatStyle.Flat;
            this.btnTest.ForeColor = Color.White;
            this.btnTest.Location = new Point(20, 325);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new Size(130, 35);
            this.btnTest.TabIndex = 12;
            this.btnTest.Text = "Kiểm tra kết nối";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += this.BtnTest_Click;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = Color.FromArgb(46, 204, 113);
            this.btnSave.Enabled = false;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(160, 325);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(130, 35);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Lưu & Đóng";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += this.BtnSave_Click;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(300, 325);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(130, 35);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += this.BtnCancel_Click;
            // 
            // lblInfo
            // 
            this.lblInfo.Font = new Font("Segoe UI", 9F);
            this.lblInfo.ForeColor = Color.FromArgb(41, 128, 185);
            this.lblInfo.Location = new Point(20, 375);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new Size(400, 50);
            this.lblInfo.TabIndex = 15;
            this.lblInfo.Text = "Đối với mạng LAN, nhập địa chỉ IP của máy chủ SQL Server\r\nVí dụ: 192.168.1.100 hoặc 192.168.1.100\\SQLEXPRESS";
            // 
            // FormConnectionConfig
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(450, 440);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUserId);
            this.Controls.Add(this.lblUserId);
            this.Controls.Add(this.rbSqlAuth);
            this.Controls.Add(this.rbWindowsAuth);
            this.Controls.Add(this.lblAuthType);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblServer);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConnectionConfig";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Cấu hình kết nối SQL Server";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtServer;
        private TextBox txtDatabase;
        private RadioButton rbWindowsAuth;
        private RadioButton rbSqlAuth;
        private TextBox txtUserId;
        private TextBox txtPassword;
        private Button btnTest;
        private Button btnSave;
        private Label lblStatus;
    }
}
