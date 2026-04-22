using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormBookDetailPublic
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelLeft = null!;
        private PictureBox picBook = null!;
        private Label lblNoImage = null!;
        private Label lblStatus = null!;
        private Label lblQuantity = null!;
        private Panel panelRight = null!;
        private Label lblBookTitle = null!;
        private Label lblISBNLabel = null!;
        private Label lblISBNValue = null!;
        private Label lblAuthorLabel = null!;
        private Label lblAuthorValue = null!;
        private Label lblCategoryLabel = null!;
        private Label lblCategoryValue = null!;
        private Label lblPublisherLabel = null!;
        private Label lblPublisherValue = null!;
        private Label lblPublishYearLabel = null!;
        private Label lblPublishYearValue = null!;
        private Label lblLocationLabel = null!;
        private Label lblLocationValue = null!;
        private Label lblPriceLabel = null!;
        private Label lblPriceValue = null!;
        private Label lblDescriptionTitle = null!;
        private TextBox txtDescription = null!;
        private Label lblFeeNote = null!;
        private Panel panelBottom = null!;
        private Button btnBorrow = null!;
        private Button btnClose = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                picBook.Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelLeft = new Panel();
            this.picBook = new PictureBox();
            this.lblNoImage = new Label();
            this.lblStatus = new Label();
            this.lblQuantity = new Label();
            this.panelRight = new Panel();
            this.lblBookTitle = new Label();
            this.lblISBNLabel = new Label();
            this.lblISBNValue = new Label();
            this.lblAuthorLabel = new Label();
            this.lblAuthorValue = new Label();
            this.lblCategoryLabel = new Label();
            this.lblCategoryValue = new Label();
            this.lblPublisherLabel = new Label();
            this.lblPublisherValue = new Label();
            this.lblPublishYearLabel = new Label();
            this.lblPublishYearValue = new Label();
            this.lblLocationLabel = new Label();
            this.lblLocationValue = new Label();
            this.lblPriceLabel = new Label();
            this.lblPriceValue = new Label();
            this.lblDescriptionTitle = new Label();
            this.txtDescription = new TextBox();
            this.lblFeeNote = new Label();
            this.panelBottom = new Panel();
            this.btnBorrow = new Button();
            this.btnClose = new Button();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.picBook).BeginInit();
            this.panelRight.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = Color.FromArgb(248, 249, 250);
            this.panelLeft.Controls.Add(this.picBook);
            this.panelLeft.Controls.Add(this.lblStatus);
            this.panelLeft.Controls.Add(this.lblQuantity);
            this.panelLeft.Location = new Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new Size(200, 420);
            this.panelLeft.TabIndex = 0;
            // 
            // picBook
            // 
            this.picBook.BackColor = Color.White;
            this.picBook.BorderStyle = BorderStyle.FixedSingle;
            this.picBook.Controls.Add(this.lblNoImage);
            this.picBook.Location = new Point(15, 20);
            this.picBook.Name = "picBook";
            this.picBook.Size = new Size(170, 220);
            this.picBook.SizeMode = PictureBoxSizeMode.Zoom;
            this.picBook.TabIndex = 0;
            this.picBook.TabStop = false;
            // 
            // lblNoImage
            // 
            this.lblNoImage.Dock = DockStyle.Fill;
            this.lblNoImage.Font = new Font("Segoe UI", 40F);
            this.lblNoImage.Location = new Point(0, 0);
            this.lblNoImage.Name = "lblNoImage";
            this.lblNoImage.Size = new Size(168, 218);
            this.lblNoImage.TabIndex = 0;
            this.lblNoImage.Text = "📖";
            this.lblNoImage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblStatus.ForeColor = Color.FromArgb(46, 204, 113);
            this.lblStatus.Location = new Point(15, 250);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(84, 20);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "✅ Còn sách";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblQuantity.Location = new Point(15, 275);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new Size(134, 15);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Số lượng: 0/0 bản";
            // 
            // panelRight
            // 
            this.panelRight.AutoScroll = true;
            this.panelRight.BackColor = Color.White;
            this.panelRight.Controls.Add(this.lblBookTitle);
            this.panelRight.Controls.Add(this.lblISBNLabel);
            this.panelRight.Controls.Add(this.lblISBNValue);
            this.panelRight.Controls.Add(this.lblAuthorLabel);
            this.panelRight.Controls.Add(this.lblAuthorValue);
            this.panelRight.Controls.Add(this.lblCategoryLabel);
            this.panelRight.Controls.Add(this.lblCategoryValue);
            this.panelRight.Controls.Add(this.lblPublisherLabel);
            this.panelRight.Controls.Add(this.lblPublisherValue);
            this.panelRight.Controls.Add(this.lblPublishYearLabel);
            this.panelRight.Controls.Add(this.lblPublishYearValue);
            this.panelRight.Controls.Add(this.lblLocationLabel);
            this.panelRight.Controls.Add(this.lblLocationValue);
            this.panelRight.Controls.Add(this.lblPriceLabel);
            this.panelRight.Controls.Add(this.lblPriceValue);
            this.panelRight.Controls.Add(this.lblDescriptionTitle);
            this.panelRight.Controls.Add(this.txtDescription);
            this.panelRight.Controls.Add(this.lblFeeNote);
            this.panelRight.Location = new Point(200, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new Size(485, 360);
            this.panelRight.TabIndex = 1;
            // 
            // lblBookTitle
            // 
            this.lblBookTitle.AutoSize = true;
            this.lblBookTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblBookTitle.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblBookTitle.Location = new Point(10, 15);
            this.lblBookTitle.MaximumSize = new Size(450, 50);
            this.lblBookTitle.Name = "lblBookTitle";
            this.lblBookTitle.Size = new Size(111, 25);
            this.lblBookTitle.TabIndex = 0;
            this.lblBookTitle.Text = "Tên sách";
            // 
            // lblISBNLabel
            // 
            this.lblISBNLabel.ForeColor = Color.Gray;
            this.lblISBNLabel.Location = new Point(10, 60);
            this.lblISBNLabel.Name = "lblISBNLabel";
            this.lblISBNLabel.Size = new Size(90, 22);
            this.lblISBNLabel.TabIndex = 1;
            this.lblISBNLabel.Text = "🔢 ISBN:";
            // 
            // lblISBNValue
            // 
            this.lblISBNValue.AutoSize = true;
            this.lblISBNValue.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblISBNValue.Location = new Point(100, 63);
            this.lblISBNValue.MaximumSize = new Size(350, 30);
            this.lblISBNValue.Name = "lblISBNValue";
            this.lblISBNValue.Size = new Size(29, 15);
            this.lblISBNValue.TabIndex = 2;
            this.lblISBNValue.Text = "N/A";
            // 
            // lblAuthorLabel
            // 
            this.lblAuthorLabel.ForeColor = Color.Gray;
            this.lblAuthorLabel.Location = new Point(10, 85);
            this.lblAuthorLabel.Name = "lblAuthorLabel";
            this.lblAuthorLabel.Size = new Size(90, 22);
            this.lblAuthorLabel.TabIndex = 3;
            this.lblAuthorLabel.Text = "✍️ Tác giả:";
            // 
            // lblAuthorValue
            // 
            this.lblAuthorValue.AutoSize = true;
            this.lblAuthorValue.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblAuthorValue.Location = new Point(100, 88);
            this.lblAuthorValue.MaximumSize = new Size(350, 30);
            this.lblAuthorValue.Name = "lblAuthorValue";
            this.lblAuthorValue.Size = new Size(52, 15);
            this.lblAuthorValue.TabIndex = 4;
            this.lblAuthorValue.Text = "Chưa rõ";
            // 
            // lblCategoryLabel
            // 
            this.lblCategoryLabel.ForeColor = Color.Gray;
            this.lblCategoryLabel.Location = new Point(10, 110);
            this.lblCategoryLabel.Name = "lblCategoryLabel";
            this.lblCategoryLabel.Size = new Size(90, 22);
            this.lblCategoryLabel.TabIndex = 5;
            this.lblCategoryLabel.Text = "📁 Thể loại:";
            // 
            // lblCategoryValue
            // 
            this.lblCategoryValue.AutoSize = true;
            this.lblCategoryValue.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblCategoryValue.Location = new Point(100, 113);
            this.lblCategoryValue.MaximumSize = new Size(350, 30);
            this.lblCategoryValue.Name = "lblCategoryValue";
            this.lblCategoryValue.Size = new Size(87, 15);
            this.lblCategoryValue.TabIndex = 6;
            this.lblCategoryValue.Text = "Chưa phân loại";
            // 
            // lblPublisherLabel
            // 
            this.lblPublisherLabel.ForeColor = Color.Gray;
            this.lblPublisherLabel.Location = new Point(10, 135);
            this.lblPublisherLabel.Name = "lblPublisherLabel";
            this.lblPublisherLabel.Size = new Size(90, 22);
            this.lblPublisherLabel.TabIndex = 7;
            this.lblPublisherLabel.Text = "🏢 NXB:";
            // 
            // lblPublisherValue
            // 
            this.lblPublisherValue.AutoSize = true;
            this.lblPublisherValue.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblPublisherValue.Location = new Point(100, 138);
            this.lblPublisherValue.MaximumSize = new Size(350, 30);
            this.lblPublisherValue.Name = "lblPublisherValue";
            this.lblPublisherValue.Size = new Size(29, 15);
            this.lblPublisherValue.TabIndex = 8;
            this.lblPublisherValue.Text = "N/A";
            // 
            // lblPublishYearLabel
            // 
            this.lblPublishYearLabel.ForeColor = Color.Gray;
            this.lblPublishYearLabel.Location = new Point(10, 160);
            this.lblPublishYearLabel.Name = "lblPublishYearLabel";
            this.lblPublishYearLabel.Size = new Size(90, 22);
            this.lblPublishYearLabel.TabIndex = 9;
            this.lblPublishYearLabel.Text = "📅 Năm XB:";
            // 
            // lblPublishYearValue
            // 
            this.lblPublishYearValue.AutoSize = true;
            this.lblPublishYearValue.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblPublishYearValue.Location = new Point(100, 163);
            this.lblPublishYearValue.MaximumSize = new Size(350, 30);
            this.lblPublishYearValue.Name = "lblPublishYearValue";
            this.lblPublishYearValue.Size = new Size(29, 15);
            this.lblPublishYearValue.TabIndex = 10;
            this.lblPublishYearValue.Text = "N/A";
            // 
            // lblLocationLabel
            // 
            this.lblLocationLabel.ForeColor = Color.Gray;
            this.lblLocationLabel.Location = new Point(10, 185);
            this.lblLocationLabel.Name = "lblLocationLabel";
            this.lblLocationLabel.Size = new Size(90, 22);
            this.lblLocationLabel.TabIndex = 11;
            this.lblLocationLabel.Text = "📍 Vị trí:";
            // 
            // lblLocationValue
            // 
            this.lblLocationValue.AutoSize = true;
            this.lblLocationValue.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblLocationValue.Location = new Point(100, 188);
            this.lblLocationValue.MaximumSize = new Size(350, 30);
            this.lblLocationValue.Name = "lblLocationValue";
            this.lblLocationValue.Size = new Size(29, 15);
            this.lblLocationValue.TabIndex = 12;
            this.lblLocationValue.Text = "N/A";
            // 
            // lblPriceLabel
            // 
            this.lblPriceLabel.ForeColor = Color.Gray;
            this.lblPriceLabel.Location = new Point(10, 210);
            this.lblPriceLabel.Name = "lblPriceLabel";
            this.lblPriceLabel.Size = new Size(90, 22);
            this.lblPriceLabel.TabIndex = 13;
            this.lblPriceLabel.Text = "💰 Giá trị:";
            // 
            // lblPriceValue
            // 
            this.lblPriceValue.AutoSize = true;
            this.lblPriceValue.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblPriceValue.Location = new Point(100, 213);
            this.lblPriceValue.MaximumSize = new Size(350, 30);
            this.lblPriceValue.Name = "lblPriceValue";
            this.lblPriceValue.Size = new Size(29, 15);
            this.lblPriceValue.TabIndex = 14;
            this.lblPriceValue.Text = "N/A";
            // 
            // lblDescriptionTitle
            // 
            this.lblDescriptionTitle.AutoSize = true;
            this.lblDescriptionTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblDescriptionTitle.Location = new Point(10, 245);
            this.lblDescriptionTitle.Name = "lblDescriptionTitle";
            this.lblDescriptionTitle.Size = new Size(73, 19);
            this.lblDescriptionTitle.TabIndex = 15;
            this.lblDescriptionTitle.Text = "📝 Mô tả:";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = Color.FromArgb(250, 250, 250);
            this.txtDescription.Location = new Point(10, 267);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.Size = new Size(450, 60);
            this.txtDescription.TabIndex = 16;
            this.txtDescription.Text = "Chưa có mô tả";
            // 
            // lblFeeNote
            // 
            this.lblFeeNote.AutoSize = true;
            this.lblFeeNote.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            this.lblFeeNote.ForeColor = Color.FromArgb(41, 128, 185);
            this.lblFeeNote.Location = new Point(10, 337);
            this.lblFeeNote.Name = "lblFeeNote";
            this.lblFeeNote.Size = new Size(234, 13);
            this.lblFeeNote.TabIndex = 17;
            this.lblFeeNote.Text = "💡 Mượn sách MIỄN PHÍ | Chỉ thu phí khi trả trễ";
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = Color.FromArgb(248, 249, 250);
            this.panelBottom.Controls.Add(this.btnBorrow);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Location = new Point(200, 360);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new Size(485, 60);
            this.panelBottom.TabIndex = 2;
            // 
            // btnBorrow
            // 
            this.btnBorrow.BackColor = Color.FromArgb(46, 204, 113);
            this.btnBorrow.FlatAppearance.BorderSize = 0;
            this.btnBorrow.FlatStyle = FlatStyle.Flat;
            this.btnBorrow.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnBorrow.ForeColor = Color.White;
            this.btnBorrow.Location = new Point(15, 10);
            this.btnBorrow.Name = "btnBorrow";
            this.btnBorrow.Size = new Size(220, 40);
            this.btnBorrow.TabIndex = 0;
            this.btnBorrow.Text = "Đăng nhập để mượn sách";
            this.btnBorrow.UseVisualStyleBackColor = false;
            this.btnBorrow.Click += this.BtnBorrow_Click;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = Color.FromArgb(52, 152, 219);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(245, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(100, 40);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += this.BtnClose_Click;
            // 
            // FormBookDetailPublic
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(685, 420);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Text = "Chi tiết sách";
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBookDetailPublic";
            this.StartPosition = FormStartPosition.CenterParent;
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.picBook).EndInit();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
