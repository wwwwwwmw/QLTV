using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormRenewBook
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblBook = null!;
        private Label lblMember = null!;
        private Label lblCurrentDue = null!;
        private Label lblDays = null!;
        private Label lblNewDue = null!;
        private NumericUpDown numDays = null!;
        private Button btnRenew = null!;
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
            this.lblBook = new Label();
            this.lblMember = new Label();
            this.lblCurrentDue = new Label();
            this.lblDays = new Label();
            this.lblNewDue = new Label();
            this.numDays = new NumericUpDown();
            this.btnRenew = new Button();
            this.btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)this.numDays).BeginInit();
            this.SuspendLayout();
            // 
            // lblBook
            // 
            this.lblBook.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            this.lblBook.Location = new Point(20, 20);
            this.lblBook.Name = "lblBook";
            this.lblBook.Size = new Size(350, 25);
            this.lblBook.TabIndex = 0;
            this.lblBook.Text = "Sách:";
            // 
            // lblMember
            // 
            this.lblMember.AutoSize = true;
            this.lblMember.Location = new Point(20, 52);
            this.lblMember.Name = "lblMember";
            this.lblMember.Size = new Size(48, 15);
            this.lblMember.TabIndex = 1;
            this.lblMember.Text = "Độc giả:";
            // 
            // lblCurrentDue
            // 
            this.lblCurrentDue.AutoSize = true;
            this.lblCurrentDue.Location = new Point(20, 82);
            this.lblCurrentDue.Name = "lblCurrentDue";
            this.lblCurrentDue.Size = new Size(104, 15);
            this.lblCurrentDue.TabIndex = 2;
            this.lblCurrentDue.Text = "Hạn trả hiện tại: --";
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = true;
            this.lblDays.Location = new Point(20, 122);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new Size(117, 15);
            this.lblDays.TabIndex = 3;
            this.lblDays.Text = "Gia hạn thêm (ngày):";
            // 
            // lblNewDue
            // 
            this.lblNewDue.AutoSize = true;
            this.lblNewDue.Font = new Font("Segoe UI", 10F);
            this.lblNewDue.Location = new Point(20, 155);
            this.lblNewDue.Name = "lblNewDue";
            this.lblNewDue.Size = new Size(89, 19);
            this.lblNewDue.TabIndex = 5;
            this.lblNewDue.Text = "Hạn trả mới:";
            // 
            // numDays
            // 
            this.numDays.Location = new Point(160, 120);
            this.numDays.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDays.Name = "numDays";
            this.numDays.Size = new Size(80, 23);
            this.numDays.TabIndex = 4;
            this.numDays.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numDays.ValueChanged += this.NumDays_ValueChanged;
            // 
            // btnRenew
            // 
            this.btnRenew.BackColor = Color.FromArgb(37, 99, 235);
            this.btnRenew.FlatAppearance.BorderSize = 0;
            this.btnRenew.FlatStyle = FlatStyle.Flat;
            this.btnRenew.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnRenew.ForeColor = Color.White;
            this.btnRenew.Location = new Point(80, 195);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new Size(100, 35);
            this.btnRenew.TabIndex = 6;
            this.btnRenew.Text = "Gia hạn";
            this.btnRenew.UseVisualStyleBackColor = false;
            this.btnRenew.Click += this.BtnRenew_Click;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = Color.FromArgb(107, 114, 128);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(200, 195);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 35);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += this.BtnCancelRenew_Click;
            // 
            // FormRenewBook
            // 
            this.AcceptButton = this.btnRenew;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(246, 248, 252);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(400, 280);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRenew);
            this.Controls.Add(this.lblNewDue);
            this.Controls.Add(this.numDays);
            this.Controls.Add(this.lblDays);
            this.Controls.Add(this.lblCurrentDue);
            this.Controls.Add(this.lblMember);
            this.Controls.Add(this.lblBook);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRenewBook";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Gia hạn sách";
            ((System.ComponentModel.ISupportInitialize)this.numDays).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
