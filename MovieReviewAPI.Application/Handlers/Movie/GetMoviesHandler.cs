using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Actor;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetMoviesHandler: IRequestHandler<GetMoviesQuery, IEnumerable<MovieDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetMoviesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken) {
            var movies = await _context.Movies.Include(m => m.Actors).ToListAsync(cancellationToken);
            return movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                DurationMinutes = m.DurationMinutes,
                Rating = m.Rating,
                Actors = m.Actors?.Select(a => a.Name).ToList() ?? new List<string>(),
            }).ToList();
        }
    }
}