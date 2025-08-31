using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Services;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto?> GetMovieByIdAsync(int id);
    Task<MovieDto> CreateMovieAsync(CreateMovieDto movieDto);
    Task<bool> UpdateMovieAsync(int id, UpdateMovieDto movieDto);
    Task<bool> PatchMovieAsync(int id, PatchMovieDto moviedto);
    Task<bool> DeleteMovieAsync(int id);
}
