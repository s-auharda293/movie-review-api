using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Movie;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorageService;

        public MoviesController(IMediator mediator, IFileStorageService fileStorageService)
        {
            _mediator = mediator;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _mediator.Send(new GetMoviesQuery());
            return Ok(movies);
        }

        [HttpPost]
        [Route("query")]
        public async Task<IActionResult> SearchMovies(SearchMoviesQuery searchMoviesQuery)
        {
            var movies = await _mediator.Send(searchMoviesQuery);
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMovie(Guid id)
        {
            var movie = await _mediator.Send(new GetMovieByIdQuery(id));
            return movie.IsSuccess ? Ok(movie) : NotFound(movie);
        }

        [HttpPost]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> PostMovie(CreateMovieCommand createMovieCommand)
        {
            var movie = await _mediator.Send(createMovieCommand);
            return movie.IsSuccess ? CreatedAtAction(nameof(GetMovie),
                                            new { id = movie?.Value?.Id },
                                            movie) : BadRequest(movie);
        }

        [HttpPut]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> PutMovie(UpdateMovieCommand updateMovieCommand)
        {

            var updated = await _mediator.Send(updateMovieCommand);
            return updated.IsSuccess ? Ok(updated) : NotFound(updated);
        }

        [HttpPatch]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> PatchMovie(PatchMovieCommand patchMovieCommand)
        {
            var patched = await _mediator.Send(patchMovieCommand);
            return patched.IsSuccess ? Ok(patched) : BadRequest(patched);
        }

        [HttpDelete]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteMovie(DeleteMovieCommand deleteMovieCommand)
        {
            var deleted = await _mediator.Send(deleteMovieCommand);
            return deleted.IsSuccess ? NoContent() : NotFound(deleted);
        }



        [HttpPost("upload-file")]
        public async Task<IActionResult> Upload(
         [FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = request.File.OpenReadStream();
            var url = await _fileStorageService.UploadFileAsync(stream, request.File.FileName,"local");
            return Ok(new { Url = url });
        }

        [HttpDelete("delete-file")]
        public async Task<IActionResult> Delete([FromQuery] string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return BadRequest("File URL is required.");

            await _fileStorageService.DeleteFileAsync(fileUrl);
            return Ok(new { Message = "File deleted successfully." });
        }


        [HttpPut("update-file")]
        public async Task<IActionResult> Update([FromForm] FileUpdateRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("New file is required.");

            if (string.IsNullOrEmpty(request.oldFileUrl))
                return BadRequest("Old file URL is required.");

            using var stream = request.File.OpenReadStream();
            var newUrl = await _fileStorageService.UpdateFileAsync(stream, request.oldFileUrl, request.File.FileName);

            return Ok(new { Url = newUrl });
        }




    }

}
