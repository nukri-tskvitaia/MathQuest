using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public enum LoginResult
    {
        Success,
        RequiresTwoFactor,
        InvalidCredentials,
        AlreadySignedIn,
        LockedOut
    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
