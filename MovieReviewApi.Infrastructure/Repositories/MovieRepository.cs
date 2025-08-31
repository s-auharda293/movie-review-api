using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Persistence;

namespace MovieReviewApi.Infrastructure.Repositories;

    public class MovieRepository : IMovieRepository
    {
        private readonly MovieReviewDbContext _dbContext;
    public MovieRepository(MovieReviewDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
            return await _dbContext.Movies.Include(m => m.Actors).ToListAsync();
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _dbContext.Movies.Include(m=>m.Actors).FirstOrDefaultAsync(m=>m.Id==id);
    }


    public async Task AddAsync(Movie movie)
    {
        await _dbContext.Movies.AddAsync(movie);
    }
    public Task UpdateAsync(Movie movie)
    {
        _dbContext.Movies.Update(movie);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Movie movie)
    { 
        _dbContext.Movies.Remove(movie);
        return Task.CompletedTask;
    }   

    public async Task SaveChangesAsync()
    {
       await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByIdsAsync(List<int> ids)
    {
        return await _dbContext.Movies
            .Where(m => ids.Contains(m.Id))
            .ToListAsync();
    }

}

