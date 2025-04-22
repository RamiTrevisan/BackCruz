using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Application.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates an email address
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use MailAddress for basic validation
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates a phone number (basic validation)
        /// </summary>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Remove spaces, dashes, parentheses
            var cleaned = Regex.Replace(phoneNumber, @"[\s\-\(\)]", "");

            // Check if it's only digits and has a reasonable length
            return Regex.IsMatch(cleaned, @"^\d{7,15}$");
        }

        /// <summary>
        /// Validates password strength
        /// </summary>
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return password.Length >= 8 && // Minimum length
                   password.Any(char.IsUpper) && // At least one uppercase
                   password.Any(char.IsLower) && // At least one lowercase
                   password.Any(char.IsDigit); // At least one digit
        }
    }
}
