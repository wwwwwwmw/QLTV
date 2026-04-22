using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormBarcodeScanner
    {
        private System.ComponentModel.IContainer? components = null;

        private ComboBox _cboCamera = null!;
        private Button _btnStart = null!;
        private Button _btnStop = null!;
        private Label _lblStatus = null!;
        private PictureBox _picPreview = null!;
        private TextBox _txtManual = null!;
        private Button _btnUseManual = null!;

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
            _cboCamera = new ComboBox();
            _btnStart = new Button();
            _btnStop = new Button();
            _lblStatus = new Label();
            _picPreview = new PictureBox();
            _txtManual = new TextBox();
            _btnUseManual = new Button();
            ((System.ComponentModel.ISupportInitialize)_picPreview).BeginInit();
            SuspendLayout();
            // 
            // _cboCamera
            // 
            _cboCamera.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboCamera.FormattingEnabled = true;
            _cboCamera.Location = new Point(12, 12);
            _cboCamera.Name = "_cboCamera";
            _cboCamera.Size = new Size(220, 23);
            _cboCamera.TabIndex = 0;
            // 
            // _btnStart
            // 
            _btnStart.Location = new Point(242, 10);
            _btnStart.Name = "_btnStart";
            _btnStart.Size = new Size(100, 30);
            _btnStart.TabIndex = 1;
            _btnStart.Text = "Bắt đầu";
            _btnStart.UseVisualStyleBackColor = true;
            _btnStart.Click += BtnStart_Click;
            // 
            // _btnStop
            // 
            _btnStop.Location = new Point(352, 10);
            _btnStop.Name = "_btnStop";
            _btnStop.Size = new Size(100, 30);
            _btnStop.TabIndex = 2;
            _btnStop.Text = "Dừng";
            _btnStop.UseVisualStyleBackColor = true;
            _btnStop.Click += BtnStop_Click;
            // 
            // _lblStatus
            // 
            _lblStatus.Location = new Point(462, 14);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(270, 24);
            _lblStatus.TabIndex = 3;
            // 
            // _picPreview
            // 
            _picPreview.BorderStyle = BorderStyle.FixedSingle;
            _picPreview.Location = new Point(12, 52);
            _picPreview.Name = "_picPreview";
            _picPreview.Size = new Size(720, 400);
            _picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            _picPreview.TabIndex = 4;
            _picPreview.TabStop = false;
            // 
            // _txtManual
            // 
            _txtManual.Location = new Point(12, 466);
            _txtManual.Name = "_txtManual";
            _txtManual.PlaceholderText = "Nhập barcode...";
            _txtManual.Size = new Size(520, 23);
            _txtManual.TabIndex = 5;
            _txtManual.KeyDown += TxtManual_KeyDown;
            // 
            // _btnUseManual
            // 
            _btnUseManual.Location = new Point(542, 464);
            _btnUseManual.Name = "_btnUseManual";
            _btnUseManual.Size = new Size(190, 34);
            _btnUseManual.TabIndex = 6;
            _btnUseManual.Text = "Dùng barcode";
            _btnUseManual.UseVisualStyleBackColor = true;
            _btnUseManual.Click += BtnUseManual_Click;
            // 
            // FormBarcodeScanner
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(744, 521);
            Controls.Add(_btnUseManual);
            Controls.Add(_txtManual);
            Controls.Add(_picPreview);
            Controls.Add(_lblStatus);
            Controls.Add(_btnStop);
            Controls.Add(_btnStart);
            Controls.Add(_cboCamera);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormBarcodeScanner";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quét mã vạch";
            ((System.ComponentModel.ISupportInitialize)_picPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
