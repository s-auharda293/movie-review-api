using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Persistence;

namespace MovieReviewApi.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieReviewDbContext _dbcontext;

        public ReviewRepository(MovieReviewDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync() =>
            await _dbcontext.Reviews.Include(r => r.Movie).ToListAsync();

        public async Task<IEnumerable<Review>> GetByMovieIdAsync(Guid movieId) =>
            await _dbcontext.Reviews
                          .Where(r => r.MovieId == movieId)
                          .Include(r => r.Movie)
                          .ToListAsync();

        public async Task<Review?> GetByIdAsync(Guid id) =>
            await _dbcontext.Reviews
                          .Include(r => r.Movie)
                          .FirstOrDefaultAsync(r => r.Id == id);

        public async Task AddAsync(Review review) =>
            await _dbcontext.Reviews.AddAsync(review);

        public Task UpdateAsync(Review review) {
            _dbcontext.Reviews.Update(review);
            return Task.CompletedTask;
        }


        public Task DeleteAsync(Review review)
        {
            _dbcontext.Reviews.Remove(review);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync() =>
            await _dbcontext.SaveChangesAsync();
    }
}
