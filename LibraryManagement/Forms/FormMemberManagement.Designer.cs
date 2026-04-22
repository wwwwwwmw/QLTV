using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormMemberManagement
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle = null!;
        private Panel panelSearch = null!;
        private Label lblType = null!;
        private Button btnAdd = null!;
        private Button btnEdit = null!;
        private Button btnDelete = null!;
        private Button btnHistory = null!;
        private Button btnPayFine = null!;
        private Button btnRefresh = null!;
        private Panel panelDetail = null!;
        private Label lblDetailTitle = null!;
        private Label lblMemberCode = null!;
        private Label lblFullName = null!;
        private Label lblGender = null!;
        private Label lblDateOfBirth = null!;
        private Label lblPhone = null!;
        private Label lblEmail = null!;
        private Label lblIdentityCard = null!;
        private Label lblAddress = null!;
        private Label lblMemberTypeDetail = null!;
        private Label lblExpiryDate = null!;
        private Label lblFineLabel = null!;
        private Label lblNotes = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private DataGridViewTextBoxColumn colMemberId = null!;
        private DataGridViewTextBoxColumn colMemberCode = null!;
        private DataGridViewTextBoxColumn colFullName = null!;
        private DataGridViewTextBoxColumn colGender = null!;
        private DataGridViewTextBoxColumn colPhone = null!;
        private DataGridViewTextBoxColumn colMemberType = null!;
        private DataGridViewTextBoxColumn colExpiryDate = null!;
        private DataGridViewTextBoxColumn colTotalFine = null!;
        private DataGridViewTextBoxColumn colStatus = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            lblTitle = new Label();
            panelSearch = new Panel();
            chkActiveOnly = new CheckBox();
            cboMemberType = new ComboBox();
            lblType = new Label();
            txtSearch = new TextBox();
            dgvMembers = new DataGridView();
            colMemberId = new DataGridViewTextBoxColumn();
            colMemberCode = new DataGridViewTextBoxColumn();
            colFullName = new DataGridViewTextBoxColumn();
            colGender = new DataGridViewTextBoxColumn();
            colPhone = new DataGridViewTextBoxColumn();
            colMemberType = new DataGridViewTextBoxColumn();
            colExpiryDate = new DataGridViewTextBoxColumn();
            colTotalFine = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnHistory = new Button();
            btnPayFine = new Button();
            btnRefresh = new Button();
            panelDetail = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            txtNotes = new TextBox();
            lblNotes = new Label();
            lblTotalFine = new Label();
            lblFineLabel = new Label();
            dtpExpiryDate = new DateTimePicker();
            lblExpiryDate = new Label();
            cboMemberTypeDetail = new ComboBox();
            lblMemberTypeDetail = new Label();
            txtAddress = new TextBox();
            lblAddress = new Label();
            txtIdentityCard = new TextBox();
            lblIdentityCard = new Label();
            txtEmail = new TextBox();
            lblEmail = new Label();
            txtPhone = new TextBox();
            lblPhone = new Label();
            dtpDateOfBirth = new DateTimePicker();
            lblDateOfBirth = new Label();
            cboGender = new ComboBox();
            lblGender = new Label();
            txtFullName = new TextBox();
            lblFullName = new Label();
            txtMemberCode = new TextBox();
            lblMemberCode = new Label();
            lblDetailTitle = new Label();
            panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMembers).BeginInit();
            panelDetail.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblTitle.Location = new Point(20, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(207, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Quản lý độc giả";
            // 
            // panelSearch
            // 
            panelSearch.BackColor = Color.White;
            panelSearch.Controls.Add(chkActiveOnly);
            panelSearch.Controls.Add(cboMemberType);
            panelSearch.Controls.Add(lblType);
            panelSearch.Controls.Add(txtSearch);
            panelSearch.Location = new Point(20, 66);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(800, 64);
            panelSearch.TabIndex = 1;
            // 
            // chkActiveOnly
            // 
            chkActiveOnly.AutoSize = true;
            chkActiveOnly.Checked = true;
            chkActiveOnly.CheckState = CheckState.Checked;
            chkActiveOnly.Location = new Point(500, 22);
            chkActiveOnly.Name = "chkActiveOnly";
            chkActiveOnly.Size = new Size(132, 19);
            chkActiveOnly.TabIndex = 3;
            chkActiveOnly.Text = "Chỉ đang hoạt động";
            chkActiveOnly.UseVisualStyleBackColor = true;
            chkActiveOnly.CheckedChanged += ChkActiveOnly_CheckedChanged;
            // 
            // cboMemberType
            // 
            cboMemberType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMemberType.FormattingEnabled = true;
            cboMemberType.Items.AddRange(new object[] { "-- Tất cả --", "Thường", "VIP", "Sinh viên", "Giáo viên" });
            cboMemberType.Location = new Point(330, 20);
            cboMemberType.Name = "cboMemberType";
            cboMemberType.Size = new Size(150, 23);
            cboMemberType.TabIndex = 2;
            cboMemberType.SelectedIndexChanged += CboMemberType_SelectedIndexChanged;
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Location = new Point(270, 23);
            lblType.Name = "lblType";
            lblType.Size = new Size(52, 15);
            lblType.TabIndex = 1;
            lblType.Text = "Loại thẻ:";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(12, 19);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm theo mã, tên, SĐT...";
            txtSearch.Size = new Size(250, 25);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // dgvMembers
            // 
            dgvMembers.AllowUserToAddRows = false;
            dgvMembers.AllowUserToDeleteRows = false;
            dgvMembers.BackgroundColor = Color.White;
            dgvMembers.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(37, 99, 235);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvMembers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvMembers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMembers.Columns.AddRange(new DataGridViewColumn[] { colMemberId, colMemberCode, colFullName, colGender, colPhone, colMemberType, colExpiryDate, colTotalFine, colStatus });
            dgvMembers.Location = new Point(20, 142);
            dgvMembers.Name = "dgvMembers";
            dgvMembers.ReadOnly = true;
            dgvMembers.RowHeadersVisible = false;
            dgvMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMembers.Size = new Size(800, 340);
            dgvMembers.TabIndex = 2;
            dgvMembers.CellDoubleClick += DgvMembers_CellDoubleClick;
            dgvMembers.SelectionChanged += DgvMembers_SelectionChanged;
            // 
            // colMemberId
            // 
            colMemberId.HeaderText = "ID";
            colMemberId.Name = "MemberID";
            colMemberId.ReadOnly = true;
            colMemberId.Visible = false;
            // 
            // colMemberCode
            // 
            colMemberCode.HeaderText = "Mã thẻ";
            colMemberCode.Name = "colMemberCode";
            colMemberCode.ReadOnly = true;
            colMemberCode.Width = 70;
            // 
            // colFullName
            // 
            colFullName.HeaderText = "Họ tên";
            colFullName.Name = "colFullName";
            colFullName.ReadOnly = true;
            colFullName.Width = 140;
            // 
            // colGender
            // 
            colGender.HeaderText = "Giới tính";
            colGender.Name = "colGender";
            colGender.ReadOnly = true;
            colGender.Width = 75;
            // 
            // colPhone
            // 
            colPhone.HeaderText = "Điện thoại";
            colPhone.Name = "colPhone";
            colPhone.ReadOnly = true;
            // 
            // colMemberType
            // 
            colMemberType.HeaderText = "Loại thẻ";
            colMemberType.Name = "colMemberType";
            colMemberType.ReadOnly = true;
            colMemberType.Width = 85;
            // 
            // colExpiryDate
            // 
            colExpiryDate.HeaderText = "Hạn thẻ";
            colExpiryDate.Name = "colExpiryDate";
            colExpiryDate.ReadOnly = true;
            colExpiryDate.Width = 95;
            // 
            // colTotalFine
            // 
            colTotalFine.HeaderText = "Nợ phạt";
            colTotalFine.Name = "colTotalFine";
            colTotalFine.ReadOnly = true;
            colTotalFine.Width = 80;
            // 
            // colStatus
            // 
            colStatus.HeaderText = "Trạng thái";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 85;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(37, 99, 235);
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(20, 492);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(116, 38);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Thêm mới";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += BtnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(59, 130, 246);
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(144, 492);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(90, 38);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(242, 492);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 38);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnHistory
            // 
            btnHistory.BackColor = Color.FromArgb(99, 102, 241);
            btnHistory.Cursor = Cursors.Hand;
            btnHistory.FlatAppearance.BorderSize = 0;
            btnHistory.FlatStyle = FlatStyle.Flat;
            btnHistory.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnHistory.ForeColor = Color.White;
            btnHistory.Location = new Point(340, 492);
            btnHistory.Name = "btnHistory";
            btnHistory.Size = new Size(120, 38);
            btnHistory.TabIndex = 6;
            btnHistory.Text = "Lịch sử mượn";
            btnHistory.UseVisualStyleBackColor = false;
            btnHistory.Click += BtnHistory_Click;
            // 
            // btnPayFine
            // 
            btnPayFine.BackColor = Color.FromArgb(245, 158, 11);
            btnPayFine.Cursor = Cursors.Hand;
            btnPayFine.FlatAppearance.BorderSize = 0;
            btnPayFine.FlatStyle = FlatStyle.Flat;
            btnPayFine.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPayFine.ForeColor = Color.White;
            btnPayFine.Location = new Point(468, 492);
            btnPayFine.Name = "btnPayFine";
            btnPayFine.Size = new Size(106, 38);
            btnPayFine.TabIndex = 7;
            btnPayFine.Text = "Đóng phạt";
            btnPayFine.UseVisualStyleBackColor = false;
            btnPayFine.Click += BtnPayFine_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(107, 114, 128);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(582, 492);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(112, 38);
            btnRefresh.TabIndex = 8;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // panelDetail
            // 
            panelDetail.BackColor = Color.White;
            panelDetail.Controls.Add(btnCancel);
            panelDetail.Controls.Add(btnSave);
            panelDetail.Controls.Add(txtNotes);
            panelDetail.Controls.Add(lblNotes);
            panelDetail.Controls.Add(lblTotalFine);
            panelDetail.Controls.Add(lblFineLabel);
            panelDetail.Controls.Add(dtpExpiryDate);
            panelDetail.Controls.Add(lblExpiryDate);
            panelDetail.Controls.Add(cboMemberTypeDetail);
            panelDetail.Controls.Add(lblMemberTypeDetail);
            panelDetail.Controls.Add(txtAddress);
            panelDetail.Controls.Add(lblAddress);
            panelDetail.Controls.Add(txtIdentityCard);
            panelDetail.Controls.Add(lblIdentityCard);
            panelDetail.Controls.Add(txtEmail);
            panelDetail.Controls.Add(lblEmail);
            panelDetail.Controls.Add(txtPhone);
            panelDetail.Controls.Add(lblPhone);
            panelDetail.Controls.Add(dtpDateOfBirth);
            panelDetail.Controls.Add(lblDateOfBirth);
            panelDetail.Controls.Add(cboGender);
            panelDetail.Controls.Add(lblGender);
            panelDetail.Controls.Add(txtFullName);
            panelDetail.Controls.Add(lblFullName);
            panelDetail.Controls.Add(txtMemberCode);
            panelDetail.Controls.Add(lblMemberCode);
            panelDetail.Controls.Add(lblDetailTitle);
            panelDetail.Location = new Point(840, 66);
            panelDetail.Name = "panelDetail";
            panelDetail.Padding = new Padding(15);
            panelDetail.Size = new Size(380, 493);
            panelDetail.TabIndex = 9;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(107, 114, 128);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(121, 441);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 34);
            btnCancel.TabIndex = 26;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(37, 99, 235);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(15, 441);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 34);
            btnSave.TabIndex = 25;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // txtNotes
            // 
            txtNotes.Font = new Font("Segoe UI", 9F);
            txtNotes.Location = new Point(105, 397);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(250, 38);
            txtNotes.TabIndex = 24;
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.Font = new Font("Segoe UI", 9F);
            lblNotes.Location = new Point(10, 397);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(51, 15);
            lblNotes.TabIndex = 23;
            lblNotes.Text = "Ghi chú:";
            // 
            // lblTotalFine
            // 
            lblTotalFine.AutoSize = true;
            lblTotalFine.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalFine.ForeColor = Color.FromArgb(192, 57, 43);
            lblTotalFine.Location = new Point(105, 363);
            lblTotalFine.Name = "lblTotalFine";
            lblTotalFine.Size = new Size(30, 19);
            lblTotalFine.TabIndex = 22;
            lblTotalFine.Text = "0 đ";
            // 
            // lblFineLabel
            // 
            lblFineLabel.AutoSize = true;
            lblFineLabel.Font = new Font("Segoe UI", 9F);
            lblFineLabel.Location = new Point(15, 363);
            lblFineLabel.Name = "lblFineLabel";
            lblFineLabel.Size = new Size(53, 15);
            lblFineLabel.TabIndex = 21;
            lblFineLabel.Text = "Nợ phạt:";
            // 
            // dtpExpiryDate
            // 
            dtpExpiryDate.Format = DateTimePickerFormat.Short;
            dtpExpiryDate.Location = new Point(105, 337);
            dtpExpiryDate.Name = "dtpExpiryDate";
            dtpExpiryDate.Size = new Size(150, 23);
            dtpExpiryDate.TabIndex = 20;
            // 
            // lblExpiryDate
            // 
            lblExpiryDate.AutoSize = true;
            lblExpiryDate.Font = new Font("Segoe UI", 9F);
            lblExpiryDate.Location = new Point(15, 337);
            lblExpiryDate.Name = "lblExpiryDate";
            lblExpiryDate.Size = new Size(52, 15);
            lblExpiryDate.TabIndex = 19;
            lblExpiryDate.Text = "Hạn thẻ:";
            // 
            // cboMemberTypeDetail
            // 
            cboMemberTypeDetail.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMemberTypeDetail.FormattingEnabled = true;
            cboMemberTypeDetail.Items.AddRange(new object[] { "Thường", "VIP", "Sinh viên", "Giáo viên" });
            cboMemberTypeDetail.Location = new Point(105, 308);
            cboMemberTypeDetail.Name = "cboMemberTypeDetail";
            cboMemberTypeDetail.Size = new Size(150, 23);
            cboMemberTypeDetail.TabIndex = 18;
            // 
            // lblMemberTypeDetail
            // 
            lblMemberTypeDetail.AutoSize = true;
            lblMemberTypeDetail.Font = new Font("Segoe UI", 9F);
            lblMemberTypeDetail.Location = new Point(15, 308);
            lblMemberTypeDetail.Name = "lblMemberTypeDetail";
            lblMemberTypeDetail.Size = new Size(52, 15);
            lblMemberTypeDetail.TabIndex = 17;
            lblMemberTypeDetail.Text = "Loại thẻ:";
            // 
            // txtAddress
            // 
            txtAddress.Font = new Font("Segoe UI", 9F);
            txtAddress.Location = new Point(105, 269);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(250, 23);
            txtAddress.TabIndex = 16;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Font = new Font("Segoe UI", 9F);
            lblAddress.Location = new Point(15, 272);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(46, 15);
            lblAddress.TabIndex = 15;
            lblAddress.Text = "Địa chỉ:";
            // 
            // txtIdentityCard
            // 
            txtIdentityCard.Font = new Font("Segoe UI", 9F);
            txtIdentityCard.Location = new Point(105, 237);
            txtIdentityCard.Name = "txtIdentityCard";
            txtIdentityCard.Size = new Size(150, 23);
            txtIdentityCard.TabIndex = 14;
            // 
            // lblIdentityCard
            // 
            lblIdentityCard.AutoSize = true;
            lblIdentityCard.Font = new Font("Segoe UI", 9F);
            lblIdentityCard.Location = new Point(15, 240);
            lblIdentityCard.Name = "lblIdentityCard";
            lblIdentityCard.Size = new Size(42, 15);
            lblIdentityCard.TabIndex = 13;
            lblIdentityCard.Text = "CCCD:";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.Location = new Point(105, 205);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(250, 23);
            txtEmail.TabIndex = 12;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9F);
            lblEmail.Location = new Point(15, 208);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 11;
            lblEmail.Text = "Email:";
            // 
            // txtPhone
            // 
            txtPhone.Font = new Font("Segoe UI", 9F);
            txtPhone.Location = new Point(105, 173);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(150, 23);
            txtPhone.TabIndex = 10;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Font = new Font("Segoe UI", 9F);
            lblPhone.Location = new Point(15, 176);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(64, 15);
            lblPhone.TabIndex = 9;
            lblPhone.Text = "Điện thoại:";
            // 
            // dtpDateOfBirth
            // 
            dtpDateOfBirth.Format = DateTimePickerFormat.Short;
            dtpDateOfBirth.Location = new Point(105, 141);
            dtpDateOfBirth.Name = "dtpDateOfBirth";
            dtpDateOfBirth.Size = new Size(150, 23);
            dtpDateOfBirth.TabIndex = 8;
            // 
            // lblDateOfBirth
            // 
            lblDateOfBirth.AutoSize = true;
            lblDateOfBirth.Font = new Font("Segoe UI", 9F);
            lblDateOfBirth.Location = new Point(15, 144);
            lblDateOfBirth.Name = "lblDateOfBirth";
            lblDateOfBirth.Size = new Size(63, 15);
            lblDateOfBirth.TabIndex = 7;
            lblDateOfBirth.Text = "Ngày sinh:";
            // 
            // cboGender
            // 
            cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGender.FormattingEnabled = true;
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cboGender.Location = new Point(105, 109);
            cboGender.Name = "cboGender";
            cboGender.Size = new Size(100, 23);
            cboGender.TabIndex = 6;
            // 
            // lblGender
            // 
            lblGender.AutoSize = true;
            lblGender.Font = new Font("Segoe UI", 9F);
            lblGender.Location = new Point(15, 112);
            lblGender.Name = "lblGender";
            lblGender.Size = new Size(55, 15);
            lblGender.TabIndex = 5;
            lblGender.Text = "Giới tính:";
            // 
            // txtFullName
            // 
            txtFullName.Font = new Font("Segoe UI", 9F);
            txtFullName.Location = new Point(105, 77);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(250, 23);
            txtFullName.TabIndex = 4;
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Font = new Font("Segoe UI", 9F);
            lblFullName.Location = new Point(15, 80);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(46, 15);
            lblFullName.TabIndex = 3;
            lblFullName.Text = "Họ tên:";
            // 
            // txtMemberCode
            // 
            txtMemberCode.BackColor = Color.FromArgb(245, 245, 245);
            txtMemberCode.Font = new Font("Segoe UI", 9F);
            txtMemberCode.Location = new Point(105, 45);
            txtMemberCode.Name = "txtMemberCode";
            txtMemberCode.ReadOnly = true;
            txtMemberCode.Size = new Size(250, 23);
            txtMemberCode.TabIndex = 2;
            // 
            // lblMemberCode
            // 
            lblMemberCode.AutoSize = true;
            lblMemberCode.Font = new Font("Segoe UI", 9F);
            lblMemberCode.Location = new Point(15, 48);
            lblMemberCode.Name = "lblMemberCode";
            lblMemberCode.Size = new Size(47, 15);
            lblMemberCode.TabIndex = 1;
            lblMemberCode.Text = "Mã thẻ:";
            // 
            // lblDetailTitle
            // 
            lblDetailTitle.AutoSize = true;
            lblDetailTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDetailTitle.Location = new Point(15, 10);
            lblDetailTitle.Name = "lblDetailTitle";
            lblDetailTitle.Size = new Size(144, 21);
            lblDetailTitle.TabIndex = 0;
            lblDetailTitle.Text = "Thông tin độc giả";
            // 
            // FormMemberManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 252);
            ClientSize = new Size(1240, 571);
            Controls.Add(panelDetail);
            Controls.Add(btnRefresh);
            Controls.Add(btnPayFine);
            Controls.Add(btnHistory);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(dgvMembers);
            Controls.Add(panelSearch);
            Controls.Add(lblTitle);
            DoubleBuffered = true;
            Name = "FormMemberManagement";
            Text = "Quản lý độc giả";
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMembers).EndInit();
            panelDetail.ResumeLayout(false);
            panelDetail.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dgvMembers;
        private TextBox txtSearch;
        private ComboBox cboMemberType;
        private CheckBox chkActiveOnly;
        private TextBox txtMemberCode;
        private TextBox txtFullName;
        private ComboBox cboGender;
        private DateTimePicker dtpDateOfBirth;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtIdentityCard;
        private TextBox txtAddress;
        private ComboBox cboMemberTypeDetail;
        private DateTimePicker dtpExpiryDate;
        private TextBox txtNotes;
        private Label lblTotalFine;
    }
}
