/*

using Business.Interfaces;
using Business.Models;
using FluentAssertions;
using MathQuest.Server;
using MathQuestWebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MathQuest.Tests.IntegrationTests
{
    [TestFixture]
    public class MessageControllerIntegrationTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task GetUserAll_ValidToken_ReturnsOk()
        {
            // Arrange
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/api/message/users");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetUserAll_Unauthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/message/users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetSpecific_ValidToken_ReturnsOk()
        {
            // Arrange
            var username = "testuser";
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"/api/message/user/{username}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetSpecific_InvalidUsername_ReturnsBadRequest()
        {
            // Arrange
            var invalidUsername = "nonexistentuser";
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"/api/message/user/{invalidUsername}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetSpecific_Unauthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/message/user/testuser");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetSingle_ValidToken_ReturnsOk()
        {
            // Arrange
            var username = "testuser";
            var messageId = 1;
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"/api/message/user/{username}/{messageId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetSingle_InvalidMessageId_ReturnsNotFound()
        {
            // Arrange
            var username = "testuser";
            var invalidMessageId = 999;
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"/api/message/user/{username}/{invalidMessageId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetSingle_Unauthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/message/user/testuser/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Add_ValidToken_ReturnsOk()
        {
            // Arrange
            var model = new MessageModel { SenderId = "123456", ReceiverId = "654321", Text = "Test message" };
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/message/add", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task Add_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var invalidModel = new MessageModel(); // Invalid because UserId, ReceiverId, and Content are required
            var json = JsonConvert.SerializeObject(invalidModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/message/add", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Add_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var model = new MessageModel { SenderId = "123456", ReceiverId = "654321", Text = "Test message" };
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/message/add", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
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