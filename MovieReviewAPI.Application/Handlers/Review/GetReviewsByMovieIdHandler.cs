using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Review;

public class GetReviewsByMovieIdHandler : IRequestHandler<GetReviewsByMovieIdQuery, IEnumerable<ReviewDto>>
{
    private readonly IApplicationDbContext _context;

    public GetReviewsByMovieIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReviewDto>> Handle(GetReviewsByMovieIdQuery request, CancellationToken cancellationToken)
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

        return reviewDtos;
    }
}
