using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class UpdateReviewHandler:IRequestHandler<UpdateReviewCommand,Result<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;
        public UpdateReviewHandler(IApplicationDbContext context)
        {
            _context = context;   
        }

        public async Task<Result<ReviewDto>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken) {

            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id);
            if (review == null) return Result<ReviewDto>.Failure(ReviewErrors.NotFound);

            review.UserName = request.dto.UserName;
            review.Comment = request.dto.Comment;
            review.Rating = request.dto.Rating;

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
