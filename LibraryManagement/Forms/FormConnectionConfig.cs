using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form cấu hình kết nối SQL Server
    /// </summary>
    public partial class FormConnectionConfig : Form
    {
        public FormConnectionConfig()
        {
            InitializeComponent();
            
            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);
            Load += FormConnectionConfig_Load;
        }

        private void FormConnectionConfig_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
        }

        private void AuthType_Changed(object? sender, EventArgs e)
        {
            bool sqlAuth = rbSqlAuth.Checked;
            txtUserId.Enabled = sqlAuth;
            txtPassword.Enabled = sqlAuth;
            btnSave.Enabled = false;
        }

        private void BtnTest_Click(object? sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Đang kiểm tra kết nối...";
                lblStatus.ForeColor = Color.Blue;
                Application.DoEvents();

                string connStr = BuildConnectionString();

                if (DatabaseConnection.TestConnection(connStr, out string error))
                {
                    lblStatus.Text = "✓ Kết nối thành công!";
                    lblStatus.ForeColor = Color.Green;
                    btnSave.Enabled = true;
                }
                else
                {
                    lblStatus.Text = $"✗ Lỗi: {error}";
                    lblStatus.ForeColor = Color.Red;
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"✗ Lỗi: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            DatabaseConnection.UpdateConnectionString(
                txtServer.Text.Trim(),
                txtDatabase.Text.Trim(),
                rbWindowsAuth.Checked,
                txtUserId.Text.Trim(),
                txtPassword.Text
            );

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private string BuildConnectionString()
        {
            var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
            {
                DataSource = txtServer.Text.Trim(),
                InitialCatalog = txtDatabase.Text.Trim(),
                IntegratedSecurity = rbWindowsAuth.Checked,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true
            };

            if (rbSqlAuth.Checked)
            {
                builder.UserID = txtUserId.Text.Trim();
                builder.Password = txtPassword.Text;
            }

            return builder.ConnectionString;
        }
    }
}
