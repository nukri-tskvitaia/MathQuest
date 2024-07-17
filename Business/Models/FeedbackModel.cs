using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }

        [Required]
        public string FeedbackDescription { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string FeedbackText { get; set; } = string.Empty;

        [Required]
        public DateTime FeedbackDate { get; set; } = DateTime.Now;
    }
}
