using MediatR;
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
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByMovie(GetReviewsByMovieIdQuery getReviewsByMovieIdQuery) { 
            var review = await _mediator.Send(getReviewsByMovieIdQuery);
            if (review.IsSuccess) return Ok(review);
            return NotFound(review);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview ([FromRoute] GetReviewByIdQuery getReviewByIdQuery)
        {
            var review = await _mediator.Send(getReviewByIdQuery);
            return review.IsSuccess ? Ok(review) : NotFound(review);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create(CreateReviewCommand createReviewCommand)
        {
                var review = await _mediator.Send(createReviewCommand);
                return review.IsSuccess ? CreatedAtAction(nameof(GetReview),
                                               new { id = review?.Value?.Id },
                                               review) : BadRequest(review);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateReviewCommand updateReviewCommand)
        {
            var updated = await _mediator.Send(updateReviewCommand);
            return updated.IsSuccess ? Ok(updated) : NotFound(updated);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(PatchReviewCommand patchReviewCommand)
        {
            var patched = await _mediator.Send(patchReviewCommand);
            return patched.IsSuccess ? Ok(patched) : BadRequest(patched);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete( DeleteReviewCommand deleteReviewCommand)
        {
            var deleted = await _mediator.Send(deleteReviewCommand);
            return deleted.IsSuccess ? NoContent() : NotFound(deleted);
        }
    }
}
