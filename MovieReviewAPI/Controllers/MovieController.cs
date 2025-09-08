using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Actor;
using MovieReviewApi.Application.Commands.Movie;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MoviesController(IMediator mediator)
        {
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
        public async Task<IActionResult> GetMovie(Guid id, CancellationToken cancellationToken)
        {
            var movie = await _mediator.Send(new GetMovieByIdQuery(id), cancellationToken);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie( CreateMovieDto dto, CancellationToken cancellationToken)
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
        public async Task<IActionResult> PutMovie(Guid id, UpdateMovieDto dto, CancellationToken cancellationToken)
        {

            try { 
            var updated = await _mediator.Send(new UpdateMovieCommand(id, dto), cancellationToken);
            return updated ? Ok() : NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchMovie( Guid id,  PatchMovieDto dto, CancellationToken cancellationToken)
        {
            try { 
            var patched = await _mediator.Send(new PatchMovieCommand(id, dto),cancellationToken);
            return patched ? NoContent() : NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMovie( Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _mediator.Send(new DeleteMovieCommand(id),cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
    }
}
