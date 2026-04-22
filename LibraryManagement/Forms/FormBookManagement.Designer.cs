using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormBookManagement
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Panel panelSearch;
        private Label lblCategory;
        private Label lblAuthor;

        private DataGridView dgvBooks;
        private TextBox txtSearch;
        private ComboBox cboCategory;
        private ComboBox cboAuthor;
        private CheckBox chkAvailableOnly;

        private Panel panelDetail;
        private Label lblDetailTitle;
        private Panel panelImage;

        private TextBox txtISBN;
        private TextBox txtTitle;
        private ComboBox cboCategoryDetail;
        private ComboBox cboAuthorDetail;
        private ComboBox cboPublisher;
        private NumericUpDown numYear;
        private NumericUpDown numPrice;
        private NumericUpDown numTotalCopies;
        private TextBox txtLocation;
        private TextBox txtDescription;

        private PictureBox picBookImage;
        private Button btnBrowseImage;
        private Button btnRemoveImage;
        private Button btnViewDetail;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnSave;
        private Button btnCancel;

        private Label lblISBN;
        private Label lblBookTitle;
        private Label lblCategoryDetail;
        private Label lblAuthorDetail;
        private Label lblPublisher;
        private Label lblYear;
        private Label lblPrice;
        private Label lblCopies;
        private Label lblLocation;
        private Label lblDescription;

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
            lblTitle = new Label();
            panelSearch = new Panel();
            txtSearch = new TextBox();
            lblCategory = new Label();
            cboCategory = new ComboBox();
            lblAuthor = new Label();
            cboAuthor = new ComboBox();
            chkAvailableOnly = new CheckBox();
            dgvBooks = new DataGridView();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            panelDetail = new Panel();
            lblDetailTitle = new Label();
            panelImage = new Panel();
            picBookImage = new PictureBox();
            btnBrowseImage = new Button();
            btnRemoveImage = new Button();
            btnViewDetail = new Button();
            lblISBN = new Label();
            txtISBN = new TextBox();
            lblBookTitle = new Label();
            txtTitle = new TextBox();
            lblCategoryDetail = new Label();
            cboCategoryDetail = new ComboBox();
            lblAuthorDetail = new Label();
            cboAuthorDetail = new ComboBox();
            lblPublisher = new Label();
            cboPublisher = new ComboBox();
            lblYear = new Label();
            numYear = new NumericUpDown();
            lblPrice = new Label();
            numPrice = new NumericUpDown();
            lblCopies = new Label();
            numTotalCopies = new NumericUpDown();
            lblLocation = new Label();
            txtLocation = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
            panelDetail.SuspendLayout();
            panelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBookImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numYear).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTotalCopies).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblTitle.Location = new Point(18, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(247, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Kho Sách Thư Viện";
            // 
            // panelSearch
            // 
            panelSearch.BackColor = Color.White;
            panelSearch.Controls.Add(txtSearch);
            panelSearch.Controls.Add(lblCategory);
            panelSearch.Controls.Add(cboCategory);
            panelSearch.Controls.Add(lblAuthor);
            panelSearch.Controls.Add(cboAuthor);
            panelSearch.Controls.Add(chkAvailableOnly);
            panelSearch.Location = new Point(18, 51);
            panelSearch.Margin = new Padding(3, 2, 3, 2);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(718, 48);
            panelSearch.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(14, 13);
            txtSearch.Margin = new Padding(3, 2, 3, 2);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm theo tên sách, ISBN...";
            txtSearch.Size = new Size(202, 25);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(229, 15);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(52, 15);
            lblCategory.TabIndex = 1;
            lblCategory.Text = "Thể loại:";
            // 
            // cboCategory
            // 
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.Location = new Point(285, 13);
            cboCategory.Margin = new Padding(3, 2, 3, 2);
            cboCategory.Name = "cboCategory";
            cboCategory.Size = new Size(132, 23);
            cboCategory.TabIndex = 2;
            cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;
            // 
            // lblAuthor
            // 
            lblAuthor.AutoSize = true;
            lblAuthor.Location = new Point(427, 15);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new Size(47, 15);
            lblAuthor.TabIndex = 3;
            lblAuthor.Text = "Tác giả:";
            // 
            // cboAuthor
            // 
            cboAuthor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAuthor.Location = new Point(481, 13);
            cboAuthor.Margin = new Padding(3, 2, 3, 2);
            cboAuthor.Name = "cboAuthor";
            cboAuthor.Size = new Size(132, 23);
            cboAuthor.TabIndex = 4;
            cboAuthor.SelectedIndexChanged += CboAuthor_SelectedIndexChanged;
            // 
            // chkAvailableOnly
            // 
            chkAvailableOnly.AutoSize = true;
            chkAvailableOnly.Location = new Point(623, 14);
            chkAvailableOnly.Margin = new Padding(3, 2, 3, 2);
            chkAvailableOnly.Name = "chkAvailableOnly";
            chkAvailableOnly.Size = new Size(94, 19);
            chkAvailableOnly.TabIndex = 5;
            chkAvailableOnly.Text = "Chỉ còn sách";
            chkAvailableOnly.UseVisualStyleBackColor = true;
            chkAvailableOnly.CheckedChanged += ChkAvailableOnly_CheckedChanged;
            // 
            // dgvBooks
            // 
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.AllowUserToDeleteRows = false;
            dgvBooks.BackgroundColor = Color.White;
            dgvBooks.BorderStyle = BorderStyle.None;
            dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBooks.Location = new Point(18, 106);
            dgvBooks.Margin = new Padding(3, 2, 3, 2);
            dgvBooks.MultiSelect = false;
            dgvBooks.Name = "dgvBooks";
            dgvBooks.ReadOnly = true;
            dgvBooks.RowHeadersVisible = false;
            dgvBooks.RowTemplate.Height = 29;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.Size = new Size(718, 280);
            dgvBooks.TabIndex = 2;
            dgvBooks.CellDoubleClick += DgvBooks_CellDoubleClick;
            dgvBooks.SelectionChanged += DgvBooks_SelectionChanged;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(37, 99, 235);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(18, 399);
            btnAdd.Margin = new Padding(3, 2, 3, 2);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(103, 28);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Thêm mới";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += BtnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(59, 130, 246);
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(128, 399);
            btnEdit.Margin = new Padding(3, 2, 3, 2);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(79, 28);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(214, 399);
            btnDelete.Margin = new Padding(3, 2, 3, 2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(79, 28);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(107, 114, 128);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(299, 399);
            btnRefresh.Margin = new Padding(3, 2, 3, 2);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(96, 28);
            btnRefresh.TabIndex = 6;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // panelDetail
            // 
            panelDetail.AutoScroll = true;
            panelDetail.BackColor = Color.White;
            panelDetail.Controls.Add(lblDetailTitle);
            panelDetail.Controls.Add(panelImage);
            panelDetail.Controls.Add(btnBrowseImage);
            panelDetail.Controls.Add(btnRemoveImage);
            panelDetail.Controls.Add(btnViewDetail);
            panelDetail.Controls.Add(lblISBN);
            panelDetail.Controls.Add(txtISBN);
            panelDetail.Controls.Add(lblBookTitle);
            panelDetail.Controls.Add(txtTitle);
            panelDetail.Controls.Add(lblCategoryDetail);
            panelDetail.Controls.Add(cboCategoryDetail);
            panelDetail.Controls.Add(lblAuthorDetail);
            panelDetail.Controls.Add(cboAuthorDetail);
            panelDetail.Controls.Add(lblPublisher);
            panelDetail.Controls.Add(cboPublisher);
            panelDetail.Controls.Add(lblYear);
            panelDetail.Controls.Add(numYear);
            panelDetail.Controls.Add(lblPrice);
            panelDetail.Controls.Add(numPrice);
            panelDetail.Controls.Add(lblCopies);
            panelDetail.Controls.Add(numTotalCopies);
            panelDetail.Controls.Add(lblLocation);
            panelDetail.Controls.Add(txtLocation);
            panelDetail.Controls.Add(lblDescription);
            panelDetail.Controls.Add(txtDescription);
            panelDetail.Controls.Add(btnSave);
            panelDetail.Controls.Add(btnCancel);
            panelDetail.Location = new Point(752, 51);
            panelDetail.Margin = new Padding(3, 2, 3, 2);
            panelDetail.Name = "panelDetail";
            panelDetail.Size = new Size(315, 478);
            panelDetail.TabIndex = 7;
            // 
            // lblDetailTitle
            // 
            lblDetailTitle.AutoSize = true;
            lblDetailTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDetailTitle.Location = new Point(13, 8);
            lblDetailTitle.Name = "lblDetailTitle";
            lblDetailTitle.Size = new Size(136, 21);
            lblDetailTitle.TabIndex = 0;
            lblDetailTitle.Text = "Chi tiết đầu sách";
            // 
            // panelImage
            // 
            panelImage.BackColor = Color.FromArgb(245, 245, 245);
            panelImage.BorderStyle = BorderStyle.FixedSingle;
            panelImage.Controls.Add(picBookImage);
            panelImage.Location = new Point(13, 30);
            panelImage.Margin = new Padding(3, 2, 3, 2);
            panelImage.Name = "panelImage";
            panelImage.Size = new Size(105, 113);
            panelImage.TabIndex = 1;
            // 
            // picBookImage
            // 
            picBookImage.BackColor = Color.FromArgb(245, 245, 245);
            picBookImage.Cursor = Cursors.Hand;
            picBookImage.Dock = DockStyle.Fill;
            picBookImage.Location = new Point(0, 0);
            picBookImage.Margin = new Padding(3, 2, 3, 2);
            picBookImage.Name = "picBookImage";
            picBookImage.Size = new Size(103, 111);
            picBookImage.SizeMode = PictureBoxSizeMode.Zoom;
            picBookImage.TabIndex = 0;
            picBookImage.TabStop = false;
            picBookImage.Click += BtnBrowseImage_Click;
            picBookImage.Paint += PicBookImage_Paint;
            // 
            // btnBrowseImage
            // 
            btnBrowseImage.BackColor = Color.FromArgb(59, 130, 246);
            btnBrowseImage.FlatAppearance.BorderSize = 0;
            btnBrowseImage.FlatStyle = FlatStyle.Flat;
            btnBrowseImage.Font = new Font("Segoe UI", 8F);
            btnBrowseImage.ForeColor = Color.White;
            btnBrowseImage.Location = new Point(127, 31);
            btnBrowseImage.Margin = new Padding(3, 2, 3, 2);
            btnBrowseImage.Name = "btnBrowseImage";
            btnBrowseImage.Size = new Size(61, 21);
            btnBrowseImage.TabIndex = 2;
            btnBrowseImage.Text = "Chọn ảnh";
            btnBrowseImage.UseVisualStyleBackColor = false;
            btnBrowseImage.Click += BtnBrowseImage_Click;
            // 
            // btnRemoveImage
            // 
            btnRemoveImage.BackColor = Color.FromArgb(239, 68, 68);
            btnRemoveImage.FlatAppearance.BorderSize = 0;
            btnRemoveImage.FlatStyle = FlatStyle.Flat;
            btnRemoveImage.Font = new Font("Segoe UI", 8F);
            btnRemoveImage.ForeColor = Color.White;
            btnRemoveImage.Location = new Point(206, 31);
            btnRemoveImage.Margin = new Padding(3, 2, 3, 2);
            btnRemoveImage.Name = "btnRemoveImage";
            btnRemoveImage.Size = new Size(57, 21);
            btnRemoveImage.TabIndex = 3;
            btnRemoveImage.Text = "Xóa ảnh";
            btnRemoveImage.UseVisualStyleBackColor = false;
            btnRemoveImage.Click += BtnRemoveImage_Click;
            // 
            // btnViewDetail
            // 
            btnViewDetail.BackColor = Color.FromArgb(37, 99, 235);
            btnViewDetail.FlatAppearance.BorderSize = 0;
            btnViewDetail.FlatStyle = FlatStyle.Flat;
            btnViewDetail.Font = new Font("Segoe UI", 8F);
            btnViewDetail.ForeColor = Color.White;
            btnViewDetail.Location = new Point(127, 66);
            btnViewDetail.Margin = new Padding(3, 2, 3, 2);
            btnViewDetail.Name = "btnViewDetail";
            btnViewDetail.Size = new Size(136, 21);
            btnViewDetail.TabIndex = 4;
            btnViewDetail.Text = "Xem chi tiết";
            btnViewDetail.UseVisualStyleBackColor = false;
            btnViewDetail.Click += BtnViewDetail_Click;
            // 
            // lblISBN
            // 
            lblISBN.AutoSize = true;
            lblISBN.Location = new Point(17, 157);
            lblISBN.Name = "lblISBN";
            lblISBN.Size = new Size(35, 15);
            lblISBN.TabIndex = 5;
            lblISBN.Text = "ISBN:";
            // 
            // txtISBN
            // 
            txtISBN.Location = new Point(77, 154);
            txtISBN.Margin = new Padding(3, 2, 3, 2);
            txtISBN.Name = "txtISBN";
            txtISBN.Size = new Size(210, 23);
            txtISBN.TabIndex = 6;
            // 
            // lblBookTitle
            // 
            lblBookTitle.AutoSize = true;
            lblBookTitle.Location = new Point(17, 184);
            lblBookTitle.Name = "lblBookTitle";
            lblBookTitle.Size = new Size(56, 15);
            lblBookTitle.TabIndex = 7;
            lblBookTitle.Text = "Tên sách:";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(77, 181);
            txtTitle.Margin = new Padding(3, 2, 3, 2);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(210, 23);
            txtTitle.TabIndex = 8;
            // 
            // lblCategoryDetail
            // 
            lblCategoryDetail.AutoSize = true;
            lblCategoryDetail.Location = new Point(16, 211);
            lblCategoryDetail.Name = "lblCategoryDetail";
            lblCategoryDetail.Size = new Size(52, 15);
            lblCategoryDetail.TabIndex = 9;
            lblCategoryDetail.Text = "Thể loại:";
            // 
            // cboCategoryDetail
            // 
            cboCategoryDetail.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoryDetail.Location = new Point(77, 209);
            cboCategoryDetail.Margin = new Padding(3, 2, 3, 2);
            cboCategoryDetail.Name = "cboCategoryDetail";
            cboCategoryDetail.Size = new Size(210, 23);
            cboCategoryDetail.TabIndex = 10;
            // 
            // lblAuthorDetail
            // 
            lblAuthorDetail.AutoSize = true;
            lblAuthorDetail.Location = new Point(16, 239);
            lblAuthorDetail.Name = "lblAuthorDetail";
            lblAuthorDetail.Size = new Size(47, 15);
            lblAuthorDetail.TabIndex = 11;
            lblAuthorDetail.Text = "Tác giả:";
            // 
            // cboAuthorDetail
            // 
            cboAuthorDetail.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAuthorDetail.Location = new Point(77, 236);
            cboAuthorDetail.Margin = new Padding(3, 2, 3, 2);
            cboAuthorDetail.Name = "cboAuthorDetail";
            cboAuthorDetail.Size = new Size(210, 23);
            cboAuthorDetail.TabIndex = 12;
            // 
            // lblPublisher
            // 
            lblPublisher.AutoSize = true;
            lblPublisher.Location = new Point(16, 266);
            lblPublisher.Name = "lblPublisher";
            lblPublisher.Size = new Size(33, 15);
            lblPublisher.TabIndex = 13;
            lblPublisher.Text = "NXB:";
            // 
            // cboPublisher
            // 
            cboPublisher.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPublisher.Location = new Point(77, 263);
            cboPublisher.Margin = new Padding(3, 2, 3, 2);
            cboPublisher.Name = "cboPublisher";
            cboPublisher.Size = new Size(210, 23);
            cboPublisher.TabIndex = 14;
            // 
            // lblYear
            // 
            lblYear.AutoSize = true;
            lblYear.Location = new Point(13, 292);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(53, 15);
            lblYear.TabIndex = 15;
            lblYear.Text = "Năm XB:";
            // 
            // numYear
            // 
            numYear.Location = new Point(77, 290);
            numYear.Margin = new Padding(3, 2, 3, 2);
            numYear.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            numYear.Minimum = new decimal(new int[] { 1900, 0, 0, 0 });
            numYear.Name = "numYear";
            numYear.Size = new Size(70, 23);
            numYear.TabIndex = 16;
            numYear.Value = new decimal(new int[] { 2024, 0, 0, 0 });
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(12, 317);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(27, 15);
            lblPrice.TabIndex = 17;
            lblPrice.Text = "Giá:";
            // 
            // numPrice
            // 
            numPrice.Location = new Point(77, 317);
            numPrice.Margin = new Padding(3, 2, 3, 2);
            numPrice.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(88, 23);
            numPrice.TabIndex = 18;
            numPrice.ThousandsSeparator = true;
            // 
            // lblCopies
            // 
            lblCopies.AutoSize = true;
            lblCopies.Location = new Point(12, 344);
            lblCopies.Name = "lblCopies";
            lblCopies.Size = new Size(57, 15);
            lblCopies.TabIndex = 19;
            lblCopies.Text = "Số lượng:";
            // 
            // numTotalCopies
            // 
            numTotalCopies.Location = new Point(77, 344);
            numTotalCopies.Margin = new Padding(3, 2, 3, 2);
            numTotalCopies.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numTotalCopies.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTotalCopies.Name = "numTotalCopies";
            numTotalCopies.Size = new Size(61, 23);
            numTotalCopies.TabIndex = 20;
            numTotalCopies.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblLocation
            // 
            lblLocation.AutoSize = true;
            lblLocation.Location = new Point(17, 371);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(34, 15);
            lblLocation.TabIndex = 21;
            lblLocation.Text = "Vị trí:";
            // 
            // txtLocation
            // 
            txtLocation.Location = new Point(77, 371);
            txtLocation.Margin = new Padding(3, 2, 3, 2);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(210, 23);
            txtLocation.TabIndex = 22;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(16, 398);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(41, 15);
            lblDescription.TabIndex = 23;
            lblDescription.Text = "Mô tả:";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(77, 398);
            txtDescription.Margin = new Padding(3, 2, 3, 2);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(210, 38);
            txtDescription.TabIndex = 24;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(37, 99, 235);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(13, 440);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(88, 26);
            btnSave.TabIndex = 25;
            btnSave.Text = "💾 Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(107, 114, 128);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(116, 440);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 26);
            btnCancel.TabIndex = 26;
            btnCancel.Text = "❌ Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // FormBookManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 252);
            ClientSize = new Size(1102, 566);
            Controls.Add(lblTitle);
            Controls.Add(panelSearch);
            Controls.Add(dgvBooks);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Controls.Add(panelDetail);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormBookManagement";
            Text = "Quản lý sách";
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
            panelDetail.ResumeLayout(false);
            panelDetail.PerformLayout();
            panelImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picBookImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)numYear).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTotalCopies).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}