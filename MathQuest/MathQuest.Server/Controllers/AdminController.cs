using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Business.Validations;
using static QRCoder.PayloadGenerator;

namespace MathQuest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IFeedbackService _feedbackService;

        public AdminController(IUserService userService, IMessageService messageService, IFeedbackService feedbackService)
        {
            _userService = userService;
            _messageService = messageService;
            _feedbackService = feedbackService;
        }

        // Role Management
        [HttpGet("user/role/{email}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRoles([FromRoute] string email)
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
                var result = await _userService
                    .GetUserRolesByEmailAsync(email).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("user/role"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserRole([FromBody] UserRoleModel model)
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

            var user = await _userService.GetUserByEmailAsync(model.Email).ConfigureAwait(false);
            if (user == null || user.Id == Id)
            {
                return BadRequest();
            }

            try
            {
                var result = await _userService
                    .AddUserToRoleAsync(user, model.RoleName)
                    .ConfigureAwait(false);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            } 
        }

        [HttpDelete("user/role"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserRole([FromBody] UserRoleModel model)
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

            var user = await _userService.GetUserByEmailAsync(model.Email).ConfigureAwait(false);
            if (user == null || user.Id == Id)
            {
                return BadRequest();
            }

            try
            {
                var result = await _userService
                    .RemoveUserFromRoleAsync(user, model.RoleName).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/admin/get-users
        [HttpGet("get-users"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var users = await _userService.GetAllUsersAsync().ConfigureAwait(false);

            if (users == null || !users.Any())
            {
                return NoContent();
            }

            return Ok(users);
        }

        [HttpGet("get-user/{email}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> GetByEmail(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var user = await _userService.GetUserByEmailAsync(email).ConfigureAwait(false);

            if (user == null)
            {
                return NoContent();
            }

            return Ok(user);
        }

        [HttpDelete("delete-user/{email}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string email)
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

            var user = await _userService.GetUserByEmailAsync(email).ConfigureAwait(false);
            if (user == null || user.Id == Id)
            {
                return BadRequest();
            }

            var result = await _userService.DeleteUserByEmailAsync(HttpContext, email).ConfigureAwait(false);

            if (!result)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            return Ok(new { Message = "User deleted successfully" });
        }

        [HttpGet("get-all-messages"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMessages()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var messages = await _messageService.GetAllUsersMessagesAsync().ConfigureAwait(false);

            return Ok(messages);
        }

        [HttpPost("user-lockout"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Lockout([FromBody] LockoutModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(new { Message = "Invalid lockout request." });
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Id == null)
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByEmailAsync(model.Email).ConfigureAwait(false);
            if (user == null || user.Id == Id)
            {
                return BadRequest();
            }

            var result = await _userService.LockOutUserAsync(model).ConfigureAwait(false);

            if (result == LockoutResult.UserNotFound)
            {
                return NotFound(new { Message = "User not found." });
            }

            if (result == LockoutResult.AlreadyLockedOut || result == LockoutResult.LockoutFailed)
            {
                return BadRequest(new { Message = "Lockout failed." });
            }

            return Ok(new { Message = "User locked out successfully." });
        }

        // User Feedback

        [HttpGet("feedbacks"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            try
            {
                var result = await _feedbackService
                    .GetAllFeedbacksAsync(fromDate, toDate)
                    .ConfigureAwait(false);
                return Ok(result);
            }
            catch (MathQuestException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("feedbacks/{userId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersAll([FromRoute] string userId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Id == null)
            {
                return Unauthorized();
            }

            try
            {
                var result = await _feedbackService
                    .GetUsersAllFeedbacksByUserIdAsync(userId, fromDate, toDate).ConfigureAwait(false);
                return Ok(result);
            }
            catch (MathQuestException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("feedbacks/{userId}/{feedbackId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersSingle([FromRoute] string userId, [FromRoute] int feedbackId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Id == null)
            {
                return Unauthorized();
            }

            try
            {
                var result = await _feedbackService
                    .GetSingleUserFeedbackAsync(userId, feedbackId).ConfigureAwait(false);
                return Ok(result);
            }
            catch (MathQuestException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("feedbacks/{userId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeedbacksInPeriod([FromRoute] string userId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Id == null)
            {
                return Unauthorized();
            }

            try
            {
                await _feedbackService
                    .DeleteUserFeedbacksInPeriodAsync(userId, fromDate, toDate)
                    .ConfigureAwait(false);
                return Ok();
            }
            catch (MathQuestException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occured");
            }
        }

        [HttpDelete("feedbacks/{userId}/{feedbackId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserSpecificFeedback([FromRoute] string userId, [FromRoute] int feedbackId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Id == null)
            {
                return Unauthorized();
            }

            try
            {
                await _feedbackService
                    .DeleteFeedbackByUserIdAsync(userId, feedbackId)
                    .ConfigureAwait(false);
                return Ok();
            }
            catch (MathQuestException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occured");
            }
        }
    }
}
