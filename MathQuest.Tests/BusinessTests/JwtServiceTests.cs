/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Data;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Business.Tests.Services
{
    [TestFixture]
    public class JwtServiceTests
    {
        private Mock<MathQuestDbContext> _dbContextMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IOptions<JwtSettingsModel>> _jwtSettingsMock;
        private IJwtService _jwtService;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<MathQuestDbContext>(new DbContextOptions<MathQuestDbContext>());
            _userManagerMock = MockUserManager<User>();
            _jwtSettingsMock = new Mock<IOptions<JwtSettingsModel>>();
            _jwtSettingsMock.Setup(x => x.Value).Returns(new JwtSettingsModel
            {
                Key = "test_key",
                Issuer = "test_issuer",
                Audience = "test_audience",
                AccessTokenExpirationMinutes = 30,
                RefreshTokenExpirationDays = 30,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true
            });

            _jwtService = new JwtService(
                _dbContextMock.Object,
                _userManagerMock.Object,
                _jwtSettingsMock.Object);
        }

        [Test]
        public async Task GenerateJwtAccessTokenAsync_ShouldGenerateToken()
        {
            // Arrange
            var user = new User { Id = "user-id", Email = "test@example.com" };

            // Mock UserManager
            _userManagerMock.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

            // Act
            var token = await _jwtService.GenerateJwtAccessTokenAsync(user);

            // Assert
            Assert.That(token, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void GenerateRefreshToken_ShouldGenerateToken()
        {
            // Act
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Assert
            Assert.That(refreshToken, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void GetPrincipalFromExpiredToken_ShouldReturnPrincipal()
        {
            // Arrange
            var accessToken = "test_access_token";

            // Act
            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);

            // Assert
            Assert.That(principal, Is.InstanceOf<ClaimsPrincipal>());
        }

        [Test]
        public async Task GetUserFromPrincipalAsync_ShouldReturnUser()
        {
            // Arrange
            var userId = "user-id";
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, "test@example.com")
            }));

            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(new User { Id = userId });

            // Act
            var user = await _jwtService.GetUserFromPrincipalAsync(principal);

            // Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(userId));
        }

        [Test]
        public async Task StoreRefreshTokenAsync_ShouldStoreToken()
        {
            // Arrange
            var userId = "user-id";
            var refreshToken = "test_refresh_token";
            var rememberMe = true;

            _dbContextMock.Setup(x => x.RefreshTokens.Add(It.IsAny<RefreshToken>())).Verifiable();
            _dbContextMock.Setup(x => x.SaveChangesAsync(default)).Returns(Task.FromResult(0)).Verifiable();

            // Act
            await _jwtService.StoreRefreshTokenAsync(userId, refreshToken, rememberMe);

            // Assert
            _dbContextMock.Verify();
        }

        [Test]
        public async Task RemoveRefreshTokenAsync_ShouldRemoveToken()
        {
            // Arrange
            var refreshToken = "test_refresh_token";
            var tokenEntity = new RefreshToken { Token = refreshToken };

            _dbContextMock.Setup(x => x.RefreshTokens.Remove(tokenEntity)).Verifiable();
            _dbContextMock.Setup(x => x.SaveChangesAsync(default)).Returns(Task.FromResult(0)).Verifiable();

            // Act
            await _jwtService.RemoveRefreshTokenAsync(refreshToken);

            // Assert
            _dbContextMock.Verify();
        }

        [Test]
        public async Task GetUserByRefreshTokenAsync_ShouldReturnUser()
        {
            // Arrange
            var refreshToken = "test_refresh_token";
            var user = new User { Id = "user-id" };
            var tokenEntity = new RefreshToken { Token = refreshToken, User = user };

            _dbContextMock.Setup(x => x.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken, CancellationToken.None))
                .ReturnsAsync(tokenEntity);

            // Act
            var result = await _jwtService.GetUserByRefreshTokenAsync(refreshToken);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(user.Id));
        }

        [Test]
        public async Task IsRefreshTokenValidAsync_ShouldReturnTrueIfValid()
        {
            // Arrange
            var refreshToken = "test_refresh_token";
            var tokenEntity = new RefreshToken { Token = refreshToken, Expires = DateTime.UtcNow.AddDays(1) };

            _dbContextMock.Setup(x => x.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken, CancellationToken.None))
                .ReturnsAsync(tokenEntity);

            // Act
            var result = await _jwtService.IsRefreshTokenValidAsync(refreshToken);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task UpdateRefreshTokenAsync_ShouldUpdateToken()
        {
            // Arrange
            var userId = "user-id";
            var oldRefreshToken = "old_refresh_token";
            var newRefreshToken = "new_refresh_token";
            var rememberMe = true;

            var tokenEntity = new RefreshToken { Token = oldRefreshToken, UserId = userId };

            _dbContextMock.Setup(x => x.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == oldRefreshToken && rt.UserId == userId, CancellationToken.None))
                .ReturnsAsync(tokenEntity);

            _dbContextMock.Setup(x => x.SaveChangesAsync(default)).Returns(Task.FromResult(0)).Verifiable();

            // Act
            await _jwtService.UpdateRefreshTokenAsync(userId, oldRefreshToken, newRefreshToken, rememberMe);

            // Assert
            Assert.That(tokenEntity.Token, Is.EqualTo(newRefreshToken));
            Assert.That(tokenEntity.Expires.Date, Is.EqualTo(rememberMe ? DateTime.Now.AddMonths(1).Date : DateTime.Now.AddDays(_jwtSettingsMock.Object.Value.RefreshTokenExpirationDays).Date));
            _dbContextMock.Verify();
        }

        private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContextMock = null;
            _userManagerMock = null;
            _jwtSettingsMock = null;
            _jwtService = null;
        }
    }
}

*/