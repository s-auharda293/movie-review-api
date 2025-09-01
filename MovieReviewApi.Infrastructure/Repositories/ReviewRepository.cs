using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Persistence;

namespace MovieReviewApi.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieReviewDbContext _context;

        public ReviewRepository(MovieReviewDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync() =>
            await _context.Reviews.Include(r => r.Movie).ToListAsync();

        public async Task<IEnumerable<Review>> GetByMovieIdAsync(int movieId) =>
            await _context.Reviews
                          .Where(r => r.MovieId == movieId)
                          .Include(r => r.Movie)
                          .ToListAsync();

        public async Task<Review?> GetByIdAsync(int id) =>
            await _context.Reviews
                          .Include(r => r.Movie)
                          .FirstOrDefaultAsync(r => r.Id == id);

        public async Task AddAsync(Review review) =>
            await _context.Reviews.AddAsync(review);

        public Task UpdateAsync(Review review) {
            _context.Reviews.Update(review);
            return Task.CompletedTask;
        }


        public Task DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
