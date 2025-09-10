using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Actor;
using MovieReviewApi.Domain.Entities;

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
            return movie.IsSuccess? Ok(movie) : NotFound(movie);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie( CreateMovieDto dto, CancellationToken cancellationToken)
        {
                var movie = await _mediator.Send(new CreateMovieCommand(dto),cancellationToken);
                return movie.IsSuccess ? CreatedAtAction(nameof(GetMovie),
                                                new { id = movie?.Value?.Id },
                                                movie) : BadRequest(movie);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutMovie(Guid id, UpdateMovieDto dto, CancellationToken cancellationToken)
        {

            var updated = await _mediator.Send(new UpdateMovieCommand(id, dto), cancellationToken);
                return updated.IsSuccess ? Ok(updated) : NotFound(updated.Error);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchMovie( Guid id,  PatchMovieDto dto, CancellationToken cancellationToken)
        {
            var patched = await _mediator.Send(new PatchMovieCommand(id, dto),cancellationToken);
            return patched.IsSuccess ? Ok(patched) : BadRequest(patched);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMovie( Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _mediator.Send(new DeleteMovieCommand(id),cancellationToken);
            return deleted.IsSuccess ? NoContent() : NotFound(deleted);
        }
    }
}
