using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class ExerciseRating : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int ExerciseId { get; set; }

        [Required]
        public decimal Rating { get; set; }

        [Required]
        public DateTime RatingDate { get; set; }

        public string? FeedbackText { get; set; } = string.Empty;

        public User? User { get; }

        public Exercise? Exercise { get; }
    }
}
