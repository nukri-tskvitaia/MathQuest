using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Message : BaseEntity
    {
        [Required]
        public string SenderId { get; set; } = string.Empty;

        [Required]
        public string ReceiverId { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public DateTime SentDate { get; set; }

        public User? Sender { get; }

        public User? Receiver { get; }
    }
}
