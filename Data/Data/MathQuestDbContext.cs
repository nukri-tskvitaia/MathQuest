using Data.Entities;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class MathQuestDbContext(DbContextOptions<MathQuestDbContext> options) : IdentityDbContext<User>(options)
    {
        //private readonly AbstractDataFactory _factory = factory;

        // Normal Entity Tables
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<Theory> Theories { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<ExerciseRating> ExerciseRatings { get; set; }

        public DbSet<TheoryRating> TheoryRatings { get; set; }

        public DbSet<Leaderboard> Leaderboards { get; set; }

        public DbSet<UserAchievement> UserAchievements { get; set; }

        public DbSet<UserExercise> UserExercises { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        public DbSet<UserTheory> UserTheories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Renaming identity tables
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            // User
            builder.Entity<User>(entity =>
            {
                entity
                    .HasOne(user => user.Person)
                    .WithOne()
                    .HasForeignKey<User>(u => u.PersonId)
                    .OnDelete(DeleteBehavior.Cascade); // deleting person deletes user but not vice-verca
                entity
                    .HasOne(user => user.Grade)
                    .WithMany(grade => grade.Users)
                    .HasForeignKey(u => u.GradeId);
                entity
                    .HasOne(user => user.Leaderboard)
                    .WithOne(leaderboard => leaderboard.User)
                    .HasForeignKey<Leaderboard>(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Person entity
            builder.Entity<Person>(entity =>
            {
                entity.Property(p => p.Id);//.ValueGeneratedOnAdd();
            });

            // Refresh Token entity
            builder.Entity<RefreshToken>(entity =>
            {
                entity.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // Feedback entity
            builder.Entity<Feedback>(entity =>
            {
                entity
                    .HasOne(feedback => feedback.User)
                    .WithMany(user => user.Feedbacks)
                    .HasForeignKey(f => f.UserId);
            });

            // UserAchievement entity
            builder.Entity<UserAchievement>(entity =>
            {
                entity
                    .HasOne(userAchievement => userAchievement.User)
                    .WithMany(user => user.UserAchievements)
                    .HasForeignKey(ua => ua.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity
                    .HasOne(userAchievement => userAchievement.Achievement)
                    .WithMany(achievement => achievement.UserAchievements)
                    .HasForeignKey(ua => ua.AchievementId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Friendship entity
            builder.Entity<Friend>(entity =>
            {
                entity
                    .HasOne(friend => friend.RequestedBy)
                    .WithMany(user => user.SentFriendRequests)
                    .HasForeignKey(f => f.RequestedById)
                    .OnDelete(DeleteBehavior.ClientCascade);
                entity
                    .HasOne(friend => friend.RequestedTo)
                    .WithMany(user => user.ReceivedFriendRequests)
                    .HasForeignKey(f => f.RequestedToId)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            // Message entity
            builder.Entity<Message>(entity =>
            {
                entity
                    .HasOne(message => message.Sender)
                    .WithMany(sender => sender.MessagesSent)
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(message => message.Receiver)
                    .WithMany(receiver => receiver.MessagesReceived)
                    .HasForeignKey(m => m.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UserNotification
            builder.Entity<UserNotification>(entity =>
            {
                entity
                    .HasOne(userNotification => userNotification.User)
                    .WithMany(user => user.UserNotifications)
                    .HasForeignKey(un => un.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity
                    .HasOne(userNotification => userNotification.Notification)
                    .WithMany(notification => notification.UserNotifications)
                    .HasForeignKey(un => un.NotificationId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // UserExercise entity
            builder.Entity<UserExercise>(entity =>
            {
                entity
                    .HasOne(userExercise => userExercise.User)
                    .WithMany(user => user.UserExercises)
                    .HasForeignKey(ue => ue.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity
                    .HasOne(userExercise => userExercise.Exercise)
                    .WithMany(exercise => exercise.UserExercises)
                    .HasForeignKey(ue => ue.ExerciseId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // UserTheory entity
            builder.Entity<UserTheory>(entity =>
            {
                entity
                    .HasOne(userTheory => userTheory.User)
                    .WithMany(user => user.UserTheories)
                    .HasForeignKey(ut => ut.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity
                    .HasOne(userTheory => userTheory.Theory)
                    .WithMany(theory => theory.UserTheories)
                    .HasForeignKey(ut => ut.TheoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // TheoryRating entity
            builder.Entity<TheoryRating>(entity =>
            {
                entity
                    .HasOne(theoryRating => theoryRating.Theory)
                    .WithMany(theory => theory.TheoryRatings)
                    .HasForeignKey(tr => tr.TheoryId)
                    .OnDelete(DeleteBehavior.ClientCascade);
                entity
                    .HasOne(theoryRating => theoryRating.User)
                    .WithMany(user => user.TheoryRatings)
                    .HasForeignKey(tr => tr.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ExerciseRating entity
            builder.Entity<ExerciseRating>(entity =>
            {
                entity
                    .HasOne(exerciseRating => exerciseRating.Exercise)
                    .WithMany(exercise => exercise.ExerciseRatings)
                    .HasForeignKey(er => er.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientCascade);
                entity
                    .HasOne(exerciseRating => exerciseRating.User)
                    .WithMany(user => user.ExerciseRatings)
                    .HasForeignKey(er => er.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed Initial Data For Testing
            //builder.Entity<Grade>().HasData(_factory.GetGradeData());
            //builder.Entity<IdentityRole>().HasData(_factory.GetIdentityRoleData());
        }
    }
}
