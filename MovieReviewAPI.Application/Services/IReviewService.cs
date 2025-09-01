using MovieReviewApi.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<IEnumerable<ReviewDto>> GetReviewsByMovieIdAsync(int movieId);
        Task<ReviewDto?> GetReviewByIdAsync(int id);
        Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto);
        Task<bool> UpdateReviewAsync(int id, UpdateReviewDto dto);
        Task<bool> PatchReviewAsync(int id, PatchReviewDto dto);
        Task<bool> DeleteReviewAsync(int id);
    }
}
