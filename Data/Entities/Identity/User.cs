using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities.Identity
{
    public class User : IdentityUser
    {
        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public DateTime? LastLogin { get; set; } = DateTime.Now;

        public bool? ActiveStatus { get; set; }

        public string? ProfilePictureUrl { get; set; }

        // Foreign Keys
        public int PersonId { get; set; }

        public int GradeId { get; set; }

        // Navigational Properties
        public Person? Person { get; set; }

        public Grade? Grade { get; } // M:1 (one user one grade) - and in grade it is defined as: (one grade many users)

        public Leaderboard? Leaderboard { get; } // 1:1

        public ICollection<RefreshToken>? RefreshTokens { get; }

        public ICollection<UserTheory>? UserTheories { get; }

        public ICollection<TheoryRating>? TheoryRatings { get; }

        public ICollection<UserExercise>? UserExercises { get; }

        public ICollection<ExerciseRating>? ExerciseRatings { get; }

        public ICollection<UserNotification>? UserNotifications { get; }

        public ICollection<UserAchievement>? UserAchievements { get; }

        public ICollection<Feedback>? Feedbacks { get; }

        public ICollection<Friend>? SentFriendRequests { get; set; } = new List<Friend>();

        public ICollection<Friend>? ReceivedFriendRequests { get; set; } = new List<Friend>();

        public ICollection<Message>? MessagesSent { get; }

        public ICollection<Message>? MessagesReceived { get; }
    }
}
