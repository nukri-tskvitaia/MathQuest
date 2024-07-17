using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public enum LockoutResult
    {
        Success,
        UserNotFound,
        AlreadyLockedOut,
        LockoutFailed
    }
    public class LockoutModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTimeOffset LockoutEnd { get; set; }
    }
}
