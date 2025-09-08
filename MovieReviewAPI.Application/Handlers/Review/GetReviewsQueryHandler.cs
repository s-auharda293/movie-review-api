using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Handlers.Review
{
    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, IEnumerable<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetReviewsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            {
                var reviews = await _context.Reviews.Include(r => r.Movie).ToListAsync(cancellationToken);

                return reviews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    MovieId = r.MovieId,
                    UserName = r.UserName,
                    Comment = r.Comment,
                    Rating = r.Rating
                }).ToList();
            }
        }
    }
}
