using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Utils
{
    public static class ThemeManager
    {
        // =====================================
        // Figma "Serene Curator" Inspired Theme
        // =====================================

        public static readonly Color FormBackColor = Color.FromArgb(243, 246, 251);
        public static readonly Color SideMenuColor = Color.FromArgb(255, 255, 255);
        public static readonly Color SideMenuText = Color.FromArgb(51, 65, 85);
        public static readonly Color SideMenuHover = Color.FromArgb(239, 246, 255);

        public static readonly Color PrimaryColor = Color.FromArgb(59, 130, 246);
        public static readonly Color ButtonHoverColor = Color.FromArgb(37, 99, 235);
        public static readonly Color ButtonDownColor = Color.FromArgb(30, 64, 175);

        public static readonly Color TextColor = Color.FromArgb(15, 23, 42);
        public static readonly Color MutedTextColor = Color.FromArgb(100, 116, 139);
        public static readonly Color TextLightColor = Color.White;

        public static readonly Color PanelBackColor = Color.White;

        public static readonly Color GridHeaderBackColor = Color.FromArgb(248, 250, 252);
        public static readonly Color GridHeaderForeColor = Color.FromArgb(71, 85, 105);
        public static readonly Color GridSelectionColor = Color.FromArgb(219, 234, 254);
        public static readonly Color BorderColor = Color.FromArgb(226, 232, 240);

        public static readonly Font RegularFont = new Font("Segoe UI", 10f, FontStyle.Regular);
        public static readonly Font BoldFont = new Font("Segoe UI", 10f, FontStyle.Bold);
        public static readonly Font HeaderFont = new Font("Segoe UI", 16f, FontStyle.Bold);

        public static void ApplyTheme(Form form)
        {
            if (form == null) return;
            form.BackColor = FormBackColor;
            form.ForeColor = TextColor;
            form.Font = RegularFont;

            ApplyThemeRecursive(form.Controls);
        }

        private static void ApplyThemeRecursive(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is Panel pnl)
                {
                    if (pnl.Name.Contains("Menu") || pnl.Dock == DockStyle.Left)
                    {
                        pnl.BackColor = SideMenuColor;
                    }
                    else if (pnl.Name.Contains("Header") || pnl.Dock == DockStyle.Top)
                    {
                        pnl.BackColor = Color.White;
                        pnl.Padding = new Padding(0, 0, 0, 1);
                    }
                    else if (pnl.Name.Contains("Content") || pnl.Dock == DockStyle.Fill)
                    {
                        pnl.BackColor = FormBackColor;
                    }
                    else
                    {
                        pnl.BackColor = PanelBackColor;
                    }
                }
                else if (control is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = BorderColor;
                    btn.Font = BoldFont;
                    btn.Cursor = Cursors.Hand;
                    btn.UseVisualStyleBackColor = false;

                    if (btn.Parent != null && (btn.Parent.Name.Contains("Menu") || btn.Parent.Dock == DockStyle.Left))
                    {
                        btn.BackColor = SideMenuColor;
                        btn.ForeColor = SideMenuText;
                        btn.TextAlign = ContentAlignment.MiddleLeft;
                        btn.FlatAppearance.BorderSize = 0;

                        btn.MouseEnter += (s, e) => { btn.BackColor = SideMenuHover; btn.ForeColor = TextColor; };
                        btn.MouseLeave += (s, e) => { btn.BackColor = SideMenuColor; btn.ForeColor = SideMenuText; };
                    }
                    else
                    {
                        btn.BackColor = PrimaryColor;
                        btn.ForeColor = TextLightColor;

                        btn.MouseEnter += (s, e) => { btn.BackColor = ButtonHoverColor; };
                        btn.MouseLeave += (s, e) => { btn.BackColor = PrimaryColor; };
                        btn.MouseDown += (s, e) => { btn.BackColor = ButtonDownColor; };
                    }
                }
                else if (control is DataGridView dgv)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.BorderStyle = BorderStyle.FixedSingle;
                    dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    dgv.GridColor = BorderColor;

                    dgv.DefaultCellStyle.SelectionBackColor = GridSelectionColor;
                    dgv.DefaultCellStyle.SelectionForeColor = TextColor;
                    dgv.DefaultCellStyle.BackColor = Color.White;
                    dgv.DefaultCellStyle.ForeColor = TextColor;
                    dgv.DefaultCellStyle.Font = RegularFont;
                    dgv.DefaultCellStyle.Padding = new Padding(4, 6, 4, 6);

                    dgv.EnableHeadersVisualStyles = false;
                    dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderBackColor;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = GridHeaderForeColor;
                    dgv.ColumnHeadersDefaultCellStyle.Font = BoldFont;
                    dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 8, 4, 8);
                    dgv.ColumnHeadersHeight = 44;

                    dgv.RowHeadersVisible = false;
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgv.RowTemplate.Height = 40;
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 252, 255);
                }
                else if (control is Label lbl)
                {
                    if (lbl.Parent != null && (lbl.Parent.BackColor.R + lbl.Parent.BackColor.G + lbl.Parent.BackColor.B) < 600 && lbl.Parent.BackColor != Color.Transparent)
                    {
                        lbl.ForeColor = Color.White;
                    }
                    else if (lbl.Font.Size >= 14f) { lbl.ForeColor = TextColor; }
                    else if (lbl.Font.Bold) { lbl.ForeColor = TextColor; }
                    else { lbl.ForeColor = MutedTextColor; }
                }
                else if (control is TextBox txt)
                {
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.BackColor = Color.White;
                    txt.ForeColor = TextColor;
                    txt.Font = RegularFont;
                }
                else if (control is ComboBox cb)
                {
                    cb.FlatStyle = FlatStyle.Flat;
                    cb.BackColor = Color.White;
                    cb.ForeColor = TextColor;
                    cb.Font = RegularFont;
                }
                else if (control is DateTimePicker dtp)
                {
                    dtp.CalendarForeColor = TextColor;
                    dtp.CalendarMonthBackground = Color.White;
                    dtp.Font = RegularFont;
                }
                else if (control is NumericUpDown num)
                {
                    num.Font = RegularFont;
                    num.BackColor = Color.White;
                    num.ForeColor = TextColor;
                }

                if (control.HasChildren)
                {
                    ApplyThemeRecursive(control.Controls);
                }
            }
        }
    }
}

