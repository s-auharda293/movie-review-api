using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Review;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;
        private readonly IMediator _mediator;

        public ReviewsController(IReviewService service, IMediator mediator)
        {
            _service = service;
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
        public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewDto dto,CancellationToken cancellationToken)
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
        public async Task<IActionResult> Update([FromRoute]Guid id,[FromBody] UpdateReviewDto dto)
        {
            var result = await _service.UpdateReviewAsync(id, dto);
                if (!result) return NotFound();
                return NoContent();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid id,[FromBody] PatchReviewDto dto)
        {
            try { 
            var result = await _service.PatchReviewAsync(id, dto);
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
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _service.DeleteReviewAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
