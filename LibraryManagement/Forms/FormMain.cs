using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Services;
using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form chính của ứng dụng - Dashboard
    /// </summary>
    public partial class FormMain : Form
    {
        private System.Windows.Forms.Timer timerDateTime = null!;
        private System.Windows.Forms.Timer timerOverdueEmail = null!;
        private bool overdueEmailRunning;
        private EmailService emailService = new EmailService();

        // Dashboard controls
        private Label lblTotalBooks = null!;
        private Label lblTotalMembers = null!;
        private Label lblBorrowing = null!;
        private Label lblOverdue = null!;
        private Label lblTodayBorrow = null!;
        private Label lblTodayReturn = null!;
        private Chart chartBorrowByDay = null!;
        private Chart chartTopBooks = null!;
        private Label lblChartBorrowByDay = null!;
        private Label lblChartTopBooks = null!;
        private DataGridView dgvRecentBorrows = null!;
        private DataGridView dgvOverdueList = null!;
        private Label lblRecentLabel = null!;
        private Label lblOverdueLabel = null!;
        private bool isLoggingOut = false;
        private FormManagerWorkspace? managerWorkspace;

        public FormMain()
        {
            InitializeComponent();

            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                try
                {
                    lblCurrentUser.Text = "Người dùng: Designer Preview";
                    lblDateTime.Text = $"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                    panelMenu.Controls.Clear();
                    panelContent.Controls.Clear();
                    SetupMenu();
                    SetupDashboard();
                    PanelHeader_Resize(null, EventArgs.Empty);
                }
                catch
                {
                }
            }

            Load += FormMain_Load;
        }

        private void FormMain_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            BackColor = LibraryManagement.Utils.ThemeManager.FormBackColor;

            lblCurrentUser.Text = $"Người dùng: {CurrentUser.User?.FullName ?? "Người dùng"} ({CurrentUser.User?.Role ?? "N/A"})";
            lblDateTime.Text = $"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            panelMenu.Controls.Clear();
            panelContent.Controls.Clear();

            SetupMenu();

            panelContent.Resize -= PanelContent_Resize;
            panelContent.Resize += PanelContent_Resize;

            panelHeader.Resize -= PanelHeader_Resize;
            panelHeader.Resize += PanelHeader_Resize;
            PanelHeader_Resize(null, EventArgs.Empty);

            timerDateTime = new System.Windows.Forms.Timer { Interval = 1000 };
            timerDateTime.Tick += TimerDateTime_Tick;
            timerDateTime.Start();

            SetupDashboard();
            LoadDashboard();

            StartOverdueEmailTimer();
        }

        private void TimerDateTime_Tick(object? sender, EventArgs e)
        {
            lblDateTime.Text = $"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
        }

        private void StartOverdueEmailTimer()
        {
            timerOverdueEmail?.Stop();
            timerOverdueEmail = new System.Windows.Forms.Timer { Interval = 60 * 60 * 1000 };
            timerOverdueEmail.Tick += TimerOverdueEmail_Tick;
            timerOverdueEmail.Start();

            _ = RunOverdueEmailAsync();
        }

        private async void TimerOverdueEmail_Tick(object? sender, EventArgs e)
        {
            await RunOverdueEmailAsync();
        }

        private async Task RunOverdueEmailAsync()
        {
            if (overdueEmailRunning) return;
            overdueEmailRunning = true;
            try
            {
                await emailService.SendDailyOverdueRemindersAsync();
            }
            catch
            {
            }
            finally
            {
                overdueEmailRunning = false;
            }
        }

        private void PanelHeader_Resize(object? sender, EventArgs e)
        {
            if (lblCurrentUser != null)
                lblCurrentUser.Location = new Point(panelHeader.Width - 320, 10);

            if (lblDateTime != null)
                lblDateTime.Location = new Point(panelHeader.Width - 320, 32);
        }

        private void SetupMenu()
        {
            int y = 18;
            int btnHeight = 44;
            int spacing = 6;

            var brandPanel = new Panel
            {
                BackColor = Color.FromArgb(15, 23, 42),
                Location = new Point(12, y),
                Size = new Size(228, 86)
            };

            var lblBrand = new Label
            {
                Text = "Library Hub",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(14, 14),
                AutoSize = true
            };

            var lblBrandSub = new Label
            {
                Text = "Không gian quản lý thư viện",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(191, 219, 254),
                Location = new Point(14, 48),
                AutoSize = true
            };

            brandPanel.Controls.Add(lblBrand);
            brandPanel.Controls.Add(lblBrandSub);
            panelMenu.Controls.Add(brandPanel);
            y += 98;

            AddMenuButton("Trang chủ", y, BtnHome_Click);
            y += btnHeight + spacing;

            AddMenuButton("Quản lý Sách", y, BtnBooks_Click);
            y += btnHeight + spacing;

            AddMenuButton("Quản lý Danh mục", y, BtnCategories_Click);
            y += btnHeight + spacing;

            AddMenuButton("Quản lý Tác giả", y, BtnAuthors_Click);
            y += btnHeight + spacing;

            AddMenuButton("Quản lý NXB", y, BtnPublishers_Click);
            y += btnHeight + spacing;

            AddMenuButton("Quản lý Độc giả", y, BtnMembers_Click);
            y += btnHeight + spacing;

            AddMenuButton("Mượn sách", y, BtnBorrow_Click);
            y += btnHeight + spacing;

            AddMenuButton("Trả sách", y, BtnReturn_Click);
            y += btnHeight + spacing;

            AddMenuButton("Chatbot Sách AI", y, BtnBookChatbot_Click);
            y += btnHeight + spacing;

            AddMenuButton("Báo cáo thống kê", y, BtnReport_Click);
            y += btnHeight + spacing;

            AddMenuButton("Trung tâm Email", y, BtnMailCenter_Click);
            y += btnHeight + spacing;

            AddMenuButton("Quản lý hàng đợi", y, BtnReservationQueue_Click);
            y += btnHeight + spacing;

            if (CurrentUser.HasPermission(User.ROLE_MANAGER))
            {
                AddMenuButton("Không gian quản lý", y, BtnManagerWorkspace_Click);
                y += btnHeight + spacing;
            }

            if (CurrentUser.User?.IsAdmin == true)
            {
                y += 14;
                AddMenuButton("Quản lý Tài khoản", y, BtnUsers_Click);
                y += btnHeight + spacing;

                AddMenuButton("Cấu hình hệ thống", y, BtnSettings_Click);
                y += btnHeight + spacing;
            }

            var btnLogout = new Button
            {
                Text = "Đăng xuất",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(215, 60, 95),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(228, 42),
                Location = new Point(12, panelMenu.Height - 64),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;
            panelMenu.Controls.Add(btnLogout);
        }

        private void AddMenuButton(string text, int y, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(228, 44),
                Location = new Point(12, y),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
            btn.Click += onClick;
            panelMenu.Controls.Add(btn);
        }

        private void BtnHome_Click(object? sender, EventArgs e)
        {
            ClearContent();
            SetupDashboard();
            LoadDashboard();
        }

        private void BtnBooks_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormBookManagement());
        }

        private void BtnMembers_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormMemberManagement());
        }

        private void BtnCategories_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormCategoryManagement());
        }

        private void BtnAuthors_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormAuthorManagement());
        }

        private void BtnPublishers_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormPublisherManagement());
        }

        private void BtnBorrow_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormBorrow());
        }

        private void BtnReturn_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormReturn());
        }

        private void BtnReport_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormReport());
        }

        private void BtnBookChatbot_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormBookChatbot());
        }

        private void BtnMailCenter_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormMailCenter());
        }

        private void BtnUsers_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormUserManagement());
        }

        private void BtnReservationQueue_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormReservationQueue());
        }

        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormSettings());
        }

        private void BtnManagerWorkspace_Click(object? sender, EventArgs e)
        {
            if (managerWorkspace == null || managerWorkspace.IsDisposed)
            {
                managerWorkspace = new FormManagerWorkspace();
                managerWorkspace.FormClosed += (_, _) => managerWorkspace = null;
            }

            managerWorkspace.Show();
            managerWorkspace.BringToFront();
        }

        private void SetupDashboard()
        {
            panelContent.Controls.Clear();

            panelContent.BackColor = Color.FromArgb(241, 245, 249);

            int margin = 18;
            int contentWidth = Math.Max(900, panelContent.ClientSize.Width - margin * 2);

            var heroPanel = new Panel
            {
                Location = new Point(margin, 16),
                Size = new Size(contentWidth, 120),
                BackColor = Color.FromArgb(30, 64, 175)
            };
            heroPanel.Paint += (_, e) =>
            {
                using var brush = new LinearGradientBrush(heroPanel.ClientRectangle,
                    Color.FromArgb(30, 64, 175), Color.FromArgb(15, 23, 42), 20f);
                e.Graphics.FillRectangle(brush, heroPanel.ClientRectangle);
            };

            var lblHeroTitle = new Label
            {
                Text = "Bảng Điều Khiển Thư Viện",
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                Location = new Point(20, 14),
                AutoSize = true
            };

            var lblHeroSub = new Label
            {
                Text = "Theo dõi tình trạng mượn trả theo thời gian thực",
                ForeColor = Color.FromArgb(191, 219, 254),
                Font = new Font("Segoe UI", 10F),
                Location = new Point(22, 56),
                AutoSize = true
            };

            var btnRefresh = new Button
            {
                Text = "Làm mới dữ liệu",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(147, 197, 253),
                ForeColor = Color.FromArgb(15, 23, 42),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(146, 38),
                Location = new Point(heroPanel.Width - 166, 18),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefreshDashboard_Click;

            heroPanel.Controls.Add(lblHeroTitle);
            heroPanel.Controls.Add(lblHeroSub);
            heroPanel.Controls.Add(btnRefresh);
            panelContent.Controls.Add(heroPanel);

            int cardGap = 14;
            int cardWidth = (contentWidth - cardGap * 2) / 3;
            int cardHeight = 98;
            int cardTop1 = heroPanel.Bottom + 16;
            int cardTop2 = cardTop1 + cardHeight + cardGap;
            int cardX1 = margin;
            int cardX2 = cardX1 + cardWidth + cardGap;
            int cardX3 = cardX2 + cardWidth + cardGap;

            var card1 = CreateStatCard("Tổng đầu sách", "0", Color.FromArgb(59, 130, 246), cardX1, cardTop1, cardWidth, cardHeight);
            lblTotalBooks = (Label)card1.Controls["lblValue"]!;
            panelContent.Controls.Add(card1);

            var card2 = CreateStatCard("Tổng độc giả", "0", Color.FromArgb(16, 185, 129), cardX2, cardTop1, cardWidth, cardHeight);
            lblTotalMembers = (Label)card2.Controls["lblValue"]!;
            panelContent.Controls.Add(card2);

            var card3 = CreateStatCard("Sách đang mượn", "0", Color.FromArgb(99, 102, 241), cardX3, cardTop1, cardWidth, cardHeight);
            lblBorrowing = (Label)card3.Controls["lblValue"]!;
            panelContent.Controls.Add(card3);

            var card4 = CreateStatCard("Sách quá hạn", "0", Color.FromArgb(239, 68, 68), cardX1, cardTop2, cardWidth, cardHeight);
            lblOverdue = (Label)card4.Controls["lblValue"]!;
            panelContent.Controls.Add(card4);

            var card5 = CreateStatCard("Mượn hôm nay", "0", Color.FromArgb(245, 158, 11), cardX2, cardTop2, cardWidth, cardHeight);
            lblTodayBorrow = (Label)card5.Controls["lblValue"]!;
            panelContent.Controls.Add(card5);

            var card6 = CreateStatCard("Trả hôm nay", "0", Color.FromArgb(14, 165, 233), cardX3, cardTop2, cardWidth, cardHeight);
            lblTodayReturn = (Label)card6.Controls["lblValue"]!;
            panelContent.Controls.Add(card6);

            int sectionGap = 14;
            int sectionTop = cardTop2 + cardHeight + 18;
            int sectionWidth = (contentWidth - sectionGap) / 2;
            int sectionHeight = 246;

            var pnlBorrowChart = CreateSectionPanel("Biểu đồ mượn theo ngày", margin, sectionTop, sectionWidth, sectionHeight, out lblChartBorrowByDay);
            chartBorrowByDay = CreateChart(14, 42, sectionWidth - 28, sectionHeight - 56);
            chartBorrowByDay.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            pnlBorrowChart.Controls.Add(chartBorrowByDay);
            panelContent.Controls.Add(pnlBorrowChart);

            var pnlTopBookChart = CreateSectionPanel("Top sách mượn nhiều", margin + sectionWidth + sectionGap, sectionTop, sectionWidth, sectionHeight, out lblChartTopBooks);
            chartTopBooks = CreateChart(14, 42, sectionWidth - 28, sectionHeight - 56);
            chartTopBooks.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            pnlTopBookChart.Controls.Add(chartTopBooks);
            panelContent.Controls.Add(pnlTopBookChart);

            int gridTop = sectionTop + sectionHeight + 14;
            int gridHeight = Math.Max(220, panelContent.ClientSize.Height - gridTop - 24);

            var pnlRecent = CreateSectionPanel("Mượn sách gần đây", margin, gridTop, sectionWidth, gridHeight, out lblRecentLabel);
            dgvRecentBorrows = CreateDataGridView(14, 42, sectionWidth - 28, gridHeight - 56);
            dgvRecentBorrows.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dgvRecentBorrows.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentBorrows.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvRecentBorrows.DefaultCellStyle.ForeColor = Color.Black;
            dgvRecentBorrows.Columns.Add("BorrowCode", "Mã phiếu");
            dgvRecentBorrows.Columns.Add("MemberName", "Độc giả");
            dgvRecentBorrows.Columns.Add("BookTitle", "Tên sách");
            dgvRecentBorrows.Columns.Add("BorrowDate", "Ngày mượn");
            dgvRecentBorrows.Columns.Add("DueDate", "Hạn trả");
            pnlRecent.Controls.Add(dgvRecentBorrows);
            panelContent.Controls.Add(pnlRecent);

            var pnlOverdue = CreateSectionPanel("Danh sách quá hạn", margin + sectionWidth + sectionGap, gridTop, sectionWidth, gridHeight, out lblOverdueLabel);
            lblOverdueLabel.ForeColor = Color.FromArgb(220, 38, 38);
            dgvOverdueList = CreateDataGridView(14, 42, sectionWidth - 28, gridHeight - 56);
            dgvOverdueList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dgvOverdueList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOverdueList.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvOverdueList.DefaultCellStyle.ForeColor = Color.Black;
            dgvOverdueList.Columns.Add("MemberName", "Độc giả");
            dgvOverdueList.Columns.Add("BookTitle", "Tên sách");
            dgvOverdueList.Columns.Add("DueDate", "Hạn trả");
            dgvOverdueList.Columns.Add("DaysOverdue", "Quá hạn");
            pnlOverdue.Controls.Add(dgvOverdueList);
            panelContent.Controls.Add(pnlOverdue);

            AdjustGridLayout();
        }

        private void BtnRefreshDashboard_Click(object? sender, EventArgs e)
        {
            LoadDashboard();
        }

        private Panel CreateStatCard(string title, string value, Color color, int x, int y, int width, int height)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = color
            };
            panel.Paint += (_, e) =>
            {
                using var brush = new LinearGradientBrush(panel.ClientRectangle,
                    ControlPaint.Light(color, 0.12f), ControlPaint.Dark(color, 0.18f), 35f);
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            };

            var lblTitle = new Label
            {
                Text = title,
                Name = "lblTitle",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 231, 255),
                Location = new Point(14, 12),
                Size = new Size(width - 20, 25)
            };

            var lblValueCard = new Label
            {
                Text = value,
                Name = "lblValue",
                Font = new Font("Segoe UI Semibold", 28F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(14, 40),
                Size = new Size(width - 20, 50),
                TextAlign = ContentAlignment.MiddleLeft
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValueCard);
            return panel;
        }

        private Panel CreateSectionPanel(string title, int x, int y, int width, int height, out Label lblTitle)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(14, 12),
                AutoSize = true
            };
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private DataGridView CreateDataGridView(int x, int y, int width, int height)
        {
            var dgv = new DataGridView
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            };

            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = LibraryManagement.Utils.ThemeManager.SideMenuHover,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(5)
            };

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9),
                Padding = new Padding(5)
            };

            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(245, 245, 245)
            };

            dgv.EnableHeadersVisualStyles = false;
            return dgv;
        }

        private Chart CreateChart(int x, int y, int width, int height)
        {
            var chart = new Chart
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.White,
                BorderlineColor = Color.FromArgb(226, 232, 240),
                BorderlineDashStyle = ChartDashStyle.Solid,
                BorderlineWidth = 1
            };

            var area = new ChartArea("Main")
            {
                BackColor = Color.White
            };
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(226, 232, 240);
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.LabelStyle.ForeColor = Color.FromArgb(100, 116, 139);
            area.AxisY.LabelStyle.ForeColor = Color.FromArgb(100, 116, 139);
            chart.ChartAreas.Add(area);

            chart.Legends.Clear();
            chart.Series.Clear();
            return chart;
        }

        private void LoadDashboard()
        {
            try
            {
                var dashboardService = new DashboardService();
                var stats = dashboardService.GetSummary();

                lblTotalBooks.Text = stats.TotalBooks.ToString("N0");
                lblTotalMembers.Text = stats.TotalMembers.ToString("N0");
                lblBorrowing.Text = stats.BorrowingCount.ToString("N0");
                lblOverdue.Text = stats.OverdueCount.ToString("N0");
                lblTodayBorrow.Text = stats.TodayBorrows.ToString("N0");
                lblTodayReturn.Text = stats.TodayReturns.ToString("N0");

                var borrowByDay = dashboardService.GetBorrowByDay(14);
                chartBorrowByDay.Series.Clear();
                var seriesBorrow = new Series("Borrow")
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 3,
                    Color = LibraryManagement.Utils.ThemeManager.PrimaryColor,
                    XValueType = ChartValueType.String
                };
                foreach (var p in borrowByDay)
                {
                    seriesBorrow.Points.AddXY(p.Date.ToString("dd/MM"), p.BorrowCount);
                }
                chartBorrowByDay.Series.Add(seriesBorrow);

                var topBooks = dashboardService.GetTopBorrowedBooks(5);
                chartTopBooks.Series.Clear();
                var seriesTop = new Series("TopBooks")
                {
                    ChartType = SeriesChartType.Bar,
                    Color = LibraryManagement.Utils.ThemeManager.SideMenuHover,
                    XValueType = ChartValueType.String
                };
                foreach (var b in topBooks)
                {
                    seriesTop.Points.AddXY(b.Title, b.BorrowCount);
                }
                chartTopBooks.Series.Add(seriesTop);

                var borrowDAO = new BorrowRecordDAO();
                var recentBorrows = borrowDAO.Search(status: BorrowRecord.STATUS_BORROWING);
                dgvRecentBorrows.Rows.Clear();
                foreach (var borrow in recentBorrows.Take(10))
                {
                    dgvRecentBorrows.Rows.Add(
                        borrow.BorrowCode,
                        borrow.MemberName,
                        borrow.BookTitle,
                        borrow.BorrowDate.ToString("dd/MM/yyyy"),
                        borrow.DueDate.ToString("dd/MM/yyyy")
                    );
                }

                var overdueList = borrowDAO.GetOverdueRecords();
                dgvOverdueList.Rows.Clear();
                foreach (var record in overdueList.Take(10))
                {
                    int daysOverdue = (int)(DateTime.Now - record.DueDate).TotalDays;
                    dgvOverdueList.Rows.Add(
                        record.MemberName,
                        record.BookTitle,
                        record.DueDate.ToString("dd/MM/yyyy"),
                        $"{daysOverdue} Ngày"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearContent()
        {
            panelContent.Controls.Clear();
        }

        private void OpenForm(Form form)
        {
            ClearContent();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void PanelContent_Resize(object? sender, EventArgs e)
        {
            AdjustGridLayout();
        }

        private void AdjustGridLayout()
        {
            if (panelContent == null || dgvRecentBorrows == null || dgvOverdueList == null || chartBorrowByDay == null || chartTopBooks == null)
                return;

            int margin = 18;
            int sectionGap = 14;
            int cardGap = 14;
            int contentWidth = Math.Max(900, panelContent.ClientSize.Width - margin * 2);
            int cardWidth = (contentWidth - cardGap * 2) / 3;
            int cardHeight = 98;

            var hero = panelContent.Controls.OfType<Panel>().FirstOrDefault(p => p.Controls.OfType<Button>().Any(b => b.Text == "Làm mới dữ liệu"));
            if (hero == null) return;

            hero.Location = new Point(margin, 16);
            hero.Size = new Size(contentWidth, 120);

            var refresh = hero.Controls.OfType<Button>().FirstOrDefault();
            if (refresh != null)
                refresh.Location = new Point(hero.Width - 166, 18);

            int cardTop1 = hero.Bottom + 16;
            int cardTop2 = cardTop1 + cardHeight + cardGap;
            int cardX1 = margin;
            int cardX2 = cardX1 + cardWidth + cardGap;
            int cardX3 = cardX2 + cardWidth + cardGap;

            var statCards = panelContent.Controls.OfType<Panel>()
                .Where(p => p.Controls.OfType<Label>().Any(l => l.Name == "lblValue"))
                .Take(6)
                .ToList();

            if (statCards.Count == 6)
            {
                statCards[0].Bounds = new Rectangle(cardX1, cardTop1, cardWidth, cardHeight);
                statCards[1].Bounds = new Rectangle(cardX2, cardTop1, cardWidth, cardHeight);
                statCards[2].Bounds = new Rectangle(cardX3, cardTop1, cardWidth, cardHeight);
                statCards[3].Bounds = new Rectangle(cardX1, cardTop2, cardWidth, cardHeight);
                statCards[4].Bounds = new Rectangle(cardX2, cardTop2, cardWidth, cardHeight);
                statCards[5].Bounds = new Rectangle(cardX3, cardTop2, cardWidth, cardHeight);
            }

            int sectionTop = cardTop2 + cardHeight + 18;
            int sectionWidth = (contentWidth - sectionGap) / 2;
            int sectionHeight = 246;
            int gridTop = sectionTop + sectionHeight + 14;
            int gridHeight = Math.Max(220, panelContent.ClientSize.Height - gridTop - 24);

            Panel? borrowChartPanel = chartBorrowByDay.Parent as Panel;
            Panel? topChartPanel = chartTopBooks.Parent as Panel;
            Panel? recentPanel = dgvRecentBorrows.Parent as Panel;
            Panel? overduePanel = dgvOverdueList.Parent as Panel;

            if (borrowChartPanel != null)
            {
                borrowChartPanel.Bounds = new Rectangle(margin, sectionTop, sectionWidth, sectionHeight);
                chartBorrowByDay.Bounds = new Rectangle(14, 42, sectionWidth - 28, sectionHeight - 56);
            }

            if (topChartPanel != null)
            {
                topChartPanel.Bounds = new Rectangle(margin + sectionWidth + sectionGap, sectionTop, sectionWidth, sectionHeight);
                chartTopBooks.Bounds = new Rectangle(14, 42, sectionWidth - 28, sectionHeight - 56);
            }

            if (recentPanel != null)
            {
                recentPanel.Bounds = new Rectangle(margin, gridTop, sectionWidth, gridHeight);
                dgvRecentBorrows.Bounds = new Rectangle(14, 42, sectionWidth - 28, gridHeight - 56);
            }

            if (overduePanel != null)
            {
                overduePanel.Bounds = new Rectangle(margin + sectionWidth + sectionGap, gridTop, sectionWidth, gridHeight);
                dgvOverdueList.Bounds = new Rectangle(14, 42, sectionWidth - 28, gridHeight - 56);
            }
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var logDAO = new ActivityLogDAO();
                logDAO.Log("Đăng xuất hệ thống");
                isLoggingOut = true;
                CurrentUser.Logout();
                Close();
            }
        }

        private void FormMain_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (isLoggingOut)
            {
                timerDateTime?.Stop();
                timerOverdueEmail?.Stop();
                return;
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show("Bạn có chắc muốn thoát ứng dụng?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    timerDateTime?.Stop();
                    timerOverdueEmail?.Stop();
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log("Thoát ứng dụng");
                }
            }
        }
    }
}




