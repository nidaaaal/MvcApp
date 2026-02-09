namespace MvcApp.Models.Common
{
    public class AccountResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = null!;
    }
}
