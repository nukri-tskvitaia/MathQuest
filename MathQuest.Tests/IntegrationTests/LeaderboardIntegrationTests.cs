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
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MathQuest.Tests.IntegrationTests
{
    [TestFixture]
    public class LeaderboardControllerIntegrationTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task GetBestUsers_ValidToken_ReturnsOk()
        {
            // Arrange
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/api/leaderboard/user/scores");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetBestUsers_Unauthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/leaderboard/user/scores");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetUserScore_ValidToken_ReturnsOk()
        {
            // Arrange
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/api/leaderboard/user/score");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetUserScore_Unauthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/leaderboard/user/score");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task AddPoints_ValidToken_ReturnsOk()
        {
            // Arrange
            var model = new UserScoreDto { Points = 100 };
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/leaderboard/user/store-points", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task AddPoints_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var invalidModel = new UserScoreDto(); // Missing required Points field
            var json = JsonConvert.SerializeObject(invalidModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = await GetAccessTokenAsync();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/leaderboard/user/store-points", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task AddPoints_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var model = new UserScoreDto { Points = 100 };
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/leaderboard/user/store-points", content);

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