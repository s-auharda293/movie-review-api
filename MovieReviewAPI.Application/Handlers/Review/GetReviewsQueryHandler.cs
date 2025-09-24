using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Review;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, Result<IEnumerable<ReviewDto>>>
    {
        private readonly IApplicationDbContext _context;
        public GetReviewsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<IEnumerable<ReviewDto>>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            {
                var reviews = await _context.Reviews.Include(r => r.Movie).Include(r=>r.User).ToListAsync(cancellationToken);

                var reviewDtos = reviews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    MovieId = r.MovieId,
                    UserId = Guid.Parse(r.User!.Id),
                    UserName = r.User!.UserName,
                    Comment = r.Comment,
                    Rating = r.Rating
                }).ToList();


                return Result<IEnumerable<ReviewDto>>.Success(reviewDtos);
            }
        }
    }
}
