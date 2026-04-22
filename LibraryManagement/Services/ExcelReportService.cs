using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using ClosedXML.Excel;
using Dapper;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Services
{
    public sealed class ExcelReportService
    {
        public const string SheetMembers = "DocGia";
        public const string SheetBooks = "Sach";
        public const string SheetBorrows = "Muon";
        public const string SheetReturns = "Tra";
        public const string SheetOverdue = "TreHan";
        public const string SheetUnpaidFines = "PhatChuaThu";
        public const string SheetOutOfStock = "SachHet";
        public const string SheetTopBooks = "TopSachMuon";
        public const string SheetTopMembers = "TopDocGiaMuon";
        public const string SheetDailyStats = "ThongKeNgay";

        private static readonly string[] MembersHeaders =
        {
            "MaDocGia", "HoTen", "GioiTinh", "NgaySinh", "DiaChi", "DienThoai", "Email",
            "CCCD", "LoaiThe", "NgayThamGia", "NgayHetHan", "TongNoPhat", "HoatDong", "GhiChu"
        };

        private static readonly string[] BooksHeaders =
        {
            "ISBN", "MaVach", "TieuDe", "TheLoai", "TacGia", "NhaXuatBan", "NamXuatBan",
            "Gia", "TongBan", "BanCoSan", "ViTri", "MoTa", "HoatDong"
        };

        private static readonly string[] BorrowHeaders =
        {
            "MaPhieu", "MaDocGia", "ISBN", "MaVach", "TieuDe", "NgayMuon", "HanTra", "NgayTra",
            "SoLuong", "TrangThai", "TienPhat", "DaDongPhat", "GhiChu", "NhanVien"
        };

        private static readonly Dictionary<string, string[]> ImportableSheetHeaders = new(StringComparer.OrdinalIgnoreCase)
        {
            [SheetMembers] = MembersHeaders,
            [SheetBooks] = BooksHeaders,
            [SheetBorrows] = BorrowHeaders,
            [SheetReturns] = BorrowHeaders,
            [SheetOverdue] = BorrowHeaders
        };

        private static readonly Dictionary<string, string> SheetDisplayNames = new(StringComparer.OrdinalIgnoreCase)
        {
            [SheetMembers] = "Danh sách độc giả",
            [SheetBooks] = "Danh sách sách",
            [SheetBorrows] = "Danh sách mượn",
            [SheetReturns] = "Danh sách trả",
            [SheetOverdue] = "Danh sách trễ hạn",
            [SheetUnpaidFines] = "Danh sách phạt chưa thu",
            [SheetOutOfStock] = "Danh sách sách hết",
            [SheetTopBooks] = "Top sách mượn",
            [SheetTopMembers] = "Top độc giả mượn",
            [SheetDailyStats] = "Thống kê theo ngày"
        };

        public sealed class ImportResult
        {
            public int InsertedCount { get; set; }
            public int UpdatedCount { get; set; }
            public List<string> Warnings { get; } = new();
            public bool HasErrors => Warnings.Count > 0;
        }

        public static IReadOnlyDictionary<string, string> GetAvailableSheetOptions() => SheetDisplayNames;

        public void ExportWorkbook(string filePath, DateTime fromDate, DateTime toDate, IReadOnlyCollection<string> selectedSheets)
        {
            if (selectedSheets == null || selectedSheets.Count == 0)
            {
                throw new InvalidOperationException("Bạn chưa chọn dữ liệu cần xuất.");
            }

            DateTime from = fromDate.Date;
            DateTime to = toDate.Date.AddDays(1).AddTicks(-1);

            using var workbook = new XLWorkbook();

            if (selectedSheets.Contains(SheetMembers, StringComparer.OrdinalIgnoreCase))
                ExportMembersSheet(workbook, from, to);

            if (selectedSheets.Contains(SheetBooks, StringComparer.OrdinalIgnoreCase))
                ExportBooksSheet(workbook, from, to);

            if (selectedSheets.Contains(SheetBorrows, StringComparer.OrdinalIgnoreCase))
                ExportBorrowsSheet(workbook, from, to);

            if (selectedSheets.Contains(SheetReturns, StringComparer.OrdinalIgnoreCase))
                ExportReturnsSheet(workbook, from, to);

            if (selectedSheets.Contains(SheetOverdue, StringComparer.OrdinalIgnoreCase))
                ExportOverdueSheet(workbook, from, to);

            if (selectedSheets.Contains(SheetUnpaidFines, StringComparer.OrdinalIgnoreCase))
                ExportUnpaidFinesSheet(workbook);

            if (selectedSheets.Contains(SheetOutOfStock, StringComparer.OrdinalIgnoreCase))
                ExportOutOfStockSheet(workbook);

            if (selectedSheets.Contains(SheetTopBooks, StringComparer.OrdinalIgnoreCase))
                ExportTopBooksSheet(workbook, from, to);

            if (selectedSheets.Contains(SheetTopMembers, StringComparer.OrdinalIgnoreCase))
                ExportTopMembersSheet(workbook, from, to);

            if (selectedSheets.Contains(SheetDailyStats, StringComparer.OrdinalIgnoreCase))
                ExportDailyStatsSheet(workbook, fromDate, toDate);

            workbook.SaveAs(filePath);
        }

        public ImportResult ImportWorkbook(string filePath)
        {
            var result = new ImportResult();

            using var workbook = new XLWorkbook(filePath);
            var availableImportSheets = workbook.Worksheets
                .Where(ws => ImportableSheetHeaders.ContainsKey(ws.Name))
                .ToList();

            if (availableImportSheets.Count == 0)
            {
                result.Warnings.Add("Không tìm thấy sheet hợp lệ để nhập. Sheet hỗ trợ: DocGia, Sach, Muon, Tra, TreHan.");
                return result;
            }

            foreach (var worksheet in availableImportSheets)
            {
                ValidateSheetHeaders(worksheet, result.Warnings);
            }

            if (result.Warnings.Count > 0)
                return result;

            using var conn = DatabaseConnection.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                var context = new ImportContext(conn, transaction);

                foreach (var worksheet in availableImportSheets)
                {
                    switch (worksheet.Name)
                    {
                        case SheetMembers:
                            ImportMembersSheet(worksheet, context, result);
                            break;
                        case SheetBooks:
                            ImportBooksSheet(worksheet, context, result);
                            break;
                        case SheetBorrows:
                            ImportBorrowLikeSheet(worksheet, context, result, null);
                            break;
                        case SheetReturns:
                            ImportBorrowLikeSheet(worksheet, context, result, BorrowRecord.STATUS_RETURNED);
                            break;
                        case SheetOverdue:
                            ImportBorrowLikeSheet(worksheet, context, result, BorrowRecord.STATUS_OVERDUE);
                            break;
                    }
                }

                RecalculateAvailableCopies(conn, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result.Warnings.Add($"Lỗi hệ thống, đã hủy nhập dữ liệu: {ex.Message}");
            }

            return result;
        }

        private static void ExportMembersSheet(XLWorkbook workbook, DateTime from, DateTime to)
        {
            using var conn = DatabaseConnection.GetConnection();
            var rows = conn.Query(@"
                SELECT MemberCode, FullName, Gender, DateOfBirth, Address, Phone, Email, IdentityCard,
                       MemberType, JoinDate, ExpiryDate, TotalFine, IsActive, Notes
                FROM Members
                WHERE CreatedDate BETWEEN @FromDate AND @ToDate
                ORDER BY FullName", new { FromDate = from, ToDate = to }).ToList();

            var ws = workbook.Worksheets.Add(SheetMembers);
            WriteHeader(ws, MembersHeaders);

            int r = 2;
            foreach (var row in rows)
            {
                SetCellValue(ws.Cell(r, 1), row.MemberCode);
                SetCellValue(ws.Cell(r, 2), row.FullName);
                SetCellValue(ws.Cell(r, 3), row.Gender);
                SetCellValue(ws.Cell(r, 4), row.DateOfBirth);
                SetCellValue(ws.Cell(r, 5), row.Address);
                SetCellValue(ws.Cell(r, 6), row.Phone);
                SetCellValue(ws.Cell(r, 7), row.Email);
                SetCellValue(ws.Cell(r, 8), row.IdentityCard);
                SetCellValue(ws.Cell(r, 9), row.MemberType);
                SetCellValue(ws.Cell(r, 10), row.JoinDate);
                SetCellValue(ws.Cell(r, 11), row.ExpiryDate);
                SetCellValue(ws.Cell(r, 12), row.TotalFine);
                SetCellValue(ws.Cell(r, 13), (row.IsActive is bool active && active) ? "1" : "0");
                SetCellValue(ws.Cell(r, 14), row.Notes);
                r++;
            }

            FormatSheet(ws);
        }

        private static void ExportBooksSheet(XLWorkbook workbook, DateTime from, DateTime to)
        {
            using var conn = DatabaseConnection.GetConnection();
            var rows = conn.Query(@"
                SELECT b.ISBN, b.Barcode, b.Title,
                       c.CategoryName, a.AuthorName, p.PublisherName,
                       b.PublishYear, b.Price, b.TotalCopies, b.AvailableCopies,
                       b.Location, b.Description, b.IsActive
                FROM Books b
                LEFT JOIN Categories c ON c.CategoryID = b.CategoryID
                LEFT JOIN Authors a ON a.AuthorID = b.AuthorID
                LEFT JOIN Publishers p ON p.PublisherID = b.PublisherID
                WHERE b.CreatedDate BETWEEN @FromDate AND @ToDate
                ORDER BY b.Title", new { FromDate = from, ToDate = to }).ToList();

            var ws = workbook.Worksheets.Add(SheetBooks);
            WriteHeader(ws, BooksHeaders);

            int r = 2;
            foreach (var row in rows)
            {
                SetCellValue(ws.Cell(r, 1), row.ISBN);
                SetCellValue(ws.Cell(r, 2), row.Barcode);
                SetCellValue(ws.Cell(r, 3), row.Title);
                SetCellValue(ws.Cell(r, 4), row.CategoryName);
                SetCellValue(ws.Cell(r, 5), row.AuthorName);
                SetCellValue(ws.Cell(r, 6), row.PublisherName);
                SetCellValue(ws.Cell(r, 7), row.PublishYear);
                SetCellValue(ws.Cell(r, 8), row.Price);
                SetCellValue(ws.Cell(r, 9), row.TotalCopies);
                SetCellValue(ws.Cell(r, 10), row.AvailableCopies);
                SetCellValue(ws.Cell(r, 11), row.Location);
                SetCellValue(ws.Cell(r, 12), row.Description);
                SetCellValue(ws.Cell(r, 13), (row.IsActive is bool active && active) ? "1" : "0");
                r++;
            }

            FormatSheet(ws);
        }

        private static void ExportBorrowsSheet(XLWorkbook workbook, DateTime from, DateTime to)
        {
            using var conn = DatabaseConnection.GetConnection();
            var rows = conn.Query(@"
                SELECT br.BorrowCode, m.MemberCode, b.ISBN, b.Barcode, b.Title,
                       br.BorrowDate, br.DueDate, br.ReturnDate, br.Quantity, br.Status,
                       br.FineAmount, br.FinePaid, br.Notes, u.Username AS StaffUsername
                FROM BorrowRecords br
                INNER JOIN Members m ON m.MemberID = br.MemberID
                INNER JOIN Books b ON b.BookID = br.BookID
                LEFT JOIN Users u ON u.UserID = br.StaffID
                WHERE br.BorrowDate BETWEEN @FromDate AND @ToDate
                ORDER BY br.BorrowDate DESC", new { FromDate = from, ToDate = to }).ToList();

            var ws = workbook.Worksheets.Add(SheetBorrows);
            WriteHeader(ws, BorrowHeaders);
            WriteBorrowRows(ws, rows);
            FormatSheet(ws);
        }

        private static void ExportReturnsSheet(XLWorkbook workbook, DateTime from, DateTime to)
        {
            using var conn = DatabaseConnection.GetConnection();
            var rows = conn.Query(@"
                SELECT br.BorrowCode, m.MemberCode, b.ISBN, b.Barcode, b.Title,
                       br.BorrowDate, br.DueDate, br.ReturnDate, br.Quantity, br.Status,
                       br.FineAmount, br.FinePaid, br.Notes, u.Username AS StaffUsername
                FROM BorrowRecords br
                INNER JOIN Members m ON m.MemberID = br.MemberID
                INNER JOIN Books b ON b.BookID = br.BookID
                LEFT JOIN Users u ON u.UserID = br.StaffID
                WHERE br.Status = N'Đã trả' AND br.ReturnDate BETWEEN @FromDate AND @ToDate
                ORDER BY br.ReturnDate DESC", new { FromDate = from, ToDate = to }).ToList();

            var ws = workbook.Worksheets.Add(SheetReturns);
            WriteHeader(ws, BorrowHeaders);
            WriteBorrowRows(ws, rows);
            FormatSheet(ws);
        }

        private static void ExportOverdueSheet(XLWorkbook workbook, DateTime from, DateTime to)
        {
            using var conn = DatabaseConnection.GetConnection();
            var rows = conn.Query(@"
                SELECT br.BorrowCode, m.MemberCode, b.ISBN, b.Barcode, b.Title,
                       br.BorrowDate, br.DueDate, br.ReturnDate, br.Quantity, br.Status,
                       br.FineAmount, br.FinePaid, br.Notes, u.Username AS StaffUsername
                FROM BorrowRecords br
                INNER JOIN Members m ON m.MemberID = br.MemberID
                INNER JOIN Books b ON b.BookID = br.BookID
                LEFT JOIN Users u ON u.UserID = br.StaffID
                WHERE br.Status = N'Quá hạn' AND br.DueDate BETWEEN @FromDate AND @ToDate
                ORDER BY br.DueDate", new { FromDate = from, ToDate = to }).ToList();

            var ws = workbook.Worksheets.Add(SheetOverdue);
            WriteHeader(ws, BorrowHeaders);
            WriteBorrowRows(ws, rows);
            FormatSheet(ws);
        }

        private static void ExportUnpaidFinesSheet(XLWorkbook workbook)
        {
            using var conn = DatabaseConnection.GetConnection();
            var rows = conn.Query(@"
                SELECT MemberCode, FullName, Phone, TotalFine
                FROM Members
                WHERE IsActive = 1 AND TotalFine > 0
                ORDER BY TotalFine DESC").ToList();

            var ws = workbook.Worksheets.Add(SheetUnpaidFines);
            WriteHeader(ws, "MaDocGia", "HoTen", "DienThoai", "TongNoPhat");

            int r = 2;
            foreach (var row in rows)
            {
                ws.Cell(r, 1).Value = row.MemberCode;
                ws.Cell(r, 2).Value = row.FullName;
                ws.Cell(r, 3).Value = row.Phone;
                ws.Cell(r, 4).Value = row.TotalFine;
                r++;
            }

            FormatSheet(ws);
        }

        private static void ExportOutOfStockSheet(XLWorkbook workbook)
        {
            using var conn = DatabaseConnection.GetConnection();
            var rows = conn.Query(@"
                SELECT b.ISBN, b.Barcode, b.Title, c.CategoryName, a.AuthorName,
                       b.TotalCopies, b.AvailableCopies
                FROM Books b
                LEFT JOIN Categories c ON c.CategoryID = b.CategoryID
                LEFT JOIN Authors a ON a.AuthorID = b.AuthorID
                WHERE b.IsActive = 1 AND b.AvailableCopies <= 0
                ORDER BY b.Title").ToList();

            var ws = workbook.Worksheets.Add(SheetOutOfStock);
            WriteHeader(ws, "ISBN", "MaVach", "TieuDe", "TheLoai", "TacGia", "TongBan", "BanCoSan");

            int r = 2;
            foreach (var row in rows)
            {
                ws.Cell(r, 1).Value = row.ISBN;
                ws.Cell(r, 2).Value = row.Barcode;
                ws.Cell(r, 3).Value = row.Title;
                ws.Cell(r, 4).Value = row.CategoryName;
                ws.Cell(r, 5).Value = row.AuthorName;
                ws.Cell(r, 6).Value = row.TotalCopies;
                ws.Cell(r, 7).Value = row.AvailableCopies;
                r++;
            }

            FormatSheet(ws);
        }

        private static void ExportTopBooksSheet(XLWorkbook workbook, DateTime from, DateTime to)
        {
            var dao = new BorrowRecordDAO();
            var rows = dao.GetMostBorrowedBooks(from, to, 30);
            var ws = workbook.Worksheets.Add(SheetTopBooks);
            WriteHeader(ws, "STT", "TieuDe", "TacGia", "SoLuongKho", "LuotMuon");

            int r = 2;
            int idx = 1;
            foreach (var row in rows)
            {
                ws.Cell(r, 1).Value = idx++;
                ws.Cell(r, 2).Value = row.BookTitle;
                ws.Cell(r, 3).Value = row.AuthorName;
                ws.Cell(r, 4).Value = row.Quantity;
                ws.Cell(r, 5).Value = row.BorrowCount;
                r++;
            }

            FormatSheet(ws);
        }

        private static void ExportTopMembersSheet(XLWorkbook workbook, DateTime from, DateTime to)
        {
            var dao = new BorrowRecordDAO();
            var rows = dao.GetTopBorrowers(from, to, 30);
            var ws = workbook.Worksheets.Add(SheetTopMembers);
            WriteHeader(ws, "STT", "MaDocGia", "HoTen", "DienThoai", "LuotMuon");

            int r = 2;
            int idx = 1;
            foreach (var row in rows)
            {
                ws.Cell(r, 1).Value = idx++;
                ws.Cell(r, 2).Value = row.MemberCode;
                ws.Cell(r, 3).Value = row.MemberName;
                ws.Cell(r, 4).Value = row.Phone;
                ws.Cell(r, 5).Value = row.BorrowCount;
                r++;
            }

            FormatSheet(ws);
        }

        private static void ExportDailyStatsSheet(XLWorkbook workbook, DateTime fromDate, DateTime toDate)
        {
            var dao = new BorrowRecordDAO();
            var rows = dao.GetDailyStats(fromDate, toDate);
            var ws = workbook.Worksheets.Add(SheetDailyStats);
            WriteHeader(ws, "Ngay", "SoLuongMuon", "SoLuongTra", "DocGiaMoi");

            int r = 2;
            foreach (var row in rows)
            {
                ws.Cell(r, 1).Value = row.Date;
                ws.Cell(r, 2).Value = row.BorrowCount;
                ws.Cell(r, 3).Value = row.ReturnCount;
                ws.Cell(r, 4).Value = row.NewMembers;
                r++;
            }

            FormatSheet(ws);
        }

        private static void WriteBorrowRows(IXLWorksheet ws, List<dynamic> rows)
        {
            int r = 2;
            foreach (var row in rows)
            {
                SetCellValue(ws.Cell(r, 1), row.BorrowCode);
                SetCellValue(ws.Cell(r, 2), row.MemberCode);
                SetCellValue(ws.Cell(r, 3), row.ISBN);
                SetCellValue(ws.Cell(r, 4), row.Barcode);
                SetCellValue(ws.Cell(r, 5), row.Title);
                SetCellValue(ws.Cell(r, 6), row.BorrowDate);
                SetCellValue(ws.Cell(r, 7), row.DueDate);
                SetCellValue(ws.Cell(r, 8), row.ReturnDate);
                SetCellValue(ws.Cell(r, 9), row.Quantity);
                SetCellValue(ws.Cell(r, 10), row.Status);
                SetCellValue(ws.Cell(r, 11), row.FineAmount);
                SetCellValue(ws.Cell(r, 12), row.FinePaid);
                SetCellValue(ws.Cell(r, 13), row.Notes);
                SetCellValue(ws.Cell(r, 14), row.StaffUsername);
                r++;
            }
        }

        private static void SetCellValue(IXLCell cell, object? value)
        {
            if (value == null || value is DBNull)
            {
                cell.Value = string.Empty;
                return;
            }

            if (value is DateTime dt)
            {
                cell.Value = dt;
                return;
            }

            if (value is DateTimeOffset dto)
            {
                cell.Value = dto.DateTime;
                return;
            }

            if (value is bool b)
            {
                cell.Value = b ? 1 : 0;
                return;
            }

            cell.Value = value.ToString() ?? string.Empty;
        }

        private static void WriteHeader(IXLWorksheet worksheet, params string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
            }

            var headerRange = worksheet.Range(1, 1, 1, headers.Length);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(37, 99, 235);
            headerRange.Style.Font.FontColor = XLColor.White;
        }

        private static void FormatSheet(IXLWorksheet worksheet)
        {
            var usedRange = worksheet.RangeUsed();
            if (usedRange != null)
            {
                usedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                usedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                usedRange.SetAutoFilter();
            }

            worksheet.Columns().AdjustToContents();
        }

        private static void ValidateSheetHeaders(IXLWorksheet worksheet, List<string> errors)
        {
            if (!ImportableSheetHeaders.TryGetValue(worksheet.Name, out var expectedHeaders))
                return;

            for (int i = 0; i < expectedHeaders.Length; i++)
            {
                string actualHeader = worksheet.Cell(1, i + 1).GetString().Trim();
                if (!string.Equals(actualHeader, expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add($"Sheet '{worksheet.Name}' sai cột tại vị trí {i + 1}: cần '{expectedHeaders[i]}', nhận '{actualHeader}'.");
                }
            }
        }

        private static void ImportMembersSheet(IXLWorksheet worksheet, ImportContext context, ImportResult result)
        {
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;
            for (int row = 2; row <= lastRow; row++)
            {
                string memberCode = worksheet.Cell(row, 1).GetString().Trim();
                if (string.IsNullOrWhiteSpace(memberCode))
                    continue;

                string fullName = worksheet.Cell(row, 2).GetString().Trim();
                if (string.IsNullOrWhiteSpace(fullName))
                {
                    result.Warnings.Add($"Sheet DocGia dòng {row}: thiếu HoTen.");
                    continue;
                }

                string gender = worksheet.Cell(row, 3).GetString().Trim();
                DateTime? dob = ParseNullableDate(worksheet.Cell(row, 4));
                string address = worksheet.Cell(row, 5).GetString().Trim();
                string phone = worksheet.Cell(row, 6).GetString().Trim();
                string email = worksheet.Cell(row, 7).GetString().Trim();
                string identityCard = worksheet.Cell(row, 8).GetString().Trim();
                string memberType = worksheet.Cell(row, 9).GetString().Trim();
                DateTime joinDate = ParseNullableDate(worksheet.Cell(row, 10)) ?? DateTime.Today;
                DateTime? expiryDate = ParseNullableDate(worksheet.Cell(row, 11));
                decimal totalFine = ParseDecimal(worksheet.Cell(row, 12).GetString());
                bool isActive = ParseBool(worksheet.Cell(row, 13).GetString(), defaultValue: true);
                string notes = worksheet.Cell(row, 14).GetString().Trim();

                int existingId = context.Connection.ExecuteScalar<int>(
                    "SELECT ISNULL((SELECT MemberID FROM Members WHERE MemberCode = @MemberCode), 0)",
                    new { MemberCode = memberCode }, context.Transaction);

                if (existingId > 0)
                {
                    context.Connection.Execute(@"
                        UPDATE Members
                        SET FullName = @FullName,
                            Gender = @Gender,
                            DateOfBirth = @DateOfBirth,
                            Address = @Address,
                            Phone = @Phone,
                            Email = @Email,
                            IdentityCard = @IdentityCard,
                            MemberType = @MemberType,
                            JoinDate = @JoinDate,
                            ExpiryDate = @ExpiryDate,
                            TotalFine = @TotalFine,
                            IsActive = @IsActive,
                            Notes = @Notes,
                            UpdatedDate = GETDATE()
                        WHERE MemberID = @MemberID",
                        new
                        {
                            MemberID = existingId,
                            FullName = fullName,
                            Gender = NullIfEmpty(gender),
                            DateOfBirth = dob,
                            Address = NullIfEmpty(address),
                            Phone = NullIfEmpty(phone),
                            Email = NullIfEmpty(email),
                            IdentityCard = NullIfEmpty(identityCard),
                            MemberType = string.IsNullOrWhiteSpace(memberType) ? Member.TYPE_NORMAL : memberType,
                            JoinDate = joinDate,
                            ExpiryDate = expiryDate,
                            TotalFine = totalFine,
                            IsActive = isActive,
                            Notes = NullIfEmpty(notes)
                        }, context.Transaction);
                    result.UpdatedCount++;
                    context.RegisterMember(memberCode, existingId);
                }
                else
                {
                    int newId = context.Connection.ExecuteScalar<int>(@"
                        INSERT INTO Members
                            (MemberCode, FullName, Gender, DateOfBirth, Address, Phone, Email, IdentityCard,
                             MemberType, JoinDate, ExpiryDate, TotalFine, IsActive, Notes)
                        VALUES
                            (@MemberCode, @FullName, @Gender, @DateOfBirth, @Address, @Phone, @Email, @IdentityCard,
                             @MemberType, @JoinDate, @ExpiryDate, @TotalFine, @IsActive, @Notes);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);",
                        new
                        {
                            MemberCode = memberCode,
                            FullName = fullName,
                            Gender = NullIfEmpty(gender),
                            DateOfBirth = dob,
                            Address = NullIfEmpty(address),
                            Phone = NullIfEmpty(phone),
                            Email = NullIfEmpty(email),
                            IdentityCard = NullIfEmpty(identityCard),
                            MemberType = string.IsNullOrWhiteSpace(memberType) ? Member.TYPE_NORMAL : memberType,
                            JoinDate = joinDate,
                            ExpiryDate = expiryDate,
                            TotalFine = totalFine,
                            IsActive = isActive,
                            Notes = NullIfEmpty(notes)
                        }, context.Transaction);
                    result.InsertedCount++;
                    context.RegisterMember(memberCode, newId);
                }
            }
        }

        private static void ImportBooksSheet(IXLWorksheet worksheet, ImportContext context, ImportResult result)
        {
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;
            for (int row = 2; row <= lastRow; row++)
            {
                string title = worksheet.Cell(row, 3).GetString().Trim();
                if (string.IsNullOrWhiteSpace(title))
                    continue;

                string isbn = worksheet.Cell(row, 1).GetString().Trim();
                string barcode = worksheet.Cell(row, 2).GetString().Trim();
                string categoryName = worksheet.Cell(row, 4).GetString().Trim();
                string authorName = worksheet.Cell(row, 5).GetString().Trim();
                string publisherName = worksheet.Cell(row, 6).GetString().Trim();

                int? categoryId = context.GetCategoryId(categoryName);
                int? authorId = context.GetAuthorId(authorName);
                int? publisherId = context.GetPublisherId(publisherName);

                int? publishYear = ParseNullableInt(worksheet.Cell(row, 7).GetString());
                decimal price = ParseDecimal(worksheet.Cell(row, 8).GetString());
                int totalCopies = ParseNullableInt(worksheet.Cell(row, 9).GetString()) ?? 1;
                int availableCopies = ParseNullableInt(worksheet.Cell(row, 10).GetString()) ?? totalCopies;
                if (availableCopies < 0) availableCopies = 0;
                if (availableCopies > totalCopies) availableCopies = totalCopies;
                string location = worksheet.Cell(row, 11).GetString().Trim();
                string description = worksheet.Cell(row, 12).GetString().Trim();
                bool isActive = ParseBool(worksheet.Cell(row, 13).GetString(), defaultValue: true);

                int existingId = context.GetBookId(isbn, barcode, title);
                if (existingId > 0)
                {
                    context.Connection.Execute(@"
                        UPDATE Books
                        SET ISBN = @ISBN,
                            Barcode = @Barcode,
                            Title = @Title,
                            CategoryID = @CategoryID,
                            AuthorID = @AuthorID,
                            PublisherID = @PublisherID,
                            PublishYear = @PublishYear,
                            Price = @Price,
                            TotalCopies = @TotalCopies,
                            AvailableCopies = @AvailableCopies,
                            Location = @Location,
                            Description = @Description,
                            IsActive = @IsActive,
                            UpdatedDate = GETDATE()
                        WHERE BookID = @BookID",
                        new
                        {
                            BookID = existingId,
                            ISBN = NullIfEmpty(isbn),
                            Barcode = NullIfEmpty(barcode),
                            Title = title,
                            CategoryID = categoryId,
                            AuthorID = authorId,
                            PublisherID = publisherId,
                            PublishYear = publishYear,
                            Price = price,
                            TotalCopies = totalCopies,
                            AvailableCopies = availableCopies,
                            Location = NullIfEmpty(location),
                            Description = NullIfEmpty(description),
                            IsActive = isActive
                        }, context.Transaction);
                    result.UpdatedCount++;
                    context.RegisterBook(existingId, isbn, barcode, title);
                }
                else
                {
                    int newId = context.Connection.ExecuteScalar<int>(@"
                        INSERT INTO Books
                            (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID,
                             PublishYear, Price, TotalCopies, AvailableCopies, Location, Description, IsActive)
                        VALUES
                            (@ISBN, @Barcode, @Title, @CategoryID, @AuthorID, @PublisherID,
                             @PublishYear, @Price, @TotalCopies, @AvailableCopies, @Location, @Description, @IsActive);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);",
                        new
                        {
                            ISBN = NullIfEmpty(isbn),
                            Barcode = NullIfEmpty(barcode),
                            Title = title,
                            CategoryID = categoryId,
                            AuthorID = authorId,
                            PublisherID = publisherId,
                            PublishYear = publishYear,
                            Price = price,
                            TotalCopies = totalCopies,
                            AvailableCopies = availableCopies,
                            Location = NullIfEmpty(location),
                            Description = NullIfEmpty(description),
                            IsActive = isActive
                        }, context.Transaction);
                    result.InsertedCount++;
                    context.RegisterBook(newId, isbn, barcode, title);
                }
            }
        }

        private static void ImportBorrowLikeSheet(IXLWorksheet worksheet, ImportContext context, ImportResult result, string? forceStatus)
        {
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;
            for (int row = 2; row <= lastRow; row++)
            {
                string borrowCode = worksheet.Cell(row, 1).GetString().Trim();
                if (string.IsNullOrWhiteSpace(borrowCode))
                    continue;

                string memberCode = worksheet.Cell(row, 2).GetString().Trim();
                int memberId = context.GetMemberId(memberCode);
                if (memberId <= 0)
                {
                    result.Warnings.Add($"Sheet {worksheet.Name} dòng {row}: không tìm thấy độc giả '{memberCode}'.");
                    continue;
                }

                string isbn = worksheet.Cell(row, 3).GetString().Trim();
                string barcode = worksheet.Cell(row, 4).GetString().Trim();
                string title = worksheet.Cell(row, 5).GetString().Trim();
                int bookId = context.GetBookId(isbn, barcode, title);
                if (bookId <= 0)
                {
                    result.Warnings.Add($"Sheet {worksheet.Name} dòng {row}: không tìm thấy sách (ISBN='{isbn}', Mã vạch='{barcode}', Tên='{title}').");
                    continue;
                }

                DateTime borrowDate = ParseNullableDate(worksheet.Cell(row, 6)) ?? DateTime.Today;
                DateTime dueDate = ParseNullableDate(worksheet.Cell(row, 7)) ?? borrowDate.AddDays(7);
                DateTime? returnDate = ParseNullableDate(worksheet.Cell(row, 8));
                int quantity = ParseNullableInt(worksheet.Cell(row, 9).GetString()) ?? 1;
                if (quantity <= 0) quantity = 1;

                string status = string.IsNullOrWhiteSpace(forceStatus)
                    ? worksheet.Cell(row, 10).GetString().Trim()
                    : forceStatus;

                if (string.IsNullOrWhiteSpace(status))
                    status = BorrowRecord.STATUS_BORROWING;

                decimal fineAmount = ParseDecimal(worksheet.Cell(row, 11).GetString());
                bool finePaid = ParseBool(worksheet.Cell(row, 12).GetString(), defaultValue: false);
                string notes = worksheet.Cell(row, 13).GetString().Trim();
                string staffUsername = worksheet.Cell(row, 14).GetString().Trim();
                int? staffId = context.GetStaffId(staffUsername);

                if (!IsValidBorrowStatus(status))
                {
                    result.Warnings.Add($"Sheet {worksheet.Name} dòng {row}: trạng thái '{status}' không hợp lệ.");
                    continue;
                }

                if (status == BorrowRecord.STATUS_RETURNED && !returnDate.HasValue)
                    returnDate = dueDate;

                int existingId = context.Connection.ExecuteScalar<int>(
                    "SELECT ISNULL((SELECT BorrowID FROM BorrowRecords WHERE BorrowCode = @BorrowCode), 0)",
                    new { BorrowCode = borrowCode }, context.Transaction);

                if (existingId > 0)
                {
                    context.Connection.Execute(@"
                        UPDATE BorrowRecords
                        SET MemberID = @MemberID,
                            BookID = @BookID,
                            BorrowDate = @BorrowDate,
                            DueDate = @DueDate,
                            ReturnDate = @ReturnDate,
                            Quantity = @Quantity,
                            Status = @Status,
                            FineAmount = @FineAmount,
                            FinePaid = @FinePaid,
                            Notes = @Notes,
                            StaffID = @StaffID,
                            UpdatedDate = GETDATE()
                        WHERE BorrowID = @BorrowID",
                        new
                        {
                            BorrowID = existingId,
                            MemberID = memberId,
                            BookID = bookId,
                            BorrowDate = borrowDate,
                            DueDate = dueDate,
                            ReturnDate = returnDate,
                            Quantity = quantity,
                            Status = status,
                            FineAmount = fineAmount,
                            FinePaid = finePaid,
                            Notes = NullIfEmpty(notes),
                            StaffID = staffId
                        }, context.Transaction);
                    result.UpdatedCount++;
                }
                else
                {
                    context.Connection.Execute(@"
                        INSERT INTO BorrowRecords
                            (BorrowCode, MemberID, BookID, BorrowDate, DueDate, ReturnDate, Quantity,
                             Status, FineAmount, FinePaid, Notes, StaffID)
                        VALUES
                            (@BorrowCode, @MemberID, @BookID, @BorrowDate, @DueDate, @ReturnDate, @Quantity,
                             @Status, @FineAmount, @FinePaid, @Notes, @StaffID)",
                        new
                        {
                            BorrowCode = borrowCode,
                            MemberID = memberId,
                            BookID = bookId,
                            BorrowDate = borrowDate,
                            DueDate = dueDate,
                            ReturnDate = returnDate,
                            Quantity = quantity,
                            Status = status,
                            FineAmount = fineAmount,
                            FinePaid = finePaid,
                            Notes = NullIfEmpty(notes),
                            StaffID = staffId
                        }, context.Transaction);
                    result.InsertedCount++;
                }
            }
        }

        private static bool IsValidBorrowStatus(string status)
        {
            return string.Equals(status, BorrowRecord.STATUS_BORROWING, StringComparison.OrdinalIgnoreCase)
                || string.Equals(status, BorrowRecord.STATUS_RETURNED, StringComparison.OrdinalIgnoreCase)
                || string.Equals(status, BorrowRecord.STATUS_OVERDUE, StringComparison.OrdinalIgnoreCase)
                || string.Equals(status, BorrowRecord.STATUS_LOST, StringComparison.OrdinalIgnoreCase);
        }

        private static void RecalculateAvailableCopies(IDbConnection conn, IDbTransaction tx)
        {
            conn.Execute(@"
                ;WITH Borrowing AS (
                    SELECT BookID, COUNT(*) AS BorrowingCount
                    FROM BorrowRecords
                    WHERE Status IN (N'Đang mượn', N'Quá hạn')
                    GROUP BY BookID
                )
                UPDATE b
                SET AvailableCopies =
                    CASE
                        WHEN b.TotalCopies - ISNULL(br.BorrowingCount, 0) < 0 THEN 0
                        ELSE b.TotalCopies - ISNULL(br.BorrowingCount, 0)
                    END,
                    UpdatedDate = GETDATE()
                FROM Books b
                LEFT JOIN Borrowing br ON br.BookID = b.BookID", transaction: tx);
        }

        private static DateTime? ParseNullableDate(IXLCell cell)
        {
            if (cell.IsEmpty())
                return null;

            if (cell.TryGetValue<DateTime>(out var dt))
                return dt;

            string text = cell.GetString().Trim();
            if (string.IsNullOrWhiteSpace(text))
                return null;

            string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "M/d/yyyy" };
            if (DateTime.TryParseExact(text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;

            if (DateTime.TryParse(text, out dt))
                return dt;

            return null;
        }

        private static int? ParseNullableInt(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return int.TryParse(value.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed)
                ? parsed
                : null;
        }

        private static decimal ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            string normalized = value.Trim().Replace(" ", string.Empty);
            if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.GetCultureInfo("vi-VN"), out decimal parsed))
                return parsed;

            if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed))
                return parsed;

            return 0;
        }

        private static bool ParseBool(string? value, bool defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            string text = value.Trim();
            if (text == "1") return true;
            if (text == "0") return false;
            if (bool.TryParse(text, out bool parsed)) return parsed;

            return text.Equals("co", StringComparison.OrdinalIgnoreCase)
                || text.Equals("có", StringComparison.OrdinalIgnoreCase)
                || text.Equals("hoat dong", StringComparison.OrdinalIgnoreCase)
                || text.Equals("hoạt động", StringComparison.OrdinalIgnoreCase)
                || text.Equals("active", StringComparison.OrdinalIgnoreCase);
        }

        private static object? NullIfEmpty(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private sealed class ImportContext
        {
            private readonly Dictionary<string, int> memberByCode = new(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<string, int> bookByIsbn = new(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<string, int> bookByBarcode = new(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<string, int> bookByTitle = new(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<string, int?> categoryByName = new(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<string, int?> authorByName = new(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<string, int?> publisherByName = new(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<string, int?> staffByUsername = new(StringComparer.OrdinalIgnoreCase);

            public ImportContext(IDbConnection connection, IDbTransaction transaction)
            {
                Connection = connection;
                Transaction = transaction;
                WarmupCaches();
            }

            public IDbConnection Connection { get; }
            public IDbTransaction Transaction { get; }

            public int GetMemberId(string memberCode)
            {
                if (string.IsNullOrWhiteSpace(memberCode)) return 0;
                return memberByCode.TryGetValue(memberCode.Trim(), out int id) ? id : 0;
            }

            public void RegisterMember(string memberCode, int memberId)
            {
                if (string.IsNullOrWhiteSpace(memberCode) || memberId <= 0)
                    return;

                memberByCode[memberCode.Trim()] = memberId;
            }

            public int GetBookId(string isbn, string barcode, string title)
            {
                if (!string.IsNullOrWhiteSpace(isbn) && bookByIsbn.TryGetValue(isbn.Trim(), out int byIsbn))
                    return byIsbn;
                if (!string.IsNullOrWhiteSpace(barcode) && bookByBarcode.TryGetValue(barcode.Trim(), out int byBarcode))
                    return byBarcode;
                if (!string.IsNullOrWhiteSpace(title) && bookByTitle.TryGetValue(title.Trim(), out int byTitle))
                    return byTitle;
                return 0;
            }

            public void RegisterBook(int bookId, string isbn, string barcode, string title)
            {
                if (bookId <= 0)
                    return;

                if (!string.IsNullOrWhiteSpace(isbn))
                    bookByIsbn[isbn.Trim()] = bookId;

                if (!string.IsNullOrWhiteSpace(barcode))
                    bookByBarcode[barcode.Trim()] = bookId;

                if (!string.IsNullOrWhiteSpace(title))
                    bookByTitle[title.Trim()] = bookId;
            }

            public int? GetCategoryId(string categoryName)
            {
                if (string.IsNullOrWhiteSpace(categoryName)) return null;
                if (categoryByName.TryGetValue(categoryName.Trim(), out int? existing))
                    return existing;

                int newId = Connection.ExecuteScalar<int>(
                    "INSERT INTO Categories (CategoryName) VALUES (@CategoryName); SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new { CategoryName = categoryName.Trim() }, Transaction);
                categoryByName[categoryName.Trim()] = newId;
                return newId;
            }

            public int? GetAuthorId(string authorName)
            {
                if (string.IsNullOrWhiteSpace(authorName)) return null;
                if (authorByName.TryGetValue(authorName.Trim(), out int? existing))
                    return existing;

                int newId = Connection.ExecuteScalar<int>(
                    "INSERT INTO Authors (AuthorName) VALUES (@AuthorName); SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new { AuthorName = authorName.Trim() }, Transaction);
                authorByName[authorName.Trim()] = newId;
                return newId;
            }

            public int? GetPublisherId(string publisherName)
            {
                if (string.IsNullOrWhiteSpace(publisherName)) return null;
                if (publisherByName.TryGetValue(publisherName.Trim(), out int? existing))
                    return existing;

                int newId = Connection.ExecuteScalar<int>(
                    "INSERT INTO Publishers (PublisherName) VALUES (@PublisherName); SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new { PublisherName = publisherName.Trim() }, Transaction);
                publisherByName[publisherName.Trim()] = newId;
                return newId;
            }

            public int? GetStaffId(string staffUsername)
            {
                if (string.IsNullOrWhiteSpace(staffUsername)) return null;
                return staffByUsername.TryGetValue(staffUsername.Trim(), out int? id) ? id : null;
            }

            private void WarmupCaches()
            {
                foreach (var row in Connection.Query("SELECT MemberID, MemberCode FROM Members", transaction: Transaction))
                {
                    if (row.MemberCode != null)
                        memberByCode[(string)row.MemberCode] = (int)row.MemberID;
                }

                foreach (var row in Connection.Query("SELECT BookID, ISBN, Barcode, Title FROM Books", transaction: Transaction))
                {
                    int id = (int)row.BookID;
                    if (row.ISBN != null && !string.IsNullOrWhiteSpace((string)row.ISBN))
                        bookByIsbn[(string)row.ISBN] = id;
                    if (row.Barcode != null && !string.IsNullOrWhiteSpace((string)row.Barcode))
                        bookByBarcode[(string)row.Barcode] = id;
                    if (row.Title != null && !string.IsNullOrWhiteSpace((string)row.Title))
                        bookByTitle[(string)row.Title] = id;
                }

                foreach (var row in Connection.Query("SELECT CategoryID, CategoryName FROM Categories", transaction: Transaction))
                {
                    if (row.CategoryName != null)
                        categoryByName[(string)row.CategoryName] = (int)row.CategoryID;
                }

                foreach (var row in Connection.Query("SELECT AuthorID, AuthorName FROM Authors", transaction: Transaction))
                {
                    if (row.AuthorName != null)
                        authorByName[(string)row.AuthorName] = (int)row.AuthorID;
                }

                foreach (var row in Connection.Query("SELECT PublisherID, PublisherName FROM Publishers", transaction: Transaction))
                {
                    if (row.PublisherName != null)
                        publisherByName[(string)row.PublisherName] = (int)row.PublisherID;
                }

                foreach (var row in Connection.Query("SELECT UserID, Username FROM Users", transaction: Transaction))
                {
                    if (row.Username != null)
                        staffByUsername[(string)row.Username] = (int)row.UserID;
                }
            }
        }
    }
}
