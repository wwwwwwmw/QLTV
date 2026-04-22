using System;

namespace LibraryManagement.Models
{
    /// <summary>
    /// Model đại diện cho sách trong thư viện
    /// </summary>
    public class Book
    {
        public int BookID { get; set; }
        public string? ISBN { get; set; }
        public string? BookCode { get; set; }
        public string? Barcode { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? CategoryID { get; set; }
        public int? AuthorID { get; set; }
        public int? PublisherID { get; set; }
        public int? PublishYear { get; set; }
        public decimal Price { get; set; }
        public int TotalCopies { get; set; } = 1;
        public int AvailableCopies { get; set; } = 1;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties (for display)
        public string? CategoryName { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }

        // Computed properties
        public int BorrowedCopies => TotalCopies - AvailableCopies;
        public bool IsAvailable => AvailableCopies > 0;
        public string StatusDisplay => IsAvailable ? $"Còn {AvailableCopies} bản" : "Hết sách";

        // Aliases for UI compatibility
        public int Quantity => TotalCopies;
        public int AvailableQuantity => AvailableCopies;
        public int BorrowedQuantity => BorrowedCopies;
    }

    /// <summary>
    /// Model đại diện cho thể loại sách
    /// </summary>
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public override string ToString() => CategoryName;
    }

    /// <summary>
    /// Model đại diện cho tác giả
    /// </summary>
    public class Author
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public string? Country { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public override string ToString() => AuthorName;
    }

    /// <summary>
    /// Model đại diện cho nhà xuất bản
    /// </summary>
    public class Publisher
    {
        public int PublisherID { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public override string ToString() => PublisherName;
    }
}
