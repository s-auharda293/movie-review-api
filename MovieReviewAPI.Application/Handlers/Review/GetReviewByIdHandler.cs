using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Review;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class GetReviewByIdHandler:IRequestHandler<GetReviewByIdQuery,Result<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetReviewByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ReviewDto>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                          .Include(r => r.User)
                          .Include(r => r.Movie)
                          .FirstOrDefaultAsync(r => r.Id == request.Id,cancellationToken);
            if (review == null) return Result<ReviewDto>.Failure(ReviewErrors.NotFound);

            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                MovieId = review.MovieId,
                UserId = Guid.Parse(review.User!.Id), //when we fetch review and include user from db, UserId is stored as string but dto is guid so parse into guid
                UserName = review.User?.UserName,
                Comment = review.Comment,
                Rating = review.Rating
            };

            return Result<ReviewDto>.Success(reviewDto);
        }

    }
}
