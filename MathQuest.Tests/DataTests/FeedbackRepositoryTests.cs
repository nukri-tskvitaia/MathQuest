using Castle.Core.Resource;
using Data.Entities;
using Data.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathQuest.Tests.DataTests
{
    [TestFixture]
    public class FeedbackRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task FeedbackRepositoryGetByIdAsyncReturnsSingleValue(int id)
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var feedbackRepository = new FeedbackRepository(context);

            var feedback = await feedbackRepository.GetByIdAsync(id);

            var expected = ExpectedFeedbacks.FirstOrDefault(x => x.Id == id);

            Assert.That(feedback, Is.EqualTo(expected).Using(new FeedbackEqualityComparer()), message: "GetByIdAsync method works incorrectly");
        }

        [Test]
        public async Task FeedbackRepositoryGetAllAsyncReturnsAllValues()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var feedbackRepository = new FeedbackRepository(context);

            var feedbacks = await feedbackRepository.GetAllAsync();

            Assert.That(feedbacks, Is.EqualTo(ExpectedFeedbacks).Using(new FeedbackEqualityComparer()), message: "GetAllAsync method works incorrectly");
        }

        [Test]
        public async Task FeedbackRepositoryAddAsyncAddsValueToDatabase()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var feedbackRepository = new FeedbackRepository(context);
            var feedback = new Feedback
            {
                Id = 4,
                UserId = "4",
                FeedbackDescription = "Feature",
                FeedbackText = "New feature request.",
                FeedbackDate = DateTime.UtcNow
            };

            await feedbackRepository.AddAsync(feedback);
            await context.SaveChangesAsync();

            Assert.That(context.Feedbacks.Count(), Is.EqualTo(4), message: "AddAsync method works incorrectly");
        }

        [Test]
        public async Task FeedbackRepositoryDeleteByIdAsyncDeletesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var feedbackRepository = new FeedbackRepository(context);

            await feedbackRepository.DeleteByIdAsync(1);
            await context.SaveChangesAsync();

            Assert.That(context.Feedbacks.Count(), Is.EqualTo(2), message: "DeleteByIdAsync works incorrectly");
        }

        [Test]
        public async Task FeedbackRepositoryUpdateUpdatesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var feedbackRepository = new FeedbackRepository(context);
            var feedback = new Feedback
            {
                Id = 2,
                UserId = "2",
                FeedbackDescription = "Site",
                FeedbackText = "Updated feedback text.",
                FeedbackDate = new DateTime(2023, 11, 6, 0, 0, 0, DateTimeKind.Local)
            };

            feedbackRepository.Update(feedback);
            await context.SaveChangesAsync();

            var updatedFeedback = await feedbackRepository.GetByIdAsync(2);
            Assert.That(updatedFeedback, Is.EqualTo(feedback).Using(new FeedbackEqualityComparer()), message: "Update method works incorrectly");
        }

        [Test]
        public async Task FeedbackRepositoryGetUserFeedbacksByUserIdAsyncReturnsCorrectValues()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var feedbackRepository = new FeedbackRepository(context);
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 12, 31);

            var feedbacks = await feedbackRepository.GetUserFeedbacksByUserIdAsync("2", fromDate, toDate);

            var expectedFeedbacks = ExpectedFeedbacks.Where(x => x.UserId == "2" && x.FeedbackDate >= fromDate && x.FeedbackDate <= toDate);

            Assert.That(feedbacks, Is.EqualTo(expectedFeedbacks).Using(new FeedbackEqualityComparer()), message: "GetUserFeedbacksByUserIdAsync method works incorrectly");
        }

        [Test]
        public async Task FeedbackRepositoryGetByUserAndFeedbackIdsAsyncReturnsCorrectValue()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var feedbackRepository = new FeedbackRepository(context);

            var feedback = await feedbackRepository.GetByUserAndFeedbackIdsAsync("2", 2);

            var expected = ExpectedFeedbacks.FirstOrDefault(x => x.UserId == "2" && x.Id == 2);

            Assert.That(feedback, Is.EqualTo(expected).Using(new FeedbackEqualityComparer()), message: "GetByUserAndFeedbackIdsAsync method works incorrectly");
        }

        private static IEnumerable<Feedback> ExpectedFeedbacks =>
            new[]
            {
                new Feedback { Id = 1, UserId = "1", FeedbackDescription = "Site", FeedbackText = "It is very good. No suggestions", FeedbackDate = new DateTime(2023, 11, 12, 0, 0, 0, DateTimeKind.Local) },
                new Feedback { Id = 2, UserId = "2", FeedbackDescription = "Site", FeedbackText = "Thank you very much!", FeedbackDate = new DateTime(2023, 11, 6, 0, 0, 0, DateTimeKind.Local) },
                new Feedback { Id = 3, UserId = "3", FeedbackDescription = "Site", FeedbackText = "It is very good. Add more quizzes", FeedbackDate = new DateTime(2023, 3, 12, 0, 0, 0, DateTimeKind.Local) }
            };
    }
}