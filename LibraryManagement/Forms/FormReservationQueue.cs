using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Services;

namespace LibraryManagement.Forms
{
    public partial class FormReservationQueue : Form
    {
        private readonly ReservationDAO reservationDAO = new ReservationDAO();
        private readonly EmailService emailService = new EmailService();

        public FormReservationQueue()
        {
            InitializeComponent();
            InitializeStatusFilter();
            LibraryManagement.Utils.ThemeManager.ApplyTheme(this);
            Load += FormReservationQueue_Load;
        }

        private void InitializeStatusFilter()
        {
            cboStatus.Items.Clear();
            cboStatus.Items.AddRange(new object[]
            {
                "Tất cả",
                ReservationDAO.STATUS_PENDING,
                ReservationDAO.STATUS_NOTIFIED,
                ReservationDAO.STATUS_FULFILLED,
                ReservationDAO.STATUS_CANCELLED,
                ReservationDAO.STATUS_EXPIRED
            });
            cboStatus.SelectedIndex = 0;
        }

        private async void FormReservationQueue_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            await ReloadAsync();
        }

        private async void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            await ReloadAsync();
        }

        private async void CboStatus_SelectedIndexChanged(object? sender, EventArgs e)
        {
            await ReloadAsync();
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await ReloadAsync();
        }

        private async void BtnNotify_Click(object? sender, EventArgs e)
        {
            var item = GetSelected();
            if (item == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng hàng chờ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(item.MemberEmail))
            {
                MessageBox.Show("Độc giả chưa có email để gửi thông báo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool sent = await emailService.SendReservationAvailableAsync(item.MemberEmail, item.MemberName, item.BookTitle);
            if (!sent)
            {
                string details = string.IsNullOrWhiteSpace(emailService.LastError)
                    ? "Không có chi tiết lỗi từ dịch vụ gửi email."
                    : emailService.LastError;
                MessageBox.Show($"Gửi email thất bại.\nChi tiết: {details}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            reservationDAO.MarkAsNotified(item.ReservationID);
            MessageBox.Show("Đã gửi email và cập nhật trạng thái Đã thông báo.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await ReloadAsync();
        }

        private async void BtnFulfill_Click(object? sender, EventArgs e)
        {
            var item = GetSelected();
            if (item == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng hàng chờ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool ok = reservationDAO.UpdateStatus(item.ReservationID, ReservationDAO.STATUS_FULFILLED);
            if (ok)
            {
                MessageBox.Show("Đã cập nhật trạng thái Đã nhận.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await ReloadAsync();
            }
        }

        private async void BtnCancel_Click(object? sender, EventArgs e)
        {
            var item = GetSelected();
            if (item == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng hàng chờ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dr = MessageBox.Show("Xác nhận hủy đặt trước của dòng đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                return;
            }

            bool ok = reservationDAO.UpdateStatus(item.ReservationID, ReservationDAO.STATUS_CANCELLED);
            if (ok)
            {
                MessageBox.Show("Đã hủy đặt trước.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await ReloadAsync();
            }
        }

        private async Task ReloadAsync()
        {
            await Task.Run(() => reservationDAO.BulkExpireOverdueReservations());

            string? keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
            string? status = cboStatus.SelectedIndex <= 0 ? null : cboStatus.SelectedItem?.ToString();

            var data = await Task.Run(() => reservationDAO.SearchQueue(keyword, status));
            dgvQueue.DataSource = data;

            foreach (DataGridViewRow row in dgvQueue.Rows)
            {
                if (row.DataBoundItem is not ReservationQueueItem item) continue;

                if (item.Status == ReservationDAO.STATUS_PENDING)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 230);
                }
                else if (item.Status == ReservationDAO.STATUS_NOTIFIED)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(235, 255, 240);
                }
                else if (item.Status == ReservationDAO.STATUS_EXPIRED)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                }
            }

            int total = data.Count;
            int pending = data.Count(x => x.Status == ReservationDAO.STATUS_PENDING);
            int notified = data.Count(x => x.Status == ReservationDAO.STATUS_NOTIFIED);
            lblSummary.Text = $"Tổng: {total} | Chờ: {pending} | Đã thông báo: {notified}";
        }

        private ReservationQueueItem? GetSelected()
        {
            if (dgvQueue.CurrentRow?.DataBoundItem is ReservationQueueItem item)
            {
                return item;
            }
            return null;
        }
    }
}
