using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Utils;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form cài đặt hệ thống
    /// </summary>
    public partial class FormSettings : Form
    {
        private SystemSettingDAO settingDAO = new SystemSettingDAO();
        private Panel? heroPanel;
        private Label? heroSubtitle;

        public FormSettings()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaSettingsLayout();
                }
                catch
                {
                }
            }

            this.Load += FormSettings_Load;
            this.Resize += FormSettings_Resize;
        }

        private void FormSettings_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                ApplyFigmaSettingsLayout();
                UpdateConnectionInfo();
                ConfigureInputValidation();
                LoadSettings();
                LoadLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void FormSettings_Resize(object? sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            ApplyFigmaSettingsLayout();
        }

        private void ApplyFigmaSettingsLayout()
        {
            BackColor = Color.FromArgb(241, 245, 249);

            if (heroPanel == null)
            {
                heroPanel = new Panel
                {
                    Name = "settingsHeroPanel",
                    BackColor = Color.FromArgb(30, 64, 175)
                };
                heroPanel.Paint += (_, e) =>
                {
                    using var brush = new LinearGradientBrush(heroPanel.ClientRectangle,
                        Color.FromArgb(30, 64, 175), Color.FromArgb(15, 23, 42), 18f);
                    e.Graphics.FillRectangle(brush, heroPanel.ClientRectangle);
                };
                Controls.Add(heroPanel);

                heroSubtitle = new Label
                {
                    Text = "Quản trị thông tin thư viện, quy định và nhật ký vận hành",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(191, 219, 254),
                    AutoSize = true
                };
                heroPanel.Controls.Add(heroSubtitle);
            }

            int margin = 16;
            int heroHeight = 108;
            int gap = 12;
            int bodyTop = margin + heroHeight + gap;
            int bodyHeight = ClientSize.Height - bodyTop - margin;
            int leftWidth = Math.Max(540, (ClientSize.Width - margin * 2 - gap) * 46 / 100);
            int rightWidth = ClientSize.Width - margin * 2 - gap - leftWidth;

            heroPanel.Bounds = new Rectangle(margin, margin, ClientSize.Width - margin * 2, heroHeight);

            lblTitle.Parent = heroPanel;
            lblTitle.Text = "Cấu hình hệ thống";
            lblTitle.Location = new Point(20, 14);
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            if (heroSubtitle != null)
            {
                heroSubtitle.Location = new Point(22, 64);
            }

            panelSettings.BackColor = Color.White;
            panelSettings.BorderStyle = BorderStyle.FixedSingle;
            panelSettings.Location = new Point(margin, bodyTop);
            panelSettings.Size = new Size(leftWidth, bodyHeight);

            panelLogs.BackColor = Color.White;
            panelLogs.BorderStyle = BorderStyle.FixedSingle;
            panelLogs.Location = new Point(panelSettings.Right + gap, bodyTop);
            panelLogs.Size = new Size(rightWidth, bodyHeight);

            btnSave.BackColor = Color.FromArgb(37, 99, 235);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;

            btnTestConn.BackColor = Color.FromArgb(59, 130, 246);
            btnTestConn.ForeColor = Color.White;
            btnTestConn.FlatStyle = FlatStyle.Flat;
            btnTestConn.FlatAppearance.BorderSize = 0;

            btnClearLogs.BackColor = Color.FromArgb(239, 68, 68);
            btnClearLogs.ForeColor = Color.White;
            btnClearLogs.FlatStyle = FlatStyle.Flat;
            btnClearLogs.FlatAppearance.BorderSize = 0;

            btnRefreshLogs.BackColor = Color.FromArgb(59, 130, 246);
            btnRefreshLogs.ForeColor = Color.White;
            btnRefreshLogs.FlatStyle = FlatStyle.Flat;
            btnRefreshLogs.FlatAppearance.BorderSize = 0;

            dgvLogs.EnableHeadersVisualStyles = false;
            dgvLogs.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(4)
            };
            dgvLogs.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(3)
            };
        }

        private void UpdateConnectionInfo()
        {
            string connString;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                connString = "Design Mode Connection String...";
            }
            else
            {
                connString = DatabaseConnection.ConnectionString;
            }

            lblConnInfo.Text = $"Connection String:\n{(connString.Length > 60 ? connString.Substring(0, 60) + "..." : connString)}";
        }

        private void BtnTestConn_Click(object? sender, EventArgs e)
        {
            try
            {
                bool success = DatabaseConnection.TestConnection(out string message);
                MessageBox.Show(success ? "Kết nối thành công!" : message, success ? "Thành công" : "Lỗi",
                    MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClearLogs_Click(object? sender, EventArgs e)
        {
            if (CurrentUser.User?.Role != User.ROLE_ADMIN)
            {
                MessageBox.Show("Chỉ Admin mới có quyền xóa nhật ký!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Xóa tất cả nhật ký hoạt động cũ hơn 30 ngày?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var logDAO = new ActivityLogDAO();
                logDAO.ClearOldLogs(30);
                LoadLogs();
                MessageBox.Show("Đã xóa nhật ký cũ!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRefreshLogs_Click(object? sender, EventArgs e)
        {
            LoadLogs();
        }

        private void LoadSettings()
        {
            try
            {
                txtLibraryName.Text = settingDAO.GetValue(SystemSetting.KEY_LIBRARY_NAME, "Thư viện");
                txtAddress.Text = settingDAO.GetValue(SystemSetting.KEY_LIBRARY_ADDRESS, "");
                txtPhone.Text = settingDAO.GetValue(SystemSetting.KEY_LIBRARY_PHONE, "");
                txtEmail.Text = settingDAO.GetValue(SystemSetting.KEY_LIBRARY_EMAIL, "");

                numBorrowDays.Value = settingDAO.GetIntValue(SystemSetting.KEY_MAX_BORROW_DAYS, 14);
                numMaxBooks.Value = settingDAO.GetIntValue(SystemSetting.KEY_MAX_BOOKS_PER_BORROW, 5);
                numFinePerDay.Value = settingDAO.GetDecimalValue(SystemSetting.KEY_FINE_PER_DAY, 10000);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải cài đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLogs()
        {
            try
            {
                var logDAO = new ActivityLogDAO();
                var logs = logDAO.GetRecentLogs(100);

                dgvLogs.Rows.Clear();
                foreach (var log in logs)
                {
                    dgvLogs.Rows.Add(
                        log.LogTime.ToString("dd/MM HH:mm:ss"),
                        log.Username,
                        log.Action
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải nhật ký: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureInputValidation()
        {
            txtPhone.MaxLength = 11;
            txtPhone.KeyPress -= DigitOnlyKeyPress;
            txtPhone.KeyPress += DigitOnlyKeyPress;
        }

        private static void DigitOnlyKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLibraryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thư viện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLibraryName.Focus();
                return;
            }

            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (!string.IsNullOrWhiteSpace(phone) && !VietnamInputValidator.IsValidVietnamPhoneFlexible(phone))
            {
                MessageBox.Show("Số điện thoại thư viện phải gồm 10 hoặc 11 chữ số và bắt đầu bằng số 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(email) && !VietnamInputValidator.IsValidEmail(email))
            {
                MessageBox.Show("Email thư viện không đúng định dạng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                settingDAO.SaveValue(SystemSetting.KEY_LIBRARY_NAME, txtLibraryName.Text.Trim());
                settingDAO.SaveValue(SystemSetting.KEY_LIBRARY_ADDRESS, txtAddress.Text.Trim());
                settingDAO.SaveValue(SystemSetting.KEY_LIBRARY_PHONE, phone);
                settingDAO.SaveValue(SystemSetting.KEY_LIBRARY_EMAIL, email);

                settingDAO.SaveValue(SystemSetting.KEY_MAX_BORROW_DAYS, numBorrowDays.Value.ToString());
                settingDAO.SaveValue(SystemSetting.KEY_MAX_BOOKS_PER_BORROW, numMaxBooks.Value.ToString());
                settingDAO.SaveValue(SystemSetting.KEY_FINE_PER_DAY, numFinePerDay.Value.ToString());

                // Log
                var logDAO = new ActivityLogDAO();
                logDAO.Log("Cập nhật cài đặt hệ thống", "SystemSettings", 0);

                MessageBox.Show("Lưu cài đặt thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu cài đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
