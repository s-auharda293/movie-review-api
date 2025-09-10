using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
{
    private readonly IApplicationDbContext _context;

    public CreateReviewHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == request.dto.MovieId, cancellationToken);
        if (movie == null)
            return Result<ReviewDto>.Failure(ReviewErrors.MovieNotFound);

        var review = new Review
        {
            MovieId = request.dto.MovieId,
            UserName = request.dto.UserName,
            Comment = request.dto.Comment,
            Rating = request.dto.Rating
        };

        await _context.Reviews.AddAsync(review, cancellationToken);
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
