using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
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
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll(CancellationToken cancellationToken) { 
            var reviews = await _mediator.Send(new GetReviewsQuery(), cancellationToken);
            return Ok(reviews);
        }


        [HttpGet]
        [Route("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByMovie(Guid movieId, CancellationToken cancellationToken) { 
            var review = await _mediator.Send(new GetReviewsByMovieIdQuery(movieId), cancellationToken);
            if (review.IsSuccess) return Ok(review);
            return NotFound(review);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(Guid id, CancellationToken cancellationToken)
        {
            var review = await _mediator.Send(new GetReviewByIdQuery(id), cancellationToken);
            return review.IsSuccess ? Ok(review) : NotFound(review);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create(CreateReviewDto dto,CancellationToken cancellationToken)
        {
                var review = await _mediator.Send(new CreateReviewCommand(dto),cancellationToken);
                return review.IsSuccess ? CreatedAtAction(nameof(GetReview),
                                               new { id = review?.Value?.Id },
                                               review) : BadRequest(review);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateReviewDto dto, CancellationToken cancellationToken)
        {
            var updated = await _mediator.Send(new UpdateReviewCommand(id, dto), cancellationToken);
            return updated.IsSuccess ? Ok(updated) : NotFound(updated);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Patch(Guid id, PatchReviewDto dto, CancellationToken cancellationToken)
        {
            var patched = await _mediator.Send(new PatchReviewCommand(id, dto),cancellationToken);
            return patched.IsSuccess ? Ok(patched) : BadRequest(patched);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete( Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _mediator.Send(new DeleteReviewCommand(id), cancellationToken);
            return deleted.IsSuccess ? NoContent() : NotFound(deleted);
        }
    }
}
