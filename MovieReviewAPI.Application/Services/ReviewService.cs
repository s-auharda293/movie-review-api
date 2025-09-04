using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;
        private readonly IMovieRepository _movieRepository;

        public ReviewService(IReviewRepository repository, IMovieRepository movieRepository)
        {
            _repository = repository;
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _repository.GetAllAsync();
            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                MovieId = r.MovieId,
                UserName = r.UserName,
                Comment = r.Comment,
                Rating = r.Rating
            }).ToList();
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByMovieIdAsync(Guid movieId)
        {
            var reviews = await _repository.GetByMovieIdAsync(movieId);
            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                MovieId = r.MovieId,
                UserName = r.UserName,
                Comment = r.Comment,
                Rating = r.Rating
            }).ToList();
        }

        public async Task<ReviewDto?> GetReviewByIdAsync(Guid id)
        {
            var r = await _repository.GetByIdAsync(id);
            if (r == null) return null;

            return new ReviewDto
            {
                Id = r.Id,
                MovieId = r.MovieId,
                UserName = r.UserName,
                Comment = r.Comment,
                Rating = r.Rating
            };
        }

        public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto)
        {
            var review = new Review
            {
                MovieId = dto.MovieId,
                UserName = dto.UserName,
                Comment = dto.Comment,
                Rating = dto.Rating
            };

            var movie = await _movieRepository.GetByIdAsync(dto.MovieId);
            if (movie == null) {
                throw new ArgumentException("Cannot post review because movie doesn't exist");
            }

            await _repository.AddAsync(review);
            await _repository.SaveChangesAsync();

            return new ReviewDto
            {
                Id = review.Id,
                MovieId = review.MovieId,
                UserName = review.UserName,
                Comment = review.Comment,
                Rating = review.Rating
            };
        }

        public async Task<bool> UpdateReviewAsync(Guid id, UpdateReviewDto dto)
        {
            var review = await _repository.GetByIdAsync(id);
            if (review == null) return false;

            review.UserName = dto.UserName;
            review.Comment = dto.Comment;
            review.Rating = dto.Rating;

            await _repository.UpdateAsync(review);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchReviewAsync(Guid id, PatchReviewDto dto)
        {
            var review = await _repository.GetByIdAsync(id);
            if (review == null) return false;

            if (!string.IsNullOrEmpty(dto.Comment)) review.Comment = dto.Comment;
            if (dto.Rating.HasValue) review.Rating = dto.Rating.Value;
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteReviewAsync(Guid id)
        {
            var review = await _repository.GetByIdAsync(id);
            if (review == null) return false;

            await _repository.DeleteAsync(review);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
