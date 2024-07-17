using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MathQuest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessageController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        [HttpGet("users"), Authorize]
        public async Task<IActionResult> GetUserAll()
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

            var messages = await _messageService.GetUsersAllMessagesAsync(userId).ConfigureAwait(false);

            return Ok(messages);
        }

        [HttpGet("user/{username}"), Authorize]
        public async Task<IActionResult> GetSpecific([FromRoute] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var otherUser = await _userService.GetUserByUsernameAsync(username).ConfigureAwait(false);

            if (userId == null)
            {
                return Unauthorized();
            }

            if (otherUser == null)
            {
                return BadRequest();
            }

            var messages = await _messageService.GetUsersMessagesAsync(userId, otherUser.Id).ConfigureAwait(false);

            if (!messages.Any())
            {
                return NotFound();
            }

            return Ok(messages);
        }

        [HttpGet("user/{username}/{id}"), Authorize]
        public async Task<IActionResult> GetSingle(string username, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var otherUser = await _userService.GetUserByUsernameAsync(username).ConfigureAwait(false);

            if (userId == null)
            {
                return Unauthorized();
            }

            if (otherUser == null)
            {
                return BadRequest();
            }

            var message = await _messageService.GetMessageByIdsAsync(userId, otherUser.Id, id).ConfigureAwait(false);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        [HttpPost("add"), Authorize]
        public async Task<IActionResult> Add([FromBody] MessageModel model)
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

            await _messageService.AddMessageAsync(model).ConfigureAwait(false);

            return Ok();
        }
    }
}
