using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using LibraryManagement.Models;

namespace LibraryManagement.Data
{
    /// <summary>
    /// Data Access Object cho BorrowRecord - Quản lý mượn trả sách
    /// </summary>
    public class BorrowRecordDAO
    {
        /// <summary>
        /// Lấy tất cả phiếu mượn
        /// </summary>
        public List<BorrowRecord> GetAll(string? status = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<BorrowRecord>(
                    @"SELECT br.*, m.FullName AS MemberName, m.MemberCode, m.Email AS MemberEmail,
                        b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode, u.FullName AS StaffName
                      FROM BorrowRecords br
                      INNER JOIN Members m ON br.MemberID = m.MemberID
                      INNER JOIN Books b ON br.BookID = b.BookID
                      LEFT JOIN Users u ON br.StaffID = u.UserID
                      WHERE @Status IS NULL OR br.Status = @Status
                      ORDER BY br.BorrowDate DESC",
                    new { Status = status }).AsList();
            }
        }

        /// <summary>
        /// Tìm kiếm phiếu mượn
        /// </summary>
        public List<BorrowRecord> Search(string? keyword = null, string? status = null,
            DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<BorrowRecord>(
                    @"SELECT br.*, m.FullName AS MemberName, m.MemberCode,
                        m.Email AS MemberEmail, b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode, u.FullName AS StaffName
                      FROM BorrowRecords br
                      INNER JOIN Members m ON br.MemberID = m.MemberID
                      INNER JOIN Books b ON br.BookID = b.BookID
                      LEFT JOIN Users u ON br.StaffID = u.UserID
                      WHERE (@Keyword IS NULL OR br.BorrowCode LIKE '%' + @Keyword + '%'
                             OR m.MemberCode LIKE '%' + @Keyword + '%' 
                             OR m.FullName LIKE '%' + @Keyword + '%'
                             OR b.Title LIKE '%' + @Keyword + '%'
                             OR b.ISBN LIKE '%' + @Keyword + '%'
                             OR b.Barcode LIKE '%' + @Keyword + '%')
                        AND (@Status IS NULL OR br.Status = @Status)
                        AND (@FromDate IS NULL OR br.BorrowDate >= @FromDate)
                        AND (@ToDate IS NULL OR br.BorrowDate <= @ToDate)
                      ORDER BY br.BorrowDate DESC",
                    new { Keyword = keyword, Status = status, FromDate = fromDate, ToDate = toDate }).AsList();
            }
        }

        public async Task<List<BorrowRecord>> SearchAsync(string? keyword = null, string? status = null,
            DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                var rows = await conn.QueryAsync<BorrowRecord>(
                    @"SELECT br.*, m.FullName AS MemberName, m.MemberCode,
                        m.Email AS MemberEmail, b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode, u.FullName AS StaffName
                      FROM BorrowRecords br
                      INNER JOIN Members m ON br.MemberID = m.MemberID
                      INNER JOIN Books b ON br.BookID = b.BookID
                      LEFT JOIN Users u ON br.StaffID = u.UserID
                      WHERE (@Keyword IS NULL OR br.BorrowCode LIKE '%' + @Keyword + '%'
                             OR m.MemberCode LIKE '%' + @Keyword + '%' 
                             OR m.FullName LIKE '%' + @Keyword + '%'
                             OR b.Title LIKE '%' + @Keyword + '%'
                             OR b.ISBN LIKE '%' + @Keyword + '%'
                             OR b.Barcode LIKE '%' + @Keyword + '%')
                        AND (@Status IS NULL OR br.Status = @Status)
                        AND (@FromDate IS NULL OR br.BorrowDate >= @FromDate)
                        AND (@ToDate IS NULL OR br.BorrowDate <= @ToDate)
                      ORDER BY br.BorrowDate DESC",
                    new { Keyword = keyword, Status = status, FromDate = fromDate, ToDate = toDate });
                return rows.AsList();
            }
        }

        /// <summary>
        /// Lấy phiếu mượn theo ID
        /// </summary>
        public BorrowRecord? GetById(int borrowId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<BorrowRecord>(
                    @"SELECT br.*, m.FullName AS MemberName, m.MemberCode,
                        m.Email AS MemberEmail, b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode, u.FullName AS StaffName
                      FROM BorrowRecords br
                      INNER JOIN Members m ON br.MemberID = m.MemberID
                      INNER JOIN Books b ON br.BookID = b.BookID
                      LEFT JOIN Users u ON br.StaffID = u.UserID
                      WHERE br.BorrowID = @BorrowID",
                    new { BorrowID = borrowId });
            }
        }

        public async Task<BorrowRecord?> GetByIdAsync(int borrowId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<BorrowRecord>(
                    @"SELECT br.*, m.FullName AS MemberName, m.MemberCode,
                        m.Email AS MemberEmail, b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode, u.FullName AS StaffName
                      FROM BorrowRecords br
                      INNER JOIN Members m ON br.MemberID = m.MemberID
                      INNER JOIN Books b ON br.BookID = b.BookID
                      LEFT JOIN Users u ON br.StaffID = u.UserID
                      WHERE br.BorrowID = @BorrowID",
                    new { BorrowID = borrowId });
            }
        }

        /// <summary>
        /// Lấy phiếu mượn theo mã
        /// </summary>
        public BorrowRecord? GetByCode(string borrowCode)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<BorrowRecord>(
                    @"SELECT br.*, m.FullName AS MemberName, m.MemberCode,
                        m.Email AS MemberEmail, b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode, u.FullName AS StaffName
                      FROM BorrowRecords br
                      INNER JOIN Members m ON br.MemberID = m.MemberID
                      INNER JOIN Books b ON br.BookID = b.BookID
                      LEFT JOIN Users u ON br.StaffID = u.UserID
                      WHERE br.BorrowCode = @BorrowCode",
                    new { BorrowCode = borrowCode });
            }
        }

        /// <summary>
        /// Lấy danh sách sách đang mượn của thành viên
        /// </summary>
        public List<BorrowRecord> GetMemberBorrowings(int memberId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<BorrowRecord>(
                    @"SELECT br.*, b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode
                      FROM BorrowRecords br
                      INNER JOIN Books b ON br.BookID = b.BookID
                      WHERE br.MemberID = @MemberID AND br.Status IN (N'Đang mượn', N'Quá hạn')
                      ORDER BY br.BorrowDate DESC",
                    new { MemberID = memberId }).AsList();
            }
        }

        public async Task<List<BorrowRecord>> GetMemberBorrowingsAsync(int memberId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                var rows = await conn.QueryAsync<BorrowRecord>(
                    @"SELECT br.*, b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode
                      FROM BorrowRecords br
                      INNER JOIN Books b ON br.BookID = b.BookID
                      WHERE br.MemberID = @MemberID AND br.Status IN (N'Đang mượn', N'Quá hạn')
                      ORDER BY br.BorrowDate DESC",
                    new { MemberID = memberId });
                return rows.AsList();
            }
        }

        /// <summary>
        /// Mượn sách
        /// </summary>
        public (bool Success, string Message) BorrowBook(int memberId, int bookId, int staffId, int? dueDays = null)
            => BorrowBookAsync(memberId, bookId, staffId, dueDays).GetAwaiter().GetResult();

        public async Task<(bool Success, string Message)> BorrowBookAsync(int memberId, int bookId, int staffId, int? dueDays = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string? memberType = await conn.ExecuteScalarAsync<string>(
                            "SELECT MemberType FROM Members WHERE MemberID = @MemberID",
                            new { MemberID = memberId }, transaction);
                        int maxBorrowDaysByType = Member.GetMaxBorrowDaysByType(memberType);
                        if (!dueDays.HasValue || dueDays.Value <= 0)
                        {
                            dueDays = maxBorrowDaysByType;
                        }

                        if (dueDays.Value > maxBorrowDaysByType)
                        {
                            transaction.Rollback();
                            return (false, $"Loại thẻ {memberType ?? Member.TYPE_NORMAL} chỉ được mượn tối đa {maxBorrowDaysByType} ngày");
                        }

                        int reserveAffected = await conn.ExecuteAsync(
                            @"UPDATE Books WITH (UPDLOCK, ROWLOCK)
                              SET AvailableCopies = AvailableCopies - 1, UpdatedDate = GETDATE()
                              WHERE BookID = @BookID AND AvailableCopies > 0",
                            new { BookID = bookId }, transaction);

                        if (reserveAffected <= 0)
                        {
                            transaction.Rollback();
                            return (false, "Sách đã hết, không thể mượn");
                        }

                        int overdueCount = await conn.ExecuteScalarAsync<int>(
                            "SELECT COUNT(*) FROM BorrowRecords WHERE MemberID = @MemberID AND Status = N'Quá hạn'",
                            new { MemberID = memberId }, transaction);

                        if (overdueCount > 0)
                        {
                            transaction.Rollback();
                            return (false, "Độc giả có sách quá hạn chưa trả");
                        }

                        int maxBooks = await conn.ExecuteScalarAsync<int>(
                            "SELECT CAST(SettingValue AS INT) FROM SystemSettings WHERE SettingKey = 'MaxBooksPerMember'",
                            transaction: transaction);
                        if (maxBooks == 0) maxBooks = 5;

                        int borrowingCount = await conn.ExecuteScalarAsync<int>(
                            "SELECT COUNT(*) FROM BorrowRecords WHERE MemberID = @MemberID AND Status = N'Đang mượn'",
                            new { MemberID = memberId }, transaction);

                        if (borrowingCount >= maxBooks)
                        {
                            transaction.Rollback();
                            return (false, $"Độc giả đã mượn đủ số sách tối đa ({maxBooks} cuốn)");
                        }

                        string borrowCode = GenerateBorrowCode();

                        await conn.ExecuteAsync(
                            @"INSERT INTO BorrowRecords (BorrowCode, MemberID, BookID, BorrowDate, DueDate, Status, StaffID)
                              VALUES (@BorrowCode, @MemberID, @BookID, GETDATE(), DATEADD(DAY, @DueDays, GETDATE()), N'Đang mượn', @StaffID)",
                            new { BorrowCode = borrowCode, MemberID = memberId, BookID = bookId, DueDays = dueDays, StaffID = staffId },
                            transaction);

                        transaction.Commit();
                        return (true, $"Mượn sách thành công. Mã phiếu: {borrowCode}");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return (false, $"Lỗi: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Trả sách
        /// </summary>
        public (bool Success, string Message, decimal FineAmount) ReturnBook(int borrowId, int staffId)
            => ReturnBookAsync(borrowId, staffId).GetAwaiter().GetResult();

        public async Task<(bool Success, string Message, decimal FineAmount)> ReturnBookAsync(int borrowId, int staffId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var borrow = await conn.QueryFirstOrDefaultAsync<BorrowRecord>(
                            @"SELECT * FROM BorrowRecords 
                              WHERE BorrowID = @BorrowID AND Status IN (N'Đang mượn', N'Quá hạn')",
                            new { BorrowID = borrowId }, transaction);

                        if (borrow == null)
                        {
                            transaction.Rollback();
                            return (false, "Không tìm thấy phiếu mượn hoặc sách đã được trả", 0);
                        }

                        decimal fineAmount = 0;
                        if (DateTime.Now > borrow.DueDate)
                        {
                            decimal finePerDay = await conn.ExecuteScalarAsync<decimal>(
                                "SELECT CAST(SettingValue AS DECIMAL) FROM SystemSettings WHERE SettingKey = 'FinePerDay'",
                                transaction: transaction);
                            if (finePerDay == 0) finePerDay = 10000;

                            int daysOverdue = (int)(DateTime.Now - borrow.DueDate).TotalDays;
                            fineAmount = daysOverdue * finePerDay;
                        }

                        await conn.ExecuteAsync(
                            @"UPDATE BorrowRecords SET 
                                ReturnDate = GETDATE(), Status = N'Đã trả', 
                                FineAmount = @FineAmount, UpdatedDate = GETDATE()
                              WHERE BorrowID = @BorrowID",
                            new { BorrowID = borrowId, FineAmount = fineAmount }, transaction);

                        await conn.ExecuteAsync(
                            "UPDATE Books SET AvailableCopies = AvailableCopies + 1 WHERE BookID = @BookID",
                            new { BookID = borrow.BookID }, transaction);

                        if (fineAmount > 0)
                        {
                            await conn.ExecuteAsync(
                                "UPDATE Members SET TotalFine = TotalFine + @FineAmount WHERE MemberID = @MemberID",
                                new { MemberID = borrow.MemberID, FineAmount = fineAmount }, transaction);
                        }

                        transaction.Commit();

                        string message = fineAmount > 0
                            ? $"Trả sách thành công. Tiền phạt: {fineAmount:N0} VNĐ"
                            : "Trả sách thành công";

                        return (true, message, fineAmount);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return (false, $"Lỗi: {ex.Message}", 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gia hạn sách
        /// </summary>
        public (bool Success, string Message) RenewBook(int borrowId, int additionalDays)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                var borrow = GetById(borrowId);
                if (borrow == null)
                    return (false, "Không tìm thấy phiếu mượn");

                if (borrow.Status != BorrowRecord.STATUS_BORROWING)
                    return (false, "Chỉ có thể gia hạn sách đang mượn chưa quá hạn");

                if (borrow.DueDate < DateTime.Now)
                    return (false, "Sách đã quá hạn, không thể gia hạn");

                int affected = conn.Execute(
                    @"UPDATE BorrowRecords SET 
                        DueDate = DATEADD(DAY, @AdditionalDays, DueDate), 
                        UpdatedDate = GETDATE()
                      WHERE BorrowID = @BorrowID",
                    new { BorrowID = borrowId, AdditionalDays = additionalDays });

                return affected > 0
                    ? (true, $"Gia hạn thành công {additionalDays} ngày")
                    : (false, "Không thể gia hạn");
            }
        }

        /// <summary>
        /// Cập nhật trạng thái quá hạn cho các phiếu mượn
        /// </summary>
        public int UpdateOverdueStatus()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Execute(
                    @"UPDATE BorrowRecords SET Status = N'Quá hạn'
                      WHERE Status = N'Đang mượn' AND DueDate < GETDATE()");
            }
        }

        public async Task<int> UpdateOverdueStatusAsync()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return await conn.ExecuteAsync(
                    @"UPDATE BorrowRecords SET Status = N'Quá hạn'
                      WHERE Status = N'Đang mượn' AND DueDate < GETDATE()");
            }
        }

        /// <summary>
        /// Lấy danh sách phiếu mượn quá hạn
        /// </summary>
        public List<BorrowRecord> GetOverdueRecords()
        {
            // Cập nhật trạng thái trước
            UpdateOverdueStatus();

            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<BorrowRecord>(
                    @"SELECT br.*, m.FullName AS MemberName, m.MemberCode, m.Phone, m.Email AS MemberEmail,
                        b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode
                      FROM BorrowRecords br
                      INNER JOIN Members m ON br.MemberID = m.MemberID
                      INNER JOIN Books b ON br.BookID = b.BookID
                      WHERE br.Status = N'Quá hạn'
                      ORDER BY br.DueDate").AsList();
            }
        }

        /// <summary>
        /// Thống kê mượn trả
        /// </summary>
        public DashboardStats GetDashboardStats()
        {
            // Cập nhật trạng thái quá hạn trước
            UpdateOverdueStatus();

            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<DashboardStats>(
                    @"SELECT 
                        (SELECT COUNT(*) FROM Books WHERE IsActive = 1) AS TotalBooks,
                        (SELECT ISNULL(SUM(TotalCopies), 0) FROM Books WHERE IsActive = 1) AS TotalCopies,
                        (SELECT ISNULL(SUM(AvailableCopies), 0) FROM Books WHERE IsActive = 1) AS AvailableCopies,
                        (SELECT COUNT(*) FROM Members WHERE IsActive = 1) AS TotalMembers,
                        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = N'Đang mượn') AS BorrowingCount,
                        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = N'Quá hạn') AS OverdueCount,
                        (SELECT COUNT(*) FROM BorrowRecords WHERE CAST(BorrowDate AS DATE) = CAST(GETDATE() AS DATE)) AS TodayBorrows,
                        (SELECT COUNT(*) FROM BorrowRecords WHERE CAST(ReturnDate AS DATE) = CAST(GETDATE() AS DATE)) AS TodayReturns")
                    ?? new DashboardStats();
            }
        }

        /// <summary>
        /// Lấy lịch sử mượn sách của thành viên
        /// </summary>
        public List<BorrowRecord> GetMemberHistory(int memberId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<BorrowRecord>(
                    @"SELECT br.*, b.Title AS BookTitle, b.ISBN, b.Barcode AS BookBarcode
                      FROM BorrowRecords br
                      INNER JOIN Books b ON br.BookID = b.BookID
                      WHERE br.MemberID = @MemberID
                      ORDER BY br.BorrowDate DESC",
                    new { MemberID = memberId }).AsList();
            }
        }

        /// <summary>
        /// Đếm số lượt mượn/trả theo ngày
        /// </summary>
        public int CountByDate(DateTime date, string type)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                if (type == "Borrow")
                {
                    return conn.ExecuteScalar<int>(
                        @"SELECT COUNT(*) FROM BorrowRecords 
                          WHERE CAST(BorrowDate AS DATE) = CAST(@Date AS DATE)",
                        new { Date = date });
                }
                else // Return
                {
                    return conn.ExecuteScalar<int>(
                        @"SELECT COUNT(*) FROM BorrowRecords 
                          WHERE CAST(ReturnDate AS DATE) = CAST(@Date AS DATE)",
                        new { Date = date });
                }
            }
        }

        private static string GenerateBorrowCode()
        {
            string token = Guid.NewGuid().ToString("N")[..4].ToUpperInvariant();
            return $"PM{DateTime.UtcNow:yyMMddHHmmss}{token}";
        }

        /// <summary>
        /// Lấy sách mượn nhiều nhất
        /// </summary>
        public List<MostBorrowedBook> GetMostBorrowedBooks(DateTime fromDate, DateTime toDate, int top = 20)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<MostBorrowedBook>(
                    @"SELECT TOP(@Top) b.Title AS BookTitle, 
                        a.AuthorName, b.TotalCopies AS Quantity,
                        COUNT(br.BorrowID) AS BorrowCount
                      FROM BorrowRecords br
                      INNER JOIN Books b ON br.BookID = b.BookID
                      LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                      WHERE br.BorrowDate BETWEEN @FromDate AND @ToDate
                      GROUP BY b.BookID, b.Title, a.AuthorName, b.TotalCopies
                      ORDER BY COUNT(br.BorrowID) DESC",
                    new { FromDate = fromDate, ToDate = toDate, Top = top }).AsList();
            }
        }

        /// <summary>
        /// Lấy độc giả mượn nhiều nhất
        /// </summary>
        public List<TopBorrower> GetTopBorrowers(DateTime fromDate, DateTime toDate, int top = 20)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<TopBorrower>(
                    @"SELECT TOP(@Top) m.MemberCode, m.FullName AS MemberName, m.Phone,
                        COUNT(br.BorrowID) AS BorrowCount
                      FROM BorrowRecords br
                      INNER JOIN Members m ON br.MemberID = m.MemberID
                      WHERE br.BorrowDate BETWEEN @FromDate AND @ToDate
                      GROUP BY m.MemberID, m.MemberCode, m.FullName, m.Phone
                      ORDER BY COUNT(br.BorrowID) DESC",
                    new { FromDate = fromDate, ToDate = toDate, Top = top }).AsList();
            }
        }

        /// <summary>
        /// Thống kê theo ngày
        /// </summary>
        public List<DailyStats> GetDailyStats(DateTime fromDate, DateTime toDate)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<DailyStats>(
                    @"WITH DateRange AS (
                        SELECT CAST(@FromDate AS DATE) AS [Date]
                        UNION ALL
                        SELECT DATEADD(DAY, 1, [Date])
                        FROM DateRange
                        WHERE [Date] < CAST(@ToDate AS DATE)
                      )
                      SELECT 
                        dr.[Date],
                        (SELECT COUNT(*) FROM BorrowRecords WHERE CAST(BorrowDate AS DATE) = dr.[Date]) AS BorrowCount,
                        (SELECT COUNT(*) FROM BorrowRecords WHERE CAST(ReturnDate AS DATE) = dr.[Date]) AS ReturnCount,
                        (SELECT COUNT(*) FROM Members WHERE CAST(CreatedDate AS DATE) = dr.[Date]) AS NewMembers
                      FROM DateRange dr
                      ORDER BY dr.[Date]
                      OPTION (MAXRECURSION 365)",
                    new { FromDate = fromDate, ToDate = toDate }).AsList();
            }
        }
    }

    /// <summary>
    /// Model cho sách mượn nhiều nhất
    /// </summary>
    public class MostBorrowedBook
    {
        public string BookTitle { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public int Quantity { get; set; }
        public int BorrowCount { get; set; }
    }

    /// <summary>
    /// Model cho độc giả mượn nhiều
    /// </summary>
    public class TopBorrower
    {
        public string MemberCode { get; set; } = "";
        public string MemberName { get; set; } = "";
        public string Phone { get; set; } = "";
        public int BorrowCount { get; set; }
    }

    /// <summary>
    /// Model cho thống kê ngày
    /// </summary>
    public class DailyStats
    {
        public DateTime Date { get; set; }
        public int BorrowCount { get; set; }
        public int ReturnCount { get; set; }
        public int NewMembers { get; set; }
    }
}
