using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MathQuest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITwoFactorService _twoFactorService;
        private readonly IUserService _userService;
        private readonly IGradeService _gradeService;

        public AccountController(ITwoFactorService twoFactorService, IUserService userService, IGradeService gradeService)
        {
            _userService = userService;
            _twoFactorService = twoFactorService;
            _gradeService = gradeService;
        }

        [HttpGet("is-two-factor-enabled"), Authorize]
        public async Task<IActionResult> TwoFactorEnabled()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Id == null)
            {
                return Unauthorized();
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(Id).ConfigureAwait(false);

                if (user == null)
                {
                    return BadRequest();
                }

                var result = await _userService.IsTwoFactorEnabledAsync(user).ConfigureAwait(false);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("user/grade"), Authorize]
        public async Task<IActionResult> GetUserGrade()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Id == null)
            {
                return Unauthorized();
            }

            var gradeId = await _userService.GetUserGradeIdAsync(Id).ConfigureAwait(false);

            if (gradeId == null)
            {
                return BadRequest();
            }

            var gradeLevel = await _gradeService.GetGradeLevelByIdAsync((int)gradeId).ConfigureAwait(false);

            if (gradeLevel == null)
            {
                return BadRequest();
            }

            return Ok(new { gradeLevel });
        }

        [HttpGet("manage/user-info"), Authorize]
        public async Task<IActionResult> UserInfo()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Id == null)
            {
                return Unauthorized();
            }

            var result = await _userService.GetUserInfoAsync(Id).ConfigureAwait(false);

            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [HttpPut("manage/update/user-info"), Authorize]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Id == null)
            {
                return Unauthorized();
            }

            var (result, emailExists) = await _userService.UpdateUserInfoAsync(model, Id).ConfigureAwait(false);

            if (emailExists == EmailExists.Exists)
            {
                return BadRequest(new { Message = "Such email already exists!" });
            }

            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured.");
            }

            return Ok();
        }

        [HttpPost("manage/change-password"), Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Id == null)
            {
                return Unauthorized();
            }

            var result = await _userService.ChangePasswordAsync(Id, model.OldPassword, model.NewPassword).ConfigureAwait(false);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        // Two factor staff
        [HttpPost("manage/setup-2fa"), Authorize]
        public async Task<IActionResult> Generate2FAKey()
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

            var result = await _twoFactorService
                .GenerateTwoFactorAsync(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(result.Value.key) || result.Value.image == null)
            {
                return NotFound(new { message = "User not found or Code generation failed" });
            }

            return Ok(new { Key = result.Value.key, Image = result.Value.image });
        }

        // This is for setup verification if setup is configured correctly it will enable two factor verification
        [HttpPost("manage/verify-2fa-token"), Authorize]
        public async Task<IActionResult> Verify2FAToken([FromBody] string token)
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

            var isValid = await _twoFactorService
                .VerifyTwoFactorTokenAsync(userId, token)
                .ConfigureAwait(false);

            if (!isValid)
            {
                return BadRequest(new { Message = "Invalid token" });
            }

            return Ok(new { Message = "2FA was setup successfully" });
        }

        [HttpPost("manage/disable-2fa-token"), Authorize]
        public async Task<IActionResult> Verify2FAToken()
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

            var result = await _twoFactorService.DisableTwoFactorAsync(userId).ConfigureAwait(false);

            if (!result)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            return Ok();
        }
    }
}
