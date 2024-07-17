/*

using Business.Interfaces;
using Business.Models;
using Business.Models.DTO;
using FluentAssertions;
using MathQuest.Server;
using MathQuestWebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MathQuest.Tests.IntegrationTests
{
    [TestFixture]
    public class FriendControllerIntegrationTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task SearchFriends_ValidRequest_ReturnsListOfFriends()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/friend/search/friends");
            var token = await GetAccessTokenAsync();
            request.Headers.Add("Authorization", $"Bearer {token}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Friends");
        }

        [Test]
        public async Task SearchUser_ValidUsername_ReturnsUserInfoAndStatus()
        {
            // Arrange
            var username = "testuser";
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/friend/search/user?username={username}");
            var token = await GetAccessTokenAsync();
            request.Headers.Add("Authorization", $"Bearer {token}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("UserInfo");
            responseContent.Should().Contain("Status");
        }

        [Test]
        public async Task SearchUserById_ValidId_ReturnsUserInfoAndStatus()
        {
            // Arrange
            var userId = "123456";
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/friend/search/user/{userId}");
            var token = await GetAccessTokenAsync();
            request.Headers.Add("Authorization", $"Bearer {token}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("UserInfo");
            responseContent.Should().Contain("Status");
        }

        [Test]
        public async Task AreFriends_ValidUserIds_ReturnsBoolean()
        {
            // Arrange
            var userId = "123456";
            var otherUserId = "789012";
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/friend/are-friends/{userId}/{otherUserId}");
            var token = await GetAccessTokenAsync();
            request.Headers.Add("Authorization", $"Bearer {token}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().BeOneOf("true", "false");
        }

        [Test]
        public async Task AddFriendRequest_ValidModel_ReturnsOk()
        {
            // Arrange
            var model = new UserNameDto { UserName = "testuser" };
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/friend/add/friend", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task RemoveFriendRequest_ValidModel_ReturnsOk()
        {
            // Arrange
            var model = new UserNameDto { UserName = "testuser" };
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/friend/remove/friend", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetPendingRequests_ValidRequest_ReturnsListOfRequests()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/friend/pending-requests");
            var token = await GetAccessTokenAsync();
            request.Headers.Add("Authorization", $"Bearer {token}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Requests");
        }

        [Test]
        public async Task ConfirmFriendRequest_ValidModel_ReturnsOk()
        {
            // Arrange
            var model = new UserNameDto { UserName = "testuser" };
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/friend/confirm", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task RejectFriendRequest_ValidModel_ReturnsOk()
        {
            // Arrange
            var model = new UserNameDto { UserName = "testuser" };
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/friend/reject", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var loginModel = new LoginModel { Email = "test@example.com", Password = "P@ssw0rd" };
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authorization/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return responseObject.accessToken;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
        }
    }
}

*/