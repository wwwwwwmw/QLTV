using System;

namespace LibraryManagement.Models
{
    /// <summary>
    /// Model đại diện cho cấu hình hệ thống
    /// </summary>
    public class SystemSetting
    {
        public int SettingID { get; set; }
        public string SettingKey { get; set; } = string.Empty;
        public string? SettingValue { get; set; }
        public string? Description { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // Setting key constants
        public const string KEY_MAX_BORROW_DAYS = "MaxBorrowDays";
        public const string KEY_MAX_BOOKS_PER_MEMBER = "MaxBooksPerMember";
        public const string KEY_MAX_BOOKS_PER_BORROW = "MaxBooksPerMember"; // Alias
        public const string KEY_FINE_PER_DAY = "FinePerDay";
        public const string KEY_LOST_BOOK_FINE_PERCENT = "LostBookFinePercent";
        public const string KEY_LIBRARY_NAME = "LibraryName";
        public const string KEY_LIBRARY_ADDRESS = "LibraryAddress";
        public const string KEY_LIBRARY_PHONE = "LibraryPhone";
        public const string KEY_LIBRARY_EMAIL = "LibraryEmail";
        public const string KEY_RESERVATION_DAYS = "ReservationDays";
    }

    /// <summary>
    /// Model đại diện cho log hoạt động
    /// </summary>
    public class ActivityLog
    {
        public int LogID { get; set; }
        public int? UserID { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? TableName { get; set; }
        public int? RecordID { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? IPAddress { get; set; }
        public string? ComputerName { get; set; }
        public DateTime LogDate { get; set; } = DateTime.Now;

        // Navigation properties
        public string? Username { get; set; }

        // Alias for UI
        public DateTime LogTime => LogDate;
    }

    /// <summary>
    /// Model cho thống kê Dashboard
    /// </summary>
    public class DashboardStats
    {
        public int TotalBooks { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int TotalMembers { get; set; }
        public int BorrowingCount { get; set; }
        public int OverdueCount { get; set; }
        public int TodayBorrows { get; set; }
        public int TodayReturns { get; set; }

        // Aliases for UI
        public int BorrowingBooks => BorrowingCount;
        public int OverdueBooks => OverdueCount;

        public int BorrowedCopies => TotalCopies - AvailableCopies;
        public double BorrowRate => TotalCopies > 0 ? (double)BorrowedCopies / TotalCopies * 100 : 0;
    }
}
