using System;
using System.Collections.Generic;
using Dapper;
using LibraryManagement.Models;
using LibraryManagement.Utils;

namespace LibraryManagement.Data
{
    /// <summary>
    /// Data Access Object cho Member - Quản lý độc giả/thành viên
    /// </summary>
    public class MemberDAO
    {
        private static void ValidateMemberForPersistence(Member member)
        {
            if (string.IsNullOrWhiteSpace(member.MemberCode))
                throw new ArgumentException("Mã độc giả không được để trống.");

            if (!VietnamInputValidator.IsValidFullName(member.FullName))
                throw new ArgumentException("Họ tên độc giả không hợp lệ.");

            if (!VietnamInputValidator.IsValidVietnamMobilePhone(member.Phone))
                throw new ArgumentException("Số điện thoại độc giả phải gồm đúng 10 chữ số theo chuẩn Việt Nam.");

            if (!string.IsNullOrWhiteSpace(member.Email) && !VietnamInputValidator.IsValidEmail(member.Email))
                throw new ArgumentException("Email độc giả không đúng định dạng.");

            if (!string.IsNullOrWhiteSpace(member.IdentityCard) && !VietnamInputValidator.IsValidIdentityCard12(member.IdentityCard))
                throw new ArgumentException("Số CCCD phải gồm đúng 12 chữ số.");
        }

