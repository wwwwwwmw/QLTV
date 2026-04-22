using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormManagerWorkspace
    {
        private System.ComponentModel.IContainer? components = null;

        private Panel panelHeader = null!;
        private Label lblTitle = null!;
        private Label lblSub = null!;
        private FlowLayoutPanel flowPanel = null!;

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
            panelHeader = new Panel();
            lblTitle = new Label();
            lblSub = new Label();
            flowPanel = new FlowLayoutPanel();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(15, 23, 42);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblSub);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1080, 120);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 24);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(301, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Không Gian Quản Lý";
            // 
            // lblSub
            // 
            lblSub.AutoSize = true;
            lblSub.Font = new Font("Segoe UI", 10F);
            lblSub.ForeColor = Color.FromArgb(191, 219, 254);
            lblSub.Location = new Point(26, 72);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(458, 19);
            lblSub.TabIndex = 1;
            lblSub.Text = "Truy cập nhanh các công cụ điều hành dành cho Manager/Admin";
            // 
            // flowPanel
            // 
            flowPanel.AutoScroll = true;
            flowPanel.BackColor = Color.Transparent;
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.Location = new Point(0, 120);
            flowPanel.Name = "flowPanel";
            flowPanel.Padding = new Padding(20);
            flowPanel.Size = new Size(1080, 560);
            flowPanel.TabIndex = 1;
            flowPanel.WrapContents = true;
            // 
            // FormManagerWorkspace
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1080, 680);
            Controls.Add(flowPanel);
            Controls.Add(panelHeader);
            MinimumSize = new Size(900, 560);
            Name = "FormManagerWorkspace";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Không gian quản lý";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
        }
    }
}
