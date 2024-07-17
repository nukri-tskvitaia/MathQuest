using System.ComponentModel.DataAnnotations;

namespace Data.Entities.Identity
{
    public class RefreshToken : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Expires { get; set; }
        public User? User { get; set; }
    }
}
