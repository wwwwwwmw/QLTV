using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormBorrow
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Panel panelMember;
        private Label lblMemberTitle;
        private Label lblCode;
        private Button btnFindMember;
        private Label lblCurrentBorrow;
        private Panel panelBook;
        private Label lblBookTitle;
        private Label lblSearch;
        private Label lblBarcode;
        private Label lblRecommendations;
        private Label lblDays;
        private Button btnBorrow;

        private TextBox txtMemberCode;
        private Label lblMemberInfo;
        private Label lblMemberStatus;
        private DataGridView dgvBorrowing;
        private TextBox txtBookSearch;
        private TextBox txtBarcode;
        private Button btnScanBarcode;
        private DataGridView dgvBooks;
        private ListBox lstRecommendations;
        private NumericUpDown numDays;

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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            lblTitle = new Label();
            panelMember = new Panel();
            lblMemberTitle = new Label();
            lblCode = new Label();
            txtMemberCode = new TextBox();
            btnFindMember = new Button();
            lblMemberInfo = new Label();
            lblMemberStatus = new Label();
            lblCurrentBorrow = new Label();
            dgvBorrowing = new DataGridView();
            panelBook = new Panel();
            lblBookTitle = new Label();
            lblSearch = new Label();
            txtBookSearch = new TextBox();
            lblBarcode = new Label();
            txtBarcode = new TextBox();
            btnScanBarcode = new Button();
            dgvBooks = new DataGridView();
            lblRecommendations = new Label();
            lstRecommendations = new ListBox();
            lblDays = new Label();
            numDays = new NumericUpDown();
            btnBorrow = new Button();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            panelMember.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBorrowing).BeginInit();
            panelBook.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDays).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblTitle.Location = new Point(25, 18);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(329, 54);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Trạm Mượn Sách";
            // 
            // panelMember
            // 
            panelMember.BackColor = Color.White;
            panelMember.Controls.Add(lblMemberTitle);
            panelMember.Controls.Add(lblCode);
            panelMember.Controls.Add(txtMemberCode);
            panelMember.Controls.Add(btnFindMember);
            panelMember.Controls.Add(lblMemberInfo);
            panelMember.Controls.Add(lblMemberStatus);
            panelMember.Location = new Point(25, 90);
            panelMember.Margin = new Padding(4, 4, 4, 4);
            panelMember.Name = "panelMember";
            panelMember.Padding = new Padding(19, 19, 19, 19);
            panelMember.Size = new Size(650, 275);
            panelMember.TabIndex = 1;
            // 
            // lblMemberTitle
            // 
            lblMemberTitle.AutoSize = true;
            lblMemberTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMemberTitle.Location = new Point(19, 12);
            lblMemberTitle.Margin = new Padding(4, 0, 4, 0);
            lblMemberTitle.Name = "lblMemberTitle";
            lblMemberTitle.Size = new Size(195, 30);
            lblMemberTitle.TabIndex = 0;
            lblMemberTitle.Text = "Thông tin độc giả";
            // 
            // lblCode
            // 
            lblCode.AutoSize = true;
            lblCode.Location = new Point(19, 56);
            lblCode.Margin = new Padding(4, 0, 4, 0);
            lblCode.Name = "lblCode";
            lblCode.Size = new Size(71, 25);
            lblCode.TabIndex = 1;
            lblCode.Text = "Mã thẻ:";
            // 
            // txtMemberCode
            // 
            txtMemberCode.Font = new Font("Segoe UI", 10F);
            txtMemberCode.Location = new Point(100, 52);
            txtMemberCode.Margin = new Padding(4, 4, 4, 4);
            txtMemberCode.Name = "txtMemberCode";
            txtMemberCode.Size = new Size(186, 34);
            txtMemberCode.TabIndex = 2;
            txtMemberCode.KeyPress += TxtMemberCode_KeyPress;
            // 
            // btnFindMember
            // 
            btnFindMember.BackColor = Color.FromArgb(37, 99, 235);
            btnFindMember.FlatAppearance.BorderSize = 0;
            btnFindMember.FlatStyle = FlatStyle.Flat;
            btnFindMember.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFindMember.ForeColor = Color.White;
            btnFindMember.Location = new Point(300, 50);
            btnFindMember.Margin = new Padding(4, 4, 4, 4);
            btnFindMember.Name = "btnFindMember";
            btnFindMember.Size = new Size(120, 40);
            btnFindMember.TabIndex = 3;
            btnFindMember.Text = "Tìm kiếm";
            btnFindMember.UseVisualStyleBackColor = false;
            btnFindMember.Click += BtnFindMember_Click;
            // 
            // lblMemberInfo
            // 
            lblMemberInfo.Font = new Font("Segoe UI", 10F);
            lblMemberInfo.Location = new Point(19, 100);
            lblMemberInfo.Margin = new Padding(4, 0, 4, 0);
            lblMemberInfo.Name = "lblMemberInfo";
            lblMemberInfo.Size = new Size(575, 75);
            lblMemberInfo.TabIndex = 4;
            // 
            // lblMemberStatus
            // 
            lblMemberStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMemberStatus.Location = new Point(19, 181);
            lblMemberStatus.Margin = new Padding(4, 0, 4, 0);
            lblMemberStatus.Name = "lblMemberStatus";
            lblMemberStatus.Size = new Size(575, 31);
            lblMemberStatus.TabIndex = 5;
            // 
            // lblCurrentBorrow
            // 
            lblCurrentBorrow.AutoSize = true;
            lblCurrentBorrow.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCurrentBorrow.Location = new Point(25, 380);
            lblCurrentBorrow.Margin = new Padding(4, 0, 4, 0);
            lblCurrentBorrow.Name = "lblCurrentBorrow";
            lblCurrentBorrow.Size = new Size(177, 28);
            lblCurrentBorrow.TabIndex = 2;
            lblCurrentBorrow.Text = "Sách đang mượn:";
            // 
            // dgvBorrowing
            // 
            dgvBorrowing.AllowUserToAddRows = false;
            dgvBorrowing.AllowUserToDeleteRows = false;
            dgvBorrowing.BackgroundColor = Color.White;
            dgvBorrowing.BorderStyle = BorderStyle.None;
            dgvBorrowing.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBorrowing.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            dgvBorrowing.Location = new Point(25, 418);
            dgvBorrowing.Margin = new Padding(4, 4, 4, 4);
            dgvBorrowing.Name = "dgvBorrowing";
            dgvBorrowing.ReadOnly = true;
            dgvBorrowing.RowHeadersVisible = false;
            dgvBorrowing.RowHeadersWidth = 62;
            dgvBorrowing.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBorrowing.Size = new Size(650, 300);
            dgvBorrowing.TabIndex = 3;
            // 
            // panelBook
            // 
            panelBook.BackColor = Color.White;
            panelBook.Controls.Add(lblBookTitle);
            panelBook.Controls.Add(lblSearch);
            panelBook.Controls.Add(txtBookSearch);
            panelBook.Controls.Add(lblBarcode);
            panelBook.Controls.Add(txtBarcode);
            panelBook.Controls.Add(btnScanBarcode);
            panelBook.Controls.Add(dgvBooks);
            panelBook.Controls.Add(lblRecommendations);
            panelBook.Controls.Add(lstRecommendations);
            panelBook.Controls.Add(lblDays);
            panelBook.Controls.Add(numDays);
            panelBook.Controls.Add(btnBorrow);
            panelBook.Location = new Point(700, 90);
            panelBook.Margin = new Padding(4, 4, 4, 4);
            panelBook.Name = "panelBook";
            panelBook.Padding = new Padding(19, 19, 19, 19);
            panelBook.Size = new Size(825, 628);
            panelBook.TabIndex = 4;
            // 
            // lblBookTitle
            // 
            lblBookTitle.AutoSize = true;
            lblBookTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblBookTitle.Location = new Point(19, 12);
            lblBookTitle.Margin = new Padding(4, 0, 4, 0);
            lblBookTitle.Name = "lblBookTitle";
            lblBookTitle.Size = new Size(263, 30);
            lblBookTitle.TabIndex = 0;
            lblBookTitle.Text = "Chọn đầu sách để mượn";
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(19, 56);
            lblSearch.Margin = new Padding(4, 0, 4, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(85, 25);
            lblSearch.TabIndex = 1;
            lblSearch.Text = "Tìm sách:";
            // 
            // txtBookSearch
            // 
            txtBookSearch.Font = new Font("Segoe UI", 10F);
            txtBookSearch.Location = new Point(100, 52);
            txtBookSearch.Margin = new Padding(4, 4, 4, 4);
            txtBookSearch.Name = "txtBookSearch";
            txtBookSearch.PlaceholderText = "Nhập tên sách hoặc ISBN...";
            txtBookSearch.Size = new Size(352, 34);
            txtBookSearch.TabIndex = 2;
            txtBookSearch.TextChanged += TxtBookSearch_TextChanged;
            // 
            // lblBarcode
            // 
            lblBarcode.AutoSize = true;
            lblBarcode.Location = new Point(468, 56);
            lblBarcode.Margin = new Padding(4, 0, 4, 0);
            lblBarcode.Name = "lblBarcode";
            lblBarcode.Size = new Size(80, 25);
            lblBarcode.TabIndex = 3;
            lblBarcode.Text = "Barcode:";
            // 
            // txtBarcode
            // 
            txtBarcode.Font = new Font("Segoe UI", 10F);
            txtBarcode.Location = new Point(548, 52);
            txtBarcode.Margin = new Padding(4, 4, 4, 4);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "Quét/nhập...";
            txtBarcode.Size = new Size(136, 34);
            txtBarcode.TabIndex = 4;
            txtBarcode.KeyDown += TxtBarcode_KeyDown;
            // 
            // btnScanBarcode
            // 
            btnScanBarcode.BackColor = Color.FromArgb(59, 130, 246);
            btnScanBarcode.FlatAppearance.BorderSize = 0;
            btnScanBarcode.FlatStyle = FlatStyle.Flat;
            btnScanBarcode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnScanBarcode.ForeColor = Color.White;
            btnScanBarcode.Location = new Point(700, 50);
            btnScanBarcode.Margin = new Padding(4, 4, 4, 4);
            btnScanBarcode.Name = "btnScanBarcode";
            btnScanBarcode.Size = new Size(94, 40);
            btnScanBarcode.TabIndex = 5;
            btnScanBarcode.Text = "Quét";
            btnScanBarcode.UseVisualStyleBackColor = false;
            btnScanBarcode.Click += BtnScanBarcode_Click;
            // 
            // dgvBooks
            // 
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.AllowUserToDeleteRows = false;
            dgvBooks.BackgroundColor = Color.White;
            dgvBooks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBooks.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10 });
            dgvBooks.Location = new Point(19, 100);
            dgvBooks.Margin = new Padding(4, 4, 4, 4);
            dgvBooks.Name = "dgvBooks";
            dgvBooks.ReadOnly = true;
            dgvBooks.RowHeadersVisible = false;
            dgvBooks.RowHeadersWidth = 62;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.Size = new Size(775, 295);
            dgvBooks.TabIndex = 6;
            dgvBooks.SelectionChanged += DgvBooks_SelectionChanged;
            // 
            // lblRecommendations
            // 
            lblRecommendations.AutoSize = true;
            lblRecommendations.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRecommendations.Location = new Point(19, 405);
            lblRecommendations.Margin = new Padding(4, 0, 4, 0);
            lblRecommendations.Name = "lblRecommendations";
            lblRecommendations.Size = new Size(148, 28);
            lblRecommendations.TabIndex = 7;
            lblRecommendations.Text = "Gợi ý cho bạn:";
            // 
            // lstRecommendations
            // 
            lstRecommendations.Font = new Font("Segoe UI", 9F);
            lstRecommendations.FormattingEnabled = true;
            lstRecommendations.ItemHeight = 25;
            lstRecommendations.Location = new Point(19, 440);
            lstRecommendations.Margin = new Padding(4, 4, 4, 4);
            lstRecommendations.Name = "lstRecommendations";
            lstRecommendations.Size = new Size(774, 79);
            lstRecommendations.TabIndex = 8;
            lstRecommendations.DoubleClick += LstRecommendations_DoubleClick;
            // 
            // lblDays
            // 
            lblDays.AutoSize = true;
            lblDays.Location = new Point(19, 560);
            lblDays.Margin = new Padding(4, 0, 4, 0);
            lblDays.Name = "lblDays";
            lblDays.Size = new Size(134, 25);
            lblDays.TabIndex = 9;
            lblDays.Text = "Số ngày mượn:";
            // 
            // numDays
            // 
            numDays.Location = new Point(140, 555);
            numDays.Margin = new Padding(4, 4, 4, 4);
            numDays.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            numDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDays.Name = "numDays";
            numDays.Size = new Size(88, 31);
            numDays.TabIndex = 10;
            numDays.Value = new decimal(new int[] { 14, 0, 0, 0 });
            numDays.ValueChanged += NumDays_ValueChanged;
            // 
            // btnBorrow
            // 
            btnBorrow.BackColor = Color.FromArgb(37, 99, 235);
            btnBorrow.FlatAppearance.BorderSize = 0;
            btnBorrow.FlatStyle = FlatStyle.Flat;
            btnBorrow.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnBorrow.ForeColor = Color.White;
            btnBorrow.Location = new Point(260, 548);
            btnBorrow.Margin = new Padding(4, 4, 4, 4);
            btnBorrow.Name = "btnBorrow";
            btnBorrow.Size = new Size(172, 55);
            btnBorrow.TabIndex = 11;
            btnBorrow.Text = "Mượn sách";
            btnBorrow.UseVisualStyleBackColor = false;
            btnBorrow.Click += BtnBorrow_Click;
            // 
            // dataGridViewTextBoxColumn1 - BookTitle (dgvBorrowing)
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Tên sách";
            dataGridViewTextBoxColumn1.MinimumWidth = 8;
            dataGridViewTextBoxColumn1.Name = "BookTitle";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2 - BorrowDate (dgvBorrowing)
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Ngày mượn";
            dataGridViewTextBoxColumn2.MinimumWidth = 8;
            dataGridViewTextBoxColumn2.Name = "BorrowDate";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Width = 120;
            // 
            // dataGridViewTextBoxColumn3 - DueDate (dgvBorrowing)
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Hạn trả";
            dataGridViewTextBoxColumn3.MinimumWidth = 8;
            dataGridViewTextBoxColumn3.Name = "DueDate";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Width = 120;
            // 
            // dataGridViewTextBoxColumn4 - Status (dgvBorrowing)
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Trạng thái";
            dataGridViewTextBoxColumn4.MinimumWidth = 8;
            dataGridViewTextBoxColumn4.Name = "Status";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.Width = 120;
            // 
            // dataGridViewTextBoxColumn5 - BookID (dgvBooks)
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Mã sách";
            dataGridViewTextBoxColumn5.MinimumWidth = 8;
            dataGridViewTextBoxColumn5.Name = "BookID";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Width = 80;
            // 
            // dataGridViewTextBoxColumn6 - ISBN (dgvBooks)
            // 
            dataGridViewTextBoxColumn6.HeaderText = "ISBN";
            dataGridViewTextBoxColumn6.MinimumWidth = 8;
            dataGridViewTextBoxColumn6.Name = "ISBN";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.Width = 130;
            // 
            // dataGridViewTextBoxColumn7 - Title (dgvBooks)
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Tên sách";
            dataGridViewTextBoxColumn7.MinimumWidth = 8;
            dataGridViewTextBoxColumn7.Name = "Title";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            dataGridViewTextBoxColumn7.Width = 200;
            // 
            // dataGridViewTextBoxColumn8 - AuthorName (dgvBooks)
            // 
            dataGridViewTextBoxColumn8.HeaderText = "Tác giả";
            dataGridViewTextBoxColumn8.MinimumWidth = 8;
            dataGridViewTextBoxColumn8.Name = "AuthorName";
            dataGridViewTextBoxColumn8.ReadOnly = true;
            dataGridViewTextBoxColumn8.Width = 150;
            // 
            // dataGridViewTextBoxColumn9 - AvailableCopies (dgvBooks)
            // 
            dataGridViewTextBoxColumn9.HeaderText = "Còn lại";
            dataGridViewTextBoxColumn9.MinimumWidth = 8;
            dataGridViewTextBoxColumn9.Name = "AvailableCopies";
            dataGridViewTextBoxColumn9.ReadOnly = true;
            dataGridViewTextBoxColumn9.Width = 80;
            // 
            // dataGridViewTextBoxColumn10 - Location (dgvBooks)
            // 
            dataGridViewTextBoxColumn10.HeaderText = "Vị trí";
            dataGridViewTextBoxColumn10.MinimumWidth = 8;
            dataGridViewTextBoxColumn10.Name = "Location";
            dataGridViewTextBoxColumn10.ReadOnly = true;
            dataGridViewTextBoxColumn10.Width = 100;
            // 
            // FormBorrow
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 252);
            ClientSize = new Size(1550, 775);
            Controls.Add(lblTitle);
            Controls.Add(panelMember);
            Controls.Add(lblCurrentBorrow);
            Controls.Add(dgvBorrowing);
            Controls.Add(panelBook);
            Margin = new Padding(4, 5, 4, 5);
            Name = "FormBorrow";
            Text = "Mượn sách";
            panelMember.ResumeLayout(false);
            panelMember.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBorrowing).EndInit();
            panelBook.ResumeLayout(false);
            panelBook.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDays).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
    }
}
