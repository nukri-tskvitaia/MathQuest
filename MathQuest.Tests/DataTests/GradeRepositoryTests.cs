using Data.Entities;
using Data.Repositories;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathQuest.Tests.DataTests
{
    [TestFixture]
    public class GradeRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GradeRepositoryGetByIdAsyncReturnsSingleValue(int id)
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var gradeRepository = new GradeRepository(context);

            var grade = await gradeRepository.GetByIdAsync(id);

            var expected = ExpectedGrades.FirstOrDefault(x => x.Id == id);

            Assert.That(grade, Is.EqualTo(expected).Using(new GradeEqualityComparer()), message: "GetByIdAsync method works incorrectly");
        }

        [Test]
        public async Task GradeRepositoryAddAsyncAddsValueToDatabase()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var gradeRepository = new GradeRepository(context);
            var grade = new Grade
            {
                Id = 4,
                GradeName = "Grade 4"
            };

            await gradeRepository.AddAsync(grade);
            await context.SaveChangesAsync();

            Assert.That(context.Grades.Count(), Is.EqualTo(4), message: "AddAsync method works incorrectly");
        }

        [Test]
        public async Task GradeRepositoryGetByNameAsyncReturnsCorrectValue()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var gradeRepository = new GradeRepository(context);

            var grade = await gradeRepository.GetByNameAsync("Grade 1");

            var expected = ExpectedGrades.FirstOrDefault(x => x.GradeName == "Grade 1");

            Assert.That(grade, Is.EqualTo(expected).Using(new GradeEqualityComparer()), message: "GetByNameAsync method works incorrectly");
        }

        private static IEnumerable<Grade> ExpectedGrades =>
            new[]
            {
                new Grade { Id = 1, GradeName = "Grade 1" },
                new Grade { Id = 2, GradeName = "Grade 2" },
                new Grade { Id = 3, GradeName = "Grade 3" }
            };
    }
}