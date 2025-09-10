using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Review;
using MovieReviewApi.Domain.Entities;

public class GetReviewsByMovieIdHandler : IRequestHandler<GetReviewsByMovieIdQuery, Result<IEnumerable<ReviewDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetReviewsByMovieIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<ReviewDto>>> Handle(GetReviewsByMovieIdQuery request, CancellationToken cancellationToken)
    {
        var reviewDtos = await _context.Reviews
            .Where(r => r.MovieId == request.Id)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                MovieId = r.MovieId,
                UserName = r.UserName,
                Comment = r.Comment,
                Rating = r.Rating
            })
            .ToListAsync(cancellationToken);

        if (!reviewDtos.Any()) {
            return Result<IEnumerable<ReviewDto>>.Failure(ReviewErrors.ReviewsForNonExistentMovie);
        }

        return Result<IEnumerable<ReviewDto>>.Success(reviewDtos);
    }
}
