using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Services;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto?> GetMovieByIdAsync(Guid id);
    Task<MovieDto> CreateMovieAsync(CreateMovieDto movieDto);
    Task<bool> UpdateMovieAsync(Guid id, UpdateMovieDto movieDto);
    Task<bool> PatchMovieAsync(Guid id, PatchMovieDto moviedto);
    Task<bool> DeleteMovieAsync(Guid id);
}
