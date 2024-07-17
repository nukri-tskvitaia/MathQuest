namespace Business.Models
{
    public class TwoFactorModel
    {
        public string Email { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public bool RememberMe { get; set; }

        public bool RememberTwoFactorDevice { get; set; }
    }
}
