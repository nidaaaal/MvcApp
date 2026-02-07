using System.Text.RegularExpressions;


namespace MvcApp.Helper
{
    public static class PasswordValidator
    {
        private const int MinLength = 8;
        private const int MaxLength = 50;

        public static (bool IsValid, string ErrorMessage) Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password cannot be empty.");

            if (password.Length < MinLength)
                return (false, $"Password must be at least {MinLength} characters long.");

            if (password.Length > MaxLength)
                return (false, $"Password must not exceed {MaxLength} characters.");

            if (!Regex.IsMatch(password, "[A-Z]"))
                return (false, "Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, "[a-z]"))
                return (false, "Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, "[0-9]"))
                return (false, "Password must contain at least one number.");

            if (!Regex.IsMatch(password, "[!@#$%^&*(),.?\":{}|<>]"))
                return (false, "Password must contain at least one special character.");

            return (true, "Success");
        }
        
    }
}
