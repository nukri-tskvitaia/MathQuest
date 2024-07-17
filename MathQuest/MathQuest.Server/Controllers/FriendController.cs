using Business.Interfaces;
using Business.Models.DTO;
using MathQuest.Server.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MathQuest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFriendshipService _friendshipService;

        public FriendController(IUserService userService, IFriendshipService friendshipService)
        {
            _userService = userService;
            _friendshipService = friendshipService;
        }

        [HttpGet("search/friends"), Authorize]
        public async Task<IActionResult> SearchFriends()
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

            var user = await _userService.GetUserByIdAsync(userId).ConfigureAwait(false);

            if (user == null)
            {
                return BadRequest();
            }

            var friends = await _friendshipService.GetAllFriendsAsync(userId).ConfigureAwait(false);

            if (friends == null)
            {
                return NotFound();
            }

            return Ok( new { Friends = friends });
        }

        [HttpGet("search/user"), Authorize]
        public async Task<IActionResult> SearchUser([FromQuery] string username)
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

            var userInfo = await _friendshipService.GetFriendInfoAsync(username).ConfigureAwait(false);

            if (userInfo == null)
            {
                return NotFound();
            }

            var otherUser = await _userService.GetUserByUsernameAsync(username).ConfigureAwait(false);
            var friendshipStatus = await _friendshipService.GetFriendStatusAsync(userId, otherUser.Id).ConfigureAwait(false);

            return Ok(new { UserInfo = userInfo, Status = friendshipStatus });
        }

        [HttpGet("search/user/{id}"), Authorize]
        public async Task<IActionResult> SearchUserById([FromRoute] string id)
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

            var userInfo = await _friendshipService.GetFriendInfoByIdAsync(id).ConfigureAwait(false);

            if (userInfo == null)
            {
                return NotFound();
            }

            var friendshipStatus = await _friendshipService.GetFriendStatusAsync(userId, id).ConfigureAwait(false);

            return Ok(new { UserInfo = userInfo, Status = friendshipStatus });
        }

        [HttpGet("are-friends/{userId}/{otherUserId}"), Authorize]
        public async Task<IActionResult> AreFriends(string userId, string otherUserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUser == null)
            {
                return Unauthorized();
            }

            var result = await _friendshipService.AreFriendsAsync(userId, otherUserId).ConfigureAwait(false);

            if (!result)
            {
                return Ok(result);
            }

            return Ok(result);
        }

        [HttpPost("add/friend"), Authorize]
        public async Task<IActionResult> AddFriendRequest([FromBody] UserNameDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var otherUser = await _userService.GetUserByUsernameAsync(model.UserName).ConfigureAwait(false);

            if (userId == null || otherUser == null || userId == otherUser.Id)
            {
                return BadRequest();
            }

            var result = await _friendshipService.AddFriendRequestAsync(userId, otherUser.Id).ConfigureAwait(false);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("remove/friend"), Authorize]
        public async Task<IActionResult> RemoveFriendRequest([FromBody] UserNameDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var otherUser = await _userService.GetUserByUsernameAsync(model.UserName).ConfigureAwait(false);

            if (userId == null || otherUser == null || userId == otherUser.Id)
            {
                return BadRequest();
            }

            var result = await _friendshipService.RemoveFriendRequestAsync(userId, otherUser.Id).ConfigureAwait(false);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("pending-requests"), Authorize]
        public async Task<IActionResult> GetPendingRequests()
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

            var pendingRequests = await _friendshipService.GetPendingRequestsAsync(userId).ConfigureAwait(false);

            return Ok(new { Requests = pendingRequests });
        }

        [HttpPost("confirm"), Authorize]
        public async Task<IActionResult> ConfirmFriendRequest([FromBody] UserNameDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var otherUser = await _userService.GetUserByUsernameAsync(model.UserName).ConfigureAwait(false);

            if (userId == null || otherUser == null)
            {
                return Unauthorized();
            }

            var friendship = await _friendshipService.GetRequestUserAsync(userId, otherUser.Id).ConfigureAwait(false);

            if (friendship == null)
            {
                return BadRequest();
            }

            var result = await _friendshipService.ConfirmFriendRequestAsync(friendship).ConfigureAwait(false);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("reject"), Authorize]
        public async Task<IActionResult> RejectFriendRequest([FromBody] UserNameDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid request." });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var otherUser = await _userService.GetUserByUsernameAsync(model.UserName).ConfigureAwait(false);

            if (userId == null || otherUser == null)
            {
                return Unauthorized();
            }

            var friendship = await _friendshipService.GetRequestUserAsync(userId, otherUser.Id).ConfigureAwait(false);

            if (friendship == null)
            {
                return BadRequest();
            }

            var result = await _friendshipService.RejectFriendRequestAsync(friendship).ConfigureAwait(false);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
