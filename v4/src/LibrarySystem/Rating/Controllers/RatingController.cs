using Microsoft.AspNetCore.Mvc;
using Rating.DTO;
using Rating.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rating.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RatingController : Controller
    {

        readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("rating")]
        public async Task<IActionResult> GetUserRating([FromHeader(Name = "X-User-Name"), Required] string xUserName)
        {
            if (string.IsNullOrWhiteSpace(xUserName))
            {
                return BadRequest();

            }

            var rating = await _ratingService.GetRatingResponseByUserName(xUserName);
            return rating == null ? BadRequest() : Ok(rating);
        }

        [HttpPost("rating")]
        public async Task<IActionResult> UpdateUserRating([FromHeader(Name = "X-User-Name"), Required] string xUserName, [FromBody, Required] RatingUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(xUserName))
            {
                return BadRequest();
            }

            var rating = await _ratingService.UpdateRating(xUserName, request.lessCond, request.laterDate);
            return rating == null ? BadRequest() : Ok(rating);
        }

        [HttpDelete("rating")]
        public async Task<IActionResult> DeleteUserRating([FromHeader(Name = "X-User-Name"), Required] string xUserName)
        {
            if (string.IsNullOrWhiteSpace(xUserName))
            {
                return BadRequest();
            }

            await _ratingService.DeleteRating(xUserName);
            return Ok();
        }
    }
}