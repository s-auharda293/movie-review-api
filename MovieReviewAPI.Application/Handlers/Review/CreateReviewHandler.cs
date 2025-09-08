using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class CreateReviewHandler:IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateReviewHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new MovieReviewApi.Domain.Entities.Review()
            {
                MovieId = request.dto.MovieId,
                UserName = request.dto.UserName,
                Comment = request.dto.Comment,
                Rating = request.dto.Rating
            };

            var movie = await _context.Movies.FirstOrDefaultAsync(m=>m.Id==request.dto.MovieId, cancellationToken);
            if (movie == null)
            {
                throw new ArgumentException("Cannot post review because movie doesn't exist");
            }

            await _context.Reviews.AddAsync(review, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new ReviewDto
            {
                Id = review.Id,
                MovieId = review.MovieId,
                UserName = review.UserName,
                Comment = review.Comment,
                Rating = review.Rating
            };
        }
    }
}

