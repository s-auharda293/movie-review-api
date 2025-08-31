using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int id);
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(Movie movie);
    Task SaveChangesAsync();
    Task<IEnumerable<Movie>> GetMoviesByIdsAsync(List<int> ids);
}
