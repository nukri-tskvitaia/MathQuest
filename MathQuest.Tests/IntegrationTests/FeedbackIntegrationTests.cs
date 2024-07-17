/*
using Business.Interfaces;
using Business.Models;
using Business.Validations;
using FluentAssertions;
using MathQuest.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MathQuest.Tests.IntegrationTests
{
    [TestFixture]
    public class FeedbackControllerIntegrationTests
    {
        private HttpClient _client;
        private Mock<IFeedbackService> _feedbackServiceMock;
        private CustomWebApplicationFactory _factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _feedbackServiceMock = new Mock<IFeedbackService>();

            _factory = new CustomWebApplicationFactory(services =>
            {
                services.AddScoped(_ => _feedbackServiceMock.Object);
            });

            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Post_ValidFeedback_ReturnsOk()
        {
            // Arrange
            var feedback = new FeedbackModel { UserId = "123456", FeedbackText = "Test feedback message" };
            var json = JsonConvert.SerializeObject(feedback);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = "MockAccessToken"; // Mock access token for authorization

            _feedbackServiceMock.Setup(x => x.AddFeedbackAsync(It.IsAny<FeedbackModel>()))
                                .Returns(Task.CompletedTask);

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/feedback", content);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Post_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var invalidFeedback = new FeedbackModel(); // Invalid because UserId and FeedbackText are required
            var json = JsonConvert.SerializeObject(invalidFeedback);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = "MockAccessToken"; // Mock access token for authorization

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/feedback", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Post_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            var feedback = new FeedbackModel { UserId = "123456", FeedbackText = "Test feedback message" };
            var json = JsonConvert.SerializeObject(feedback);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/feedback", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Post_ServiceThrowsMathQuestException_ReturnsBadRequest()
        {
            // Arrange
            var feedback = new FeedbackModel { UserId = "123456", FeedbackText = "Test feedback message" };
            var json = JsonConvert.SerializeObject(feedback);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = "MockAccessToken"; // Mock access token for authorization

            _feedbackServiceMock.Setup(x => x.AddFeedbackAsync(It.IsAny<FeedbackModel>()))
                                .Throws<MathQuestException>();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/feedback", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Post_InternalServerError_ReturnsInternalServerError()
        {
            // Arrange
            var feedback = new FeedbackModel { UserId = "123456", FeedbackText = "Test feedback message" };
            var json = JsonConvert.SerializeObject(feedback);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = "MockAccessToken"; // Mock access token for authorization

            _feedbackServiceMock.Setup(x => x.AddFeedbackAsync(It.IsAny<FeedbackModel>()))
                                .Throws<Exception>();

            // Act
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsync("/api/feedback", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client.Dispose();
                _factory.Dispose();
            }
        }
    }
}

*/