using System;
using System.IO;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Services;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using QRCoder;

namespace Business.Tests.Services
{
    [TestFixture]
    public class TwoFactorServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private ITwoFactorService _twoFactorService;

        [SetUp]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _twoFactorService = new TwoFactorService(_userManagerMock.Object, null);
        }

        /*
        [Test]
        public async Task GenerateTwoFactorAsync_ShouldReturnKeyAndImage()
        {
            // Arrange
            var user = new User { Id = "test-id", Email = "test@example.com" };
            _userManagerMock.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetAuthenticatorKeyAsync(user)).ReturnsAsync("test-key");

            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            // Act
            var result = await _twoFactorService.GenerateTwoFactorAsync("test-id");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value.key, Is.Not.Null);
            Assert.That(result.Value.image, Is.Not.Null);
        } */

        [Test]
        public async Task VerifyTwoFactorTokenAsync_ShouldReturnTrue_WhenTokenValid()
        {
            // Arrange
            var user = new User { Id = "test-id" };
            _userManagerMock.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.VerifyTwoFactorTokenAsync(user, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(m => m.SetTwoFactorEnabledAsync(user, true)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _twoFactorService.VerifyTwoFactorTokenAsync("test-id", "test-token");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DisableTwoFactorAsync_ShouldReturnTrue_WhenDisabledSuccessfully()
        {
            // Arrange
            var user = new User { Id = "test-id" };
            _userManagerMock.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.SetTwoFactorEnabledAsync(user, false)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _twoFactorService.DisableTwoFactorAsync("test-id");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void SetTwoFactorRememberDeviceCookie_ShouldSetCookies()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var responseCookies = new TestResponseCookies();
            httpContext.Features.Set<IResponseCookiesFeature>(new TestResponseCookiesFeature(responseCookies));
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            // Act
            _twoFactorService.SetTwoFactorRememberDeviceCookie(httpContext, "test-id");

            // Assert
            Assert.That(responseCookies.Cookies.ContainsKey("remember_device"), Is.True);
            Assert.That(responseCookies.Cookies.ContainsKey("twofactor-userid"), Is.True);
        }


        [Test]
        public void DeleteTwoFactorRememberDeviceCookie_ShouldDeleteCookies()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var responseCookies = new TestResponseCookies();
            responseCookies.Append("remember_device", "some value");
            responseCookies.Append("twofactor-userid", "some value");
            httpContext.Features.Set<IResponseCookiesFeature>(new TestResponseCookiesFeature(responseCookies));
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            // Act
            _twoFactorService.DeleteTwoFactorRememberDeviceCookie(httpContext);

            // Assert
            Assert.That(responseCookies.Cookies.ContainsKey("remember_device"), Is.False);
            Assert.That(responseCookies.Cookies.ContainsKey("twofactor-userid"), Is.False);
        }
    }

    public class TestResponseCookiesFeature : IResponseCookiesFeature
    {
        public TestResponseCookiesFeature(IResponseCookies cookies)
        {
            Cookies = cookies;
        }

        public IResponseCookies Cookies { get; }
    }

    public class TestResponseCookies : IResponseCookies
    {
        public Dictionary<string, string> Cookies { get; } = new Dictionary<string, string>();

        public void Append(string key, string value)
        {
            Cookies[key] = value;
        }

        public void Append(string key, string value, CookieOptions options)
        {
            Cookies[key] = value;
        }

        public void Delete(string key)
        {
            Cookies.Remove(key);
        }

        public void Delete(string key, CookieOptions options)
        {
            Cookies.Remove(key);
        }
    }
}