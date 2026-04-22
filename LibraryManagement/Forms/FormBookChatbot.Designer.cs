using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormBookChatbot
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle = null!;
        private RichTextBox rtbChat = null!;
        private TextBox txtMessage = null!;
        private Button btnSend = null!;
        private Button btnClear = null!;
        private Label lblHint = null!;

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
            lblTitle = new Label();
            rtbChat = new RichTextBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            btnClear = new Button();
            lblHint = new Label();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblTitle.Location = new Point(20, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(276, 46);
            lblTitle.Text = "Chatbot Sách AI";

            // lblHint
            lblHint.AutoSize = true;
            lblHint.Font = new Font("Segoe UI", 9F);
            lblHint.ForeColor = Color.DimGray;
            lblHint.Location = new Point(20, 64);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(516, 20);
            lblHint.Text = "Gợi ý: 'Tôi cần sách truyền cảm hứng', 'Tìm sách tâm lý cho người mới bắt đầu'";

            // rtbChat
            rtbChat.BackColor = Color.White;
            rtbChat.BorderStyle = BorderStyle.FixedSingle;
            rtbChat.Font = new Font("Segoe UI", 10F);
            rtbChat.Location = new Point(20, 96);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.Size = new Size(980, 454);
            rtbChat.TabIndex = 0;
            rtbChat.Text = "";

            // txtMessage
            txtMessage.Font = new Font("Segoe UI", 10F);
            txtMessage.Location = new Point(20, 562);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.PlaceholderText = "Nhập mô tả sách bạn muốn tìm hoặc nhu cầu để được đề xuất...";
            txtMessage.Size = new Size(830, 70);
            txtMessage.TabIndex = 1;
            txtMessage.KeyDown += TxtMessage_KeyDown;

            // btnSend
            btnSend.BackColor = Color.FromArgb(37, 99, 235);
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(865, 562);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(135, 33);
            btnSend.TabIndex = 2;
            btnSend.Text = "Gửi";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += BtnSend_Click;

            // btnClear
            btnClear.BackColor = Color.FromArgb(107, 114, 128);
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(865, 600);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(135, 33);
            btnClear.TabIndex = 3;
            btnClear.Text = "Xóa chat";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += BtnClear_Click;

            // FormBookChatbot
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 252);
            ClientSize = new Size(1020, 646);
            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(rtbChat);
            Controls.Add(txtMessage);
            Controls.Add(btnSend);
            Controls.Add(btnClear);
            Name = "FormBookChatbot";
            Text = "Chatbot Sách AI";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
