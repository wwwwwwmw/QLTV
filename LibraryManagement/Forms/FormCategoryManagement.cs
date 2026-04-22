using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form quản lý danh mục/thể loại sách.
    /// </summary>
    public sealed partial class FormCategoryManagement : Form
    {
        private readonly CategoryDAO categoryDAO = new();

        private Category? currentCategory;

        public FormCategoryManagement()
        {
            InitializeComponent();
            Load += FormCategoryManagement_Load;
        }

        private void FormCategoryManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                var categories = categoryDAO.GetAll(!chkShowInactive.Checked);

                dgvCategories.Rows.Clear();
                foreach (var category in categories)
                {
                    int rowIndex = dgvCategories.Rows.Add(
                        category.CategoryID,
                        category.CategoryName,
                        category.Description,
                        category.IsActive ? "Hoạt động" : "Ngừng"
                    );

                    if (!category.IsActive)
                    {
                        dgvCategories.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
                    }
                }

                if (dgvCategories.Rows.Count == 0)
                {
                    currentCategory = null;
                    txtCategoryName.Clear();
                    txtDescription.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChkShowInactive_CheckedChanged(object? sender, EventArgs e)
        {
            LoadCategories();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadCategories();
        }

        private void DgvCategories_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvCategories.CurrentRow == null || dgvCategories.CurrentRow.Cells["CategoryID"].Value == null)
                return;

            int categoryId = Convert.ToInt32(dgvCategories.CurrentRow.Cells["CategoryID"].Value);
            var category = categoryDAO.GetById(categoryId);
            if (category == null)
                return;

            currentCategory = category;
            txtCategoryName.Text = category.CategoryName;
            txtDescription.Text = category.Description ?? string.Empty;
        }

        private void BtnAddNew_Click(object? sender, EventArgs e)
        {
            currentCategory = null;
            txtCategoryName.Clear();
            txtDescription.Clear();
            txtCategoryName.Focus();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string name = txtCategoryName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return;
            }

            try
            {
                if (currentCategory == null)
                {
                    categoryDAO.Insert(new Category
                    {
                        CategoryName = name,
                        Description = txtDescription.Text.Trim()
                    });
                    MessageBox.Show("Thêm danh mục thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    currentCategory.CategoryName = name;
                    currentCategory.Description = txtDescription.Text.Trim();
                    categoryDAO.Update(currentCategory);
                    MessageBox.Show("Cập nhật danh mục thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                BtnAddNew_Click(null, EventArgs.Empty);
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (currentCategory == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa danh mục '{currentCategory.CategoryName}'?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                categoryDAO.Delete(currentCategory.CategoryID);
                MessageBox.Show("Xóa danh mục thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BtnAddNew_Click(null, EventArgs.Empty);
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
