using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormMailCenter
    {
        private System.ComponentModel.IContainer? components = null;

        private Panel panelMain = null!;
        private Label lblTitle = null!;
        private Label lblTemplate = null!;
        private ComboBox cboTemplate = null!;
        private Label lblTo = null!;
        private TextBox txtTo = null!;
        private Label lblName = null!;
        private TextBox txtName = null!;
        private Label lblBook = null!;
        private TextBox txtBookTitle = null!;
        private Label lblBorrow = null!;
        private TextBox txtBorrowCode = null!;
        private Label lblDue = null!;
        private DateTimePicker dtpDueDate = null!;
        private Button btnGenerate = null!;
        private Button btnSend = null!;
        private Label lblSubject = null!;
        private TextBox txtSubject = null!;
        private Label lblPreview = null!;
        private RichTextBox rtbPreview = null!;
        private Label lblStatus = null!;

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
            panelMain = new Panel();
            lblTitle = new Label();
            lblTemplate = new Label();
            cboTemplate = new ComboBox();
            lblTo = new Label();
            txtTo = new TextBox();
            lblName = new Label();
            txtName = new TextBox();
            lblBook = new Label();
            txtBookTitle = new TextBox();
            lblBorrow = new Label();
            txtBorrowCode = new TextBox();
            lblDue = new Label();
            dtpDueDate = new DateTimePicker();
            btnGenerate = new Button();
            btnSend = new Button();
            lblSubject = new Label();
            txtSubject = new TextBox();
            lblPreview = new Label();
            rtbPreview = new RichTextBox();
            lblStatus = new Label();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Controls.Add(lblTitle);
            panelMain.Controls.Add(lblTemplate);
            panelMain.Controls.Add(cboTemplate);
            panelMain.Controls.Add(lblTo);
            panelMain.Controls.Add(txtTo);
            panelMain.Controls.Add(lblName);
            panelMain.Controls.Add(txtName);
            panelMain.Controls.Add(lblBook);
            panelMain.Controls.Add(txtBookTitle);
            panelMain.Controls.Add(lblBorrow);
            panelMain.Controls.Add(txtBorrowCode);
            panelMain.Controls.Add(lblDue);
            panelMain.Controls.Add(dtpDueDate);
            panelMain.Controls.Add(btnGenerate);
            panelMain.Controls.Add(btnSend);
            panelMain.Controls.Add(lblSubject);
            panelMain.Controls.Add(txtSubject);
            panelMain.Controls.Add(lblPreview);
            panelMain.Controls.Add(rtbPreview);
            panelMain.Controls.Add(lblStatus);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(16);
            panelMain.Size = new Size(980, 700);
            panelMain.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(310, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Công cụ gửi email tự động";
            // 
            // lblTemplate
            // 
            lblTemplate.AutoSize = true;
            lblTemplate.Location = new Point(12, 60);
            lblTemplate.Name = "lblTemplate";
            lblTemplate.Size = new Size(58, 15);
            lblTemplate.TabIndex = 1;
            lblTemplate.Text = "Mẫu email";
            // 
            // cboTemplate
            // 
            cboTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTemplate.FormattingEnabled = true;
            cboTemplate.Items.AddRange(new object[] { "1. Thông báo đăng ký thành viên thành công", "2. Thông báo mượn sách thành công", "3. Thông báo trễ hạn trả sách", "4. Thông báo trả sách thành công", "5. Thông báo tạm khóa tài khoản", "6. Thông báo đã có sách đặt trước" });
            cboTemplate.Location = new Point(12, 84);
            cboTemplate.Name = "cboTemplate";
            cboTemplate.Size = new Size(430, 23);
            cboTemplate.TabIndex = 2;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(12, 124);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(101, 15);
            lblTo.TabIndex = 3;
            lblTo.Text = "Email người nhận";
            // 
            // txtTo
            // 
            txtTo.Location = new Point(12, 148);
            txtTo.Name = "txtTo";
            txtTo.PlaceholderText = "example@email.com";
            txtTo.Size = new Size(430, 23);
            txtTo.TabIndex = 4;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(12, 186);
            lblName.Name = "lblName";
            lblName.Size = new Size(85, 15);
            lblName.TabIndex = 5;
            lblName.Text = "Tên người nhận";
            // 
            // txtName
            // 
            txtName.Location = new Point(12, 210);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "Nguyễn Văn A";
            txtName.Size = new Size(430, 23);
            txtName.TabIndex = 6;
            // 
            // lblBook
            // 
            lblBook.AutoSize = true;
            lblBook.Location = new Point(12, 248);
            lblBook.Name = "lblBook";
            lblBook.Size = new Size(51, 15);
            lblBook.TabIndex = 7;
            lblBook.Text = "Tên sách";
            // 
            // txtBookTitle
            // 
            txtBookTitle.Location = new Point(12, 272);
            txtBookTitle.Name = "txtBookTitle";
            txtBookTitle.PlaceholderText = "Tên sách";
            txtBookTitle.Size = new Size(430, 23);
            txtBookTitle.TabIndex = 8;
            // 
            // lblBorrow
            // 
            lblBorrow.AutoSize = true;
            lblBorrow.Location = new Point(12, 310);
            lblBorrow.Name = "lblBorrow";
            lblBorrow.Size = new Size(50, 15);
            lblBorrow.TabIndex = 9;
            lblBorrow.Text = "Mã phiếu";
            // 
            // txtBorrowCode
            // 
            txtBorrowCode.Location = new Point(12, 334);
            txtBorrowCode.Name = "txtBorrowCode";
            txtBorrowCode.PlaceholderText = "PM...";
            txtBorrowCode.Size = new Size(430, 23);
            txtBorrowCode.TabIndex = 10;
            // 
            // lblDue
            // 
            lblDue.AutoSize = true;
            lblDue.Location = new Point(12, 372);
            lblDue.Name = "lblDue";
            lblDue.Size = new Size(122, 15);
            lblDue.TabIndex = 11;
            lblDue.Text = "Hạn trả / mốc thời gian";
            // 
            // dtpDueDate
            // 
            dtpDueDate.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpDueDate.Format = DateTimePickerFormat.Custom;
            dtpDueDate.Location = new Point(12, 396);
            dtpDueDate.Name = "dtpDueDate";
            dtpDueDate.Size = new Size(430, 23);
            dtpDueDate.TabIndex = 12;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(12, 444);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(210, 38);
            btnGenerate.TabIndex = 13;
            btnGenerate.Text = "Tạo nội dung";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += BtnGenerate_Click;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(232, 444);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(210, 38);
            btnSend.TabIndex = 14;
            btnSend.Text = "Gửi email";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += BtnSend_Click;
            // 
            // lblSubject
            // 
            lblSubject.AutoSize = true;
            lblSubject.Location = new Point(470, 60);
            lblSubject.Name = "lblSubject";
            lblSubject.Size = new Size(77, 15);
            lblSubject.TabIndex = 15;
            lblSubject.Text = "Tiêu đề email";
            // 
            // txtSubject
            // 
            txtSubject.Location = new Point(470, 84);
            txtSubject.Name = "txtSubject";
            txtSubject.Size = new Size(470, 23);
            txtSubject.TabIndex = 16;
            // 
            // lblPreview
            // 
            lblPreview.AutoSize = true;
            lblPreview.Location = new Point(470, 124);
            lblPreview.Name = "lblPreview";
            lblPreview.Size = new Size(78, 15);
            lblPreview.TabIndex = 17;
            lblPreview.Text = "Nội dung HTML";
            // 
            // rtbPreview
            // 
            rtbPreview.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbPreview.Location = new Point(470, 148);
            rtbPreview.Name = "rtbPreview";
            rtbPreview.Size = new Size(470, 500);
            rtbPreview.TabIndex = 18;
            rtbPreview.Text = "";
            // 
            // lblStatus
            // 
            lblStatus.ForeColor = Color.DarkBlue;
            lblStatus.Location = new Point(12, 498);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(430, 48);
            lblStatus.TabIndex = 19;
            // 
            // FormMailCenter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(980, 700);
            Controls.Add(panelMain);
            Name = "FormMailCenter";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Trung tâm gửi email";
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ResumeLayout(false);
        }
    }
}
