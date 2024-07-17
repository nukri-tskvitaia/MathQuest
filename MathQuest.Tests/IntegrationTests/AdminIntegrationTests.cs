/*
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;

namespace MathQuest.Tests.IntegrationTests
{
    [TestFixture]
    public class AdminControllerIntegrationTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task GetUserRoles_ValidEmail_ReturnsOk()
        {
            // Arrange
            var email = "test@example.com";
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/user/role/{email}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetUserRoles_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            var email = "invalid-email";
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/user/role/{email}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task AddUserRole_ValidModel_ReturnsOk()
        {
            // Arrange
            var model = new UserRoleModel
            {
                Email = "test@example.com",
                RoleName = "Admin"
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/admin/user/role");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task AddUserRole_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var model = new UserRoleModel
            {
                Email = "invalid-email", // This might simulate an invalid scenario
                RoleName = "Admin"
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/admin/user/role");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task DeleteUserRole_ValidModel_ReturnsOk()
        {
            // Arrange
            var model = new UserRoleModel
            {
                Email = "test@example.com",
                RoleName = "Admin"
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/admin/user/role");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task DeleteUserRole_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var model = new UserRoleModel
            {
                Email = "invalid-email", // This might simulate an invalid scenario
                RoleName = "Admin"
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/admin/user/role");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetUsers_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/admin/get-users");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetByEmail_ExistingEmail_ReturnsOk()
        {
            // Arrange
            var email = "test@example.com";
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/get-user/{email}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetByEmail_NonExistingEmail_ReturnsNoContent()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/get-user/{email}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public async Task DeleteUser_ExistingEmail_ReturnsOk()
        {
            // Arrange
            var email = "test@example.com";
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/admin/delete-user/{email}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task DeleteUser_NonExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/admin/delete-user/{email}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetAllMessages_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/admin/get-all-messages");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Lockout_ValidModel_ReturnsOk()
        {
            // Arrange
            var model = new LockoutModel
            {
                Email = "test@example.com",
                LockoutEnd = DateTime.UtcNow.AddHours(1)
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/admin/user-lockout");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Lockout_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var model = new LockoutModel
            {
                Email = "invalid-email", // This might simulate an invalid scenario
                LockoutEnd = DateTime.UtcNow.AddHours(1)
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/admin/user-lockout");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");
            request.Content = content;

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetAllFeedbacks_ValidDateRange_ReturnsOk()
        {
            // Arrange
            var fromDate = DateTime.UtcNow.Date.AddDays(-7);
            var toDate = DateTime.UtcNow.Date;
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/feedbacks?fromDate={fromDate}&toDate={toDate}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAllFeedbacks_InvalidDateRange_ReturnsBadRequest()
        {
            // Arrange
            var fromDate = DateTime.UtcNow.Date;
            var toDate = DateTime.UtcNow.Date.AddDays(-7); // Invalid range
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/feedbacks?fromDate={fromDate}&toDate={toDate}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetUsersAllFeedbacksByUserId_ValidUserIdAndDateRange_ReturnsOk()
        {
            // Arrange
            var userId = "123456"; // Replace with a valid user ID
            var fromDate = DateTime.UtcNow.Date.AddDays(-7);
            var toDate = DateTime.UtcNow.Date;
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/feedbacks/{userId}?fromDate={fromDate}&toDate={toDate}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetUsersAllFeedbacksByUserId_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            var userId = "invalid-id"; // Invalid user ID
            var fromDate = DateTime.UtcNow.Date.AddDays(-7);
            var toDate = DateTime.UtcNow.Date;
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/feedbacks/{userId}?fromDate={fromDate}&toDate={toDate}");
            request.Headers.Add("Authorization", "Bearer YourAccessTokenHere");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task DeleteFeedback_ValidId_ReturnsNoContent()
        {
            // Arrange
            var feedbackId = 1; // Replace with an existing feedback ID in your database
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/authorization/feedback/{feedbackId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public async Task DeleteFeedback_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var feedbackId = -1; // Replace with a non-existing feedback ID in your database
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/authorization/feedback/{feedbackId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task DeleteFeedback_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var feedbackId = 1; // Replace with an existing feedback ID in your database
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/authorization/feedback/{feedbackId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
        }
    }
}

*/