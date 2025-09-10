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
            var r = await _context.Reviews
                          .Include(r => r.Movie)
                          .FirstOrDefaultAsync(r => r.Id == request.Id,cancellationToken);
            if (r == null) return Result<ReviewDto>.Failure(ReviewErrors.NotFound);

            var reviewDto = new ReviewDto
            {
                Id = r.Id,
                MovieId = r.MovieId,
                UserName = r.UserName,
                Comment = r.Comment,
                Rating = r.Rating
            };

            return Result<ReviewDto>.Success(reviewDto);
        }

    }
}
