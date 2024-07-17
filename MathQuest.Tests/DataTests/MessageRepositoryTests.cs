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
    public class MessageRepositoryTests
    {
        /*
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task MessageRepositoryGetByIdAsyncReturnsSingleValue(int id)
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var messageRepository = new MessageRepository(context);

            var message = await messageRepository.GetMessageByIdsAsync("1", "2", id);

            var expected = ExpectedMessages.FirstOrDefault(x => x.Id == id);

            Assert.That(message, Is.EqualTo(expected).Using(new MessageEqualityComparer()), message: "GetMessageByIdsAsync method works incorrectly");
        } */
        /*
        [Test]
        public async Task MessageRepositoryGetAllMessagesAsyncReturnsAllValues()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var messageRepository = new MessageRepository(context);

            var messageGroups = await messageRepository.GetAllMessagesAsync();

            var expectedMessageGroups = ExpectedMessages.GroupBy(x => new { x.SenderId, x.ReceiverId })
                .OrderByDescending(g => g.Max(m => m.SentDate))
                .Select(g => g.OrderByDescending(m => m.SentDate).Take(10));

            Assert.That(messageGroups, Is.EqualTo(expectedMessageGroups).Using(new MessageEqualityComparer()), message: "GetAllMessagesAsync method works incorrectly");
        } */

        [Test]
        public async Task MessageRepositoryAddMessageAsyncAddsValueToDatabase()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var messageRepository = new MessageRepository(context);
            var message = new Message
            {
                Id = 4,
                SenderId = "3",
                ReceiverId = "4",
                Text = "New message.",
                SentDate = DateTime.UtcNow
            };

            await messageRepository.AddMessageAsync(message);
            await context.SaveChangesAsync();

            Assert.That(context.Messages.Count(), Is.EqualTo(4), message: "AddMessageAsync method works incorrectly");
        }

        [Test]
        public async Task MessageRepositoryDeleteMessageByIdAsyncDeletesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var messageRepository = new MessageRepository(context);

            await messageRepository.DeleteMessageByIdsAsync("1", "2", 1);
            await context.SaveChangesAsync();

            Assert.That(context.Messages.Count(), Is.EqualTo(2), message: "DeleteMessageByIdsAsync method works incorrectly");
        }

        /*
        [Test]
        public async Task MessageRepositoryUpdateUpdatesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var messageRepository = new MessageRepository(context);
            var message = new Message
            {
                Id = 2,
                SenderId = "2",
                ReceiverId = "1",
                Text = "Updated message text.",
                SentDate = new DateTime(2023, 7, 12, 10, 5, 0, DateTimeKind.Local)
            };

            messageRepository.Update(message);
            await context.SaveChangesAsync();

            var updatedMessage = await messageRepository.GetMessageByIdsAsync("2", "1", 2);
            Assert.That(updatedMessage, Is.EqualTo(message).Using(new MessageEqualityComparer()), message: "Update method works incorrectly");
        } */

        /*
        [Test]
        public async Task MessageRepositoryGetUsersAllMessagesAsyncReturnsCorrectValues()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var messageRepository = new MessageRepository(context);

            var messageGroups = await messageRepository.GetUsersAllMessagesAsync("1");

            var expectedMessageGroups = ExpectedMessages.Where(x => x.SenderId == "1" || x.ReceiverId == "1")
                .GroupBy(x => new { x.SenderId, x.ReceiverId })
                .OrderByDescending(g => g.Max(m => m.SentDate))
                .Select(g => g.OrderByDescending(m => m.SentDate).Take(10));

            Assert.That(messageGroups, Is.EqualTo(expectedMessageGroups).Using(new MessageEqualityComparer()), message: "GetUsersAllMessagesAsync method works incorrectly");
        } */

        /*
        [Test]
        public async Task MessageRepositoryGetMessagesByIdsAsyncReturnsCorrectValues()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var messageRepository = new MessageRepository(context);

            var messages = await messageRepository.GetMessagesByIdsAsync("1", "2");

            var expectedMessages = ExpectedMessages.Where(x => (x.SenderId == "1" && x.ReceiverId == "2") || (x.SenderId == "2" && x.ReceiverId == "1"));

            Assert.That(messages, Is.EqualTo(expectedMessages).Using(new MessageEqualityComparer()), message: "GetMessagesByIdsAsync method works incorrectly");
        } */

        private static IEnumerable<Message> ExpectedMessages =>
            new[]
            {
                new Message { Id = 1, SenderId = "1", ReceiverId = "2", Text = "Hello!", SentDate = new DateTime(2023, 7, 12, 10, 0, 0, DateTimeKind.Local) },
                new Message { Id = 2, SenderId = "2", ReceiverId = "1", Text = "Hi there!", SentDate = new DateTime(2023, 7, 12, 10, 5, 0, DateTimeKind.Local) },
                new Message { Id = 3, SenderId = "1", ReceiverId = "3", Text = "How are you?", SentDate = new DateTime(2023, 7, 12, 11, 0, 0, DateTimeKind.Local) }
            };
    }
}