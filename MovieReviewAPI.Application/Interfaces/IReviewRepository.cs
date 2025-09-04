using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<IEnumerable<Review>> GetByMovieIdAsync(Guid movieId);
        Task<Review?> GetByIdAsync(Guid id);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Review review);
        Task SaveChangesAsync();
    }
}
