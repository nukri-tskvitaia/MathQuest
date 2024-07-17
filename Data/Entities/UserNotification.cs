using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class UserNotification : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int NotificationId { get; set; }

        public User? User { get; }

        public Notification? Notification { get; }
    }
}
