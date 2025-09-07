using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Actor;
using MovieReviewApi.Application.Services;
using MovieReviewApi.Application.Commands.Movie;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;
        private readonly IMediator _mediator;

        public MoviesController(IMovieService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies(CancellationToken cancellationToken)
        {
            var movies = await _mediator.Send(new GetMoviesQuery(),cancellationToken);
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMovie([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var movie = await _mediator.Send(new GetMovieByIdQuery(id), cancellationToken);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] CreateMovieDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _mediator.Send(new CreateMovieCommand(dto),cancellationToken);
                return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
            }
            catch (ArgumentException e) {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutMovie([FromRoute] Guid id, [FromBody] UpdateMovieDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try { 
            var updated = await _service.UpdateMovieAsync(id, dto);
            return updated ? Ok() : NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchMovie([FromRoute] Guid id, [FromBody] PatchMovieDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try { 
            var patched = await _service.PatchMovieAsync(id, dto);
            return patched ? NoContent() : NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
        {
            var deleted = await _service.DeleteMovieAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
