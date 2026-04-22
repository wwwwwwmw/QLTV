using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagement.Data;

namespace LibraryManagement.Services
{
    public class DashboardService
    {
        private readonly BorrowRecordDAO _borrowDao = new BorrowRecordDAO();
        private readonly BookDAO _bookDao = new BookDAO();

        public DashboardSummary GetSummary()
        {
            var stats = _borrowDao.GetDashboardStats();
            return new DashboardSummary
            {
                TotalBooks = stats.TotalBooks,
                TotalMembers = stats.TotalMembers,
                BorrowingCount = stats.BorrowingCount,
                OverdueCount = stats.OverdueCount,
                TodayBorrows = stats.TodayBorrows,
                TodayReturns = stats.TodayReturns
            };
        }

        public List<BorrowByDayPoint> GetBorrowByDay(int days)
        {
            var to = DateTime.Today;
            var from = to.AddDays(-(days - 1));
            var daily = _borrowDao.GetDailyStats(from, to);
            return daily.Select(d => new BorrowByDayPoint { Date = d.Date, BorrowCount = d.BorrowCount }).ToList();
        }

        public List<TopBorrowedBookPoint> GetTopBorrowedBooks(int topN)
        {
            var rows = _bookDao.GetTopBorrowedBooks(topN);
            var result = new List<TopBorrowedBookPoint>();

            foreach (var r in rows)
            {
                try
                {
                    result.Add(new TopBorrowedBookPoint
                    {
                        Title = (string)r.Title,
                        BorrowCount = (int)r.BorrowCount
                    });
                }
                catch
                {
                }
            }

            return result;
        }
    }

    public class DashboardSummary
    {
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int BorrowingCount { get; set; }
        public int OverdueCount { get; set; }
        public int TodayBorrows { get; set; }
        public int TodayReturns { get; set; }
    }

    public class BorrowByDayPoint
    {
        public DateTime Date { get; set; }
        public int BorrowCount { get; set; }
    }

    public class TopBorrowedBookPoint
    {
        public string Title { get; set; } = "";
        public int BorrowCount { get; set; }
    }
}

