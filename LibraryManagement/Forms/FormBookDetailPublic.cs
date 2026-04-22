using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form xem chi tiết sách (công khai)
    /// </summary>
    public partial class FormBookDetailPublic : Form
    {
        private readonly Book book;

        public FormBookDetailPublic()
            : this(new Book())
        {
        }

        public FormBookDetailPublic(Book book)
        {
            this.book = book ?? new Book();
            InitializeComponent();
            
            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);
            this.Load += FormBookDetailPublic_Load;

            StyleButton(btnBorrow, Color.FromArgb(46, 204, 113), 20);
            StyleButton(btnClose, Color.FromArgb(52, 152, 219), 20);
        }

        private void FormBookDetailPublic_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            PopulateBookInfo();
        }

        private void PopulateBookInfo()
        {
            string statusText = book.AvailableCopies > 0 ? "✅ Còn sách" : "❌ Hết sách";
            Color statusColor = book.AvailableCopies > 0
                ? Color.FromArgb(46, 204, 113)
                : Color.FromArgb(231, 76, 60);

            Text = string.IsNullOrWhiteSpace(book.Title) ? "Chi tiết sách" : $"Chi tiết sách: {book.Title}";
            lblBookTitle.Text = string.IsNullOrWhiteSpace(book.Title) ? "Chưa có tiêu đề" : book.Title;
            lblStatus.Text = statusText;
            lblStatus.ForeColor = statusColor;
            lblQuantity.Text = $"Số lượng: {book.AvailableCopies}/{book.TotalCopies} bản";

            lblISBNValue.Text = string.IsNullOrWhiteSpace(book.ISBN) ? "N/A" : book.ISBN;
            lblAuthorValue.Text = string.IsNullOrWhiteSpace(book.AuthorName) ? "Chưa rõ" : book.AuthorName;
            lblCategoryValue.Text = string.IsNullOrWhiteSpace(book.CategoryName) ? "Chưa phân loại" : book.CategoryName;
            lblPublisherValue.Text = string.IsNullOrWhiteSpace(book.PublisherName) ? "N/A" : book.PublisherName;
            lblPublishYearValue.Text = book.PublishYear?.ToString() ?? "N/A";
            lblLocationValue.Text = string.IsNullOrWhiteSpace(book.Location) ? "N/A" : book.Location;
            lblPriceValue.Text = book.Price > 0 ? book.Price.ToString("N0") + " đ" : "N/A";
            txtDescription.Text = string.IsNullOrWhiteSpace(book.Description) ? "Chưa có mô tả" : book.Description;

            btnBorrow.Enabled = book.AvailableCopies > 0;
            LoadBookImage();
        }

        private void LoadBookImage()
        {
            picBook.Image?.Dispose();
            picBook.Image = null;

            if (!string.IsNullOrWhiteSpace(book.ImagePath))
            {
                string imagePath = Path.Combine(Application.StartupPath, "Images", book.ImagePath);
                if (File.Exists(imagePath))
                {
                    try
                    {
                        using (var img = Image.FromFile(imagePath))
                        {
                            picBook.Image = new Bitmap(img);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            lblNoImage.Visible = picBook.Image == null;
        }

        private void BtnBorrow_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void StyleButton(Button btn, Color backColor, int radius)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(12, 0, 12, 0);
            btn.FlatAppearance.MouseOverBackColor = Lighten(backColor, 20);
            btn.FlatAppearance.MouseDownBackColor = Darken(backColor, 15);
            btn.Resize += (s, e) => ApplyRoundedCorners(btn, radius);
            ApplyRoundedCorners(btn, radius);
        }

        private void ApplyRoundedCorners(Control c, int radius)
        {
            if (c.Width == 0 || c.Height == 0)
                return;

            using (var path = new GraphicsPath())
            {
                int r = radius;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(c.Width - r, 0, r, r, 270, 90);
                path.AddArc(c.Width - r, c.Height - r, r, r, 0, 90);
                path.AddArc(0, c.Height - r, r, r, 90, 90);
                path.CloseAllFigures();
                c.Region = new Region(path);
            }
        }

        private Color Lighten(Color color, int amount)
        {
            int r = Math.Min(255, color.R + amount);
            int g = Math.Min(255, color.G + amount);
            int b = Math.Min(255, color.B + amount);
            return Color.FromArgb(r, g, b);
        }

        private Color Darken(Color color, int amount)
        {
            int r = Math.Max(0, color.R - amount);
            int g = Math.Max(0, color.G - amount);
            int b = Math.Max(0, color.B - amount);
            return Color.FromArgb(r, g, b);
        }
    }
}
