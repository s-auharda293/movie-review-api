using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Review;

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
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ReviewDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var review = await _mediator.Send(new GetReviewByIdQuery(id), cancellationToken);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create(CreateReviewDto dto,CancellationToken cancellationToken)
        {
            try
            {
                var created = await _mediator.Send(new CreateReviewCommand(dto),cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException e) {
                return BadRequest(new { error = e.Message});
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateReviewDto dto, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateReviewCommand(id, dto), cancellationToken);
                if (!result) return NotFound();
                return NoContent();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Patch(Guid id, PatchReviewDto dto, CancellationToken cancellationToken)
        {
            try { 
            var result = await _mediator.Send(new PatchReviewCommand(id, dto),cancellationToken);
            if (!result) return NotFound();
            return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete( Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteReviewCommand(id), cancellationToken);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
