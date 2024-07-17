using Business.Interfaces;
using Business.Models;
using Business.Validations;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MathQuest.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] FeedbackModel feedback)
        {
            if (!ModelState.IsValid || feedback == null)
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
                await _feedbackService.AddFeedbackAsync(feedback).ConfigureAwait(false);
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
