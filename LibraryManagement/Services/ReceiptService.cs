using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using ZXing;
using ZXing.Common;

namespace LibraryManagement.Services
{
    public class ReceiptService
    {
        public string PrintBorrowReceipt(BorrowReceiptData data)
        {
            string filePath = BuildFilePath("PhieuMuon", data.BorrowCode);
            ExportBorrowPdf(filePath, data);
            return filePath;
        }

        public string PrintReturnReceipt(ReturnReceiptData data)
        {
            string filePath = BuildFilePath("PhieuTra", data.BorrowCode);
            ExportReturnPdf(filePath, data);
            return filePath;
        }

        public static string GetLibraryName()
        {
            return ConfigurationManager.AppSettings["AppName"] ?? "Library";
        }

        private static string BuildFilePath(string prefix, string borrowCode)
        {
            string baseDir = Path.Combine(Application.StartupPath, "Receipts");
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            string safeBorrowCode = string.IsNullOrWhiteSpace(borrowCode)
                ? "UNKNOWN"
                : string.Concat(borrowCode.Where(c => char.IsLetterOrDigit(c) || c == '-' || c == '_'));
            string fileName = $"{prefix}_{safeBorrowCode}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            return Path.Combine(baseDir, fileName);
        }

        private static void ExportBorrowPdf(string filePath, BorrowReceiptData data)
        {
            using var doc = new PdfDocument();
            doc.Info.Title = $"Phieu muon {data.BorrowCode}";

            PdfPage page = doc.AddPage();
            page.Width = XUnit.FromMillimeter(148);
            page.Height = XUnit.FromMillimeter(210);

            using XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
            XFont boldFont = new XFont("Arial", 11, XFontStyle.Bold);
            XFont normalFont = new XFont("Arial", 10, XFontStyle.Regular);

            int y = 30;
            y = DrawCentered(gfx, page, data.LibraryName, titleFont, y);
            y += 8;
            y = DrawLine(gfx, $"PHIEU MUON: {data.BorrowCode}", boldFont, y);
            y = DrawLine(gfx, $"Doc gia: {data.MemberName} ({data.MemberCode})", normalFont, y);
            y = DrawLine(gfx, $"Ngay muon: {data.BorrowDate:dd/MM/yyyy HH:mm}", normalFont, y);
            y = DrawLine(gfx, $"Han tra: {data.DueDate:dd/MM/yyyy}", normalFont, y);
            if (!string.IsNullOrWhiteSpace(data.StaffName))
            {
                y = DrawLine(gfx, $"Thu thu: {data.StaffName}", normalFont, y);
            }

            y += 8;
            y = DrawLine(gfx, "Thong tin sach:", boldFont, y);
            y = DrawLine(gfx, $"- {data.BookTitle}", normalFont, y);
            if (!string.IsNullOrWhiteSpace(data.BookBarcode))
            {
                y = DrawLine(gfx, $"Barcode sach: {data.BookBarcode}", normalFont, y);
            }
            if (!string.IsNullOrWhiteSpace(data.BookIsbn))
            {
                y = DrawLine(gfx, $"ISBN: {data.BookIsbn}", normalFont, y);
            }

            y += 10;
            DrawBorrowCodeBarcode(gfx, data.BorrowCode, y);
            doc.Save(filePath);
        }

