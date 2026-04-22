using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Services
{
    public class RecommendationService
    {
        public List<Book> GetRecommendationsForMember(int memberId, int take = 5)
        {
            RefreshRecommendationsForMember(memberId, Math.Max(take, 10));

            using var conn = DatabaseConnection.GetConnection();
            string sql = @"
SELECT TOP(@Take) b.*, c.CategoryName, a.AuthorName, p.PublisherName
FROM BookRecommendations r
INNER JOIN Books b ON r.BookID = b.BookID
LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
WHERE r.MemberID = @MemberID AND b.IsActive = 1
ORDER BY r.Score DESC, b.Title";

            return conn.Query<Book>(sql, new { MemberID = memberId, Take = take }).AsList();
        }

        public void RefreshRecommendationsForMember(int memberId, int take = 10)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();

            conn.Execute("DELETE FROM BookRecommendations WHERE MemberID = @MemberID", new { MemberID = memberId }, tx);

            var topCategories = conn.Query<int?>(
                @"SELECT TOP 3 b.CategoryID
                  FROM BorrowRecords br
                  INNER JOIN Books b ON br.BookID = b.BookID
                  WHERE br.MemberID = @MemberID AND b.CategoryID IS NOT NULL
                  GROUP BY b.CategoryID
                  ORDER BY COUNT(*) DESC",
                new { MemberID = memberId }, tx).Where(x => x.HasValue).Select(x => x!.Value).ToList();

            int inserted = 0;

            if (topCategories.Count > 0)
            {
                inserted += conn.Execute(@"
INSERT INTO BookRecommendations (MemberID, BookID, Score, Reason)
SELECT TOP(@Take) @MemberID, b.BookID,
       (ISNULL((SELECT COUNT(*) FROM BorrowRecords br2 WHERE br2.BookID = b.BookID), 0) * 10) + 50 AS Score,
       N'Theo thể loại bạn hay mượn' AS Reason
FROM Books b
WHERE b.IsActive = 1
  AND b.CategoryID IN @CategoryIDs
  AND b.BookID NOT IN (
        SELECT br.BookID FROM BorrowRecords br
        WHERE br.MemberID = @MemberID AND br.Status IN (N'Đang mượn', N'Quá hạn')
  )
ORDER BY (ISNULL((SELECT COUNT(*) FROM BorrowRecords br2 WHERE br2.BookID = b.BookID), 0)) DESC, b.Title",
                    new { MemberID = memberId, Take = take, CategoryIDs = topCategories }, tx);
            }

            int remaining = Math.Max(0, take - inserted);
            if (remaining > 0)
            {
                conn.Execute(@"
INSERT INTO BookRecommendations (MemberID, BookID, Score, Reason)
SELECT TOP(@Take) @MemberID, b.BookID,
       (ISNULL((SELECT COUNT(*) FROM BorrowRecords br2 WHERE br2.BookID = b.BookID), 0) * 10) AS Score,
       N'Sách phổ biến' AS Reason
FROM Books b
WHERE b.IsActive = 1
  AND b.BookID NOT IN (
        SELECT br.BookID FROM BorrowRecords br
        WHERE br.MemberID = @MemberID AND br.Status IN (N'Đang mượn', N'Quá hạn')
  )
  AND b.BookID NOT IN (SELECT BookID FROM BookRecommendations WHERE MemberID = @MemberID)
ORDER BY (ISNULL((SELECT COUNT(*) FROM BorrowRecords br2 WHERE br2.BookID = b.BookID), 0)) DESC, b.Title",
                    new { MemberID = memberId, Take = remaining }, tx);
            }

            tx.Commit();
        }
    }

    public interface IBookAiRecommender
    {
        System.Threading.Tasks.Task<IReadOnlyList<int>> RecommendBookIdsAsync(int memberId, int take);
    }
}

