using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Services;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;

        public MoviesController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _service.GetAllMoviesAsync();
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMovie([FromRoute] int id)
        {
            var movie = await _service.GetMovieByIdAsync(id);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] CreateMovieDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var movie = await _service.CreateMovieAsync(dto);
                return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
            }
            catch (ArgumentException e) {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutMovie([FromRoute] int id, [FromBody] UpdateMovieDto dto)
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
        public async Task<IActionResult> PatchMovie([FromRoute] int id, [FromBody] PatchMovieDto dto)
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
        public async Task<IActionResult> DeleteMovie([FromRoute] int id)
        {
            var deleted = await _service.DeleteMovieAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
