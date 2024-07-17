using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Feedback : BaseEntity
    {
        [Required]
        public string FeedbackDescription { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string FeedbackText { get; set; } = string.Empty;

        [Required]
        public DateTime FeedbackDate { get; set; }

        public User? User { get; }
    }
}
