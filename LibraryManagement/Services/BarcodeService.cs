using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.Data;
using ZXing;
using ZXing.Common;

namespace LibraryManagement.Services
{
    public class BarcodeService
    {
        private readonly BookDAO _bookDao = new BookDAO();

        public string GenerateUniqueBarcode()
        {
            for (int i = 0; i < 20; i++)
            {
                string code = $"BC{DateTime.UtcNow:yyyyMMddHHmmssfff}{Guid.NewGuid():N}".Substring(0, 24);
                if (!_bookDao.BarcodeExists(code))
                    return code;
            }

            return $"BC{Guid.NewGuid():N}".Substring(0, 24);
        }

        public string? TryDecodeBarcode(Bitmap bitmap)
        {
            try
            {
                var formats = new[]
                {
                    BarcodeFormat.CODE_128,
                    BarcodeFormat.CODE_39,
                    BarcodeFormat.EAN_13,
                    BarcodeFormat.EAN_8,
                    BarcodeFormat.QR_CODE
                };

                var hints = new System.Collections.Generic.Dictionary<DecodeHintType, object>
                {
                    { DecodeHintType.TRY_HARDER, true },
                    { DecodeHintType.POSSIBLE_FORMATS, formats }
                };

                var reader = new MultiFormatReader { Hints = hints };

                using var srcBitmap = Ensure24bpp(bitmap);
                var rect = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);
                var data = srcBitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                try
                {
                    int bytesCount = Math.Abs(data.Stride) * data.Height;
                    byte[] bytes = new byte[bytesCount];
                    Marshal.Copy(data.Scan0, bytes, 0, bytesCount);

                    var source = new RGBLuminanceSource(bytes, srcBitmap.Width, srcBitmap.Height, RGBLuminanceSource.BitmapFormat.BGR24);
                    var binary = new BinaryBitmap(new HybridBinarizer(source));
                    var result = reader.decode(binary);
                    return string.IsNullOrWhiteSpace(result?.Text) ? null : result.Text.Trim();
                }
                finally
                {
                    srcBitmap.UnlockBits(data);
                }
            }
            catch
            {
                return null;
            }
        }

        private static Bitmap Ensure24bpp(Bitmap bitmap)
        {
            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
                return (Bitmap)bitmap.Clone();

            var clone = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
            using var g = Graphics.FromImage(clone);
            g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
            return clone;
        }

        public Task<string?> ScanWithWebcamAsync(IWin32Window owner, CancellationToken cancellationToken)
        {
            using var form = new Forms.FormBarcodeScanner(this, cancellationToken);
            return Task.FromResult(form.ShowDialog(owner) == DialogResult.OK ? form.BarcodeText : null);
        }
    }
}
