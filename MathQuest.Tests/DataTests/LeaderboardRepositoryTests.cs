using Data.Entities;
using Data.Repositories;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathQuest.Tests.DataTests
{
    [TestFixture]
    public class LeaderboardRepositoryTests
    {
        [Test]
        public async Task LeaderboardRepositoryAddAsyncAddsValueToDatabase()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var leaderboardRepository = new LeaderboardRepository(context);
            var leaderboard = new Leaderboard
            {
                Id = 4,
                UserId = "4",
                Points = 250
            };

            await leaderboardRepository.AddAsync(leaderboard);
            await context.SaveChangesAsync();

            Assert.That(context.Leaderboards.Count(), Is.EqualTo(4), message: "AddAsync method works incorrectly");
        }

        [Test]
        public async Task LeaderboardRepositoryGetAllAsyncReturnsAllValues()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var leaderboardRepository = new LeaderboardRepository(context);
            var leaderboards = await leaderboardRepository.GetAllAsync();

            Assert.That(leaderboards, Is.EqualTo(ExpectedLeaderboards).Using(new LeaderboardEqualityComparer()), message: "GetAllAsync method works incorrectly");
        }

        [Test]
        public async Task LeaderboardRepositoryGetByIdAsyncReturnsSingleValue()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var leaderboardRepository = new LeaderboardRepository(context);

            var leaderboard = await leaderboardRepository.GetByIdAsync(1);

            var expected = ExpectedLeaderboards.FirstOrDefault(x => x.Id == 1);

            Assert.That(leaderboard, Is.EqualTo(expected).Using(new LeaderboardEqualityComparer()), message: "GetByIdAsync method works incorrectly");
        }

        [Test]
        public async Task LeaderboardRepositoryGetByUserIdAsyncReturnsCorrectValue()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var leaderboardRepository = new LeaderboardRepository(context);

            var leaderboard = await leaderboardRepository.GetByUserIdAsync("1");

            var expected = ExpectedLeaderboards.FirstOrDefault(x => x.UserId == "1");

            Assert.That(leaderboard, Is.EqualTo(expected).Using(new LeaderboardEqualityComparer()), message: "GetByUserIdAsync method works incorrectly");
        }

        [Test]
        public async Task LeaderboardRepositoryDeleteByIdAsyncRemovesValueFromDatabase()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var leaderboardRepository = new LeaderboardRepository(context);

            await leaderboardRepository.DeleteByIdAsync(1);
            await context.SaveChangesAsync();

            Assert.That(context.Leaderboards.Count(), Is.EqualTo(2), message: "DeleteByIdAsync method works incorrectly");
        }

        [Test]
        public void LeaderboardRepositoryUpdateUpdatesPoints()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var leaderboardRepository = new LeaderboardRepository(context);
            var leaderboard = new Leaderboard
            {
                UserId = "1",
                Points = 50
            };

            leaderboardRepository.Update(leaderboard);
            context.SaveChanges();

            var updatedLeaderboard = context.Leaderboards.FirstOrDefault(x => x.UserId == "1");

            Assert.That(updatedLeaderboard?.Points, Is.EqualTo(150), message: "Update method works incorrectly");
        }

        private static IEnumerable<Leaderboard> ExpectedLeaderboards =>
            new[]
            {
                new Leaderboard { Id = 3, UserId = "3", Points = 200 },
                new Leaderboard { Id = 2, UserId = "2", Points = 150 },
                new Leaderboard { Id = 1, UserId = "1", Points = 100 },
            };
    }
}