using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathQuest.Tests.DataTests
{
    [TestFixture]
    public class AchievementRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task AchievementRepositoryGetByIdAsyncReturnsSingleValue(int id)
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var achievementRepository = new AchievementRepository(context);

            var achievement = await achievementRepository.GetByIdAsync(id);

            var expected = ExpectedAchievements.FirstOrDefault(x => x.Id == id);

            Assert.That(achievement, Is.EqualTo(expected).Using(new AchievementEqualityComparer()), message: "GetByIdAsync method works incorrectly");
        }

        [Test]
        public async Task AchievementRepositoryGetAllAsyncReturnsAllValues()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var achievementRepository = new AchievementRepository(context);

            var achievement = await achievementRepository.GetAllAsync();

            Assert.That(achievement, Is.EqualTo(ExpectedAchievements).Using(new AchievementEqualityComparer()), message: "GetAllAsync method works incorrectly");
        }

        [Test]
        public async Task AchievementRepositoryAddAsyncAddsValueToDatabase()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var achievementRepository = new AchievementRepository(context);

            var achievement = new Achievement { Id = 4, AchievementName = "Fourth Grader", AchievementDescription = "You successfully completed 4th grade.", AchievementType = "General", AchievementDate = new DateTime(2020, 8, 18, 0, 0, 0, DateTimeKind.Local) };

            await achievementRepository.AddAsync(achievement);
            await context.SaveChangesAsync();

            Assert.That(context.Achievements.Count(), Is.EqualTo(4), message: "AddAsync method works incorrectly");
        }

        [Test]
        public async Task AchievementRepositoryDeleteByIdAsyncDeletesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var achievementRepository = new AchievementRepository(context);

            await achievementRepository.DeleteByIdAsync(3);
            await context.SaveChangesAsync();

            Assert.That(context.Achievements.Count(), Is.EqualTo(2), message: "DeleteByIdAsync works incorrectly");
        }

        [Test]
        public async Task AchievementRepositoryUpdateUpdatesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var achievementRepository = new AchievementRepository(context);

            var achievement = new Achievement { Id = 4, AchievementName = "Fourth Grader", AchievementDescription = "You successfully completed 4th grade.", AchievementType = "General", AchievementDate = new DateTime(2020, 8, 18, 0, 0, 0, DateTimeKind.Local) };

            achievementRepository.Update(achievement);
            await context.SaveChangesAsync();

            Assert.That(achievement, Is.EqualTo(new Achievement
            {
                Id = 4,
                AchievementName = "Fourth Grader",
                AchievementDescription = "You successfully completed 4th grade.",
                AchievementType = "General",
                AchievementDate = new DateTime(2020, 8, 18, 0, 0, 0, DateTimeKind.Local),
            }).Using(new AchievementEqualityComparer()), message: "Update method works incorrectly");
        }

        private static IEnumerable<Achievement> ExpectedAchievements =>
        new[]
        {
            new Achievement { Id = 1, AchievementName = "First Grader",  AchievementDescription = "You successfully completed 1st grade.", AchievementType = "General", AchievementDate = new DateTime(2021, 7, 13, 0, 0, 0, DateTimeKind.Local) },
            new Achievement { Id = 2, AchievementName = "Second Grader", AchievementDescription = "You successfully completed 2nd grade.", AchievementType = "General", AchievementDate = new DateTime(2022, 8, 18, 0, 0, 0, DateTimeKind.Local), },
            new Achievement { Id = 3, AchievementName = "Third Grader", AchievementDescription = "You successfully completed 3rd grade.", AchievementType = "General", AchievementDate = new DateTime(2020, 8, 18, 0, 0, 0, DateTimeKind.Local), }
        };
    }
}
