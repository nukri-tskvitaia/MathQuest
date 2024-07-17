using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Leaderboard : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int Points { get; set; }

        public User? User { get; set; }
    }
}