        private static void ExportReturnPdf(string filePath, ReturnReceiptData data)
        {
            using var doc = new PdfDocument();
            doc.Info.Title = $"Phieu tra {data.BorrowCode}";

            PdfPage page = doc.AddPage();
            page.Width = XUnit.FromMillimeter(148);
            page.Height = XUnit.FromMillimeter(210);

            using XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
            XFont boldFont = new XFont("Arial", 11, XFontStyle.Bold);
            XFont normalFont = new XFont("Arial", 10, XFontStyle.Regular);

            int y = 30;
            y = DrawCentered(gfx, page, data.LibraryName, titleFont, y);
            y += 8;
            y = DrawLine(gfx, $"PHIEU TRA: {data.BorrowCode}", boldFont, y);
            y = DrawLine(gfx, $"Doc gia: {data.MemberName} ({data.MemberCode})", normalFont, y);
            y = DrawLine(gfx, $"Ngay muon: {data.BorrowDate:dd/MM/yyyy}", normalFont, y);
            y = DrawLine(gfx, $"Han tra: {data.DueDate:dd/MM/yyyy}", normalFont, y);
            y = DrawLine(gfx, $"Ngay tra: {data.ReturnDate:dd/MM/yyyy HH:mm}", normalFont, y);
            if (!string.IsNullOrWhiteSpace(data.StaffName))
            {
                y = DrawLine(gfx, $"Thu thu: {data.StaffName}", normalFont, y);
            }

            y += 8;
            y = DrawLine(gfx, "Thong tin sach:", boldFont, y);
            y = DrawLine(gfx, $"- {data.BookTitle}", normalFont, y);
            if (!string.IsNullOrWhiteSpace(data.BookBarcode))
            {
                y = DrawLine(gfx, $"Barcode sach: {data.BookBarcode}", normalFont, y);
            }
            if (!string.IsNullOrWhiteSpace(data.BookIsbn))
            {
                y = DrawLine(gfx, $"ISBN: {data.BookIsbn}", normalFont, y);
            }

            y += 8;
            y = DrawLine(gfx, $"Tien phat: {data.FineAmount:N0} VND", boldFont, y);
            y += 8;
            DrawBorrowCodeBarcode(gfx, data.BorrowCode, y);

            doc.Save(filePath);
        }

        private static int DrawCentered(XGraphics gfx, PdfPage page, string text, XFont font, int y)
        {
            XSize size = gfx.MeasureString(text, font);
            double x = Math.Max(20, (page.Width - size.Width) / 2);
            gfx.DrawString(text, font, XBrushes.Black, new XPoint(x, y));
            return y + (int)size.Height + 2;
        }

        private static int DrawLine(XGraphics gfx, string text, XFont font, int y)
        {
            var rect = new XRect(20, y, 380, 200);
            gfx.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            XSize size = gfx.MeasureString(text, font);
            return y + (int)Math.Ceiling(size.Height) + 3;
        }

        private static void DrawBorrowCodeBarcode(XGraphics gfx, string borrowCode, int y)
        {
            if (string.IsNullOrWhiteSpace(borrowCode))
            {
                return;
            }

            using Bitmap barcode = GenerateCode128Barcode(borrowCode);
            using var stream = new MemoryStream();
            barcode.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            using XImage image = XImage.FromStream(() => stream);
            gfx.DrawImage(image, 20, y, 260, 70);
            gfx.DrawString(borrowCode, new XFont("Arial", 9, XFontStyle.Bold), XBrushes.Black, new XPoint(20, y + 84));
        }

        private static Bitmap GenerateCode128Barcode(string value)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Width = 650,
                    Height = 180,
                    Margin = 4,
                    PureBarcode = true
                }
            };

            var pixelData = writer.Write(value);
            var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var data = bitmap.LockBits(
                new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);

            try
            {
                Marshal.Copy(pixelData.Pixels, 0, data.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return bitmap;
        }
    }

    public class BorrowReceiptData
    {
        public string LibraryName { get; set; } = "";
        public string BorrowCode { get; set; } = "";
        public string MemberName { get; set; } = "";
        public string MemberCode { get; set; } = "";
        public string BookTitle { get; set; } = "";
        public string? BookIsbn { get; set; }
        public string? BookBarcode { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? StaffName { get; set; }
    }

    public class ReturnReceiptData
    {
        public string LibraryName { get; set; } = "";
        public string BorrowCode { get; set; } = "";
        public string MemberName { get; set; } = "";
        public string MemberCode { get; set; } = "";
        public string BookTitle { get; set; } = "";
        public string? BookIsbn { get; set; }
        public string? BookBarcode { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal FineAmount { get; set; }
        public string? StaffName { get; set; }
    }
}
