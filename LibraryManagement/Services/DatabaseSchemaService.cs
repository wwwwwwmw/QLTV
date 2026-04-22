using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Dapper;
using LibraryManagement.Data;

namespace LibraryManagement.Services
{
    public class DatabaseSchemaService
    {
        private const string ScriptFileName = "DatabaseInit.sql";

        public void Ensure()
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            string script = LoadEmbeddedScript();
            string[] batches = Regex.Split(script, @"^\s*GO\s*(?:--.*)?$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (string batch in batches)
            {
                string sql = batch.Trim();
                if (string.IsNullOrWhiteSpace(sql))
                    continue;

                conn.Execute(sql, commandTimeout: 180);
            }
        }

        private static string LoadEmbeddedScript()
        {
            Assembly asm = typeof(DatabaseSchemaService).Assembly;
            string? resourceName = asm.GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith("Database.DatabaseInit.sql", StringComparison.OrdinalIgnoreCase))
                ?? asm.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(ScriptFileName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(resourceName))
                throw new InvalidOperationException("Không tìm thấy script khởi tạo database nhúng trong ứng dụng.");

            using Stream? stream = asm.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new InvalidOperationException("Không đọc được script khởi tạo database.");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
