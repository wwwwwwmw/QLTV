using System;

namespace LibraryManagement.Models
{
    /// <summary>
    /// Model đại diện cho người dùng hệ thống
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Role { get; set; } = "Staff";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }

        // Role constants
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_MANAGER = "Manager";
        public const string ROLE_STAFF = "Staff";

        public bool IsAdmin => Role == ROLE_ADMIN;
        public bool IsManager => Role == ROLE_MANAGER;
        public bool IsStaff => Role == ROLE_STAFF;
    }

    /// <summary>
    /// Lớp quản lý phiên đăng nhập hiện tại
    /// </summary>
    public static class CurrentUser
    {
        public static User? User { get; private set; }
        public static bool IsLoggedIn => User != null;

        public static void Login(User user)
        {
            User = user;
        }

        public static void Logout()
        {
            User = null;
        }

        public static bool HasPermission(string requiredRole)
        {
            if (User == null) return false;

            return requiredRole switch
            {
                User.ROLE_STAFF => true, // Tất cả đều có quyền Staff
                User.ROLE_MANAGER => User.IsAdmin || User.IsManager,
                User.ROLE_ADMIN => User.IsAdmin,
                _ => false
            };
        }
    }
}
