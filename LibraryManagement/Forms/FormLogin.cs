using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form đăng nhập hệ thống
    /// </summary>
    public partial class FormLogin : Form
    {
        private const int MAX_ATTEMPTS = 3;
        private readonly int lockSeconds;
        private readonly UserDAO userDAO = new UserDAO();
        private readonly EmailService emailService = new EmailService();
        private readonly System.Windows.Forms.Timer lockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        private static readonly Dictionary<string, LoginLockState> LockStates = new Dictionary<string, LoginLockState>(StringComparer.OrdinalIgnoreCase);

        public FormLogin()
        {
            InitializeComponent();

            lockSeconds = ReadLockSeconds();
            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);
            Load += FormLogin_Load;
            lockTimer.Tick += LockTimer_Tick;
            txtUsername.TextChanged += TxtUsername_TextChanged;
        }

        private void SetupForm()
        {
            // Giữ lại để không ảnh hưởng cấu trúc cũ,
            // nhưng không gọi nữa vì giao diện đã chuyển sang Designer.
        }

        private void FormLogin_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                if (!DatabaseConnection.TestConnection(out string error))
                {
                    lblStatus.Text = "⚠ Không thể kết nối CSDL. Kiểm tra cấu hình.";
                    lblStatus.ForeColor = Color.Orange;
                }
                else
                {
                    lblStatus.Text = "✓ Đã kết nối đến máy chủ";
                    lblStatus.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo: " + ex.Message);
            }

            txtUsername.Focus();
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Vui lòng nhập tên đăng nhập!");
                txtUsername.Focus();
                return;
            }

            if (IsUserLocked(username, out int remainSeconds))
            {
                ShowError($"Tài khoản đang tạm khóa. Vui lòng thử lại sau {remainSeconds}s.");
                StartLockCountdown();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Vui lòng nhập mật khẩu!");
                txtPassword.Focus();
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                lblStatus.Text = "Đang đăng nhập...";
                lblStatus.ForeColor = Color.Blue;
                Application.DoEvents();

                var user = userDAO.Login(username, txtPassword.Text);

                if (user != null)
                {
                    ClearLockState(username);
                    CurrentUser.Login(user);

                    var logDAO = new ActivityLogDAO();
                    logDAO.Log("Đăng nhập hệ thống", "Users", user.UserID);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    HandleFailedLogin(username);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi kết nối: {ex.Message}");
            }
            finally
            {
                if (!IsUserLocked(txtUsername.Text.Trim(), out _))
                {
                    btnLogin.Enabled = true;
                }
            }
        }

        private void BtnConfig_Click(object? sender, EventArgs e)
        {
            using (var configForm = new FormConnectionConfig())
            {
                if (configForm.ShowDialog() == DialogResult.OK)
                {
                    FormLogin_Load(sender, e);
                }
            }
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void TxtUsername_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                txtPassword.Focus();
        }

        private void TxtPassword_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                BtnLogin_Click(sender, e);
        }

        private void ShowError(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.Red;
        }

        private void TxtUsername_TextChanged(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text)) return;

            if (IsUserLocked(txtUsername.Text.Trim(), out int remainSeconds))
            {
                ShowError($"Tài khoản đang tạm khóa. Vui lòng thử lại sau {remainSeconds}s.");
                StartLockCountdown();
            }
            else if (!btnLogin.Enabled)
            {
                btnLogin.Enabled = true;
                lockTimer.Stop();
                if (lblStatus.Text.Contains("tạm khóa", StringComparison.OrdinalIgnoreCase))
                {
                    lblStatus.Text = "";
                }
            }
        }

        private void HandleFailedLogin(string username)
        {
            var state = GetOrCreateLockState(username);
            state.FailedCount++;

            int threshold = state.StrictAfterFirstLock ? 1 : MAX_ATTEMPTS;
            int remain = Math.Max(0, threshold - state.FailedCount);

            if (state.FailedCount >= threshold)
            {
                state.FailedCount = 0;
                state.StrictAfterFirstLock = true;
                state.LockUntil = DateTime.Now.AddSeconds(lockSeconds);

                ShowError($"Đăng nhập sai quá số lần cho phép. Tạm khóa {lockSeconds}s.");
                StartLockCountdown();
                _ = NotifyLockByEmailAsync(username, state.LockUntil.Value);
                return;
            }

            ShowError($"Sai tên đăng nhập hoặc mật khẩu! Còn {remain} lần trước khi bị khóa {lockSeconds}s.");
        }

        private async System.Threading.Tasks.Task NotifyLockByEmailAsync(string username, DateTime unlockAt)
        {
            try
            {
                var user = userDAO.GetByUsername(username);
                if (user == null || string.IsNullOrWhiteSpace(user.Email))
                    return;

                await emailService.SendAccountTemporarilyLockedAsync(user.Email, username, unlockAt);
            }
            catch
            {
            }
        }

        private void StartLockCountdown()
        {
            btnLogin.Enabled = false;
            if (!lockTimer.Enabled)
            {
                lockTimer.Start();
            }
        }

        private void LockTimer_Tick(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                btnLogin.Enabled = true;
                lockTimer.Stop();
                return;
            }

            string username = txtUsername.Text.Trim();
            if (!IsUserLocked(username, out int remainSeconds))
            {
                btnLogin.Enabled = true;
                lockTimer.Stop();
                if (lblStatus.Text.Contains("tạm khóa", StringComparison.OrdinalIgnoreCase))
                {
                    lblStatus.Text = "Bạn có thể đăng nhập lại.";
                    lblStatus.ForeColor = Color.DarkGreen;
                }
                return;
            }

            ShowError($"Tài khoản đang tạm khóa. Vui lòng thử lại sau {remainSeconds}s.");
        }

        private static LoginLockState GetOrCreateLockState(string username)
        {
            if (!LockStates.TryGetValue(username, out var state))
            {
                state = new LoginLockState();
                LockStates[username] = state;
            }
            return state;
        }

        private static void ClearLockState(string username)
        {
            if (LockStates.ContainsKey(username))
            {
                LockStates.Remove(username);
            }
        }

        private static bool IsUserLocked(string username, out int remainSeconds)
        {
            remainSeconds = 0;
            if (!LockStates.TryGetValue(username, out var state))
            {
                return false;
            }

            if (!state.LockUntil.HasValue)
            {
                return false;
            }

            TimeSpan diff = state.LockUntil.Value - DateTime.Now;
            if (diff.TotalSeconds <= 0)
            {
                state.LockUntil = null;
                remainSeconds = 0;
                return false;
            }

            remainSeconds = (int)Math.Ceiling(diff.TotalSeconds);
            return true;
        }

        private static int ReadLockSeconds()
        {
            string raw = System.Configuration.ConfigurationManager.AppSettings["LoginLockSeconds"] ?? "60";
            return int.TryParse(raw, out int value) && value > 0 ? value : 60;
        }

        private sealed class LoginLockState
        {
            public int FailedCount { get; set; }
            public bool StrictAfterFirstLock { get; set; }
            public DateTime? LockUntil { get; set; }
        }
    }
}
