using Business.Interfaces;
using Business.Models;
using Business.Models.DTO;
using MathQuest.Server.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MathQuest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet("user/scores"), Authorize]
        public async Task<IActionResult> GetBestUsers()
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

            try
            {
                var result = await _leaderboardService
                    .GetBestUserScoresAsync()
                    .ConfigureAwait(false);
                return Ok(result);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpGet("user/score"), Authorize]
        public async Task<IActionResult> GetUserScore()
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

            try
            {
                var result = await _leaderboardService.GetUserScoreAsync(userId)
                    .ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("user/store-points"), Authorize]
        public async Task<IActionResult> AddPoints([FromBody] UserScoreDto model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                var result = await _leaderboardService
                    .AddUserPointsAsync(new LeaderboardDataModel
                    {
                        Points = model.Points,
                        UserId = userId,
                    }).ConfigureAwait(false);

                if (result)
                {
                    return Ok();
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while adding the data");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
