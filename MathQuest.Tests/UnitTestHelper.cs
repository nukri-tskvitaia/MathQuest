using Microsoft.EntityFrameworkCore;
using Data.Data;
using Data.Entities.Identity;
using Data.Entities;
using AutoMapper;
using Business;

namespace MathQuest.Tests
{
    internal static class UnitTestHelper
    {
        public static MathQuestDbContext GetMathQuestDbContext()
        {
            var options = new DbContextOptionsBuilder<MathQuestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new MathQuestDbContext(options);
            SeedData(context);

            return context;
        }

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        public static void SeedData(MathQuestDbContext context)
        {
            context.Users.AddRange(
                new User { Id = "1", PersonId = 1, RegistrationDate = new DateTime(2023, 2, 11, 0, 0, 0, DateTimeKind.Local) },
                new User { Id = "2", PersonId = 2, RegistrationDate = new DateTime(2020, 7, 15, 0, 0, 0, DateTimeKind.Local) },
                new User { Id = "3", PersonId = 3, RegistrationDate = new DateTime(2020, 7, 15, 0, 0, 0, DateTimeKind.Local) });
            context.People.AddRange(
                new Person { Id = 1, FirstName = "Nukri", LastName = "Tskvitaia", BirthDate = new DateTime(2001, 7, 13, 0, 0, 0, DateTimeKind.Local), Gender = GenderList.Male },
                new Person { Id = 2, FirstName = "Jaba", LastName = "Shengelia", BirthDate = new DateTime(1978, 8, 18, 0, 0, 0, DateTimeKind.Local), Gender = GenderList.Male },
                new Person { Id = 3, FirstName = "Noe", LastName = "Shengelia", BirthDate = new DateTime(1994, 8, 18, 0, 0, 0, DateTimeKind.Local), Gender = GenderList.Male });
            context.Achievements.AddRange(
                new Achievement { Id = 1, AchievementName = "First Grader", AchievementDescription = "You successfully completed 1st grade.", AchievementType = "General", AchievementDate = new DateTime(2021, 7, 13, 0, 0, 0, DateTimeKind.Local) },
                new Achievement { Id = 2, AchievementName = "Second Grader", AchievementDescription = "You successfully completed 2nd grade.", AchievementType = "General", AchievementDate = new DateTime(2022, 8, 18, 0, 0, 0, DateTimeKind.Local), },
                new Achievement { Id = 3, AchievementName = "Third Grader", AchievementDescription = "You successfully completed 3rd grade.", AchievementType = "General", AchievementDate = new DateTime(2020, 8, 18, 0, 0, 0, DateTimeKind.Local), });
            context.ExerciseRatings.AddRange(
                new ExerciseRating { UserId = "1", ExerciseId = 2, FeedbackText = "Excellent", Rating = 9, RatingDate = new DateTime(2023, 8, 18, 0, 0, 0, DateTimeKind.Local) },
                new ExerciseRating { UserId = "2", ExerciseId = 1, FeedbackText = "Satisfiable", Rating = 8, RatingDate = new DateTime(2023, 11, 12, 0, 0, 0, DateTimeKind.Local) },
                new ExerciseRating { UserId = "3", ExerciseId = 1, FeedbackText = "So-so", Rating = 7, RatingDate = new DateTime(2023, 7, 19, 0, 0, 0, DateTimeKind.Local) });
            context.Exercises.AddRange(
                new Exercise { Id = 1, GradeId = 1, Title = "First Grader", Description = "Designed for first grade.", Question = "What is 2 + 2 ?", PossibleAnswers = ["1", "2", "3", "4"], CorrectAnswer = "3", DifficultyLevel = 1 },
                new Exercise { Id = 2, GradeId = 1, Title = "First Grader", Description = "Designed for first grade.", Question = "What is 2 + 4 ?", PossibleAnswers = ["5", "6", "7", "8"], CorrectAnswer = "2", DifficultyLevel = 1 },
                new Exercise { Id = 3, GradeId = 1, Title = "First Grader", Description = "Designed for first grade.", Question = "What is 2 + 7 ?", PossibleAnswers = ["9", "10", "11", "12"], CorrectAnswer = "0", DifficultyLevel = 1 });
            context.Feedbacks.AddRange(
                new Feedback { Id = 1, UserId = "1", FeedbackDescription = "Site", FeedbackText = "It is very good. No suggestions", FeedbackDate = new DateTime(2023, 11, 12, 0, 0, 0, DateTimeKind.Local) },
                new Feedback { Id = 2, UserId = "2", FeedbackDescription = "Site", FeedbackText = "Thank you very much!", FeedbackDate = new DateTime(2023, 11, 6, 0, 0, 0, DateTimeKind.Local) },
                new Feedback { Id = 3, UserId = "3", FeedbackDescription = "Site", FeedbackText = "It is very good. Add more quizzes", FeedbackDate = new DateTime(2023, 3, 12, 0, 0, 0, DateTimeKind.Local) });
            context.Messages.AddRange(
                new Message { Id = 1, SenderId = "1", ReceiverId = "2", Text = "Hello!", SentDate = new DateTime(2023, 7, 12, 10, 0, 0, DateTimeKind.Local) },
                new Message { Id = 2, SenderId = "2", ReceiverId = "1", Text = "Hi there!", SentDate = new DateTime(2023, 7, 12, 10, 5, 0, DateTimeKind.Local) },
                new Message { Id = 3, SenderId = "1", ReceiverId = "3", Text = "How are you?", SentDate = new DateTime(2023, 7, 12, 11, 0, 0, DateTimeKind.Local) });
            context.Friends.AddRange(
                new Friend { Id = 1, RequestedById = "1", RequestedToId = "2", FriendRequestStatus = FriendRequestStatus.Pending, BecameFriendsDate = null },
                new Friend { Id = 2, RequestedById = "2", RequestedToId = "1", FriendRequestStatus = FriendRequestStatus.Approved, BecameFriendsDate = new DateTime(2023, 6, 1) },
                new Friend { Id = 3, RequestedById = "3", RequestedToId = "1", FriendRequestStatus = FriendRequestStatus.Rejected, BecameFriendsDate = null });
            context.Grades.AddRange(
                new Grade { Id = 1, GradeName = "Grade 1" },
                new Grade { Id = 2, GradeName = "Grade 2" },
                new Grade { Id = 3, GradeName = "Grade 3" });
            context.Leaderboards.AddRange(
                new Leaderboard { Id = 1, UserId = "1", Points = 100 },
                new Leaderboard { Id = 2, UserId = "2", Points = 150 },
                new Leaderboard { Id = 3, UserId = "3", Points = 200 });


            context.SaveChanges();
        }
    }
}
