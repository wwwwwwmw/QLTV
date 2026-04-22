using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormReservationQueue
    {
        private System.ComponentModel.IContainer? components = null;

        private Panel panelTop = null!;
        private Label lblTitle = null!;
        private TextBox txtSearch = null!;
        private ComboBox cboStatus = null!;
        private Button btnRefresh = null!;
        private Button btnNotify = null!;
        private Button btnFulfill = null!;
        private Button btnCancel = null!;
        private Label lblSummary = null!;
        private DataGridView dgvQueue = null!;
        private DataGridViewTextBoxColumn colReservationID = null!;
        private DataGridViewTextBoxColumn colMemberCode = null!;
        private DataGridViewTextBoxColumn colMemberName = null!;
        private DataGridViewTextBoxColumn colMemberEmail = null!;
        private DataGridViewTextBoxColumn colBookTitle = null!;
        private DataGridViewTextBoxColumn colISBN = null!;
        private DataGridViewTextBoxColumn colReservationDate = null!;
        private DataGridViewTextBoxColumn colExpiryDate = null!;
        private DataGridViewTextBoxColumn colStatus = null!;

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
            txtSearch = new TextBox();
            cboStatus = new ComboBox();
            btnRefresh = new Button();
            btnNotify = new Button();
            btnFulfill = new Button();
            btnCancel = new Button();
            lblSummary = new Label();
            dgvQueue = new DataGridView();
            colReservationID = new DataGridViewTextBoxColumn();
            colMemberCode = new DataGridViewTextBoxColumn();
            colMemberName = new DataGridViewTextBoxColumn();
            colMemberEmail = new DataGridViewTextBoxColumn();
            colBookTitle = new DataGridViewTextBoxColumn();
            colISBN = new DataGridViewTextBoxColumn();
            colReservationDate = new DataGridViewTextBoxColumn();
            colExpiryDate = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvQueue).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(txtSearch);
            panelTop.Controls.Add(cboStatus);
            panelTop.Controls.Add(btnRefresh);
            panelTop.Controls.Add(btnNotify);
            panelTop.Controls.Add(btnFulfill);
            panelTop.Controls.Add(btnCancel);
            panelTop.Controls.Add(lblSummary);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(12);
            panelTop.Size = new Size(1200, 120);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(313, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Quản lý hàng chờ đặt trước";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 56);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm theo mã thẻ, tên độc giả, email, tên sách, ISBN...";
            txtSearch.Size = new Size(380, 23);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // cboStatus
            // 
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.FormattingEnabled = true;
            cboStatus.Location = new Point(408, 56);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(180, 23);
            cboStatus.TabIndex = 2;
            cboStatus.SelectedIndexChanged += CboStatus_SelectedIndexChanged;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(604, 54);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 34);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnNotify
            // 
            btnNotify.Location = new Point(734, 54);
            btnNotify.Name = "btnNotify";
            btnNotify.Size = new Size(120, 34);
            btnNotify.TabIndex = 4;
            btnNotify.Text = "Gửi email";
            btnNotify.UseVisualStyleBackColor = true;
            btnNotify.Click += BtnNotify_Click;
            // 
            // btnFulfill
            // 
            btnFulfill.Location = new Point(864, 54);
            btnFulfill.Name = "btnFulfill";
            btnFulfill.Size = new Size(150, 34);
            btnFulfill.TabIndex = 5;
            btnFulfill.Text = "Đánh dấu đã nhận";
            btnFulfill.UseVisualStyleBackColor = true;
            btnFulfill.Click += BtnFulfill_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(1024, 54);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(140, 34);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Hủy đặt trước";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Location = new Point(12, 90);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(0, 15);
            lblSummary.TabIndex = 7;
            // 
            // dgvQueue
            // 
            dgvQueue.AllowUserToAddRows = false;
            dgvQueue.AllowUserToDeleteRows = false;
            dgvQueue.AutoGenerateColumns = false;
            dgvQueue.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQueue.Columns.AddRange(new DataGridViewColumn[] { colReservationID, colMemberCode, colMemberName, colMemberEmail, colBookTitle, colISBN, colReservationDate, colExpiryDate, colStatus });
            dgvQueue.Dock = DockStyle.Fill;
            dgvQueue.Location = new Point(0, 120);
            dgvQueue.MultiSelect = false;
            dgvQueue.Name = "dgvQueue";
            dgvQueue.ReadOnly = true;
            dgvQueue.RowHeadersVisible = false;
            dgvQueue.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvQueue.Size = new Size(1200, 640);
            dgvQueue.TabIndex = 1;
            // 
            // colReservationID
            // 
            colReservationID.DataPropertyName = "ReservationID";
            colReservationID.HeaderText = "ID";
            colReservationID.Name = "ReservationID";
            colReservationID.ReadOnly = true;
            colReservationID.Width = 70;
            // 
            // colMemberCode
            // 
            colMemberCode.DataPropertyName = "MemberCode";
            colMemberCode.HeaderText = "Mã thẻ";
            colMemberCode.Name = "MemberCode";
            colMemberCode.ReadOnly = true;
            // 
            // colMemberName
            // 
            colMemberName.DataPropertyName = "MemberName";
            colMemberName.HeaderText = "Độc giả";
            colMemberName.Name = "MemberName";
            colMemberName.ReadOnly = true;
            colMemberName.Width = 180;
            // 
            // colMemberEmail
            // 
            colMemberEmail.DataPropertyName = "MemberEmail";
            colMemberEmail.HeaderText = "Email";
            colMemberEmail.Name = "MemberEmail";
            colMemberEmail.ReadOnly = true;
            colMemberEmail.Width = 220;
            // 
            // colBookTitle
            // 
            colBookTitle.DataPropertyName = "BookTitle";
            colBookTitle.HeaderText = "Tên sách";
            colBookTitle.Name = "BookTitle";
            colBookTitle.ReadOnly = true;
            colBookTitle.Width = 250;
            // 
            // colISBN
            // 
            colISBN.DataPropertyName = "ISBN";
            colISBN.HeaderText = "ISBN";
            colISBN.Name = "ISBN";
            colISBN.ReadOnly = true;
            colISBN.Width = 130;
            // 
            // colReservationDate
            // 
            colReservationDate.DataPropertyName = "ReservationDate";
            colReservationDate.HeaderText = "Ngày đặt";
            colReservationDate.Name = "ReservationDate";
            colReservationDate.ReadOnly = true;
            colReservationDate.Width = 130;
            // 
            // colExpiryDate
            // 
            colExpiryDate.DataPropertyName = "ExpiryDate";
            colExpiryDate.HeaderText = "Hết hạn";
            colExpiryDate.Name = "ExpiryDate";
            colExpiryDate.ReadOnly = true;
            colExpiryDate.Width = 130;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Trạng thái";
            colStatus.Name = "Status";
            colStatus.ReadOnly = true;
            colStatus.Width = 120;
            // 
            // FormReservationQueue
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 760);
            Controls.Add(dgvQueue);
            Controls.Add(panelTop);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormReservationQueue";
            Text = "Quản lý hàng chờ đặt trước";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvQueue).EndInit();
            ResumeLayout(false);
        }
    }
}
