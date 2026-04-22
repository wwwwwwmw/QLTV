using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormPublic
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelHeader;
        private Panel panelSearch;
        private Panel panelBooks;
        private FlowLayoutPanel flowBooks;
        private Panel panelHighlights;
        private Label lblNewBooks;
        private FlowLayoutPanel flowNewBooks;
        private Label lblCategories;
        private FlowLayoutPanel flowCategories;
        private TextBox txtSearch;
        private ComboBox cboCategory;
        private Label lblTotalBooks;

        private Label lblHeaderTitle;
        private Button btnRegister;
        private Button btnLogin;
        private Button btnRefresh;
        private Label lblSearch;
        private Label lblCategory;

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
            panelHeader = new Panel();
            lblHeaderTitle = new Label();
            btnRegister = new Button();
            btnLogin = new Button();
            panelSearch = new Panel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblCategory = new Label();
            cboCategory = new ComboBox();
            lblTotalBooks = new Label();
            btnRefresh = new Button();
            panelHighlights = new Panel();
            lblNewBooks = new Label();
            flowNewBooks = new FlowLayoutPanel();
            lblCategories = new Label();
            flowCategories = new FlowLayoutPanel();
            panelBooks = new Panel();
            flowBooks = new FlowLayoutPanel();
            panelHeader.SuspendLayout();
            panelSearch.SuspendLayout();
            panelHighlights.SuspendLayout();
            panelBooks.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(lblHeaderTitle);
            panelHeader.Controls.Add(btnRegister);
            panelHeader.Controls.Add(btnLogin);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(4, 5, 4, 5);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1829, 90);
            panelHeader.TabIndex = 3;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.Location = new Point(24, 20);
            lblHeaderTitle.Margin = new Padding(4, 0, 4, 0);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(747, 46);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "THƯ VIỆN SÁCH  ·  Tra cứu & Mượn sách online";
            // 
            // btnRegister
            // 
            btnRegister.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRegister.BackColor = Color.FromArgb(124, 58, 237);
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRegister.ForeColor = Color.White;
            btnRegister.Location = new Point(1368, 20);
            btnRegister.Margin = new Padding(4, 5, 4, 5);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(154, 46);
            btnRegister.TabIndex = 1;
            btnRegister.Text = "Đăng ký";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += BtnRegister_Click;
            // 
            // btnLogin
            // 
            btnLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogin.BackColor = Color.FromArgb(59, 130, 246);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(1540, 20);
            btnLogin.Margin = new Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(168, 46);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += BtnLogin_Click;
            // 
            // panelSearch
            // 
            panelSearch.BackColor = Color.FromArgb(248, 250, 252);
            panelSearch.Controls.Add(lblSearch);
            panelSearch.Controls.Add(txtSearch);
            panelSearch.Controls.Add(lblCategory);
            panelSearch.Controls.Add(cboCategory);
            panelSearch.Controls.Add(lblTotalBooks);
            panelSearch.Controls.Add(btnRefresh);
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Location = new Point(0, 90);
            panelSearch.Margin = new Padding(4, 5, 4, 5);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(1829, 84);
            panelSearch.TabIndex = 2;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSearch.Location = new Point(24, 27);
            lblSearch.Margin = new Padding(4, 0, 4, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(105, 28);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Tìm kiếm:";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10.5F);
            txtSearch.Location = new Point(150, 24);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Nhập tên sách, tác giả, ISBN...";
            txtSearch.Size = new Size(430, 35);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategory.Location = new Point(678, 25);
            lblCategory.Margin = new Padding(4, 0, 4, 0);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(93, 28);
            lblCategory.TabIndex = 2;
            lblCategory.Text = "Thể loại:";
            // 
            // cboCategory
            // 
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.Font = new Font("Segoe UI", 10.5F);
            cboCategory.Location = new Point(809, 23);
            cboCategory.Margin = new Padding(4, 5, 4, 5);
            cboCategory.Name = "cboCategory";
            cboCategory.Size = new Size(260, 38);
            cboCategory.TabIndex = 3;
            cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;
            // 
            // lblTotalBooks
            // 
            lblTotalBooks.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTotalBooks.AutoSize = true;
            lblTotalBooks.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblTotalBooks.ForeColor = Color.FromArgb(30, 41, 59);
            lblTotalBooks.Location = new Point(1510, 27);
            lblTotalBooks.Margin = new Padding(4, 0, 4, 0);
            lblTotalBooks.Name = "lblTotalBooks";
            lblTotalBooks.Size = new Size(137, 30);
            lblTotalBooks.TabIndex = 4;
            lblTotalBooks.Text = "Tổng: 0 sách";
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(59, 130, 246);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(1692, 18);
            btnRefresh.Margin = new Padding(4, 5, 4, 5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 46);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // panelHighlights
            // 
            panelHighlights.BackColor = Color.FromArgb(243, 246, 251);
            panelHighlights.Controls.Add(lblNewBooks);
            panelHighlights.Controls.Add(flowNewBooks);
            panelHighlights.Controls.Add(lblCategories);
            panelHighlights.Controls.Add(flowCategories);
            panelHighlights.Dock = DockStyle.Top;
            panelHighlights.Location = new Point(0, 174);
            panelHighlights.Margin = new Padding(4, 5, 4, 5);
            panelHighlights.Name = "panelHighlights";
            panelHighlights.Size = new Size(1829, 368);
            panelHighlights.TabIndex = 1;
            // 
            // lblNewBooks
            // 
            lblNewBooks.AutoSize = true;
            lblNewBooks.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblNewBooks.ForeColor = Color.FromArgb(15, 23, 42);
            lblNewBooks.Location = new Point(26, 12);
            lblNewBooks.Margin = new Padding(4, 0, 4, 0);
            lblNewBooks.Name = "lblNewBooks";
            lblNewBooks.Size = new Size(201, 36);
            lblNewBooks.TabIndex = 0;
            lblNewBooks.Text = "TOP SÁCH MỚI";
            // 
            // flowNewBooks
            // 
            flowNewBooks.AutoScroll = true;
            flowNewBooks.BackColor = Color.Transparent;
            flowNewBooks.Location = new Point(26, 56);
            flowNewBooks.Margin = new Padding(4, 5, 4, 5);
            flowNewBooks.Name = "flowNewBooks";
            flowNewBooks.Size = new Size(1768, 196);
            flowNewBooks.TabIndex = 1;
            flowNewBooks.WrapContents = false;
            // 
            // lblCategories
            // 
            lblCategories.AutoSize = true;
            lblCategories.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblCategories.ForeColor = Color.FromArgb(15, 23, 42);
            lblCategories.Location = new Point(26, 286);
            lblCategories.Margin = new Padding(4, 0, 4, 0);
            lblCategories.Name = "lblCategories";
            lblCategories.Size = new Size(205, 36);
            lblCategories.TabIndex = 2;
            lblCategories.Text = "THỂ LOẠI SÁCH";
            // 
            // flowCategories
            // 
            flowCategories.AutoScroll = true;
            flowCategories.BackColor = Color.Transparent;
            flowCategories.Location = new Point(240, 279);
            flowCategories.Margin = new Padding(4, 5, 4, 5);
            flowCategories.Name = "flowCategories";
            flowCategories.Size = new Size(1554, 82);
            flowCategories.TabIndex = 3;
            flowCategories.WrapContents = false;
            // 
            // panelBooks
            // 
            panelBooks.Controls.Add(flowBooks);
            panelBooks.Dock = DockStyle.Fill;
            panelBooks.Location = new Point(0, 542);
            panelBooks.Margin = new Padding(4, 5, 4, 5);
            panelBooks.Name = "panelBooks";
            panelBooks.Padding = new Padding(24, 16, 24, 20);
            panelBooks.Size = new Size(1829, 686);
            panelBooks.TabIndex = 0;
            // 
            // flowBooks
            // 
            flowBooks.AutoScroll = true;
            flowBooks.Dock = DockStyle.Fill;
            flowBooks.Location = new Point(24, 16);
            flowBooks.Margin = new Padding(4, 5, 4, 5);
            flowBooks.Name = "flowBooks";
            flowBooks.Size = new Size(1781, 650);
            flowBooks.TabIndex = 0;
            // 
            // FormPublic
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 251);
            ClientSize = new Size(1829, 1200);
            Controls.Add(panelBooks);
            Controls.Add(panelHighlights);
            Controls.Add(panelSearch);
            Controls.Add(panelHeader);
            DoubleBuffered = true;
            Margin = new Padding(4, 5, 4, 5);
            Name = "FormPublic";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "📚 Thư Viện Sách - Tra cứu công khai";
            WindowState = FormWindowState.Maximized;
            Resize += FormPublic_Resize;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            panelHighlights.ResumeLayout(false);
            panelHighlights.PerformLayout();
            panelBooks.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}