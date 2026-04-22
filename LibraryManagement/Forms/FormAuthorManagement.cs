using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form quan ly tac gia.
    /// </summary>
    public sealed partial class FormAuthorManagement : Form
    {
        private readonly AuthorDAO authorDAO = new();
        private Author? currentAuthor;

        public FormAuthorManagement()
        {
            InitializeComponent();
            Load += FormAuthorManagement_Load;
        }

        private void FormAuthorManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            LoadAuthors();
        }

        private void LoadAuthors()
        {
            try
            {
                var authors = authorDAO.GetAll(!chkShowInactive.Checked);

                dgvAuthors.Rows.Clear();
                foreach (var author in authors)
                {
                    int rowIndex = dgvAuthors.Rows.Add(
                        author.AuthorID,
                        author.AuthorName,
                        author.Country,
                        author.IsActive ? "Hoat dong" : "Ngung"
                    );

                    if (!author.IsActive)
                    {
                        dgvAuthors.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
                    }
                }

                if (dgvAuthors.Rows.Count == 0)
                {
                    currentAuthor = null;
                    txtAuthorName.Clear();
                    txtCountry.Clear();
                    txtBiography.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi tai tac gia: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChkShowInactive_CheckedChanged(object? sender, EventArgs e)
        {
            LoadAuthors();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadAuthors();
        }

        private void DgvAuthors_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvAuthors.CurrentRow == null || dgvAuthors.CurrentRow.Cells["AuthorID"].Value == null)
                return;

            int authorId = Convert.ToInt32(dgvAuthors.CurrentRow.Cells["AuthorID"].Value);
            var author = authorDAO.GetById(authorId);
            if (author == null)
                return;

            currentAuthor = author;
            txtAuthorName.Text = author.AuthorName;
            txtCountry.Text = author.Country ?? string.Empty;
            txtBiography.Text = author.Biography ?? string.Empty;
        }

        private void BtnAddNew_Click(object? sender, EventArgs e)
        {
            currentAuthor = null;
            txtAuthorName.Clear();
            txtCountry.Clear();
            txtBiography.Clear();
            txtAuthorName.Focus();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string name = txtAuthorName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui long nhap ten tac gia.", "Canh bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAuthorName.Focus();
                return;
            }

            try
            {
                if (currentAuthor == null)
                {
                    authorDAO.Insert(new Author
                    {
                        AuthorName = name,
                        Country = txtCountry.Text.Trim(),
                        Biography = txtBiography.Text.Trim()
                    });
                    MessageBox.Show("Them tac gia thanh cong.", "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    currentAuthor.AuthorName = name;
                    currentAuthor.Country = txtCountry.Text.Trim();
                    currentAuthor.Biography = txtBiography.Text.Trim();
                    authorDAO.Update(currentAuthor);
                    MessageBox.Show("Cap nhat tac gia thanh cong.", "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                BtnAddNew_Click(null, EventArgs.Empty);
                LoadAuthors();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi luu tac gia: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (currentAuthor == null)
            {
                MessageBox.Show("Vui long chon tac gia can xoa.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Ban co chac muon xoa tac gia '{currentAuthor.AuthorName}'?",
                "Xac nhan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                authorDAO.Delete(currentAuthor.AuthorID);
                MessageBox.Show("Xoa tac gia thanh cong.", "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BtnAddNew_Click(null, EventArgs.Empty);
                LoadAuthors();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi xoa tac gia: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
