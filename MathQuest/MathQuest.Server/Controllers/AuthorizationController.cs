using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;

namespace MathQuestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IConsoleEmailSenderService _consoleEmailSenderService;

        public AuthorizationController(IUserService userService, IJwtService jwtService, IEmailSenderService emailSenderService, IConsoleEmailSenderService consoleEmailSenderService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _emailSenderService = emailSenderService;
            _consoleEmailSenderService = consoleEmailSenderService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid login request." });
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return BadRequest(new { Message = "User is already logged in." });
            }

            var (token, roles, signInResult) = await _userService.SignInUserAsync(model, HttpContext).ConfigureAwait(false);

            if (signInResult == LoginResult.InvalidCredentials)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            if (signInResult == LoginResult.RequiresTwoFactor)
            {
                return Ok(new { Message = "User requires 2FA", Requires2FA = true, UserEmail = model.Email });
            }

            return Ok(new { Message = "User signed in successfully", Requires2FA = false, AccessToken = token, Roles = roles });
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] TwoFactorModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var (token, roles, signInResult) = await _userService.SignInUserWithTwoFactorAsync(HttpContext, model).ConfigureAwait(false);

            if (signInResult == LoginResult.InvalidCredentials || string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Message = "Invalid Attempt" });
            }

            return Ok(new { Message = "2FA sign in completed successfully", AccessToken = token, Roles = roles });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid registration request." });
            }

            var result = await _userService.RegisterUserAsync(model).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Registration failed.", Errors = result.Errors });
            }

            // Send registration confirmation email to the user
            var confirmationToken = await _userService.GenerateEmailConfirmationTokenAsync(model.Email).ConfigureAwait(false);
            var encodedLink = HttpUtility.UrlEncode(confirmationToken);
            var confirmationLink =
                $"{Request.Scheme}://localhost:5100/confirm-email?email={model.Email}&confirmationToken={encodedLink}";

            await _consoleEmailSenderService.SendEmailAsync(
                model.Email,
                "Confirm your email",
                $"<p>Please confirm your account by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
            .ConfigureAwait(false);
            //await _emailSenderService
            //.SendEmailAsync(model.Email, "Confirm your email", $"<p>Please confirm your account by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
            //.ConfigureAwait(false);

            return Ok(new { Message = "User registered successfully. Please check your email for the confirmation link.", confirmationToken });
        }

        [HttpPost("register/resend-confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var user = await _userService.GetUserByEmailAsync(email).ConfigureAwait(false);

            if (user!.EmailConfirmed)
            {
                return BadRequest(new { Message = "Email already confirmed" });
            }

            var confirmationToken = await _userService.GenerateEmailConfirmationTokenAsync(email).ConfigureAwait(false);
            var encodedLink = HttpUtility.UrlEncode(confirmationToken);
            var confirmationLink =
                $"{Request.Scheme}://localhost:5100/confirm-email?email={user.Email}&confirmationToken={encodedLink}";

            await _consoleEmailSenderService.SendEmailAsync(
                email,
                "Confirm your email",
                $"<p>Please confirm your account by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
            .ConfigureAwait(false);
            //await _emailSenderService
                //.SendEmailAsync(email, "Confirm your email", $"<p>Please confirm your account by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
                //.ConfigureAwait(false);

            return Ok(new { Message = "Please check your email for the confirmation link.", confirmationToken });
        }

        [HttpPost("register/confirm")]
        public async Task<IActionResult> ConfirmRegistration([FromBody] ConfirmRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid confirmation request." });
            }

            var result = await _userService.ConfirmRegistrationAsync(model.Email, model.ConfirmationToken).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Registration confirmation failed.", Errors = result.Errors });
            }

            return Ok(new { Message = "Registration confirmed successfully." });
        }

        [HttpGet("search-email")]
        public async Task<IActionResult> FindEmail([FromQuery] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserByEmailAsync(email).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound(new { Message = "User with such email not found" });
            }

            var confirmationToken = await _userService.GeneratePasswordResetTokenAsync(email).ConfigureAwait(false);
            var encodedLink = HttpUtility.UrlEncode(confirmationToken);
            var confirmationLink =
                $"{Request.Scheme}://localhost:5100/reset-password?email={user.Email}&token={encodedLink}";

            await _consoleEmailSenderService.SendEmailAsync(
                email,
                "Password Reset",
                $"<p>Please reset your password by clicking this link: <a href=\"{confirmationLink}\">link</a></p>")
            .ConfigureAwait(false);
            //await _emailSenderService.SendPasswordResetEmailAsync(email, confirmationLink).ConfigureAwait(false);

            return Ok(new { Message = "User Found Successfully", confirmationToken });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Token)){
                return BadRequest(new { Message = "Invalid request" });
            }

            var result = await _userService.ResetPasswordAsync(model.Email, model.Password, model.Token).ConfigureAwait(false);

            if (!result)
            {
                return BadRequest(new { Message = "Invalid request" });
            }

            return Ok(new { Message = "Password has been reset successfully" });
        }


        [HttpPost("logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var refreshToken = HttpContext.Request.Cookies["RefreshToken"];

            if (refreshToken == null)
            {
                return Unauthorized(new { Message = "User already logged out" });
            }

            await _userService
                .SetActiveStatusAsync(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), false)
                .ConfigureAwait(false);
            
            await _userService.RemoveTokensAsync(refreshToken, HttpContext).ConfigureAwait(false);

            return Ok(new { Message = "User logged out successfully" });
        }

        // Because i am retrieving from cookies i do not have to write... RefreshToken([FromBody] TokenModel model)
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(bool rememberMe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var accessToken = HttpContext.Request.Cookies["AccessToken"];
            
            if (accessToken != null)
            {
                return NoContent();
            }

            var (refreshTokenModel, roles) = await _userService.GetRefreshTokenAsync(HttpContext, rememberMe).ConfigureAwait(false);

            if (refreshTokenModel == null)
            {
                return Unauthorized(new { Message = "Not permitted." });
            }

            return Ok(new { Message = "Tokens updated successfully", refreshTokenModel.AccessToken, roles});
        }

        [HttpGet("check-auth")]
        public Task<IActionResult> CheckAuthorization()
        {
            var accessToken = HttpContext.Request.Cookies["AccessToken"];

            if (accessToken == null)
            {
                return Task.FromResult<IActionResult>(Unauthorized());
            }

            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpGet("user-id"), Authorize]
        public Task<IActionResult> GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Task.FromResult<IActionResult>(Unauthorized());
            }

            return Task.FromResult<IActionResult>(Ok(new { UserId = userId }));
        }
    }
}