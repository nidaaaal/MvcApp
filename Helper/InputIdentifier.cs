using System.Text.RegularExpressions;

namespace MvcApp.Helper
{
    public static class InputIdentifier
    {

        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        private static readonly Regex PhoneRegex = new Regex(@"^\+?[0-9]{10}$");

        public enum InputType
        {
            Email,
            Phone,
            Invalid
        }

        public static InputType Identify(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return InputType.Invalid;

            string cleanInput = input.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            if (EmailRegex.IsMatch(input))
            {
                return InputType.Email;
            }

            if (PhoneRegex.IsMatch(cleanInput))
            {
                return InputType.Phone;
            }

            return InputType.Invalid;
        }
    }
}
