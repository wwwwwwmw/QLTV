using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormPayFine
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMember = null!;
        private Label lblCurrentFine = null!;
        private Label lblAmount = null!;
        private Label lblMethod = null!;
        private Label lblNotes = null!;
        private NumericUpDown numAmount = null!;
        private ComboBox cboMethod = null!;
        private TextBox txtNotes = null!;
        private Button btnPay = null!;
        private Button btnCancel = null!;

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
            this.lblMember = new Label();
            this.lblCurrentFine = new Label();
            this.lblAmount = new Label();
            this.lblMethod = new Label();
            this.lblNotes = new Label();
            this.numAmount = new NumericUpDown();
            this.cboMethod = new ComboBox();
            this.txtNotes = new TextBox();
            this.btnPay = new Button();
            this.btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)this.numAmount).BeginInit();
            this.SuspendLayout();
            // 
            // lblMember
            // 
            this.lblMember.AutoSize = true;
            this.lblMember.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            this.lblMember.Location = new Point(20, 20);
            this.lblMember.Name = "lblMember";
            this.lblMember.Size = new Size(66, 19);
            this.lblMember.TabIndex = 0;
            this.lblMember.Text = "Độc giả:";
            // 
            // lblCurrentFine
            // 
            this.lblCurrentFine.AutoSize = true;
            this.lblCurrentFine.ForeColor = Color.FromArgb(192, 57, 43);
            this.lblCurrentFine.Location = new Point(20, 52);
            this.lblCurrentFine.Name = "lblCurrentFine";
            this.lblCurrentFine.Size = new Size(81, 15);
            this.lblCurrentFine.TabIndex = 1;
            this.lblCurrentFine.Text = "Số tiền nợ: 0";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new Point(20, 92);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Size(78, 15);
            this.lblAmount.TabIndex = 2;
            this.lblAmount.Text = "Số tiền đóng:";
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new Point(20, 127);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new Size(59, 15);
            this.lblMethod.TabIndex = 4;
            this.lblMethod.Text = "Hình thức:";
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new Point(20, 162);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(50, 15);
            this.lblNotes.TabIndex = 6;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // numAmount
            // 
            this.numAmount.Location = new Point(120, 90);
            this.numAmount.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numAmount.Name = "numAmount";
            this.numAmount.Size = new Size(150, 23);
            this.numAmount.TabIndex = 3;
            this.numAmount.ThousandsSeparator = true;
            // 
            // cboMethod
            // 
            this.cboMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Items.AddRange(new object[] {
            "Tiền mặt",
            "Chuyển khoản"});
            this.cboMethod.Location = new Point(120, 124);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.SelectedIndex = 0;
            this.cboMethod.Size = new Size(150, 23);
            this.cboMethod.TabIndex = 5;
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new Point(120, 159);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(250, 52);
            this.txtNotes.TabIndex = 7;
            // 
            // btnPay
            // 
            this.btnPay.BackColor = Color.FromArgb(37, 99, 235);
            this.btnPay.FlatAppearance.BorderSize = 0;
            this.btnPay.FlatStyle = FlatStyle.Flat;
            this.btnPay.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnPay.ForeColor = Color.White;
            this.btnPay.Location = new Point(120, 225);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new Size(110, 35);
            this.btnPay.TabIndex = 8;
            this.btnPay.Text = "Thanh toán";
            this.btnPay.UseVisualStyleBackColor = false;
            this.btnPay.Click += this.BtnPay_Click;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = Color.FromArgb(107, 114, 128);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(240, 225);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 35);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += this.BtnCancelPayFine_Click;
            // 
            // FormPayFine
            // 
            this.AcceptButton = this.btnPay;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(246, 248, 252);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(400, 280);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPay);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.cboMethod);
            this.Controls.Add(this.numAmount);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblCurrentFine);
            this.Controls.Add(this.lblMember);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPayFine";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Đóng tiền phạt";
            ((System.ComponentModel.ISupportInitialize)this.numAmount).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
