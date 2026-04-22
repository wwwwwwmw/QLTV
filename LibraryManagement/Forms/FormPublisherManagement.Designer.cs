using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormPublisherManagement
    {
        private System.ComponentModel.IContainer? components = null;

        private Panel panelTop = null!;
        private Panel panelButtons = null!;
        private Label lblTitle = null!;
        private Label lblName = null!;
        private Label lblAddress = null!;
        private Label lblPhone = null!;
        private Label lblEmail = null!;
        private Label lblWebsite = null!;
        private TextBox txtPublisherName = null!;
        private TextBox txtAddress = null!;
        private TextBox txtPhone = null!;
        private TextBox txtEmail = null!;
        private TextBox txtWebsite = null!;
        private CheckBox chkShowInactive = null!;
        private Button btnAddNew = null!;
        private Button btnSave = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private DataGridView dgvPublishers = null!;
        private DataGridViewTextBoxColumn colPublisherID = null!;
        private DataGridViewTextBoxColumn colPublisherName = null!;
        private DataGridViewTextBoxColumn colPhone = null!;
        private DataGridViewTextBoxColumn colEmail = null!;
        private DataGridViewTextBoxColumn colWebsite = null!;
        private DataGridViewTextBoxColumn colIsActive = null!;

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
            panelTop = new Panel();
            lblTitle = new Label();
            lblName = new Label();
            txtPublisherName = new TextBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblWebsite = new Label();
            txtWebsite = new TextBox();
            chkShowInactive = new CheckBox();
            panelButtons = new Panel();
            btnAddNew = new Button();
            btnSave = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            dgvPublishers = new DataGridView();
            colPublisherID = new DataGridViewTextBoxColumn();
            colPublisherName = new DataGridViewTextBoxColumn();
            colPhone = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colWebsite = new DataGridViewTextBoxColumn();
            colIsActive = new DataGridViewTextBoxColumn();
            panelTop.SuspendLayout();
            panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPublishers).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(lblName);
            panelTop.Controls.Add(txtPublisherName);
            panelTop.Controls.Add(lblAddress);
            panelTop.Controls.Add(txtAddress);
            panelTop.Controls.Add(lblPhone);
            panelTop.Controls.Add(txtPhone);
            panelTop.Controls.Add(lblEmail);
            panelTop.Controls.Add(txtEmail);
            panelTop.Controls.Add(lblWebsite);
            panelTop.Controls.Add(txtWebsite);
            panelTop.Controls.Add(chkShowInactive);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1280, 146);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(14, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(221, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Nhà xuất bản sách";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(18, 62);
            lblName.Name = "lblName";
            lblName.Size = new Size(86, 15);
            lblName.TabIndex = 1;
            lblName.Text = "Tên NXB:";
            // 
            // txtPublisherName
            // 
            txtPublisherName.Location = new Point(110, 58);
            txtPublisherName.Name = "txtPublisherName";
            txtPublisherName.Size = new Size(240, 23);
            txtPublisherName.TabIndex = 2;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(368, 62);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(46, 15);
            lblAddress.TabIndex = 3;
            lblAddress.Text = "Địa chỉ:";
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(420, 58);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(300, 23);
            txtAddress.TabIndex = 4;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(735, 62);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(64, 15);
            lblPhone.TabIndex = 5;
            lblPhone.Text = "Điện thoại:";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(805, 58);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(170, 23);
            txtPhone.TabIndex = 6;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(18, 98);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 7;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(110, 94);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(240, 23);
            txtEmail.TabIndex = 8;
            // 
            // lblWebsite
            // 
            lblWebsite.AutoSize = true;
            lblWebsite.Location = new Point(368, 98);
            lblWebsite.Name = "lblWebsite";
            lblWebsite.Size = new Size(51, 15);
            lblWebsite.TabIndex = 9;
            lblWebsite.Text = "Website:";
            // 
            // txtWebsite
            // 
            txtWebsite.Location = new Point(420, 94);
            txtWebsite.Name = "txtWebsite";
            txtWebsite.Size = new Size(300, 23);
            txtWebsite.TabIndex = 10;
            // 
            // chkShowInactive
            // 
            chkShowInactive.AutoSize = true;
            chkShowInactive.Location = new Point(735, 96);
            chkShowInactive.Name = "chkShowInactive";
            chkShowInactive.Size = new Size(114, 19);
            chkShowInactive.TabIndex = 11;
            chkShowInactive.Text = "Hiện dữ liệu xóa";
            chkShowInactive.UseVisualStyleBackColor = true;
            chkShowInactive.CheckedChanged += ChkShowInactive_CheckedChanged;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.White;
            panelButtons.Controls.Add(btnAddNew);
            panelButtons.Controls.Add(btnSave);
            panelButtons.Controls.Add(btnDelete);
            panelButtons.Controls.Add(btnRefresh);
            panelButtons.Dock = DockStyle.Top;
            panelButtons.Location = new Point(0, 146);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(1280, 56);
            panelButtons.TabIndex = 1;
            // 
            // btnAddNew
            // 
            btnAddNew.BackColor = Color.FromArgb(37, 99, 235);
            btnAddNew.FlatAppearance.BorderSize = 0;
            btnAddNew.FlatStyle = FlatStyle.Flat;
            btnAddNew.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddNew.ForeColor = Color.White;
            btnAddNew.Location = new Point(14, 10);
            btnAddNew.Name = "btnAddNew";
            btnAddNew.Size = new Size(92, 34);
            btnAddNew.TabIndex = 0;
            btnAddNew.Text = "Thêm mới";
            btnAddNew.UseVisualStyleBackColor = false;
            btnAddNew.Click += BtnAddNew_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(16, 185, 129);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(114, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(76, 34);
            btnSave.TabIndex = 1;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(198, 10);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(76, 34);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(100, 116, 139);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(282, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(88, 34);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Tải lại";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // dgvPublishers
            // 
            dgvPublishers.AllowUserToAddRows = false;
            dgvPublishers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPublishers.BackgroundColor = Color.White;
            dgvPublishers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPublishers.Columns.AddRange(new DataGridViewColumn[] { colPublisherID, colPublisherName, colPhone, colEmail, colWebsite, colIsActive });
            dgvPublishers.Dock = DockStyle.Fill;
            dgvPublishers.Location = new Point(0, 202);
            dgvPublishers.MultiSelect = false;
            dgvPublishers.Name = "dgvPublishers";
            dgvPublishers.ReadOnly = true;
            dgvPublishers.RowHeadersVisible = false;
            dgvPublishers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPublishers.Size = new Size(1280, 518);
            dgvPublishers.TabIndex = 2;
            dgvPublishers.SelectionChanged += DgvPublishers_SelectionChanged;
            // 
            // colPublisherID
            // 
            colPublisherID.HeaderText = "ID";
            colPublisherID.Name = "PublisherID";
            colPublisherID.ReadOnly = true;
            colPublisherID.Visible = false;
            // 
            // colPublisherName
            // 
            colPublisherName.HeaderText = "Nhà xuất bản";
            colPublisherName.Name = "PublisherName";
            colPublisherName.ReadOnly = true;
            // 
            // colPhone
            // 
            colPhone.HeaderText = "Điện thoại";
            colPhone.Name = "Phone";
            colPhone.ReadOnly = true;
            // 
            // colEmail
            // 
            colEmail.HeaderText = "Email";
            colEmail.Name = "Email";
            colEmail.ReadOnly = true;
            // 
            // colWebsite
            // 
            colWebsite.HeaderText = "Website";
            colWebsite.Name = "Website";
            colWebsite.ReadOnly = true;
            // 
            // colIsActive
            // 
            colIsActive.HeaderText = "Trạng thái";
            colIsActive.Name = "IsActive";
            colIsActive.ReadOnly = true;
            // 
            // FormPublisherManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1280, 720);
            Controls.Add(dgvPublishers);
            Controls.Add(panelButtons);
            Controls.Add(panelTop);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormPublisherManagement";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý nhà xuất bản";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPublishers).EndInit();
            ResumeLayout(false);
        }
    }
}
