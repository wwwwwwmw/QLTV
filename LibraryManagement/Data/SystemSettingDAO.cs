using System;
using System.Collections.Generic;
using Dapper;
using LibraryManagement.Models;

namespace LibraryManagement.Data
{
    /// <summary>
    /// Data Access Object cho SystemSetting - Quản lý cấu hình hệ thống
    /// </summary>
    public class SystemSettingDAO
    {
        /// <summary>
        /// Lấy tất cả cấu hình
        /// </summary>
        public List<SystemSetting> GetAll()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<SystemSetting>(
                    "SELECT * FROM SystemSettings ORDER BY SettingKey").AsList();
            }
        }

        /// <summary>
        /// Lấy giá trị cấu hình theo key
        /// </summary>
        public string? GetValue(string key)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.ExecuteScalar<string>(
                    "SELECT SettingValue FROM SystemSettings WHERE SettingKey = @Key",
                    new { Key = key });
            }
        }

        /// <summary>
        /// Lấy giá trị cấu hình dạng số nguyên
        /// </summary>
        public int GetIntValue(string key, int defaultValue = 0)
        {
            string? value = GetValue(key);
            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        /// <summary>
        /// Lấy giá trị cấu hình dạng số thực
        /// </summary>
        public decimal GetDecimalValue(string key, decimal defaultValue = 0)
        {
            string? value = GetValue(key);
            return decimal.TryParse(value, out decimal result) ? result : defaultValue;
        }

        /// <summary>
        /// Cập nhật giá trị cấu hình
        /// </summary>
        public bool UpdateValue(string key, string value)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE SystemSettings SET SettingValue = @Value, UpdatedDate = GETDATE() 
                      WHERE SettingKey = @Key",
                    new { Key = key, Value = value });

                if (affected == 0)
                {
                    // Nếu chưa có thì thêm mới
                    affected = conn.Execute(
                        @"INSERT INTO SystemSettings (SettingKey, SettingValue) VALUES (@Key, @Value)",
                        new { Key = key, Value = value });
                }

                return affected > 0;
            }
        }

        /// <summary>
        /// Cập nhật nhiều cấu hình cùng lúc
        /// </summary>
        public bool UpdateValues(Dictionary<string, string> settings)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var setting in settings)
                        {
                            conn.Execute(
                                @"UPDATE SystemSettings SET SettingValue = @Value, UpdatedDate = GETDATE() 
                                  WHERE SettingKey = @Key",
                                new { Key = setting.Key, Value = setting.Value },
                                transaction);
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Lấy giá trị cấu hình với default value
        /// </summary>
        public string GetValue(string key, string defaultValue)
        {
            return GetValue(key) ?? defaultValue;
        }

        /// <summary>
        /// Lưu giá trị cấu hình (alias cho UpdateValue)
        /// </summary>
        public bool SaveValue(string key, string value)
        {
            return UpdateValue(key, value);
        }
    }

    /// <summary>
    /// Data Access Object cho ActivityLog - Quản lý log hoạt động
    /// </summary>
    public class ActivityLogDAO
    {
        /// <summary>
        /// Thêm log hoạt động
        /// </summary>
        public void Log(string action, string? tableName = null, int? recordId = null,
            string? oldValue = null, string? newValue = null)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Execute(
                        @"INSERT INTO ActivityLogs (UserID, Action, TableName, RecordID, OldValue, NewValue, IPAddress, ComputerName)
                          VALUES (@UserID, @Action, @TableName, @RecordID, @OldValue, @NewValue, @IPAddress, @ComputerName)",
                        new
                        {
                            UserID = CurrentUser.User?.UserID,
                            Action = action,
                            TableName = tableName,
                            RecordID = recordId,
                            OldValue = oldValue,
                            NewValue = newValue,
                            IPAddress = GetLocalIPAddress(),
                            ComputerName = Environment.MachineName
                        });
                }
            }
            catch
            {
                // Ignore logging errors
            }
        }

        /// <summary>
        /// Lấy log hoạt động gần đây
        /// </summary>
        public List<ActivityLog> GetRecentLogs(int count = 100)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<ActivityLog>(
                    @"SELECT TOP(@Count) al.*, u.Username
                      FROM ActivityLogs al
                      LEFT JOIN Users u ON al.UserID = u.UserID
                      ORDER BY al.LogDate DESC",
                    new { Count = count }).AsList();
            }
        }

        /// <summary>
        /// Xóa log cũ
        /// </summary>
        public int ClearOldLogs(int daysToKeep = 30)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Execute(
                    @"DELETE FROM ActivityLogs WHERE LogDate < DATEADD(DAY, -@Days, GETDATE())",
                    new { Days = daysToKeep });
            }
        }

        /// <summary>
        /// Lấy log hoạt động
        /// </summary>
        public List<ActivityLog> GetLogs(DateTime? fromDate = null, DateTime? toDate = null,
            int? userId = null, string? action = null, int maxRecords = 1000)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<ActivityLog>(
                    @"SELECT TOP(@MaxRecords) al.*, u.Username
                      FROM ActivityLogs al
                      LEFT JOIN Users u ON al.UserID = u.UserID
                      WHERE (@FromDate IS NULL OR al.LogDate >= @FromDate)
                        AND (@ToDate IS NULL OR al.LogDate <= @ToDate)
                        AND (@UserID IS NULL OR al.UserID = @UserID)
                        AND (@Action IS NULL OR al.Action LIKE '%' + @Action + '%')
                      ORDER BY al.LogDate DESC",
                    new { MaxRecords = maxRecords, FromDate = fromDate, ToDate = toDate, UserID = userId, Action = action }).AsList();
            }
        }

        /// <summary>
        /// Lấy địa chỉ IP local
        /// </summary>
        private string GetLocalIPAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch { }
            return "127.0.0.1";
        }
    }

    /// <summary>
    /// Data Access Object cho FinePayment - Quản lý thanh toán tiền phạt
    /// </summary>
    public class FinePaymentDAO
    {
        /// <summary>
        /// Thêm thanh toán tiền phạt
        /// </summary>
        public int Insert(FinePayment payment)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Thêm thanh toán
                        int paymentId = conn.ExecuteScalar<int>(
                            @"INSERT INTO FinePayments (MemberID, BorrowID, Amount, PaymentMethod, Notes, StaffID)
                              VALUES (@MemberID, @BorrowID, @Amount, @PaymentMethod, @Notes, @StaffID);
                              SELECT SCOPE_IDENTITY();",
                            payment, transaction);

                        // Cập nhật tiền phạt của thành viên
                        conn.Execute(
                            @"UPDATE Members SET TotalFine = CASE WHEN TotalFine - @Amount < 0 THEN 0 ELSE TotalFine - @Amount END
                              WHERE MemberID = @MemberID",
                            new { MemberID = payment.MemberID, Amount = payment.Amount },
                            transaction);

                        // Nếu thanh toán cho phiếu mượn cụ thể, đánh dấu đã đóng phạt
                        if (payment.BorrowID.HasValue)
                        {
                            conn.Execute(
                                "UPDATE BorrowRecords SET FinePaid = 1 WHERE BorrowID = @BorrowID",
                                new { BorrowID = payment.BorrowID },
                                transaction);
                        }

                        transaction.Commit();
                        return paymentId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Lấy lịch sử thanh toán của thành viên
        /// </summary>
        public List<FinePayment> GetMemberPayments(int memberId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<FinePayment>(
                    @"SELECT fp.*, u.FullName AS StaffName
                      FROM FinePayments fp
                      LEFT JOIN Users u ON fp.StaffID = u.UserID
                      WHERE fp.MemberID = @MemberID
                      ORDER BY fp.PaymentDate DESC",
                    new { MemberID = memberId }).AsList();
            }
        }

        /// <summary>
        /// Lấy tất cả thanh toán
        /// </summary>
        public List<FinePayment> GetAll(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<FinePayment>(
                    @"SELECT fp.*, m.FullName AS MemberName, u.FullName AS StaffName
                      FROM FinePayments fp
                      INNER JOIN Members m ON fp.MemberID = m.MemberID
                      LEFT JOIN Users u ON fp.StaffID = u.UserID
                      WHERE (@FromDate IS NULL OR fp.PaymentDate >= @FromDate)
                        AND (@ToDate IS NULL OR fp.PaymentDate <= @ToDate)
                      ORDER BY fp.PaymentDate DESC",
                    new { FromDate = fromDate, ToDate = toDate }).AsList();
            }
        }
    }

    /// <summary>
    /// Data Access Object cho Reservation - Quản lý đặt trước sách
    /// </summary>
    public class ReservationDAO
    {
        public const string STATUS_PENDING = "Chờ";
        public const string STATUS_NOTIFIED = "Đã thông báo";
        public const string STATUS_CANCELLED = "Hủy";
        public const string STATUS_FULFILLED = "Đã nhận";
        public const string STATUS_EXPIRED = "Hết hạn";

        /// <summary>
        /// Đặt trước sách
        /// </summary>
        public (bool Success, string Message) Reserve(int memberId, int bookId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                // Kiểm tra sách đã có người đặt trước chưa
                int existingCount = conn.ExecuteScalar<int>(
                    @"SELECT COUNT(*) FROM Reservations 
                      WHERE MemberID = @MemberID AND BookID = @BookID AND Status = N'Chờ'",
                    new { MemberID = memberId, BookID = bookId });

                if (existingCount > 0)
                    return (false, "Bạn đã đặt trước sách này rồi");

                // Lấy số ngày giữ đặt trước
                int reservationDays = conn.ExecuteScalar<int>(
                    "SELECT CAST(SettingValue AS INT) FROM SystemSettings WHERE SettingKey = 'ReservationDays'");
                if (reservationDays == 0) reservationDays = 3;

                conn.Execute(
                    @"INSERT INTO Reservations (MemberID, BookID, ExpiryDate, Status)
                      VALUES (@MemberID, @BookID, DATEADD(DAY, @Days, GETDATE()), N'Chờ')",
                    new { MemberID = memberId, BookID = bookId, Days = reservationDays });

                return (true, "Đặt trước sách thành công");
            }
        }

        public (bool Success, string Message) ReserveBook(int memberId, int bookId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int available = conn.ExecuteScalar<int>(
                    "SELECT AvailableCopies FROM Books WHERE BookID = @BookID",
                    new { BookID = bookId });

                if (available > 0)
                    return (false, "Sách hiện đang có sẵn, vui lòng mượn trực tiếp.");

                return Reserve(memberId, bookId);
            }
        }

        /// <summary>
        /// Hủy đặt trước
        /// </summary>
        public bool Cancel(int reservationId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Reservations SET Status = N'Hủy' WHERE ReservationID = @ReservationID",
                    new { ReservationID = reservationId });
                return affected > 0;
            }
        }

        /// <summary>
        /// Xác nhận đã nhận sách đặt trước
        /// </summary>
        public bool Fulfill(int reservationId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Reservations SET Status = N'Đã nhận' WHERE ReservationID = @ReservationID",
                    new { ReservationID = reservationId });
                return affected > 0;
            }
        }

        /// <summary>
        /// Lấy danh sách đặt trước của thành viên
        /// </summary>
        public List<Reservation> GetMemberReservations(int memberId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<Reservation>(
                    @"SELECT r.*, b.Title AS BookTitle
                      FROM Reservations r
                      INNER JOIN Books b ON r.BookID = b.BookID
                      WHERE r.MemberID = @MemberID
                      ORDER BY r.ReservationDate DESC",
                    new { MemberID = memberId }).AsList();
            }
        }

        /// <summary>
        /// Lấy danh sách đặt trước của sách
        /// </summary>
        public List<Reservation> GetBookReservations(int bookId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<Reservation>(
                    @"SELECT r.*, m.FullName AS MemberName
                      FROM Reservations r
                      INNER JOIN Members m ON r.MemberID = m.MemberID
                      WHERE r.BookID = @BookID AND r.Status = N'Chờ'
                      ORDER BY r.ReservationDate",
                    new { BookID = bookId }).AsList();
            }
        }

        public List<ReservationNotifyItem> GetPendingReservationsForBook(int bookId, int take = 50)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<ReservationNotifyItem>(
                    @"SELECT TOP (@Take)
                            r.ReservationID,
                            r.MemberID,
                            r.BookID,
                            m.FullName AS MemberName,
                            m.Email AS MemberEmail,
                            b.Title AS BookTitle,
                            r.ReservationDate AS ReservedDate
                      FROM Reservations r
                      INNER JOIN Members m ON r.MemberID = m.MemberID
                      INNER JOIN Books b ON r.BookID = b.BookID
                      WHERE r.BookID = @BookID
                        AND r.Status = N'Chờ'
                        AND m.IsActive = 1
                        AND m.Email IS NOT NULL
                        AND LTRIM(RTRIM(m.Email)) <> ''
                      ORDER BY r.ReservationDate ASC",
                    new { BookID = bookId, Take = take }).AsList();
            }
        }

        public void MarkAsNotified(int reservationId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Execute(
                    "UPDATE Reservations SET Status = N'Đã thông báo' WHERE ReservationID = @ReservationID",
                    new { ReservationID = reservationId });
            }
        }

        public List<ReservationQueueItem> SearchQueue(string? keyword = null, string? status = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<ReservationQueueItem>(
                    @"SELECT r.ReservationID,
                             r.MemberID,
                             r.BookID,
                             r.ReservationDate,
                             r.ExpiryDate,
                             r.Status,
                             r.Notes,
                             m.MemberCode,
                             m.FullName AS MemberName,
                             m.Email AS MemberEmail,
                             b.Title AS BookTitle,
                             b.ISBN
                      FROM Reservations r
                      INNER JOIN Members m ON r.MemberID = m.MemberID
                      INNER JOIN Books b ON r.BookID = b.BookID
                      WHERE (@Keyword IS NULL
                             OR m.MemberCode LIKE '%' + @Keyword + '%'
                             OR m.FullName LIKE '%' + @Keyword + '%'
                             OR m.Email LIKE '%' + @Keyword + '%'
                             OR b.Title LIKE '%' + @Keyword + '%'
                             OR b.ISBN LIKE '%' + @Keyword + '%')
                        AND (@Status IS NULL OR r.Status = @Status)
                      ORDER BY
                        CASE WHEN r.Status = N'Chờ' THEN 0 ELSE 1 END,
                        r.ReservationDate ASC",
                    new { Keyword = keyword, Status = status }).AsList();
            }
        }

        public bool UpdateStatus(int reservationId, string status)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Reservations SET Status = @Status WHERE ReservationID = @ReservationID",
                    new { ReservationID = reservationId, Status = status });
                return affected > 0;
            }
        }

        public int BulkExpireOverdueReservations()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Execute(
                    @"UPDATE Reservations
                      SET Status = N'Hết hạn'
                      WHERE Status IN (N'Chờ', N'Đã thông báo')
                        AND ExpiryDate IS NOT NULL
                        AND ExpiryDate < GETDATE()");
            }
        }

        /// <summary>
        /// Cập nhật trạng thái hết hạn cho các đặt trước
        /// </summary>
        public int UpdateExpiredStatus()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Execute(
                    @"UPDATE Reservations SET Status = N'Hết hạn'
                      WHERE Status = N'Chờ' AND ExpiryDate < GETDATE()");
            }
        }
    }

    public class ReservationNotifyItem
    {
        public int ReservationID { get; set; }
        public int MemberID { get; set; }
        public int BookID { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string MemberEmail { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public DateTime ReservedDate { get; set; }
    }

    public class ReservationQueueItem
    {
        public int ReservationID { get; set; }
        public int MemberID { get; set; }
        public int BookID { get; set; }
        public string MemberCode { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public string MemberEmail { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
