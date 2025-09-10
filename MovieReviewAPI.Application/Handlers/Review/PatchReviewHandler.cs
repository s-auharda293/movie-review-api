using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class PatchReviewHandler:IRequestHandler<PatchReviewCommand,Result<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;
        public PatchReviewHandler(IApplicationDbContext context)
        {
         _context = context;   
        }

        public async Task<Result<ReviewDto>> Handle(PatchReviewCommand request, CancellationToken cancellationToken) {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id);
            if (review == null) return Result<ReviewDto>.Failure(ReviewErrors.NotFound);

            if (!string.IsNullOrEmpty(request.dto.Comment)) review.Comment = request.dto.Comment;
            if (!string.IsNullOrEmpty(request.dto.UserName)) review.UserName = request.dto.UserName;
            if (request.dto.Rating.HasValue) review.Rating = request.dto.Rating.Value;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync(cancellationToken);

            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                MovieId = review.MovieId,
                UserName = review.UserName,
                Comment = review.Comment,
                Rating = review.Rating
            };

            return Result<ReviewDto>.Success(reviewDto);

        }
    }
}
