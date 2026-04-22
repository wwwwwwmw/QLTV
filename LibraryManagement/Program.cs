using System;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Services;

namespace LibraryManagement
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            if (!DatabaseConnection.TestConnection(out string error))
            {
                MessageBox.Show(
                    $"Không thể kết nối đến Database!\nLỗi: {error}\nVui lòng cấu hình lại kết nối.",
                    "Lỗi Kết Nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                using (var formConfig = new Forms.FormConnectionConfig())
                {
                    formConfig.ShowDialog();
                }

                if (!DatabaseConnection.TestConnection(out _))
                    return;
            }

            try
            {
                new DatabaseSchemaService().Ensure();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể khởi tạo schema và dữ liệu mẫu của database.\nLỗi: {ex.Message}",
                    "Lỗi khởi tạo dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Mở trang công khai trước - không yêu cầu đăng nhập
            // Người dùng có thể xem sách, tìm kiếm
            // Khi cần mượn sách hoặc các chức năng quản lý -> đăng nhập
            Application.Run(new Forms.FormPublic());
        }
    }
}
