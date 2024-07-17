using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Entities;
using Data.Entities.Identity;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Business.Tests.Services
{
    [TestFixture]
    public class FriendshipServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IUserService> _userServiceMock;
        private FriendshipService _friendshipService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = MockUserManager<User>();
            _userServiceMock = new Mock<IUserService>();

            _friendshipService = new FriendshipService(
                _userManagerMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _userServiceMock.Object);
        }
        /*
        [Test]
        public async Task AddFriendRequestAsync_ShouldReturnTrueOnSuccess()
        {
            // Arrange
            var userId = "user-id";
            var otherUserId = "other-user-id";

            _unitOfWorkMock.Setup(x => x.FriendshipRepository.AddAsync(It.IsAny<Friend>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(new User { Id = userId }).Verifiable();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(otherUserId)).ReturnsAsync(new User { Id = otherUserId }).Verifiable();
            _unitOfWorkMock.Setup(x => x.FriendshipRepository.GetFriendRequestStatusAsync(userId, otherUserId)).ReturnsAsync(FriendRequestStatus.Approved).Verifiable();

            // Act
            var result = await _friendshipService.AddFriendRequestAsync(userId, otherUserId);

            // Assert
            Assert.That(result, Is.True);
            _unitOfWorkMock.Verify();
        } 
        */

        /*
        [Test]
        public async Task GetAllFriendsAsync_ShouldReturnListOfFriends()
        {
            // Arrange
            var userId = "user-id";
            var user = new User { Id = userId };
            var friends = new List<User>
            {
                new User { Id = "friend-1" },
                new User { Id = "friend-2" }
            };

            _userManagerMock.Setup(x => x.Users).Returns(MockUserQueryable(friends).Object);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId)).ReturnsAsync(user).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await _friendshipService.GetAllFriendsAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            _unitOfWorkMock.Verify();
        } */

        [Test]
        public async Task GetPendingRequestsAsync_ShouldReturnListOfPendingRequests()
        {
            // Arrange
            var userId = "user-id";
            var pendingRequests = new List<Friend>
            {
                new Friend { RequestedBy = new User { Id = "pending-user-1" } },
                new Friend { RequestedBy = new User { Id = "pending-user-2" } }
            };

            _unitOfWorkMock.Setup(x => x.FriendshipRepository.GetAllPendingRequestsAsync(userId)).ReturnsAsync(pendingRequests).Verifiable();

            // Act
            var result = await _friendshipService.GetPendingRequestsAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            _unitOfWorkMock.Verify();
        }

        [Test]
        public async Task GetRequestUserAsync_ShouldReturnFriendship()
        {
            // Arrange
            var userId = "user-id";
            var otherUserId = "other-user-id";
            var friendship = new Friend { RequestedById = userId, RequestedToId = otherUserId };

            _unitOfWorkMock.Setup(x => x.FriendshipRepository.GetRequestFriendAsync(userId, otherUserId)).ReturnsAsync(friendship).Verifiable();

            // Act
            var result = await _friendshipService.GetRequestUserAsync(userId, otherUserId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestedById, Is.EqualTo(userId));
            Assert.That(result.RequestedToId, Is.EqualTo(otherUserId));
            _unitOfWorkMock.Verify();
        }

        [Test]
        public async Task ConfirmFriendRequestAsync_ShouldReturnTrueOnSuccess()
        {
            // Arrange
            var friendship = new Friend();

            _unitOfWorkMock.Setup(x => x.FriendshipRepository.ConfirmFriend(friendship)).ReturnsAsync(true).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await _friendshipService.ConfirmFriendRequestAsync(friendship);

            // Assert
            Assert.That(result, Is.True);
            _unitOfWorkMock.Verify();
        }

        [Test]
        public async Task RejectFriendRequestAsync_ShouldReturnTrueOnSuccess()
        {
            // Arrange
            var friendship = new Friend();

            _unitOfWorkMock.Setup(x => x.FriendshipRepository.RejectFriend(friendship)).ReturnsAsync(true).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await _friendshipService.RejectFriendRequestAsync(friendship);

            // Assert
            Assert.That(result, Is.True);
            _unitOfWorkMock.Verify();
        }

        [Test]
        public async Task GetFriendInfoAsync_ShouldReturnFriendInfoModel()
        {
            // Arrange
            var username = "test-user";
            var user = new User { UserName = username };
            var friendInfoModel = new FriendInfoModel { UserName = username };

            _userServiceMock.Setup(x => x.GetUserByUsernameAsync(username)).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<FriendInfoModel>(user)).Returns(friendInfoModel).Verifiable();

            // Act
            var result = await _friendshipService.GetFriendInfoAsync(username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo(username));
            _userServiceMock.Verify();
            _mapperMock.Verify();
        }

        [Test]
        public async Task GetFriendInfoByIdAsync_ShouldReturnFriendInfoModel()
        {
            // Arrange
            var userId = "user-id";
            var userName = "username";
            var user = new User { Id = userId, UserName = userName };
            var friendInfoModel = new FriendInfoModel { UserName = userName };

            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<FriendInfoModel>(user)).Returns(friendInfoModel).Verifiable();

            // Act
            var result = await _friendshipService.GetFriendInfoByIdAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo(userName));
            _userServiceMock.Verify();
            _mapperMock.Verify();
        }

        [Test]
        public async Task GetFriendStatusAsync_ShouldReturnFriendStatus()
        {
            // Arrange
            var userId = "user-id";
            var otherUserId = "other-user-id";
            var status = FriendRequestStatus.Approved;

            _unitOfWorkMock.Setup(x => x.FriendshipRepository.GetFriendRequestStatusAsync(userId, otherUserId)).ReturnsAsync(status).Verifiable();

            // Act
            var result = await _friendshipService.GetFriendStatusAsync(userId, otherUserId);

            // Assert
            Assert.That(result, Is.EqualTo("Approved"));
            _unitOfWorkMock.Verify();
        }

        [Test]
        public async Task AreFriendsAsync_ShouldReturnTrueIfFriends()
        {
            // Arrange
            var userId = "user-id";
            var otherUserId = "other-user-id";
            var status = FriendRequestStatus.Approved;

            _unitOfWorkMock.Setup(x => x.FriendshipRepository.GetFriendRequestStatusAsync(userId, otherUserId)).ReturnsAsync(status).Verifiable();

            // Act
            var result = await _friendshipService.AreFriendsAsync(userId, otherUserId);

            // Assert
            Assert.That(result, Is.True);
            _unitOfWorkMock.Verify();
        }

        [Test]
        public async Task RemoveFriendRequestAsync_ShouldReturnTrueOnSuccess()
        {
            // Arrange
            var userId = "user-id";
            var otherUserId = "other-user-id";
            var friendship = new Friend { RequestedById = userId, RequestedToId = otherUserId };

            _unitOfWorkMock.Setup(x => x.FriendshipRepository.GetByIdsAsync(userId, otherUserId)).ReturnsAsync(friendship).Verifiable();
            _unitOfWorkMock.Setup(x => x.FriendshipRepository.Delete(friendship)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await _friendshipService.RemoveFriendRequestAsync(userId, otherUserId);

            // Assert
            Assert.That(result, Is.True);
            _unitOfWorkMock.Verify();
        }

        private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private static Mock<IQueryable<T>> MockUserQueryable<T>(IEnumerable<T> list) where T : class
        {
            var queryable = list.AsQueryable();
            var mock = new Mock<IQueryable<T>>();
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            return mock;
        }
    }
}