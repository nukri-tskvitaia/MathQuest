using Data.Entities;
using Data.Entities.Identity;
using Data.Repositories;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathQuest.Tests.DataTests
{
    [TestFixture]
    public class PersonRepositoryTests
    {
        [TestCase("1")]
        [TestCase("2")]
        public async Task PersonRepositoryGetByIdAsyncReturnsSingleValue(int id)
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var personRepository = new PersonRepository(context);

            var person = await personRepository.GetByIdAsync(id);

            var expected = ExpectedPeople.FirstOrDefault(x => x.Id == id);

            Assert.That(person, Is.EqualTo(expected).Using(new PersonEqualityComparer()), message: "GetByIdAsync method works incorrectly");
        }

        [Test]
        public async Task PersonRepositoryGetAllAsyncReturnsAllValues()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var personRepository = new PersonRepository(context);

            var person = await personRepository.GetAllAsync();

            Assert.That(person, Is.EqualTo(ExpectedPeople).Using(new PersonEqualityComparer()), message: "GetAllAsync method works incorrectly");
        }

        [Test]
        public async Task PersonRepositoryAddAsyncAddsValueToDatabase()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var personRepository = new PersonRepository(context);

            var person = new Person { Id = 4, FirstName = "John", LastName = "Brzenk", Gender = GenderList.Male };

            await personRepository.AddAsync(person);
            await context.SaveChangesAsync();

            Assert.That(context.People.Count(), Is.EqualTo(4), message: "AddAsync method works incorrectly");
        }

        [Test]
        public async Task PersonRepositoryDeleteByIdAsyncDeletesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var personRepository = new PersonRepository(context);

            await personRepository.DeleteByIdAsync(3);
            await context.SaveChangesAsync();

            Assert.That(context.People.Count(), Is.EqualTo(2), message: "DeleteByIdAsync works incorrectly");
        }

        [Test]
        public async Task PersonRepositoryUpdateUpdatesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var personRepository = new PersonRepository(context);

            var person = new Person
            {
                Id = 4,
                FirstName = "Emily",
                LastName = "Brzenk",
                Gender = GenderList.Female,
                BirthDate = new DateTime(2002, 4, 18, 0, 0, 0, DateTimeKind.Local)
            };

            personRepository.Update(person);
            await context.SaveChangesAsync();

            Assert.That(person, Is.EqualTo(new Person
            {
                Id = 4,
                FirstName = "Emily",
                LastName = "Brzenk",
                Gender = GenderList.Female,
                BirthDate = new DateTime(2002, 4, 18, 0, 0, 0, DateTimeKind.Local)
            }).Using(new PersonEqualityComparer()), message: "Update method works incorrectly");
        }

        private static IEnumerable<Person> ExpectedPeople =>
        new[]
        {
                new Person { Id = 1, FirstName = "Nukri", LastName = "Tskvitaia", BirthDate = new DateTime(2001, 7, 13, 0, 0, 0, DateTimeKind.Local), Gender = GenderList.Male },
                new Person { Id = 2, FirstName = "Jaba", LastName = "Shengelia", BirthDate = new DateTime(1978, 8, 18, 0, 0, 0, DateTimeKind.Local), Gender = GenderList.Male },
                new Person { Id = 3, FirstName = "Noe", LastName = "Shengelia", BirthDate = new DateTime(1994, 8, 18, 0, 0, 0, DateTimeKind.Local), Gender = GenderList.Male }
        };
    }
}
