/*
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Business.Models;

namespace MathQuest.Tests.IntegrationTests
{
    [TestFixture]
    public class AuthorizationControllerIntegrationTests
    {
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsAccessToken()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "test@example.com", Password = "P@ssw0rd" };
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/authorization/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject.Message.Should().Be("User signed in successfully");
            responseObject.AccessToken.Should().NotBeNull();
        }

        [Test]
        public async Task Register_ValidRegistration_ReturnsConfirmationLink()
        {
            // Arrange
            var registerModel = new RegisterModel { Email = "newuser@example.com", Password = "P@ssw0rd" };
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(registerModel.Email), "Email");
            formData.Add(new StringContent(registerModel.Password), "Password");

            // Act
            var response = await _client.PostAsync("/api/authorization/register", formData);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject.Message.Should().Be("User registered successfully");
            responseObject.confirmationToken.Should().NotBeNull();
        }

        [Test]
        public async Task Verify2FA_ValidTwoFactor_ReturnsAccessToken()
        {
            // Arrange
            var twoFactorModel = new TwoFactorModel { Email = "test@example.com", Code = "123456" };
            var json = JsonConvert.SerializeObject(twoFactorModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/authorization/verify-2fa", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject.Message.Should().Be("2FA sign in completed successfully");
            responseObject.AccessToken.Should().NotBeNull();
        }

        [Test]
        public async Task ResendEmailConfirmation_ValidEmail_ReturnsConfirmationLink()
        {
            // Arrange
            var email = "test@example.com";
            var json = JsonConvert.SerializeObject(email);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/authorization/register/resend-confirmation", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject.Message.Should().Be("Please check your email for the confirmation link.");
            responseObject.confirmationToken.Should().NotBeNull();
        }

        [Test]
        public async Task ConfirmRegistration_ValidToken_ReturnsSuccessMessage()
        {
            // Arrange
            var confirmModel = new ConfirmRegistrationModel { Email = "newuser@example.com", ConfirmationToken = "valid_token" };
            var json = JsonConvert.SerializeObject(confirmModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/authorization/register/confirm", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Registration confirmed successfully");
        }

        [Test]
        public async Task FindEmail_ExistingEmail_ReturnsResetPasswordLink()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var response = await _client.GetAsync($"/api/authorization/search-email?email={email}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Please reset your password by clicking this link");
        }

        [Test]
        public async Task ForgotPassword_ValidResetRequest_ReturnsSuccessMessage()
        {
            // Arrange
            var resetModel = new ResetPasswordModel { Email = "test@example.com", Password = "NewP@ssw0rd", Token = "reset_token" };
            var json = JsonConvert.SerializeObject(resetModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/authorization/reset-password", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Password has been reset successfully");
        }

        [Test]
        public async Task Logout_ValidRequest_ReturnsSuccessMessage()
        {
            // Arrange: Perform a valid login to get cookies or tokens for authentication
            var loginModel = new LoginModel { Email = "test@example.com", Password = "P@ssw0rd" };
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/authorization/login", content);
            loginResponse.EnsureSuccessStatusCode();

            // Act
            var response = await _client.PostAsync("/api/authorization/logout", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("User logged out successfully");
        }

        [Test]
        public async Task RefreshToken_ValidRequest_ReturnsAccessToken()
        {
            // Arrange: Perform a valid login to get cookies or tokens for authentication
            var loginModel = new LoginModel { Email = "test@example.com", Password = "P@ssw0rd" };
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/authorization/login", content);
            loginResponse.EnsureSuccessStatusCode();

            // Act
            var response = await _client.PostAsync("/api/authorization/refresh?rememberMe=true", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Tokens updated successfully");
        }

        [Test]
        public async Task CheckAuthorization_ValidAccessToken_ReturnsSuccess()
        {
            // Arrange: Perform a valid login to get cookies or tokens for authentication
            var loginModel = new LoginModel { Email = "test@example.com", Password = "P@ssw0rd" };
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/authorization/login", content);
            loginResponse.EnsureSuccessStatusCode();

            // Act
            var response = await _client.GetAsync("/api/authorization/check-auth");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetUserId_ValidAccessToken_ReturnsUserId()
        {
            // Arrange: Perform a valid login to get cookies or tokens for authentication
            var loginModel = new LoginModel { Email = "test@example.com", Password = "P@ssw0rd" };
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/authorization/login", content);
            loginResponse.EnsureSuccessStatusCode();

            // Act
            var response = await _client.GetAsync("/api/authorization/user-id");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("UserId");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
        }
    }
}

*/