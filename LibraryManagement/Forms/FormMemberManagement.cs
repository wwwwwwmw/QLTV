using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Services;
using LibraryManagement.Utils;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form quản lý độc giả / thành viên
    /// </summary>
    public partial class FormMemberManagement : Form
    {
        private MemberDAO memberDAO = new MemberDAO();
        private EmailService emailService = new EmailService();
        private Member? currentMember;
        private Panel? heroPanel;
        private Label? lblHeroSubtitle;

        public FormMemberManagement()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    ApplyFigmaMemberLayout();
                }
                catch
                {
                }
            }

            this.Load += FormMemberManagement_Load;
            this.Resize += FormMemberManagement_Resize;
        }

        private void FormMemberManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                ApplyFigmaMemberLayout();
                cboMemberType.SelectedIndex = 0;
                chkActiveOnly.Checked = true;
                ConfigureInputValidation();
                ClearDetailForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void FormMemberManagement_Resize(object? sender, EventArgs e)
        {
            if (!IsHandleCreated) return;
            ApplyFigmaMemberLayout();
        }

        private void ApplyFigmaMemberLayout()
        {
            BackColor = Color.FromArgb(241, 245, 249);

            if (heroPanel == null)
            {
                heroPanel = new Panel
                {
                    Name = "heroMemberPanel",
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
                    Text = "Quản lý thông tin độc giả, trạng thái thẻ và nợ phạt",
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
            int detailWidth = 392;
            int leftWidth = ClientSize.Width - margin * 2 - detailWidth - gap;

            heroPanel.Bounds = new Rectangle(margin, margin, ClientSize.Width - margin * 2, heroHeight);

            lblTitle.Parent = heroPanel;
            lblTitle.Text = "Trung tâm độc giả";
            lblTitle.Location = new Point(20, 14);
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            if (lblHeroSubtitle != null)
            {
                lblHeroSubtitle.Location = new Point(22, 64);
            }

            panelSearch.BackColor = Color.White;
            panelSearch.BorderStyle = BorderStyle.FixedSingle;
            panelSearch.Bounds = new Rectangle(margin, bodyTop, leftWidth, 64);

            txtSearch.Location = new Point(14, 19);
            txtSearch.Size = new Size(260, 25);
            lblType.Location = new Point(286, 23);
            cboMemberType.Location = new Point(340, 19);
            cboMemberType.Size = new Size(150, 23);
            chkActiveOnly.Location = new Point(502, 22);

            int listTop = panelSearch.Bottom + gap;
            int actionTop = ClientSize.Height - margin - 38;

            dgvMembers.Location = new Point(margin, listTop);
            dgvMembers.Size = new Size(leftWidth, actionTop - listTop - 10);
            dgvMembers.EnableHeadersVisualStyles = false;
            dgvMembers.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(4)
            };

            StyleAction(btnAdd, Color.FromArgb(37, 99, 235));
            StyleAction(btnEdit, Color.FromArgb(59, 130, 246));
            StyleAction(btnDelete, Color.FromArgb(239, 68, 68));
            StyleAction(btnHistory, Color.FromArgb(99, 102, 241));
            StyleAction(btnPayFine, Color.FromArgb(245, 158, 11));
            StyleAction(btnRefresh, Color.FromArgb(107, 114, 128));

            btnAdd.Bounds = new Rectangle(margin, actionTop, 114, 38);
            btnEdit.Bounds = new Rectangle(btnAdd.Right + 8, actionTop, 90, 38);
            btnDelete.Bounds = new Rectangle(btnEdit.Right + 8, actionTop, 90, 38);
            btnHistory.Bounds = new Rectangle(btnDelete.Right + 8, actionTop, 122, 38);
            btnPayFine.Bounds = new Rectangle(btnHistory.Right + 8, actionTop, 106, 38);
            btnRefresh.Bounds = new Rectangle(btnPayFine.Right + 8, actionTop, 110, 38);

            panelDetail.BackColor = Color.White;
            panelDetail.BorderStyle = BorderStyle.FixedSingle;
            panelDetail.Location = new Point(margin + leftWidth + gap, bodyTop);
            panelDetail.Size = new Size(detailWidth, bodyHeight + 64);

            lblDetailTitle.Text = "Chi tiết độc giả";
            lblDetailTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);

            StyleAction(btnSave, Color.FromArgb(37, 99, 235));
            StyleAction(btnCancel, Color.FromArgb(107, 114, 128));
            btnSave.Text = "Lưu thay đổi";
            btnSave.Size = new Size(120, 34);
            btnCancel.Size = new Size(88, 34);
        }

        private static void StyleAction(Button button, Color color)
        {
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            SearchMembers();
        }

        private void CboMemberType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            SearchMembers();
        }

        private void ChkActiveOnly_CheckedChanged(object? sender, EventArgs e)
        {
            SearchMembers();
        }

        private void DgvMembers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            EditMember();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            AddMember();
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            EditMember();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            DeleteMember();
        }

        private void BtnHistory_Click(object? sender, EventArgs e)
        {
            ShowBorrowHistory();
        }

        private void BtnPayFine_Click(object? sender, EventArgs e)
        {
            PayFine();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            ClearDetailForm();
        }

        private void ConfigureInputValidation()
        {
            txtPhone.MaxLength = 10;
            txtIdentityCard.MaxLength = 12;

            txtPhone.KeyPress -= DigitOnlyKeyPress;
            txtIdentityCard.KeyPress -= DigitOnlyKeyPress;
            txtPhone.KeyPress += DigitOnlyKeyPress;
            txtIdentityCard.KeyPress += DigitOnlyKeyPress;
        }

        private static void DigitOnlyKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private bool ValidateMemberInput(out string phone, out string email, out string identityCard)
        {
            phone = txtPhone.Text.Trim();
            email = txtEmail.Text.Trim();
            identityCard = txtIdentityCard.Text.Trim();

            if (!VietnamInputValidator.IsValidFullName(txtFullName.Text))
            {
                MessageBox.Show("Họ tên không hợp lệ. Chỉ dùng chữ cái và khoảng trắng (2-100 ký tự).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            DateTime dob = dtpDateOfBirth.Value.Date;
            if (dob > DateTime.Today.AddYears(-6) || dob < new DateTime(1900, 1, 1))
            {
                MessageBox.Show("Ngày sinh không hợp lệ. Tuổi phải từ 6 đến 126.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDateOfBirth.Focus();
                return false;
            }

            if (!VietnamInputValidator.IsValidVietnamMobilePhone(phone))
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số theo chuẩn di động Việt Nam (ví dụ: 09xxxxxxxx).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(email) && !VietnamInputValidator.IsValidEmail(email))
            {
                MessageBox.Show("Email không đúng định dạng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(identityCard) && !VietnamInputValidator.IsValidIdentityCard12(identityCard))
            {
                MessageBox.Show("Số CCCD phải gồm đúng 12 chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdentityCard.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return false;
            }

            string memberCode = txtMemberCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(memberCode))
            {
                MessageBox.Show("Mã độc giả không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int? excludeId = currentMember?.MemberID;
            if (memberDAO.MemberCodeExists(memberCode, excludeId))
            {
                MessageBox.Show("Mã độc giả đã tồn tại. Vui lòng tải lại dữ liệu và thử lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void LoadData()
        {
            SearchMembers();
        }

        private void SearchMembers()
        {
            try
            {
                string? keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
                string? memberType = cboMemberType.SelectedIndex > 0 ? cboMemberType.SelectedItem?.ToString() : null;

                var members = memberDAO.Search(keyword, memberType, chkActiveOnly.Checked);

                dgvMembers.Rows.Clear();
                foreach (var member in members)
                {
                    var row = dgvMembers.Rows.Add(
                        member.MemberID,
                        member.MemberCode,
                        member.FullName,
                        member.Gender,
                        member.Phone,
                        member.MemberType,
                        member.ExpiryDate?.ToString("dd/MM/yyyy"),
                        member.TotalFine.ToString("N0") + " đ",
                        member.StatusDisplay
                    );

                    // Highlight if has fine or expired
                    if (member.HasFine || member.IsExpired)
                    {
                        dgvMembers.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMembers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvMembers.CurrentRow == null) return;

            int memberId = Convert.ToInt32(dgvMembers.CurrentRow.Cells["MemberID"].Value);
            currentMember = memberDAO.GetById(memberId);

            if (currentMember != null)
            {
                txtMemberCode.Text = currentMember.MemberCode;
                txtFullName.Text = currentMember.FullName;
                cboGender.SelectedItem = currentMember.Gender;
                if (currentMember.DateOfBirth.HasValue)
                    dtpDateOfBirth.Value = currentMember.DateOfBirth.Value;
                txtPhone.Text = currentMember.Phone;
                txtEmail.Text = currentMember.Email;
                txtIdentityCard.Text = currentMember.IdentityCard;
                txtAddress.Text = currentMember.Address;
                cboMemberTypeDetail.SelectedItem = currentMember.MemberType;
                if (currentMember.ExpiryDate.HasValue)
                    dtpExpiryDate.Value = currentMember.ExpiryDate.Value;
                txtNotes.Text = currentMember.Notes;
                lblTotalFine.Text = currentMember.TotalFine.ToString("N0") + " đ";
            }
        }

        private void AddMember()
        {
            currentMember = null;
            ClearDetailForm();

            // Generate new member code
            txtMemberCode.Text = memberDAO.GenerateMemberCode();
            dtpExpiryDate.Value = DateTime.Now.AddYears(1);
            cboMemberTypeDetail.SelectedIndex = 0;
            cboGender.SelectedIndex = 0;

            txtFullName.Focus();
        }

        private void EditMember()
        {
            if (dgvMembers.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtFullName.Focus();
        }

        private void DeleteMember()
        {
            if (dgvMembers.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xóa độc giả này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int memberId = Convert.ToInt32(dgvMembers.CurrentRow.Cells["MemberID"].Value);
                    if (memberDAO.Delete(memberId))
                    {
                        MessageBox.Show("Xóa độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SearchMembers();
                        ClearDetailForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowBorrowHistory()
        {
            if (currentMember == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var historyForm = new FormBorrowHistory(currentMember))
            {
                historyForm.ShowDialog();
            }
        }

        private void PayFine()
        {
            if (currentMember == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (currentMember.TotalFine <= 0)
            {
                MessageBox.Show("Độc giả này không có nợ phạt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var payForm = new FormPayFine(currentMember))
            {
                if (payForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh data
                    currentMember = memberDAO.GetById(currentMember.MemberID);
                    lblTotalFine.Text = currentMember?.TotalFine.ToString("N0") + " đ";
                    SearchMembers();
                }
            }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (!ValidateMemberInput(out string phone, out string email, out string identityCard))
            {
                return;
            }

            try
            {
                var member = currentMember ?? new Member();
                member.MemberCode = txtMemberCode.Text.Trim();
                member.FullName = txtFullName.Text.Trim();
                member.Gender = cboGender.SelectedItem?.ToString();
                member.DateOfBirth = dtpDateOfBirth.Value;
                member.Phone = phone;
                member.Email = email;
                member.IdentityCard = identityCard;
                member.Address = txtAddress.Text.Trim();
                member.MemberType = cboMemberTypeDetail.SelectedItem?.ToString() ?? Member.TYPE_NORMAL;
                member.ExpiryDate = dtpExpiryDate.Value;
                member.Notes = txtNotes.Text.Trim();

                if (currentMember == null)
                {
                    memberDAO.Insert(member);
                    MessageBox.Show("Thêm độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!string.IsNullOrWhiteSpace(member.Email))
                    {
                        bool sent = await emailService.SendMemberRegistrationSuccessAsync(member);
                        if (!sent)
                        {
                            MessageBox.Show(
                                $"Đã thêm độc giả nhưng chưa gửi được email thông báo.\n\nChi tiết: {emailService.LastError}",
                                "Cảnh báo gửi email",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    memberDAO.Update(member);
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SearchMembers();
                ClearDetailForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearDetailForm()
        {
            currentMember = null;
            txtMemberCode.Clear();
            txtFullName.Clear();
            cboGender.SelectedIndex = -1;
            dtpDateOfBirth.Value = DateTime.Now.AddYears(-20);
            txtPhone.Clear();
            txtEmail.Clear();
            txtIdentityCard.Clear();
            txtAddress.Clear();
            cboMemberTypeDetail.SelectedIndex = -1;
            dtpExpiryDate.Value = DateTime.Now.AddYears(1);
            txtNotes.Clear();
            lblTotalFine.Text = "0 đ";
        }
    }
}
