using System;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Màn hình quản lý riêng cho vai trò Manager/Admin.
    /// </summary>
    public partial class FormManagerWorkspace : Form
    {
        public FormManagerWorkspace()
        {
            InitializeComponent();

            BuildActions();
        }

        private void BuildActions()
        {
            AddActionCard("Quản lý Sách", "Thêm, sửa, xóa và theo dõi kho sách", () => new FormBookManagement().Show());
            AddActionCard("Quản lý Tác giả", "Quản trị tên tác giả, tiểu sử và quốc gia", () => new FormAuthorManagement().Show());
            AddActionCard("Quản lý NXB", "Quản trị nhà xuất bản và thông tin liên hệ", () => new FormPublisherManagement().Show());
            AddActionCard("Quản lý Độc giả", "Cập nhật hồ sơ, loại thẻ và trạng thái", () => new FormMemberManagement().Show());
            AddActionCard("Báo cáo thống kê", "Xem số liệu mượn trả và hiệu suất hoạt động", () => new FormReport().Show());
            AddActionCard("Quản lý hàng đợi", "Duyệt và thông báo đặt trước", () => new FormReservationQueue().Show());
            AddActionCard("Trung tâm Email", "Gửi email mẫu và thông báo hàng loạt", () => new FormMailCenter().Show());

            if (CurrentUser.User?.IsAdmin == true)
            {
                AddActionCard("Quản lý Tài khoản", "Phân quyền và quản trị người dùng", () => new FormUserManagement().Show());
                AddActionCard("Cấu hình hệ thống", "Thiết lập chính sách và tham số vận hành", () => new FormSettings().Show());
            }
        }

        private void AddActionCard(string title, string description, Action onOpen)
        {
            var card = new Panel
            {
                Width = 320,
                Height = 170,
                Margin = new Padding(12),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(14, 14),
                Size = new Size(286, 32)
            };

            var lblDescription = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(14, 52),
                Size = new Size(286, 56)
            };

            var btnOpen = new Button
            {
                Text = "Mở",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(37, 99, 235),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(92, 34),
                Location = new Point(14, 118),
                Cursor = Cursors.Hand
            };
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.Click += (_, _) => onOpen();

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblDescription);
            card.Controls.Add(btnOpen);
            flowPanel.Controls.Add(card);
        }
    }
}
