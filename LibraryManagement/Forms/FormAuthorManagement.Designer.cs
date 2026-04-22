using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormAuthorManagement
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle = null!;
        private Panel panelSearch = null!;
        private CheckBox chkShowInactive = null!;
        private DataGridView dgvAuthors = null!;
        private Button btnAddNew = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;

        private Panel panelDetail = null!;
        private Label lblDetailTitle = null!;
        private Label lblAuthorName = null!;
        private TextBox txtAuthorName = null!;
        private Label lblCountry = null!;
        private TextBox txtCountry = null!;
        private Label lblBiography = null!;
        private TextBox txtBiography = null!;
        private Button btnSave = null!;

        private DataGridViewTextBoxColumn colAuthorId = null!;
        private DataGridViewTextBoxColumn colAuthorName = null!;
        private DataGridViewTextBoxColumn colCountry = null!;
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
            chkShowInactive = new CheckBox();
            dgvAuthors = new DataGridView();
            colAuthorId = new DataGridViewTextBoxColumn();
            colAuthorName = new DataGridViewTextBoxColumn();
            colCountry = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            btnAddNew = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            panelDetail = new Panel();
            lblDetailTitle = new Label();
            lblAuthorName = new Label();
            txtAuthorName = new TextBox();
            lblCountry = new Label();
            txtCountry = new TextBox();
            lblBiography = new Label();
            txtBiography = new TextBox();
            btnSave = new Button();
            panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuthors).BeginInit();
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
            lblTitle.Size = new Size(217, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Quan ly tac gia";
            // 
            // panelSearch
            // 
            panelSearch.BackColor = Color.White;
            panelSearch.Controls.Add(chkShowInactive);
            panelSearch.Location = new Point(20, 66);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(800, 64);
            panelSearch.TabIndex = 1;
            // 
            // chkShowInactive
            // 
            chkShowInactive.AutoSize = true;
            chkShowInactive.Location = new Point(12, 22);
            chkShowInactive.Name = "chkShowInactive";
            chkShowInactive.Size = new Size(146, 19);
            chkShowInactive.TabIndex = 0;
            chkShowInactive.Text = "Hien thi da ngung dung";
            chkShowInactive.UseVisualStyleBackColor = true;
            chkShowInactive.CheckedChanged += ChkShowInactive_CheckedChanged;
            // 
            // dgvAuthors
            // 
            dgvAuthors.AllowUserToAddRows = false;
            dgvAuthors.AllowUserToDeleteRows = false;
            dgvAuthors.BackgroundColor = Color.White;
            dgvAuthors.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(37, 99, 235);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvAuthors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvAuthors.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAuthors.Columns.AddRange(new DataGridViewColumn[] { colAuthorId, colAuthorName, colCountry, colStatus });
            dgvAuthors.Location = new Point(20, 142);
            dgvAuthors.Name = "dgvAuthors";
            dgvAuthors.ReadOnly = true;
            dgvAuthors.RowHeadersVisible = false;
            dgvAuthors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuthors.Size = new Size(800, 340);
            dgvAuthors.TabIndex = 2;
            dgvAuthors.SelectionChanged += DgvAuthors_SelectionChanged;
            // 
            // colAuthorId
            // 
            colAuthorId.HeaderText = "ID";
            colAuthorId.Name = "AuthorID";
            colAuthorId.ReadOnly = true;
            colAuthorId.Visible = false;
            // 
            // colAuthorName
            // 
            colAuthorName.HeaderText = "Ten tac gia";
            colAuthorName.Name = "colAuthorName";
            colAuthorName.ReadOnly = true;
            colAuthorName.Width = 320;
            // 
            // colCountry
            // 
            colCountry.HeaderText = "Quoc gia";
            colCountry.Name = "colCountry";
            colCountry.ReadOnly = true;
            colCountry.Width = 220;
            // 
            // colStatus
            // 
            colStatus.HeaderText = "Trang thai";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 140;
            // 
            // btnAddNew
            // 
            btnAddNew.BackColor = Color.FromArgb(37, 99, 235);
            btnAddNew.Cursor = Cursors.Hand;
            btnAddNew.FlatAppearance.BorderSize = 0;
            btnAddNew.FlatStyle = FlatStyle.Flat;
            btnAddNew.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddNew.ForeColor = Color.White;
            btnAddNew.Location = new Point(20, 492);
            btnAddNew.Name = "btnAddNew";
            btnAddNew.Size = new Size(116, 38);
            btnAddNew.TabIndex = 3;
            btnAddNew.Text = "Them moi";
            btnAddNew.UseVisualStyleBackColor = false;
            btnAddNew.Click += BtnAddNew_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(144, 492);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 38);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Xoa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(107, 114, 128);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(242, 492);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(112, 38);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Lam moi";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // panelDetail
            // 
            panelDetail.BackColor = Color.White;
            panelDetail.Controls.Add(btnSave);
            panelDetail.Controls.Add(txtBiography);
            panelDetail.Controls.Add(lblBiography);
            panelDetail.Controls.Add(txtCountry);
            panelDetail.Controls.Add(lblCountry);
            panelDetail.Controls.Add(txtAuthorName);
            panelDetail.Controls.Add(lblAuthorName);
            panelDetail.Controls.Add(lblDetailTitle);
            panelDetail.Location = new Point(840, 66);
            panelDetail.Name = "panelDetail";
            panelDetail.Padding = new Padding(15);
            panelDetail.Size = new Size(380, 464);
            panelDetail.TabIndex = 6;
            // 
            // lblDetailTitle
            // 
            lblDetailTitle.AutoSize = true;
            lblDetailTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDetailTitle.Location = new Point(15, 10);
            lblDetailTitle.Name = "lblDetailTitle";
            lblDetailTitle.Size = new Size(141, 21);
            lblDetailTitle.TabIndex = 0;
            lblDetailTitle.Text = "Thong tin tac gia";
            // 
            // lblAuthorName
            // 
            lblAuthorName.AutoSize = true;
            lblAuthorName.Font = new Font("Segoe UI", 9F);
            lblAuthorName.Location = new Point(15, 52);
            lblAuthorName.Name = "lblAuthorName";
            lblAuthorName.Size = new Size(62, 15);
            lblAuthorName.TabIndex = 1;
            lblAuthorName.Text = "Ten tac gia:";
            // 
            // txtAuthorName
            // 
            txtAuthorName.Font = new Font("Segoe UI", 9F);
            txtAuthorName.Location = new Point(105, 49);
            txtAuthorName.Name = "txtAuthorName";
            txtAuthorName.Size = new Size(250, 23);
            txtAuthorName.TabIndex = 2;
            // 
            // lblCountry
            // 
            lblCountry.AutoSize = true;
            lblCountry.Font = new Font("Segoe UI", 9F);
            lblCountry.Location = new Point(15, 85);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new Size(56, 15);
            lblCountry.TabIndex = 3;
            lblCountry.Text = "Quoc gia:";
            // 
            // txtCountry
            // 
            txtCountry.Font = new Font("Segoe UI", 9F);
            txtCountry.Location = new Point(105, 82);
            txtCountry.Name = "txtCountry";
            txtCountry.Size = new Size(250, 23);
            txtCountry.TabIndex = 4;
            // 
            // lblBiography
            // 
            lblBiography.AutoSize = true;
            lblBiography.Font = new Font("Segoe UI", 9F);
            lblBiography.Location = new Point(15, 118);
            lblBiography.Name = "lblBiography";
            lblBiography.Size = new Size(43, 15);
            lblBiography.TabIndex = 5;
            lblBiography.Text = "Tieu su:";
            // 
            // txtBiography
            // 
            txtBiography.Font = new Font("Segoe UI", 9F);
            txtBiography.Location = new Point(105, 115);
            txtBiography.Multiline = true;
            txtBiography.Name = "txtBiography";
            txtBiography.Size = new Size(250, 260);
            txtBiography.TabIndex = 6;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(37, 99, 235);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(15, 390);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(110, 34);
            btnSave.TabIndex = 7;
            btnSave.Text = "Luu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // FormAuthorManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 252);
            ClientSize = new Size(1240, 571);
            Controls.Add(panelDetail);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnAddNew);
            Controls.Add(dgvAuthors);
            Controls.Add(panelSearch);
            Controls.Add(lblTitle);
            Name = "FormAuthorManagement";
            Text = "Quan ly tac gia";
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuthors).EndInit();
            panelDetail.ResumeLayout(false);
            panelDetail.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
