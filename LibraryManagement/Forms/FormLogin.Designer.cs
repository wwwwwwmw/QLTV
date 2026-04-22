using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;

        private Panel mainPanel;
        private Panel cardPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private Label lblPassword;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private Button btnConfig;
        private Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            mainPanel = new Panel();
            cardPanel = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblUsername = new Label();
            lblPassword = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnExit = new Button();
            btnConfig = new Button();
            lblStatus = new Label();
            cardPanel.SuspendLayout();
            mainPanel.SuspendLayout();
            SuspendLayout();

            // mainPanel
            mainPanel.BackColor = Color.FromArgb(219, 237, 232);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(btnConfig);
            mainPanel.Controls.Add(cardPanel);
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(540, 680);
            mainPanel.TabIndex = 0;

            // cardPanel
            cardPanel.Anchor = AnchorStyles.None;
            cardPanel.BackColor = Color.White;
            cardPanel.BorderStyle = BorderStyle.FixedSingle;
            cardPanel.Controls.Add(lblTitle);
            cardPanel.Controls.Add(lblSubtitle);
            cardPanel.Controls.Add(lblUsername);
            cardPanel.Controls.Add(txtUsername);
            cardPanel.Controls.Add(lblPassword);
            cardPanel.Controls.Add(txtPassword);
            cardPanel.Controls.Add(lblStatus);
            cardPanel.Controls.Add(btnLogin);
            cardPanel.Controls.Add(btnExit);
            cardPanel.Location = new Point(120, 70);
            cardPanel.Name = "cardPanel";
            cardPanel.Size = new Size(300, 520);
            cardPanel.TabIndex = 10;

            // lblTitle
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(0, 35);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(298, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Serene Curator";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblSubtitle
            lblSubtitle.Font = new Font("Segoe UI", 9F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(0, 74);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(298, 22);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Library Management System";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblUsername
            lblUsername.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(51, 65, 85);
            lblUsername.Location = new Point(28, 155);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(160, 22);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Tên đăng nhập";

            // txtUsername
            txtUsername.Font = new Font("Segoe UI", 10F);
            txtUsername.Location = new Point(28, 180);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Nhập tên đăng nhập";
            txtUsername.Size = new Size(240, 30);
            txtUsername.TabIndex = 3;
            txtUsername.KeyPress += TxtUsername_KeyPress;

            // lblPassword
            lblPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblPassword.Location = new Point(28, 226);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(120, 22);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Mật khẩu";

            // txtPassword
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.Location = new Point(28, 251);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.PlaceholderText = "Nhập mật khẩu";
            txtPassword.Size = new Size(240, 30);
            txtPassword.TabIndex = 5;
            txtPassword.KeyPress += TxtPassword_KeyPress;

            // lblStatus
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(28, 292);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(240, 40);
            lblStatus.TabIndex = 6;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;

            // btnLogin
            btnLogin.BackColor = Color.FromArgb(59, 130, 246);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(28, 350);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(240, 38);
            btnLogin.TabIndex = 7;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += BtnLogin_Click;

            // btnExit
            btnExit.BackColor = Color.White;
            btnExit.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnExit.FlatAppearance.BorderSize = 1;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 10F);
            btnExit.ForeColor = Color.FromArgb(71, 85, 105);
            btnExit.Location = new Point(28, 398);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(240, 34);
            btnExit.TabIndex = 8;
            btnExit.Text = "Thoát";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += BtnExit_Click;

            // btnConfig
            btnConfig.BackColor = Color.Transparent;
            btnConfig.FlatAppearance.BorderSize = 0;
            btnConfig.FlatStyle = FlatStyle.Flat;
            btnConfig.Font = new Font("Segoe UI", 12F);
            btnConfig.ForeColor = Color.Gray;
            btnConfig.Location = new Point(500, 10);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new Size(30, 30);
            btnConfig.TabIndex = 9;
            btnConfig.Text = "⚙";
            btnConfig.UseVisualStyleBackColor = false;
            btnConfig.Click += BtnConfig_Click;

            // FormLogin
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(219, 237, 232);
            ClientSize = new Size(540, 680);
            Controls.Add(mainPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập - Quản lý Thư viện";

            cardPanel.ResumeLayout(false);
            cardPanel.PerformLayout();
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}