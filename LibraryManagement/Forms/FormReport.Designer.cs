using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormReport
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle = null!;
        private Panel panelFilter = null!;
        private Label lblType = null!;
        private Label lblFrom = null!;
        private Label lblTo = null!;
        private Button btnGenerate = null!;
        private Button btnExport = null!;
        private Button btnImport = null!;
        private Button btnPrint = null!;
        private Label lblExportData = null!;
        private CheckBox chkSelectAllSheets = null!;
        private CheckedListBox clbExportSheets = null!;
        private Label lblQuickActions = null!;
        private Button btnBorrowReturnDetails = null!;

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
            lblTitle = new Label();
            panelFilter = new Panel();
            btnBorrowReturnDetails = new Button();
            lblQuickActions = new Label();
            btnPrint = new Button();
            btnExport = new Button();
            btnImport = new Button();
            btnGenerate = new Button();
            lblTo = new Label();
            lblFrom = new Label();
            lblType = new Label();
            lblExportData = new Label();
            chkSelectAllSheets = new CheckBox();
            clbExportSheets = new CheckedListBox();
            cboReportType = new ComboBox();
            dtpFrom = new DateTimePicker();
            dtpTo = new DateTimePicker();
            panelContent = new Panel();
            lblSummary = new Label();
            panelFilter.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblTitle.Location = new Point(20, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(269, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Thống kê và báo cáo";
            // 
            // panelFilter
            // 
            panelFilter.BackColor = Color.White;
            panelFilter.Controls.Add(btnBorrowReturnDetails);
            panelFilter.Controls.Add(lblQuickActions);
            panelFilter.Controls.Add(btnPrint);
            panelFilter.Controls.Add(btnExport);
            panelFilter.Controls.Add(btnImport);
            panelFilter.Controls.Add(btnGenerate);
            panelFilter.Controls.Add(lblTo);
            panelFilter.Controls.Add(lblFrom);
            panelFilter.Controls.Add(lblType);
            panelFilter.Controls.Add(lblExportData);
            panelFilter.Controls.Add(chkSelectAllSheets);
            panelFilter.Controls.Add(clbExportSheets);
            panelFilter.Controls.Add(cboReportType);
            panelFilter.Controls.Add(dtpFrom);
            panelFilter.Controls.Add(dtpTo);
            panelFilter.Location = new Point(20, 66);
            panelFilter.Name = "panelFilter";
            panelFilter.Size = new Size(1180, 108);
            panelFilter.TabIndex = 1;
            // 
            // btnBorrowReturnDetails
            // 
            btnBorrowReturnDetails.BackColor = Color.FromArgb(37, 99, 235);
            btnBorrowReturnDetails.Cursor = Cursors.Hand;
            btnBorrowReturnDetails.FlatAppearance.BorderSize = 0;
            btnBorrowReturnDetails.FlatStyle = FlatStyle.Flat;
            btnBorrowReturnDetails.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBorrowReturnDetails.ForeColor = Color.White;
            btnBorrowReturnDetails.Location = new Point(103, 62);
            btnBorrowReturnDetails.Name = "btnBorrowReturnDetails";
            btnBorrowReturnDetails.Size = new Size(96, 34);
            btnBorrowReturnDetails.TabIndex = 10;
            btnBorrowReturnDetails.Text = "Mượn/Trả";
            btnBorrowReturnDetails.UseVisualStyleBackColor = false;
            btnBorrowReturnDetails.Click += BtnBorrowReturnDetails_Click;
            // 
            // lblQuickActions
            // 
            lblQuickActions.AutoSize = true;
            lblQuickActions.Font = new Font("Segoe UI", 9F);
            lblQuickActions.Location = new Point(18, 70);
            lblQuickActions.Name = "lblQuickActions";
            lblQuickActions.Size = new Size(73, 15);
            lblQuickActions.TabIndex = 9;
            lblQuickActions.Text = "Xem chi tiết:";
            // 
            // btnPrint
            // 
            btnPrint.BackColor = Color.FromArgb(99, 102, 241);
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrint.ForeColor = Color.White;
            btnPrint.Location = new Point(850, 14);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(56, 34);
            btnPrint.TabIndex = 8;
            btnPrint.Text = "In";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += BtnPrint_Click;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(16, 185, 129);
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(744, 14);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(100, 34);
            btnExport.TabIndex = 7;
            btnExport.Text = "Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += BtnExport_Click;
            // 
            // btnImport
            // 
            btnImport.BackColor = Color.FromArgb(14, 116, 144);
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnImport.ForeColor = Color.White;
            btnImport.Location = new Point(912, 14);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(100, 34);
            btnImport.TabIndex = 11;
            btnImport.Text = "Nhập Excel";
            btnImport.UseVisualStyleBackColor = false;
            btnImport.Click += BtnImport_Click;
            // 
            // btnGenerate
            // 
            btnGenerate.BackColor = Color.FromArgb(37, 99, 235);
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGenerate.ForeColor = Color.White;
            btnGenerate.Location = new Point(630, 14);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(108, 34);
            btnGenerate.TabIndex = 6;
            btnGenerate.Text = "Tạo báo cáo";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += BtnGenerate_Click;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Font = new Font("Segoe UI", 9F);
            lblTo.Location = new Point(466, 20);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(31, 15);
            lblTo.TabIndex = 4;
            lblTo.Text = "Đến:";
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Font = new Font("Segoe UI", 9F);
            lblFrom.Location = new Point(314, 20);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(24, 15);
            lblFrom.TabIndex = 2;
            lblFrom.Text = "Từ:";
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Font = new Font("Segoe UI", 9F);
            lblType.Location = new Point(18, 20);
            lblType.Name = "lblType";
            lblType.Size = new Size(52, 15);
            lblType.TabIndex = 0;
            lblType.Text = "Báo cáo:";
            // 
            // lblExportData
            // 
            lblExportData.AutoSize = true;
            lblExportData.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblExportData.Location = new Point(240, 70);
            lblExportData.Name = "lblExportData";
            lblExportData.Size = new Size(78, 15);
            lblExportData.TabIndex = 12;
            lblExportData.Text = "Dữ liệu xuất:";
            // 
            // chkSelectAllSheets
            // 
            chkSelectAllSheets.AutoSize = true;
            chkSelectAllSheets.Location = new Point(336, 69);
            chkSelectAllSheets.Name = "chkSelectAllSheets";
            chkSelectAllSheets.Size = new Size(87, 19);
            chkSelectAllSheets.TabIndex = 13;
            chkSelectAllSheets.Text = "Chọn tất cả";
            chkSelectAllSheets.UseVisualStyleBackColor = true;
            chkSelectAllSheets.CheckedChanged += ChkSelectAllSheets_CheckedChanged;
            // 
            // clbExportSheets
            // 
            clbExportSheets.CheckOnClick = true;
            clbExportSheets.FormattingEnabled = true;
            clbExportSheets.Location = new Point(440, 59);
            clbExportSheets.MultiColumn = true;
            clbExportSheets.Name = "clbExportSheets";
            clbExportSheets.Size = new Size(558, 40);
            clbExportSheets.TabIndex = 14;
            clbExportSheets.ItemCheck += ClbExportSheets_ItemCheck;
            // 
            // cboReportType
            // 
            cboReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReportType.Font = new Font("Segoe UI", 9F);
            cboReportType.FormattingEnabled = true;
            cboReportType.Items.AddRange(new object[] { "Tổng quan", "Sách mượn nhiều nhất", "Độc giả mượn nhiều nhất", "Sách quá hạn", "Thống kê theo ngày", "Danh sách phạt chưa thu", "Danh sách sách hết" });
            cboReportType.Location = new Point(76, 16);
            cboReportType.Name = "cboReportType";
            cboReportType.Size = new Size(228, 23);
            cboReportType.TabIndex = 1;
            cboReportType.SelectedIndexChanged += CboReportType_SelectedIndexChanged;
            // 
            // dtpFrom
            // 
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(340, 16);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(120, 23);
            dtpFrom.TabIndex = 3;
            // 
            // dtpTo
            // 
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(502, 16);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(120, 23);
            dtpTo.TabIndex = 5;
            // 
            // panelContent
            // 
            panelContent.AutoScroll = true;
            panelContent.BackColor = Color.White;
            panelContent.Location = new Point(20, 184);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(1180, 338);
            panelContent.TabIndex = 2;
            // 
            // lblSummary
            // 
            lblSummary.Font = new Font("Segoe UI", 11F);
            lblSummary.Location = new Point(20, 532);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(1180, 30);
            lblSummary.TabIndex = 3;
            // 
            // FormReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 252);
            ClientSize = new Size(1220, 570);
            Controls.Add(lblSummary);
            Controls.Add(panelContent);
            Controls.Add(panelFilter);
            Controls.Add(lblTitle);
            DoubleBuffered = true;
            Name = "FormReport";
            Text = "Thống kê báo cáo";
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel panelContent;
        private ComboBox cboReportType;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private DataGridView dgvReport = null!;
        private Label lblSummary;
    }
}
