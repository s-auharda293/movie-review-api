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
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _mediator.Send(new GetMoviesQuery());
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMovie(Guid id)
        {
            var movie = await _mediator.Send(new GetActorByIdQuery(id));
            return movie.IsSuccess? Ok(movie) : NotFound(movie);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie(CreateMovieCommand createMovieCommand)
        {
                var movie = await _mediator.Send(createMovieCommand);
                return movie.IsSuccess ? CreatedAtAction(nameof(GetMovie),
                                                new { id = movie?.Value?.Id },
                                                movie) : BadRequest(movie);
        }

        [HttpPut]
        public async Task<IActionResult> PutMovie(UpdateMovieCommand updateMovieCommand)
        {

            var updated = await _mediator.Send(updateMovieCommand);
                return updated.IsSuccess ? Ok(updated) : NotFound(updated);
        }

        [HttpPatch]
        public async Task<IActionResult> PatchMovie(PatchMovieCommand patchMovieCommand)
        {
            var patched = await _mediator.Send(patchMovieCommand);
            return patched.IsSuccess ? Ok(patched) : BadRequest(patched);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovie(DeleteMovieCommand deleteMovieCommand)
        {
            var deleted = await _mediator.Send(deleteMovieCommand);
            return deleted.IsSuccess ? NoContent() : NotFound(deleted);
        }
    }
}
