using System;

namespace LibraryManagement.Models
{
    /// <summary>
    /// Model đại diện cho phiếu mượn sách
    /// </summary>
    public class BorrowRecord
    {
        public int BorrowID { get; set; }
        public string BorrowCode { get; set; } = string.Empty;
        public int MemberID { get; set; }
        public int BookID { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int Quantity { get; set; } = 1;
        public string Status { get; set; } = STATUS_BORROWING;
        public decimal FineAmount { get; set; }
        public bool FinePaid { get; set; }
        public string? Notes { get; set; }
        public int? StaffID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Status constants
        public const string STATUS_BORROWING = "Đang mượn";
        public const string STATUS_RETURNED = "Đã trả";
        public const string STATUS_OVERDUE = "Quá hạn";
        public const string STATUS_LOST = "Mất sách";

        // Navigation properties (for display)
        public string? MemberName { get; set; }
        public string? MemberCode { get; set; }
        public string? MemberEmail { get; set; }
        public string? BookTitle { get; set; }
        public string? ISBN { get; set; }
        public string? BookBarcode { get; set; }
        public string? StaffName { get; set; }

        // Computed properties
        public bool IsOverdue => Status == STATUS_BORROWING && DueDate < DateTime.Now;
        public bool IsReturned => ReturnDate.HasValue;
        public int DaysOverdue => IsOverdue ? (int)(DateTime.Now - DueDate).TotalDays : 0;
        public int DaysRemaining => !IsReturned && DueDate > DateTime.Now ?
            (int)(DueDate - DateTime.Now).TotalDays : 0;

        public string StatusDisplay
        {
            get
            {
                if (IsReturned) return STATUS_RETURNED;
                if (IsOverdue) return $"{STATUS_OVERDUE} ({DaysOverdue} ngày)";
                return $"{STATUS_BORROWING} (còn {DaysRemaining} ngày)";
            }
        }
    }

    /// <summary>
    /// Model đại diện cho đặt trước sách
    /// </summary>
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int MemberID { get; set; }
        public int BookID { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; } = STATUS_PENDING;
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Status constants
        public const string STATUS_PENDING = "Chờ";
        public const string STATUS_FULFILLED = "Đã nhận";
        public const string STATUS_CANCELLED = "Hủy";
        public const string STATUS_EXPIRED = "Hết hạn";

        // Navigation properties
        public string? MemberName { get; set; }
        public string? BookTitle { get; set; }
    }

    /// <summary>
    /// Model đại diện cho thanh toán tiền phạt
    /// </summary>
    public class FinePayment
    {
        public int PaymentID { get; set; }
        public int MemberID { get; set; }
        public int? BorrowID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; } = METHOD_CASH;
        public string? Notes { get; set; }
        public int? StaffID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Payment method constants
        public const string METHOD_CASH = "Tiền mặt";
        public const string METHOD_TRANSFER = "Chuyển khoản";

        // Navigation properties
        public string? MemberName { get; set; }
        public string? StaffName { get; set; }
    }
}
