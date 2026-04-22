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
    /// Form quản lý người dùng (chỉ dành cho Admin)
    /// </summary>
    public partial class FormUserManagement : Form
    {
        private UserDAO userDAO = new UserDAO();
        private User? selectedUser;
        private Panel? heroPanel;
        private Label? lblHeroSubtitle;

        public FormUserManagement()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaUserLayout();
                }
                catch
                {
                }
            }

            this.Load += FormUserManagement_Load;
            this.Resize += FormUserManagement_Resize;
        }

        private void FormUserManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // Check permission at runtime
            if (CurrentUser.User?.Role != User.ROLE_ADMIN)
            {
                this.Controls.Clear();
                var lblNoAccess = new Label
                {
                    Text = "⛔ Bạn không có quyền truy cập chức năng này!\n\nChỉ Admin mới có thể quản lý người dùng.",
                    Location = new Point(300, 200),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 14),
                    ForeColor = Color.FromArgb(231, 76, 60)
                };
                this.Controls.Add(lblNoAccess);
                return;
            }

            try
            {
                ApplyFigmaUserLayout();
                ConfigureInputValidation();
                cboRole.SelectedIndex = 0;
                SetFormEnabled(false);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void FormUserManagement_Resize(object? sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            ApplyFigmaUserLayout();
        }

        private void ApplyFigmaUserLayout()
        {
            BackColor = Color.FromArgb(241, 245, 249);

            if (heroPanel == null)
            {
                heroPanel = new Panel
                {
                    Name = "heroUserPanel",
                    BackColor = Color.FromArgb(30, 64, 175)
                };
                heroPanel.Paint += (_, e) =>
                {
                    using var brush = new LinearGradientBrush(heroPanel.ClientRectangle,
                        Color.FromArgb(30, 64, 175), Color.FromArgb(15, 23, 42), 18f);
                    e.Graphics.FillRectangle(brush, heroPanel.ClientRectangle);
                };
                Controls.Add(heroPanel);

                lblHeroSubtitle = new Label
                {
                    Text = "Quản lý tài khoản nội bộ, phân quyền và trạng thái đăng nhập",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(191, 219, 254),
                    AutoSize = true
                };
                heroPanel.Controls.Add(lblHeroSubtitle);
            }

            int margin = 16;
            int heroHeight = 108;
            int gap = 12;
            int bodyTop = margin + heroHeight + gap;
            int bodyHeight = ClientSize.Height - bodyTop - margin;
            int detailWidth = 402;
            int leftWidth = ClientSize.Width - margin * 2 - detailWidth - gap;

            heroPanel.Bounds = new Rectangle(margin, margin, ClientSize.Width - margin * 2, heroHeight);

            lblTitle.Parent = heroPanel;
            lblTitle.Text = "Quản trị tài khoản";
            lblTitle.Location = new Point(20, 14);
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            if (lblHeroSubtitle != null)
            {
                lblHeroSubtitle.Location = new Point(22, 64);
            }

            panelSearch.BackColor = Color.White;
            panelSearch.BorderStyle = BorderStyle.FixedSingle;
            panelSearch.Location = new Point(margin, bodyTop);
            panelSearch.Size = new Size(leftWidth, 64);

            lblSearch.Location = new Point(16, 23);
            txtSearch.Location = new Point(80, 19);
            txtSearch.Size = new Size(220, 25);

            lblRole.Location = new Point(312, 23);
            cboRole.Location = new Point(360, 19);
            cboRole.Size = new Size(120, 23);

            StyleActionButton(btnAdd, Color.FromArgb(37, 99, 235));
            StyleActionButton(btnRefresh, Color.FromArgb(59, 130, 246));
            btnAdd.Location = new Point(panelSearch.Width - 174, 16);
            btnAdd.Size = new Size(108, 32);
            btnRefresh.Location = new Point(panelSearch.Width - 58, 16);
            btnRefresh.Size = new Size(48, 32);

            dgvUsers.Location = new Point(margin, panelSearch.Bottom + gap);
            dgvUsers.Size = new Size(leftWidth, bodyHeight - 64 - gap);
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(4)
            };

            panelDetail.BackColor = Color.White;
            panelDetail.BorderStyle = BorderStyle.FixedSingle;
            panelDetail.Location = new Point(margin + leftWidth + gap, bodyTop);
            panelDetail.Size = new Size(detailWidth, bodyHeight);

            lblDetail.Text = "Chi tiết tài khoản";
            lblDetail.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblDetail.ForeColor = Color.FromArgb(30, 41, 59);

            StyleActionButton(btnSave, Color.FromArgb(37, 99, 235));
            StyleActionButton(btnCancel, Color.FromArgb(107, 114, 128));
            StyleActionButton(btnDelete, Color.FromArgb(239, 68, 68));
            StyleActionButton(btnResetPwd, Color.FromArgb(245, 158, 11));
        }

        private static void StyleActionButton(Button button, Color color)
        {
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            SearchUsers();
        }

        private void CboRole_SelectedIndexChanged(object? sender, EventArgs e)
        {
            SearchUsers();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            AddNew();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void DgvUsers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            EditUser();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            CancelEdit();
        }

        private void LoadData()
        {
            SearchUsers();
        }

        private void SearchUsers()
        {
            try
            {
                string? keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
                string? role = cboRole.SelectedIndex > 0 ? cboRole.SelectedItem?.ToString() : null;

                var users = userDAO.Search(keyword, role);

                dgvUsers.Rows.Clear();
                foreach (var user in users)
                {
                    var row = dgvUsers.Rows.Add(
                        user.UserID,
                        user.Username,
                        user.FullName,
                        user.Email,
                        user.Role,
                        user.IsActive ? "Hoạt động" : "Khóa",
                        user.LastLogin?.ToString("dd/MM HH:mm") ?? "-"
                    );

                    if (!user.IsActive)
                    {
                        dgvUsers.Rows[row].DefaultCellStyle.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvUsers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                selectedUser = null;
                ClearForm();
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);
            selectedUser = userDAO.GetById(userId);

            if (selectedUser != null)
            {
                DisplayUser(selectedUser);
            }
        }

        private void DisplayUser(User user)
        {
            txtUsername.Text = user.Username;
            txtFullName.Text = user.FullName;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone;
            cboUserRole.SelectedItem = user.Role;
            chkActive.Checked = user.IsActive;
            txtNewPassword.Text = "";

            txtUsername.Enabled = false; // Can't change username
        }

        private void AddNew()
        {
            selectedUser = null;
            ClearForm();
            SetFormEnabled(true);
            txtUsername.Enabled = true;
            txtUsername.Focus();
        }

        private void EditUser()
        {
            if (selectedUser == null) return;

            SetFormEnabled(true);
            txtUsername.Enabled = false;
            txtFullName.Focus();
        }

        private void CancelEdit()
        {
            if (selectedUser != null)
            {
                DisplayUser(selectedUser);
            }
            else
            {
                ClearForm();
            }
            SetFormEnabled(false);
        }

        private void ClearForm()
        {
            txtUsername.Text = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            cboUserRole.SelectedIndex = 2;
            chkActive.Checked = true;
            txtNewPassword.Text = "";
        }

        private void SetFormEnabled(bool enabled)
        {
            txtUsername.Enabled = enabled;
            txtFullName.Enabled = enabled;
            txtEmail.Enabled = enabled;
            txtPhone.Enabled = enabled;
            cboUserRole.Enabled = enabled;
            chkActive.Enabled = enabled;
            txtNewPassword.Enabled = enabled;
        }

        private void ConfigureInputValidation()
        {
            txtPhone.MaxLength = 10;
            txtUsername.MaxLength = 30;

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

        private bool ValidateUserInput(out string username, out string fullName, out string email, out string phone)
        {
            username = txtUsername.Text.Trim();
            fullName = txtFullName.Text.Trim();
            email = txtEmail.Text.Trim();
            phone = txtPhone.Text.Trim();

            if (!VietnamInputValidator.IsValidUsername(username))
            {
                MessageBox.Show("Tên đăng nhập chỉ gồm chữ, số, dấu chấm hoặc gạch dưới, dài 4-30 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (!VietnamInputValidator.IsValidFullName(fullName))
            {
                MessageBox.Show("Họ tên không hợp lệ. Chỉ dùng chữ cái và khoảng trắng (2-100 ký tự).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(email) && !VietnamInputValidator.IsValidEmail(email))
            {
                MessageBox.Show("Email không đúng định dạng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(phone) && !VietnamInputValidator.IsValidVietnamMobilePhone(phone))
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số theo chuẩn di động Việt Nam.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            if (selectedUser == null && !VietnamInputValidator.IsStrongPassword(txtNewPassword.Text))
            {
                MessageBox.Show("Mật khẩu tài khoản mới phải có ít nhất 8 ký tự, gồm cả chữ và số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return false;
            }

            if (selectedUser != null && !string.IsNullOrWhiteSpace(txtNewPassword.Text) && !VietnamInputValidator.IsStrongPassword(txtNewPassword.Text))
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 8 ký tự, gồm cả chữ và số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return false;
            }

            return true;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (selectedUser == null && string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cho tài khoản mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }

            if (!ValidateUserInput(out string username, out string fullName, out string email, out string phone))
            {
                return;
            }

            try
            {
                if (selectedUser == null)
                {
                    // Add new
                    var newUser = new User
                    {
                        Username = username,
                        FullName = fullName,
                        Email = email,
                        Phone = phone,
                        Role = cboUserRole.SelectedItem?.ToString() ?? User.ROLE_STAFF,
                        IsActive = chkActive.Checked
                    };

                    var (success, message) = userDAO.Add(newUser, txtNewPassword.Text);

                    if (success)
                    {
                        var logDAO = new ActivityLogDAO();
                        logDAO.Log($"Thêm người dùng mới: {newUser.Username}", "Users", 0);

                        MessageBox.Show("Thêm người dùng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        CancelEdit();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Update
                    selectedUser.FullName = fullName;
                    selectedUser.Email = email;
                    selectedUser.Phone = phone;
                    selectedUser.Role = cboUserRole.SelectedItem?.ToString() ?? User.ROLE_STAFF;
                    selectedUser.IsActive = chkActive.Checked;

                    var (success, message) = userDAO.Update(selectedUser);

                    if (success)
                    {
                        // Update password if provided
                        if (!string.IsNullOrWhiteSpace(txtNewPassword.Text))
                        {
                            userDAO.ChangePassword(selectedUser.UserID, txtNewPassword.Text);
                        }

                        var logDAO = new ActivityLogDAO();
                        logDAO.Log($"Cập nhật người dùng: {selectedUser.Username}", "Users", selectedUser.UserID);

                        MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        CancelEdit();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser.UserID == CurrentUser.User?.UserID)
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn xóa người dùng '{selectedUser.Username}'?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var (success, message) = userDAO.Delete(selectedUser.UserID);

                if (success)
                {
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log($"Xóa người dùng: {selectedUser.Username}", "Users", selectedUser.UserID);

                    MessageBox.Show("Xóa người dùng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    CancelEdit();
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnResetPassword_Click(object? sender, EventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Reset mật khẩu về mặc định (123456) cho user '{selectedUser.Username}'?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                userDAO.ChangePassword(selectedUser.UserID, "123456");
                MessageBox.Show("Đã reset mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
