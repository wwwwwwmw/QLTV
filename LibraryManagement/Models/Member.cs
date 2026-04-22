using System;

namespace LibraryManagement.Models
{
    /// <summary>
    /// Model đại diện cho thành viên/độc giả thư viện
    /// </summary>
    public class Member
    {
        public int MemberID { get; set; }
        public string MemberCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? IdentityCard { get; set; }
        public string MemberType { get; set; } = "Thường";
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public DateTime? ExpiryDate { get; set; }
        public decimal TotalFine { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Member type constants
        public const string TYPE_NORMAL = "Thường";
        public const string TYPE_VIP = "VIP";
        public const string TYPE_STUDENT = "Sinh viên";
        public const string TYPE_TEACHER = "Giáo viên";

        public static int GetMaxBorrowDaysByType(string? memberType)
        {
            if (string.Equals(memberType, TYPE_VIP, StringComparison.OrdinalIgnoreCase))
                return 30;
            if (string.Equals(memberType, TYPE_TEACHER, StringComparison.OrdinalIgnoreCase))
                return 14;
            if (string.Equals(memberType, TYPE_STUDENT, StringComparison.OrdinalIgnoreCase))
                return 7;
            return 7;
        }

        // Gender constants
        public const string GENDER_MALE = "Nam";
        public const string GENDER_FEMALE = "Nữ";
        public const string GENDER_OTHER = "Khác";

        // Computed properties
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.Now;
        public bool HasFine => TotalFine > 0;
        public int? Age => DateOfBirth.HasValue ?
            (int)((DateTime.Now - DateOfBirth.Value).TotalDays / 365.25) : null;

        public string StatusDisplay
        {
            get
            {
                if (!IsActive) return "Ngừng hoạt động";
                if (IsExpired) return "Hết hạn thẻ";
                if (HasFine) return $"Nợ phạt: {TotalFine:N0} đ";
                return "Hoạt động";
            }
        }
    }
}
