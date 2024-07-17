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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace Business.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private Mock<IJwtService> _jwtServiceMock;
        private Mock<ITwoFactorService> _twoFactorServiceMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userManagerMock = MockUserManager();
            _roleManagerMock = MockRoleManager();
            _jwtServiceMock = new Mock<IJwtService>();
            _twoFactorServiceMock = new Mock<ITwoFactorService>();

            _userService = new UserService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _jwtServiceMock.Object,
                _twoFactorServiceMock.Object);
        }

        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);
        }

        /*
        [Test]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User> { new User(), new User() }.AsQueryable();
            _userManagerMock.Setup(m => m.Users).Returns(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        } */

        [Test]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = "test-id" };
            _userManagerMock.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync("test-id");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("test-id"));
        }

        [Test]
        public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            _userManagerMock.Setup(m => m.FindByEmailAsync("test@example.com")).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByEmailAsync("test@example.com");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("test@example.com"));
        }

        [Test]
        public async Task GetUserByUsernameAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { UserName = "testuser" };
            _userManagerMock.Setup(m => m.FindByNameAsync("testuser")).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByUsernameAsync("testuser");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("testuser"));
        }

        [Test]
        public async Task GetUserInfoAsync_ShouldReturnUserInfo_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = "test-id", PersonId = 1 };
            var person = new Person();
            var userInfoModel = new UserInfoModel();

            _userManagerMock.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.PersonRepository.GetByIdAsync(1)).ReturnsAsync(person);
            _mapperMock.Setup(m => m.Map<UserInfoModel>(user)).Returns(userInfoModel);
            _mapperMock.Setup(m => m.Map(person, userInfoModel)).Returns(userInfoModel);

            // Act
            var result = await _userService.GetUserInfoAsync("test-id");

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /*
        [Test]
        public async Task UpdateUserInfoAsync_ShouldReturnTrue_WhenUserInfoUpdatedSuccessfully()
        {
            // Arrange
            var userInfoModel = new UserInfoModel { Email = "new@example.com" };
            var user = new User { Id = "test-id", PersonId = 1, Email = "old@example.com" };
            var person = new Person();

            _userManagerMock.Setup(m => m.FindByEmailAsync("new@example.com")).ReturnsAsync((User)null);
            _userManagerMock.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
            _mapperMock.Setup(m => m.Map<Person>(userInfoModel)).Returns(person);

            // Act
            var (result, emailExists) = await _userService.UpdateUserInfoAsync(userInfoModel, "test-id");

            // Assert
            Assert.That(result, Is.True);
            Assert.That(emailExists, Is.EqualTo(EmailExists.None));
        } */

        [Test]
        public async Task GetUserGradeIdAsync_ShouldReturnGradeId_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = "test-id", GradeId = 1 };
            _userManagerMock.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserGradeIdAsync("test-id");

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task ChangePasswordAsync_ShouldReturnTrue_WhenPasswordChangedSuccessfully()
        {
            // Arrange
            var user = new User { Id = "test-id" };
            _userManagerMock.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.ChangePasswordAsync(user, "oldPassword", "newPassword")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.ChangePasswordAsync("test-id", "oldPassword", "newPassword");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldReturnIdentityResult_WhenUserUpdated()
        {
            // Arrange
            var user = new User();
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.UpdateUserAsync(user);

            // Assert
            Assert.That(result, Is.EqualTo(IdentityResult.Success));
        }

        /*
        [Test]
        public async Task DeleteUserByEmailAsync_ShouldReturnTrue_WhenUserDeletedSuccessfully()
        {
            // Arrange
            var user = new User { Id = "test-id", PersonId = 1 };
            var person = new Person();
            var refreshToken = new RefreshToken { UserId = "test-id" };

            _userManagerMock.Setup(m => m.FindByEmailAsync("test@example.com")).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.PersonRepository.GetByIdAsync(1)).ReturnsAsync(person);
            _jwtServiceMock.Setup(j => j.RemoveRefreshTokenAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);
            _unitOfWorkMock.Setup(u => u.PersonRepository.Delete(person));
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.DeleteUserByEmailAsync(new DefaultHttpContext(), "test@example.com");

            // Assert
            Assert.That(result, Is.True);
        } */

        [Test]
        public async Task AddUserToRoleAsync_ShouldReturnIdentityResult_WhenRoleAdded()
        {
            // Arrange
            var user = new User();
            _userManagerMock.Setup(m => m.AddToRoleAsync(user, "User")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.AddUserToRoleAsync(user, "User");

            // Assert
            Assert.That(result, Is.EqualTo(IdentityResult.Success));
        }

        [Test]
        public async Task RemoveUserFromRoleAsync_ShouldReturnIdentityResult_WhenRoleRemoved()
        {
            // Arrange
            var user = new User();
            _userManagerMock.Setup(m => m.RemoveFromRoleAsync(user, "User")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.RemoveUserFromRoleAsync(user, "User");

            // Assert
            Assert.That(result, Is.EqualTo(IdentityResult.Success));
        }

        /*
        [Test]
        public async Task GetUserRolesAsync_ShouldReturnTrue_WhenUserInRole()
        {
            // Arrange
            var user = new User();
            _userManagerMock.Setup(m => m.IsInRoleAsync(user, "User")).ReturnsAsync(true);

            // Act
            var result = await _userService.GetUserRolesAsync(user.Id);

            // Assert
            Assert.That(result, Is.True);
        } */
    }
}