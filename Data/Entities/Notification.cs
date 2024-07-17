using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Notification : BaseEntity
    {
        [Required]
        public string NotificationType { get; set; } = string.Empty;

        [Required]
        public string NotificationText { get; set; } = string.Empty;

        [Required]
        public DateTime NotificationDate { get; set; }

        public ICollection<UserNotification>? UserNotifications { get; }
    }
}
