using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace LibraryManagement.Utils
{
    /// <summary>
    /// Bộ kiểm tra dữ liệu đầu vào theo chuẩn thông dụng tại Việt Nam.
    /// </summary>
    public static class VietnamInputValidator
    {
        private static readonly Regex NameRegex = new Regex(@"^[\p{L}][\p{L}\s'.-]{1,99}$", RegexOptions.Compiled);
        private static readonly Regex UsernameRegex = new Regex(@"^[a-zA-Z0-9._]{4,30}$", RegexOptions.Compiled);
        private static readonly Regex MobilePhoneRegex = new Regex(@"^0(3|5|7|8|9)\d{8}$", RegexOptions.Compiled);
        private static readonly Regex PhoneFlexibleRegex = new Regex(@"^0\d{9,10}$", RegexOptions.Compiled);
        private static readonly Regex IdentityCardRegex = new Regex(@"^\d{12}$", RegexOptions.Compiled);

        public static bool IsValidFullName(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return NameRegex.IsMatch(value.Trim());
        }

        public static bool IsValidUsername(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return UsernameRegex.IsMatch(value.Trim());
        }

        public static bool IsValidVietnamMobilePhone(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return MobilePhoneRegex.IsMatch(value.Trim());
        }

        public static bool IsValidVietnamPhoneFlexible(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return PhoneFlexibleRegex.IsMatch(value.Trim());
        }

        public static bool IsValidIdentityCard12(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return IdentityCardRegex.IsMatch(value.Trim());
        }

        public static bool IsValidEmail(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            try
            {
                _ = new MailAddress(value.Trim());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsStrongPassword(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 8)
            {
                return false;
            }

            bool hasLetter = false;
            bool hasDigit = false;
            foreach (char c in value)
            {
                if (char.IsLetter(c)) hasLetter = true;
                if (char.IsDigit(c)) hasDigit = true;
            }

            return hasLetter && hasDigit;
        }
    }
}
