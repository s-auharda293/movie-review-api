using MovieReviewApi.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<IEnumerable<ReviewDto>> GetReviewsByMovieIdAsync(Guid movieId);
        Task<ReviewDto?> GetReviewByIdAsync(Guid id);
        Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto);
        Task<bool> UpdateReviewAsync(Guid id, UpdateReviewDto dto);
        Task<bool> PatchReviewAsync(Guid id, PatchReviewDto dto);
        Task<bool> DeleteReviewAsync(Guid id);
    }
}
