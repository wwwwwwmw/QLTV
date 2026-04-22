using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form quản lý nhà xuất bản.
    /// </summary>
    public sealed partial class FormPublisherManagement : Form
    {
        private readonly PublisherDAO publisherDAO = new();

        private Publisher? currentPublisher;

        public FormPublisherManagement()
        {
            InitializeComponent();
            Load += FormPublisherManagement_Load;
        }

        private void FormPublisherManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            LoadPublishers();
        }

        private void LoadPublishers()
        {
            try
            {
                var publishers = publisherDAO.GetAll(!chkShowInactive.Checked);

                dgvPublishers.Rows.Clear();
                foreach (var publisher in publishers)
                {
                    int rowIndex = dgvPublishers.Rows.Add(
                        publisher.PublisherID,
                        publisher.PublisherName,
                        publisher.Phone,
                        publisher.Email,
                        publisher.Website,
                        publisher.IsActive ? "Hoạt động" : "Ngừng"
                    );

                    if (!publisher.IsActive)
                    {
                        dgvPublishers.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
                    }
                }

                if (dgvPublishers.Rows.Count == 0)
                {
                    currentPublisher = null;
                    txtPublisherName.Clear();
                    txtAddress.Clear();
                    txtPhone.Clear();
                    txtEmail.Clear();
                    txtWebsite.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải nhà xuất bản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChkShowInactive_CheckedChanged(object? sender, EventArgs e)
        {
            LoadPublishers();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadPublishers();
        }

        private void DgvPublishers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvPublishers.CurrentRow == null || dgvPublishers.CurrentRow.Cells["PublisherID"].Value == null)
                return;

            int publisherId = Convert.ToInt32(dgvPublishers.CurrentRow.Cells["PublisherID"].Value);
            var publisher = publisherDAO.GetById(publisherId);
            if (publisher == null)
                return;

            currentPublisher = publisher;
            txtPublisherName.Text = publisher.PublisherName;
            txtAddress.Text = publisher.Address ?? string.Empty;
            txtPhone.Text = publisher.Phone ?? string.Empty;
            txtEmail.Text = publisher.Email ?? string.Empty;
            txtWebsite.Text = publisher.Website ?? string.Empty;
        }

        private void BtnAddNew_Click(object? sender, EventArgs e)
        {
            currentPublisher = null;
            txtPublisherName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtWebsite.Clear();
            txtPublisherName.Focus();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string name = txtPublisherName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên nhà xuất bản.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPublisherName.Focus();
                return;
            }

            try
            {
                if (currentPublisher == null)
                {
                    publisherDAO.Insert(new Publisher
                    {
                        PublisherName = name,
                        Address = txtAddress.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Website = txtWebsite.Text.Trim()
                    });
                    MessageBox.Show("Thêm nhà xuất bản thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    currentPublisher.PublisherName = name;
                    currentPublisher.Address = txtAddress.Text.Trim();
                    currentPublisher.Phone = txtPhone.Text.Trim();
                    currentPublisher.Email = txtEmail.Text.Trim();
                    currentPublisher.Website = txtWebsite.Text.Trim();
                    publisherDAO.Update(currentPublisher);
                    MessageBox.Show("Cập nhật nhà xuất bản thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                BtnAddNew_Click(null, EventArgs.Empty);
                LoadPublishers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu nhà xuất bản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (currentPublisher == null)
            {
                MessageBox.Show("Vui lòng chọn nhà xuất bản cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa nhà xuất bản '{currentPublisher.PublisherName}'?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                publisherDAO.Delete(currentPublisher.PublisherID);
                MessageBox.Show("Xóa nhà xuất bản thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BtnAddNew_Click(null, EventArgs.Empty);
                LoadPublishers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa nhà xuất bản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
