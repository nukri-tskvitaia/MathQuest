using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class UserAchievement : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int AchievementId { get; set; }

        public User? User { get; }

        public Achievement? Achievement { get; }
    }
}
