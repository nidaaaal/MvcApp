namespace MvcApp.Models.Common
{
    public class AccountResult
    {
        public bool IsSuccess { get; set; }

        public int? UserId { get; set; }

        public string? Token { get; set; }

        public bool IsAdmin { get; set; }

        public string ErrorMessage { get; set; } = null!;
    }
}
