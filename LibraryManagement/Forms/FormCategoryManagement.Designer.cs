using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormCategoryManagement
    {
        private System.ComponentModel.IContainer? components = null;

        private Panel panelTop = null!;
        private Panel panelButtons = null!;
        private Label lblTitle = null!;
        private Label lblName = null!;
        private Label lblDesc = null!;
        private TextBox txtCategoryName = null!;
        private TextBox txtDescription = null!;
        private CheckBox chkShowInactive = null!;
        private Button btnAddNew = null!;
        private Button btnSave = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private DataGridView dgvCategories = null!;
        private DataGridViewTextBoxColumn colCategoryID = null!;
        private DataGridViewTextBoxColumn colCategoryName = null!;
        private DataGridViewTextBoxColumn colDescription = null!;
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
            txtCategoryName = new TextBox();
            lblDesc = new Label();
            txtDescription = new TextBox();
            chkShowInactive = new CheckBox();
            panelButtons = new Panel();
            btnAddNew = new Button();
            btnSave = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            dgvCategories = new DataGridView();
            colCategoryID = new DataGridViewTextBoxColumn();
            colCategoryName = new DataGridViewTextBoxColumn();
            colDescription = new DataGridViewTextBoxColumn();
            colIsActive = new DataGridViewTextBoxColumn();
            panelTop.SuspendLayout();
            panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(lblName);
            panelTop.Controls.Add(txtCategoryName);
            panelTop.Controls.Add(lblDesc);
            panelTop.Controls.Add(txtDescription);
            panelTop.Controls.Add(chkShowInactive);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1200, 110);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(14, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(197, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Danh mục sách";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(18, 62);
            lblName.Name = "lblName";
            lblName.Size = new Size(91, 15);
            lblName.TabIndex = 1;
            lblName.Text = "Tên danh mục:";
            // 
            // txtCategoryName
            // 
            txtCategoryName.Location = new Point(112, 58);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(240, 23);
            txtCategoryName.TabIndex = 2;
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(368, 62);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(38, 15);
            lblDesc.TabIndex = 3;
            lblDesc.Text = "Mô tả:";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(414, 58);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(360, 23);
            txtDescription.TabIndex = 4;
            // 
            // chkShowInactive
            // 
            chkShowInactive.AutoSize = true;
            chkShowInactive.Location = new Point(790, 61);
            chkShowInactive.Name = "chkShowInactive";
            chkShowInactive.Size = new Size(145, 19);
            chkShowInactive.TabIndex = 5;
            chkShowInactive.Text = "Hiện danh mục đã xóa";
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
            panelButtons.Location = new Point(0, 110);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(1200, 56);
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
            // dgvCategories
            // 
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategories.Columns.AddRange(new DataGridViewColumn[] { colCategoryID, colCategoryName, colDescription, colIsActive });
            dgvCategories.Dock = DockStyle.Fill;
            dgvCategories.Location = new Point(0, 166);
            dgvCategories.MultiSelect = false;
            dgvCategories.Name = "dgvCategories";
            dgvCategories.ReadOnly = true;
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.Size = new Size(1200, 554);
            dgvCategories.TabIndex = 2;
            dgvCategories.SelectionChanged += DgvCategories_SelectionChanged;
            // 
            // colCategoryID
            // 
            colCategoryID.HeaderText = "ID";
            colCategoryID.Name = "CategoryID";
            colCategoryID.ReadOnly = true;
            // 
            // colCategoryName
            // 
            colCategoryName.HeaderText = "Tên danh mục";
            colCategoryName.Name = "CategoryName";
            colCategoryName.ReadOnly = true;
            // 
            // colDescription
            // 
            colDescription.HeaderText = "Mô tả";
            colDescription.Name = "Description";
            colDescription.ReadOnly = true;
            // 
            // colIsActive
            // 
            colIsActive.HeaderText = "Trạng thái";
            colIsActive.Name = "IsActive";
            colIsActive.ReadOnly = true;
            // 
            // FormCategoryManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1200, 720);
            Controls.Add(dgvCategories);
            Controls.Add(panelButtons);
            Controls.Add(panelTop);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormCategoryManagement";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý danh mục";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            ResumeLayout(false);
        }
    }
}
