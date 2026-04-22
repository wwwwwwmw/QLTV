using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Dapper;
using Microsoft.Data.SqlClient;
using LibraryManagement.Models;
using LibraryManagement.Utils;

namespace LibraryManagement.Data
{
    /// <summary>
    /// Data Access Object cho User - Quản lý tài khoản người dùng
    /// </summary>
    public class UserDAO
    {
        public static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        private static (bool IsValid, string Message) ValidateUserData(User user, bool validateUsername)
        {
            if (validateUsername && !VietnamInputValidator.IsValidUsername(user.Username))
                return (false, "Tên đăng nhập chỉ gồm chữ, số, dấu chấm hoặc gạch dưới, dài 4-30 ký tự.");

            if (!VietnamInputValidator.IsValidFullName(user.FullName))
                return (false, "Họ tên không hợp lệ.");

            if (!string.IsNullOrWhiteSpace(user.Email) && !VietnamInputValidator.IsValidEmail(user.Email))
                return (false, "Email không đúng định dạng.");

            if (!string.IsNullOrWhiteSpace(user.Phone) && !VietnamInputValidator.IsValidVietnamMobilePhone(user.Phone))
                return (false, "Số điện thoại phải gồm đúng 10 chữ số theo chuẩn Việt Nam.");

            return (true, string.Empty);
        }

        private static string HashPasswordLegacySha256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            if (storedHash.StartsWith("$2", StringComparison.Ordinal))
            {
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }

            return string.Equals(HashPasswordLegacySha256(password), storedHash, StringComparison.Ordinal);
        }

        /// <summary>
        /// Đăng nhập hệ thống
        /// </summary>
        public User? Login(string username, string password)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                var auth = conn.QueryFirstOrDefault<(int UserID, string PasswordHash)>(
                    @"SELECT UserID, Password AS PasswordHash
                      FROM Users
                      WHERE Username = @Username AND IsActive = 1",
                    new { Username = username });

                if (auth.UserID == 0 || !VerifyPassword(password, auth.PasswordHash))
                {
                    return null;
                }

                if (!auth.PasswordHash.StartsWith("$2", StringComparison.Ordinal))
                {
                    string upgradedHash = HashPassword(password);
                    conn.Execute("UPDATE Users SET Password = @Password WHERE UserID = @UserID",
                        new { UserID = auth.UserID, Password = upgradedHash });
                }

                var user = conn.QueryFirstOrDefault<User>(
                    @"SELECT UserID, Username, FullName, Email, Phone, Role, IsActive, CreatedDate, LastLogin
                      FROM Users
                      WHERE UserID = @UserID",
                    new { UserID = auth.UserID });

                if (user != null)
                {
                    // Cập nhật thời gian đăng nhập
                    conn.Execute("UPDATE Users SET LastLogin = GETDATE() WHERE UserID = @UserID",
                        new { user.UserID });
                }

                return user;
            }
        }

        /// <summary>
        /// Lấy tất cả người dùng
        /// </summary>
        public List<User> GetAll()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<User>(
                    "SELECT * FROM Users ORDER BY FullName").AsList();
            }
        }

        /// <summary>
        /// Lấy người dùng theo ID
        /// </summary>
        public User? GetById(int userId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE UserID = @UserID",
                    new { UserID = userId });
            }
        }

        public User? GetByUsername(string username)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE Username = @Username",
                    new { Username = username });
            }
        }

        /// <summary>
        /// Tìm kiếm người dùng
        /// </summary>
        public List<User> Search(string? keyword = null, string? role = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<User>(
                    @"SELECT * FROM Users 
                      WHERE (@Keyword IS NULL OR Username LIKE '%' + @Keyword + '%' OR FullName LIKE '%' + @Keyword + '%')
                        AND (@Role IS NULL OR Role = @Role)
                      ORDER BY FullName",
                    new { Keyword = keyword, Role = role }).AsList();
            }
        }

        /// <summary>
        /// Kiểm tra username đã tồn tại
        /// </summary>
        public bool UsernameExists(string username, int? excludeUserId = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                if (excludeUserId.HasValue)
                    sql += " AND UserID != @ExcludeUserId";

                return conn.ExecuteScalar<int>(sql,
                    new { Username = username, ExcludeUserId = excludeUserId }) > 0;
            }
        }

        /// <summary>
        /// Thêm người dùng mới
        /// </summary>
        public (bool Success, string Message) Add(User user, string password)
        {
            var (valid, validationMessage) = ValidateUserData(user, validateUsername: true);
            if (!valid)
            {
                return (false, validationMessage);
            }

            if (!VietnamInputValidator.IsStrongPassword(password))
            {
                return (false, "Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ và số.");
            }

            if (UsernameExists(user.Username))
            {
                return (false, "Tên đăng nhập đã tồn tại!");
            }

            user.Password = HashPassword(password);

            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Execute(
                    @"INSERT INTO Users (Username, Password, FullName, Email, Phone, Role, IsActive)
                      VALUES (@Username, @Password, @FullName, @Email, @Phone, @Role, @IsActive)", user);
                return (true, "Thêm người dùng thành công!");
            }
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        public (bool Success, string Message) Update(User user)
        {
            var (valid, validationMessage) = ValidateUserData(user, validateUsername: false);
            if (!valid)
            {
                return (false, validationMessage);
            }

            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE Users SET 
                        FullName = @FullName, Email = @Email, Phone = @Phone, 
                        Role = @Role, IsActive = @IsActive
                      WHERE UserID = @UserID", user);
                return affected > 0
                    ? (true, "Cập nhật thành công!")
                    : (false, "Không tìm thấy người dùng!");
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        public (bool Success, string Message) ChangePassword(int userId, string newPassword)
        {
            if (!VietnamInputValidator.IsStrongPassword(newPassword))
            {
                return (false, "Mật khẩu mới phải có ít nhất 8 ký tự, bao gồm chữ và số.");
            }

            string hashedPassword = HashPassword(newPassword);

            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Users SET Password = @Password WHERE UserID = @UserID",
                    new { UserID = userId, Password = hashedPassword });
                return affected > 0
                    ? (true, "Đổi mật khẩu thành công!")
                    : (false, "Không tìm thấy người dùng!");
            }
        }

        /// <summary>
        /// Đổi mật khẩu với xác thực mật khẩu cũ
        /// </summary>
        public bool ChangePasswordWithVerification(int userId, string oldPassword, string newPassword)
        {
            string hashedOldPassword = HashPassword(oldPassword);
            string hashedNewPassword = HashPassword(newPassword);

            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE Users SET Password = @NewPassword 
                      WHERE UserID = @UserID AND Password = @OldPassword",
                    new { UserID = userId, OldPassword = hashedOldPassword, NewPassword = hashedNewPassword });
                return affected > 0;
            }
        }

        /// <summary>
        /// Xóa người dùng (đánh dấu không hoạt động)
        /// </summary>
        public (bool Success, string Message) Delete(int userId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Users SET IsActive = 0 WHERE UserID = @UserID",
                    new { UserID = userId });
                return affected > 0
                    ? (true, "Xóa thành công!")
                    : (false, "Không tìm thấy người dùng!");
            }
        }
    }
}
