using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Achievement : BaseEntity
    {
        [Required]
        public string AchievementName { get; set; } = string.Empty;

        [Required]
        public string AchievementDescription { get; set; } = string.Empty;

        [Required]
        public string AchievementType { get; set; } = string.Empty;

        [Required]
        public DateTime AchievementDate { get; set; }

        public ICollection<UserAchievement>? UserAchievements { get; }
    }
}
