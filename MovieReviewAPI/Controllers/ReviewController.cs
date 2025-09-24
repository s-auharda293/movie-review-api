using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Actor;
using MovieReviewApi.Application.Queries.Review;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll() { 
            var reviews = await _mediator.Send(new GetReviewsQuery());
            return Ok(reviews);
        }


        [HttpGet("by-movie-id")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByMovie(Guid id) { 
            var review = await _mediator.Send(new GetReviewsByMovieIdQuery(id));
            if (review.IsSuccess) return Ok(review);
            return NotFound(review);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview (Guid id)
        {
            var review = await _mediator.Send(new GetReviewByIdQuery(id));
            return review.IsSuccess ? Ok(review) : NotFound(review);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin+", "+UserRoles.User)]
        public async Task<ActionResult<ReviewDto>> Create(CreateReviewCommand createReviewCommand)
        {
                var review = await _mediator.Send(createReviewCommand);
                return review.IsSuccess ? CreatedAtAction(nameof(GetReview),
                                               new { id = review?.Value?.Id },
                                               review) : BadRequest(review);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ReviewDto>> GetReviewsByUser(string userId)
        {
            var query = new GetReviewsByUserQuery(userId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result.Errors);

            return Ok(result.Value);
        }

        [HttpPut]

        [Authorize(Roles = UserRoles.Admin + ", " + UserRoles.User)]
        public async Task<IActionResult> Update(UpdateReviewCommand updateReviewCommand)
        {
            var updated = await _mediator.Send(updateReviewCommand);
            return updated.IsSuccess ? Ok(updated) : NotFound(updated);
        }

        [HttpPatch]

        [Authorize(Roles = UserRoles.Admin + ", " + UserRoles.User)]
        public async Task<IActionResult> Patch(PatchReviewCommand patchReviewCommand)
        {
            var patched = await _mediator.Send(patchReviewCommand);
            return patched.IsSuccess ? Ok(patched) : BadRequest(patched);
        }

        [HttpDelete]

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        public async Task<IActionResult> Delete( DeleteReviewCommand deleteReviewCommand)
        {
            var deleted = await _mediator.Send(deleteReviewCommand);
            return deleted.IsSuccess ? NoContent() : NotFound(deleted);
        }
    }
}