        /// <summary>
        /// Lấy tất cả thành viên
        /// </summary>
        public List<Member> GetAll(bool activeOnly = true)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<Member>(
                    "SELECT * FROM Members WHERE (@ActiveOnly = 0 OR IsActive = 1) ORDER BY FullName",
                    new { ActiveOnly = activeOnly }).AsList();
            }
        }

        /// <summary>
        /// Tìm kiếm thành viên
        /// </summary>
        public List<Member> Search(string? keyword = null, string? memberType = null, bool activeOnly = true)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<Member>(
                    @"SELECT * FROM Members 
                      WHERE (@ActiveOnly = 0 OR IsActive = 1)
                        AND (@Keyword IS NULL OR MemberCode LIKE '%' + @Keyword + '%' 
                             OR FullName LIKE '%' + @Keyword + '%' OR Phone LIKE '%' + @Keyword + '%')
                        AND (@MemberType IS NULL OR MemberType = @MemberType)
                      ORDER BY FullName",
                    new { Keyword = keyword, MemberType = memberType, ActiveOnly = activeOnly }).AsList();
            }
        }

        /// <summary>
        /// Lấy thành viên theo ID
        /// </summary>
        public Member? GetById(int memberId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<Member>(
                    "SELECT * FROM Members WHERE MemberID = @MemberID",
                    new { MemberID = memberId });
            }
        }

        /// <summary>
        /// Lấy thành viên theo mã thẻ
        /// </summary>
        public Member? GetByCode(string memberCode)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<Member>(
                    "SELECT * FROM Members WHERE MemberCode = @MemberCode",
                    new { MemberCode = memberCode });
            }
        }

        /// <summary>
        /// Thêm thành viên mới
        /// </summary>
        public int Insert(Member member)
        {
            ValidateMemberForPersistence(member);

            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.ExecuteScalar<int>(
                    @"INSERT INTO Members (MemberCode, FullName, Gender, DateOfBirth, Address, Phone, 
                        Email, IdentityCard, MemberType, JoinDate, ExpiryDate, Notes)
                      VALUES (@MemberCode, @FullName, @Gender, @DateOfBirth, @Address, @Phone, 
                        @Email, @IdentityCard, @MemberType, @JoinDate, @ExpiryDate, @Notes);
                      SELECT SCOPE_IDENTITY();", member);
            }
        }

        /// <summary>
        /// Cập nhật thông tin thành viên
        /// </summary>
        public bool Update(Member member)
        {
            ValidateMemberForPersistence(member);

            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE Members SET 
                        FullName = @FullName, Gender = @Gender, DateOfBirth = @DateOfBirth,
                        Address = @Address, Phone = @Phone, Email = @Email, 
                        IdentityCard = @IdentityCard, MemberType = @MemberType,
                        ExpiryDate = @ExpiryDate, Notes = @Notes, 
                        IsActive = @IsActive, UpdatedDate = GETDATE()
                      WHERE MemberID = @MemberID", member);
                return affected > 0;
            }
        }

        /// <summary>
        /// Xóa thành viên (đánh dấu không hoạt động)
        /// </summary>
        public bool Delete(int memberId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Members SET IsActive = 0, UpdatedDate = GETDATE() WHERE MemberID = @MemberID",
                    new { MemberID = memberId });
                return affected > 0;
            }
        }

        /// <summary>
        /// Kiểm tra mã thẻ đã tồn tại
        /// </summary>
        public bool MemberCodeExists(string memberCode, int? excludeMemberId = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Members WHERE MemberCode = @MemberCode";
                if (excludeMemberId.HasValue)
                    sql += " AND MemberID != @ExcludeMemberId";

                return conn.ExecuteScalar<int>(sql,
                    new { MemberCode = memberCode, ExcludeMemberId = excludeMemberId }) > 0;
            }
        }

        /// <summary>
        /// Tạo mã thẻ thành viên tự động
        /// </summary>
        public string GenerateMemberCode()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int maxId = conn.ExecuteScalar<int>("SELECT ISNULL(MAX(MemberID), 0) + 1 FROM Members");
                return $"TV{maxId:D4}";
            }
        }

        /// <summary>
        /// Cập nhật tiền phạt của thành viên
        /// </summary>
        public bool UpdateFine(int memberId, decimal fineAmount)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Members SET TotalFine = TotalFine + @FineAmount, UpdatedDate = GETDATE() WHERE MemberID = @MemberID",
                    new { MemberID = memberId, FineAmount = fineAmount });
                return affected > 0;
            }
        }

        /// <summary>
        /// Thanh toán tiền phạt
        /// </summary>
        public bool PayFine(int memberId, decimal amount)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE Members SET TotalFine = CASE WHEN TotalFine - @Amount < 0 THEN 0 ELSE TotalFine - @Amount END, 
                        UpdatedDate = GETDATE() 
                      WHERE MemberID = @MemberID",
                    new { MemberID = memberId, Amount = amount });
                return affected > 0;
            }
        }

        /// <summary>
        /// Kiểm tra thành viên có thể mượn sách không
        /// </summary>
        public (bool CanBorrow, string Message) CanBorrow(int memberId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                var member = GetById(memberId);
                if (member == null)
                    return (false, "Không tìm thấy thành viên");

                if (!member.IsActive)
                    return (false, "Thành viên đã ngừng hoạt động");

                if (member.IsExpired)
                    return (false, "Thẻ thành viên đã hết hạn");

                if (member.TotalFine > 0)
                    return (false, $"Thành viên còn nợ phạt {member.TotalFine:N0} đ, vui lòng thanh toán trước khi mượn");

                // Kiểm tra có sách quá hạn không
                int overdueCount = conn.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM BorrowRecords WHERE MemberID = @MemberID AND Status = N'Quá hạn'",
                    new { MemberID = memberId });
                if (overdueCount > 0)
                    return (false, $"Thành viên có {overdueCount} sách quá hạn chưa trả");

                // Kiểm tra số sách đang mượn
                var settingDAO = new SystemSettingDAO();
                int maxBooks = settingDAO.GetIntValue(SystemSetting.KEY_MAX_BOOKS_PER_MEMBER, 5);

                int borrowingCount = conn.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM BorrowRecords WHERE MemberID = @MemberID AND Status = N'Đang mượn'",
                    new { MemberID = memberId });

                if (borrowingCount >= maxBooks)
                    return (false, $"Thành viên đã mượn đủ số sách tối đa ({maxBooks} cuốn)");

                return (true, "OK");
            }
        }

        /// <summary>
        /// Lấy danh sách thành viên có sách quá hạn
        /// </summary>
        public List<Member> GetMembersWithOverdueBooks()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<Member>(
                    @"SELECT DISTINCT m.* FROM Members m
                      INNER JOIN BorrowRecords br ON m.MemberID = br.MemberID
                      WHERE br.Status = N'Quá hạn' AND m.IsActive = 1
                      ORDER BY m.FullName").AsList();
            }
        }
    }
}
