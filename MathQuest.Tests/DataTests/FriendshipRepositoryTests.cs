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
    public class FriendshipRepositoryTests
    {
        /*
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task FriendshipRepositoryGetByIdsAsyncReturnsSingleValue(int id)
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var friendshipRepository = new FriendshipRepository(context);

            var friend = await friendshipRepository.GetByIdsAsync("1", "2");

            var expected = ExpectedFriends.FirstOrDefault(x => x.Id == id);

            Assert.That(friend, Is.EqualTo(expected).Using(new FriendshipEqualityComparer()), message: "GetByIdsAsync method works incorrectly");
        } */

        [Test]
        public async Task FriendshipRepositoryAddAsyncAddsValueToDatabase()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var friendshipRepository = new FriendshipRepository(context);
            var friend = new Friend
            {
                Id = 4,
                RequestedById = "4",
                RequestedToId = "1",
                FriendRequestStatus = FriendRequestStatus.Pending,
                BecameFriendsDate = null
            };

            await friendshipRepository.AddAsync(friend);
            await context.SaveChangesAsync();

            Assert.That(context.Friends.Count(), Is.EqualTo(4), message: "AddAsync method works incorrectly");
        }

        /*
        [Test]
        public async Task FriendshipRepositoryDeleteRemovesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var friendshipRepository = new FriendshipRepository(context);
            var friend = await friendshipRepository.GetByIdsAsync("1", "2");

            friendshipRepository.Delete(friend);
            await context.SaveChangesAsync();

            Assert.That(context.Friends.Count(), Is.EqualTo(2), message: "Delete method works incorrectly");
        } */

        [Test]
        public async Task FriendshipRepositoryGetAllPendingRequestsAsyncReturnsCorrectValues()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var friendshipRepository = new FriendshipRepository(context);

            var pendingFriends = await friendshipRepository.GetAllPendingRequestsAsync("1");

            var expectedPendingFriends = ExpectedFriends.Where(x => x.RequestedToId == "1" && x.FriendRequestStatus == FriendRequestStatus.Pending);

            Assert.That(pendingFriends, Is.EqualTo(expectedPendingFriends).Using(new FriendshipEqualityComparer()), message: "GetAllPendingRequestsAsync method works incorrectly");
        }

        /*
        [Test]
        public async Task FriendshipRepositoryGetRequestFriendAsyncReturnsCorrectValue()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var friendshipRepository = new FriendshipRepository(context);

            var friend = await friendshipRepository.GetRequestFriendAsync("1", "3");

            var expected = ExpectedFriends.FirstOrDefault(x => x.RequestedById == "3" && x.RequestedToId == "1");

            Assert.That(friend, Is.EqualTo(expected).Using(new FriendshipEqualityComparer()), message: "GetRequestFriendAsync method works incorrectly");
        } */

        /*
        [Test]
        public async Task FriendshipRepositoryGetFriendRequestStatusAsyncReturnsCorrectStatus()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var friendshipRepository = new FriendshipRepository(context);

            var status = await friendshipRepository.GetFriendRequestStatusAsync("1", "2");

            Assert.That(status, Is.EqualTo(FriendRequestStatus.Approved), message: "GetFriendRequestStatusAsync method works incorrectly");
        } */

        /*
        [Test]
        public async Task FriendshipRepositoryConfirmFriendUpdatesStatus()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var friendshipRepository = new FriendshipRepository(context);
            var friend = await friendshipRepository.GetRequestFriendAsync("1", "3");

            var result = await friendshipRepository.ConfirmFriend(friend);
            await context.SaveChangesAsync();

            var updatedFriend = await friendshipRepository.GetRequestFriendAsync("1", "3");
            Assert.That(updatedFriend?.FriendRequestStatus, Is.EqualTo(FriendRequestStatus.Approved), message: "ConfirmFriend method works incorrectly");
        } */

        /*
        [Test]
        public async Task FriendshipRepositoryRejectFriendRemovesEntity()
        {
            using var context = UnitTestHelper.GetMathQuestDbContext();

            var friendshipRepository = new FriendshipRepository(context);
            var friend = await friendshipRepository.GetRequestFriendAsync("1", "3");

            var result = await friendshipRepository.RejectFriend(friend);
            await context.SaveChangesAsync();

            var updatedFriend = await friendshipRepository.GetRequestFriendAsync("1", "3");
            Assert.That(updatedFriend, Is.Null, message: "RejectFriend method works incorrectly");
        } */

        private static IEnumerable<Friend> ExpectedFriends =>
            new[]
            {
                new Friend { Id = 1, RequestedById = "1", RequestedToId = "2", FriendRequestStatus = FriendRequestStatus.Pending, BecameFriendsDate = null },
                new Friend { Id = 2, RequestedById = "2", RequestedToId = "1", FriendRequestStatus = FriendRequestStatus.Approved, BecameFriendsDate = new DateTime(2023, 6, 1) },
                new Friend { Id = 3, RequestedById = "3", RequestedToId = "1", FriendRequestStatus = FriendRequestStatus.Rejected, BecameFriendsDate = null }
            };
    }
}