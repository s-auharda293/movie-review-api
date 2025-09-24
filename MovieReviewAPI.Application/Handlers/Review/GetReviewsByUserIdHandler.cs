using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;

public class GetReviewsByUserHandler : IRequestHandler<GetReviewsByUserQuery, Result<IEnumerable<ReviewDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetReviewsByUserHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<ReviewDto>>> Handle(GetReviewsByUserQuery request, CancellationToken cancellationToken)
    {
        // Fetch reviews for the given user
        var reviews = await _context.Reviews
            .Include(r => r.Movie)
            .Include(r=>r.User)
            .Where(r => r.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        if (!reviews.Any())
            return Result<IEnumerable<ReviewDto>>.Failure(ReviewErrors.ReviewsForNonExistentUser);

        var reviewDtos = reviews.Select(r => new ReviewDto
        {
            Id = r.Id,
            MovieId = r.MovieId,
            UserId = Guid.Parse(r.UserId!),
            UserName = r.User!.UserName,  // assuming UserName is stored in Review
            Comment = r.Comment,
            Rating = r.Rating
        });

        return Result<IEnumerable<ReviewDto>>.Success(reviewDtos);
    }
}
