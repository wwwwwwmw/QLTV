using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using LibraryManagement.Models;

namespace LibraryManagement.Data
{
    /// <summary>
    /// Data Access Object cho Book - Quản lý sách
    /// </summary>
    public class BookDAO
    {
        /// <summary>
        /// Lấy tất cả sách với thông tin liên quan
        /// </summary>
        public List<Book> GetAll(bool activeOnly = true)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                string sql = @"SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
                               FROM Books b
                               LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                               LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                               LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
                               WHERE (@ActiveOnly = 0 OR b.IsActive = 1)
                               ORDER BY b.Title";

                return conn.Query<Book>(sql, new { ActiveOnly = activeOnly }).AsList();
            }
        }

        public async Task<List<Book>> GetAllAsync(bool activeOnly = true)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                string sql = @"SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
                               FROM Books b
                               LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                               LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                               LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
                               WHERE (@ActiveOnly = 0 OR b.IsActive = 1)
                               ORDER BY b.Title";

                var rows = await conn.QueryAsync<Book>(sql, new { ActiveOnly = activeOnly });
                return rows.AsList();
            }
        }

        /// <summary>
        /// Tìm kiếm sách
        /// </summary>
        public List<Book> Search(string? keyword = null, int? categoryId = null,
            int? authorId = null, bool availableOnly = false)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                string sql = @"SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
                               FROM Books b
                               LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                               LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                               LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
                               WHERE b.IsActive = 1
                                 AND (@Keyword IS NULL OR b.Title LIKE '%' + @Keyword + '%' 
                                     OR b.ISBN LIKE '%' + @Keyword + '%'
                                     OR b.Barcode LIKE '%' + @Keyword + '%')
                                 AND (@CategoryID IS NULL OR b.CategoryID = @CategoryID)
                                 AND (@AuthorID IS NULL OR b.AuthorID = @AuthorID)
                                 AND (@AvailableOnly = 0 OR b.AvailableCopies > 0)
                               ORDER BY b.Title";

                return conn.Query<Book>(sql, new
                {
                    Keyword = keyword,
                    CategoryID = categoryId,
                    AuthorID = authorId,
                    AvailableOnly = availableOnly
                }).AsList();
            }
        }

        public async Task<List<Book>> SearchAsync(string? keyword = null, int? categoryId = null,
            int? authorId = null, bool availableOnly = false)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                string sql = @"SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
                               FROM Books b
                               LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                               LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                               LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
                               WHERE b.IsActive = 1
                                 AND (@Keyword IS NULL OR b.Title LIKE '%' + @Keyword + '%' 
                                     OR b.ISBN LIKE '%' + @Keyword + '%'
                                     OR b.Barcode LIKE '%' + @Keyword + '%')
                                 AND (@CategoryID IS NULL OR b.CategoryID = @CategoryID)
                                 AND (@AuthorID IS NULL OR b.AuthorID = @AuthorID)
                                 AND (@AvailableOnly = 0 OR b.AvailableCopies > 0)
                               ORDER BY b.Title";

                var rows = await conn.QueryAsync<Book>(sql, new
                {
                    Keyword = keyword,
                    CategoryID = categoryId,
                    AuthorID = authorId,
                    AvailableOnly = availableOnly
                });
                return rows.AsList();
            }
        }

        /// <summary>
        /// Lấy sách theo ID
        /// </summary>
        public Book? GetById(int bookId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<Book>(
                    @"SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
                      FROM Books b
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                      LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                      LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
                      WHERE b.BookID = @BookID",
                    new { BookID = bookId });
            }
        }

        public async Task<Book?> GetByIdAsync(int bookId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<Book>(
                    @"SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
                      FROM Books b
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                      LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                      LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
                      WHERE b.BookID = @BookID",
                    new { BookID = bookId });
            }
        }

        /// <summary>
        /// Lấy sách theo ISBN
        /// </summary>
        public Book? GetByISBN(string isbn)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<Book>(
                    @"SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
                      FROM Books b
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                      LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                      LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
                      WHERE b.ISBN = @ISBN",
                    new { ISBN = isbn });
            }
        }

        public Book? GetByBarcode(string barcode)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<Book>(
                    @"SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
                      FROM Books b
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                      LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                      LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
                      WHERE b.Barcode = @Barcode",
                    new { Barcode = barcode });
            }
        }

        public async Task<Book?> GetByBarcodeAsync(string barcode)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<Book>(
                    @"SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
                      FROM Books b
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                      LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
                      LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
                      WHERE b.Barcode = @Barcode",
                    new { Barcode = barcode });
            }
        }

        public bool BarcodeExists(string barcode, int? excludeBookId = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Books WHERE Barcode = @Barcode";
                if (excludeBookId.HasValue)
                    sql += " AND BookID != @ExcludeBookId";

                return conn.ExecuteScalar<int>(sql,
                    new { Barcode = barcode, ExcludeBookId = excludeBookId }) > 0;
            }
        }

        /// <summary>
        /// Thêm sách mới
        /// </summary>
        public int Insert(Book book)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.ExecuteScalar<int>(
                    @"INSERT INTO Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, 
                        PublishYear, Price, TotalCopies, AvailableCopies, Description, Location, ImagePath)
                      VALUES (@ISBN, @Barcode, @Title, @CategoryID, @AuthorID, @PublisherID,
                        @PublishYear, @Price, @TotalCopies, @AvailableCopies, @Description, @Location, @ImagePath);
                      SELECT SCOPE_IDENTITY();", book);
            }
        }

        public async Task<int> InsertAsync(Book book)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return await conn.ExecuteScalarAsync<int>(
                        @"INSERT INTO Books (ISBN, Barcode, Title, CategoryID, AuthorID, PublisherID, 
                                                PublishYear, Price, TotalCopies, AvailableCopies, Description, Location, ImagePath)
                                            VALUES (@ISBN, @Barcode, @Title, @CategoryID, @AuthorID, @PublisherID,
                                                @PublishYear, @Price, @TotalCopies, @AvailableCopies, @Description, @Location, @ImagePath);
                                            SELECT SCOPE_IDENTITY();", book);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sách
        /// </summary>
        public bool Update(Book book)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE Books SET 
                        ISBN = @ISBN, Barcode = @Barcode, Title = @Title, CategoryID = @CategoryID, 
                        AuthorID = @AuthorID, PublisherID = @PublisherID,
                        PublishYear = @PublishYear, Price = @Price, 
                        TotalCopies = @TotalCopies, AvailableCopies = @AvailableCopies,
                        Description = @Description, Location = @Location, 
                        ImagePath = @ImagePath, UpdatedDate = GETDATE()
                      WHERE BookID = @BookID", book);
                return affected > 0;
            }
        }

        /// <summary>
        /// Xóa sách (đánh dấu không hoạt động)
        /// </summary>
        public bool Delete(int bookId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Books SET IsActive = 0, UpdatedDate = GETDATE() WHERE BookID = @BookID",
                    new { BookID = bookId });
                return affected > 0;
            }
        }

        /// <summary>
        /// Kiểm tra ISBN đã tồn tại
        /// </summary>
        public bool ISBNExists(string isbn, int? excludeBookId = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Books WHERE ISBN = @ISBN";
                if (excludeBookId.HasValue)
                    sql += " AND BookID != @ExcludeBookId";

                return conn.ExecuteScalar<int>(sql,
                    new { ISBN = isbn, ExcludeBookId = excludeBookId }) > 0;
            }
        }

        /// <summary>
        /// Cập nhật số lượng sách sẵn có
        /// </summary>
        public bool UpdateAvailableCopies(int bookId, int change)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE Books SET AvailableCopies = AvailableCopies + @Change, UpdatedDate = GETDATE()
                      WHERE BookID = @BookID AND AvailableCopies + @Change >= 0 
                            AND AvailableCopies + @Change <= TotalCopies",
                    new { BookID = bookId, Change = change });
                return affected > 0;
            }
        }

        /// <summary>
        /// Lấy sách được mượn nhiều nhất
        /// </summary>
        public List<dynamic> GetTopBorrowedBooks(int topN = 10)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query(
                    @"SELECT TOP(@TopN) b.BookID, b.Title, b.ISBN, c.CategoryName,
                        COUNT(br.BorrowID) AS BorrowCount
                      FROM Books b
                      LEFT JOIN BorrowRecords br ON b.BookID = br.BookID
                      LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                      WHERE b.IsActive = 1
                      GROUP BY b.BookID, b.Title, b.ISBN, c.CategoryName
                      ORDER BY BorrowCount DESC",
                    new { TopN = topN }).AsList();
            }
        }
    }

    /// <summary>
    /// Data Access Object cho Category
    /// </summary>
    public class CategoryDAO
    {
        public List<Category> GetAll(bool activeOnly = true)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<Category>(
                    "SELECT * FROM Categories WHERE (@ActiveOnly = 0 OR IsActive = 1) ORDER BY CategoryName",
                    new { ActiveOnly = activeOnly }).AsList();
            }
        }

        public Category? GetById(int categoryId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<Category>(
                    "SELECT * FROM Categories WHERE CategoryID = @CategoryID",
                    new { CategoryID = categoryId });
            }
        }

        public int Insert(Category category)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.ExecuteScalar<int>(
                    @"INSERT INTO Categories (CategoryName, Description) VALUES (@CategoryName, @Description);
                      SELECT SCOPE_IDENTITY();", category);
            }
        }

        public bool Update(Category category)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Categories SET CategoryName = @CategoryName, Description = @Description WHERE CategoryID = @CategoryID",
                    category);
                return affected > 0;
            }
        }

        public bool Delete(int categoryId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Categories SET IsActive = 0 WHERE CategoryID = @CategoryID",
                    new { CategoryID = categoryId });
                return affected > 0;
            }
        }
    }

    /// <summary>
    /// Data Access Object cho Author
    /// </summary>
    public class AuthorDAO
    {
        public List<Author> GetAll(bool activeOnly = true)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<Author>(
                    "SELECT * FROM Authors WHERE (@ActiveOnly = 0 OR IsActive = 1) ORDER BY AuthorName",
                    new { ActiveOnly = activeOnly }).AsList();
            }
        }

        public Author? GetById(int authorId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<Author>(
                    "SELECT * FROM Authors WHERE AuthorID = @AuthorID",
                    new { AuthorID = authorId });
            }
        }

        public int Insert(Author author)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.ExecuteScalar<int>(
                    @"INSERT INTO Authors (AuthorName, Biography, Country) VALUES (@AuthorName, @Biography, @Country);
                      SELECT SCOPE_IDENTITY();", author);
            }
        }

        public bool Update(Author author)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Authors SET AuthorName = @AuthorName, Biography = @Biography, Country = @Country WHERE AuthorID = @AuthorID",
                    author);
                return affected > 0;
            }
        }

        public bool Delete(int authorId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Authors SET IsActive = 0 WHERE AuthorID = @AuthorID",
                    new { AuthorID = authorId });
                return affected > 0;
            }
        }
    }

    /// <summary>
    /// Data Access Object cho Publisher
    /// </summary>
    public class PublisherDAO
    {
        public List<Publisher> GetAll(bool activeOnly = true)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<Publisher>(
                    "SELECT * FROM Publishers WHERE (@ActiveOnly = 0 OR IsActive = 1) ORDER BY PublisherName",
                    new { ActiveOnly = activeOnly }).AsList();
            }
        }

        public Publisher? GetById(int publisherId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<Publisher>(
                    "SELECT * FROM Publishers WHERE PublisherID = @PublisherID",
                    new { PublisherID = publisherId });
            }
        }

        public int Insert(Publisher publisher)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.ExecuteScalar<int>(
                    @"INSERT INTO Publishers (PublisherName, Address, Phone, Email, Website) 
                      VALUES (@PublisherName, @Address, @Phone, @Email, @Website);
                      SELECT SCOPE_IDENTITY();", publisher);
            }
        }

        public bool Update(Publisher publisher)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE Publishers SET PublisherName = @PublisherName, Address = @Address, 
                        Phone = @Phone, Email = @Email, Website = @Website 
                      WHERE PublisherID = @PublisherID",
                    publisher);
                return affected > 0;
            }
        }

        public bool Delete(int publisherId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Publishers SET IsActive = 0 WHERE PublisherID = @PublisherID",
                    new { PublisherID = publisherId });
                return affected > 0;
            }
        }
    }
}
