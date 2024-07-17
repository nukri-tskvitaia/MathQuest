/*
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;
using Business.Models;

namespace MathQuest.Tests.IntegrationTests
{
    [TestFixture]
    public class AccountControllerIntegrationTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task TwoFactorEnabled_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/account/is-two-factor-enabled");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetUserGrade_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/account/user/grade");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task UserInfo_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/account/manage/user-info");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task UpdateUserInfo_WithValidModel_ReturnsOk()
        {
            // Arrange
            var model = new UserInfoModel
            {
                // Populate with valid data for update
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/account/manage/update/user-info");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task UpdateUserInfo_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var model = new UserInfoModel
            {
                // Populate with invalid data for update
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/account/manage/update/user-info");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task ChangePassword_WithValidModel_ReturnsOk()
        {
            // Arrange
            var model = new ChangePasswordModel
            {
                // Populate with valid data for password change
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/account/manage/change-password");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task ChangePassword_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var model = new ChangePasswordModel
            {
                // Populate with invalid data for password change
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/account/manage/change-password");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Generate2FAKey_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/account/manage/setup-2fa");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Verify2FAToken_WithValidToken_ReturnsOk()
        {
            // Arrange
            var token = "ValidTokenHere";

            var json = JsonConvert.SerializeObject(token);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/account/manage/verify-2fa-token");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Verify2FAToken_WithInvalidToken_ReturnsBadRequest()
        {
            // Arrange
            var token = "InvalidTokenHere";

            var json = JsonConvert.SerializeObject(token);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/account/manage/verify-2fa-token");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Disable2FAToken_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage

(HttpMethod.Post, "/api/account/manage/disable-2fa-token");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
        }
    }
}

*/