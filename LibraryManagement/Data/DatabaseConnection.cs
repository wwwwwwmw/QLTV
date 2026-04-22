using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace LibraryManagement.Data
{
    public static class DatabaseConnection
    {
        private static string? _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    // Ưu tiên lấy từ App.config
                    var configConn = ConfigurationManager.ConnectionStrings["LibraryDB"]?.ConnectionString;

                    if (!string.IsNullOrEmpty(configConn))
                    {
                        _connectionString = configConn;
                    }
                    else
                    {
                        // Fallback mặc định LocalDB (CHUẨN NHẤT)
                        _connectionString = @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
                    }
                }

                return _connectionString;
            }
            set => _connectionString = value;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public static bool TestConnection(string connectionString, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public static void UpdateConnectionString(
            string server,
            string database,
            bool useWindowsAuth = true,
            string? userId = null,
            string? password = null)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                IntegratedSecurity = useWindowsAuth,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true
            };

            if (!useWindowsAuth)
            {
                builder.UserID = userId;
                builder.Password = password;
            }

            _connectionString = builder.ConnectionString;
        }
    }
}