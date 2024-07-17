using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public enum FriendRequestStatus
    {
        None,
        Pending,
        Approved,
        Rejected,
    }

    public class Friend : BaseEntity
    {
        [Required]
        public string RequestedById { get; set; } = string.Empty; // Id of who sent request

        [Required]
        public string RequestedToId { get; set; } = string.Empty; // Id who received request

        [Required]
        public DateTime RequestTime { get; set; }

        public DateTime? BecameFriendsDate { get; set; }

        public FriendRequestStatus FriendRequestStatus { get; set; } = FriendRequestStatus.None;

        public User? RequestedBy { get; set; } = new User(); // User who sent request

        public User? RequestedTo { get; set; } = new User(); // User who received request
    }
}
