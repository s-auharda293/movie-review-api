using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class PatchMovieHandler:IRequestHandler<PatchMovieCommand,bool>
    {
        private readonly IApplicationDbContext _context;

        public PatchMovieHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(PatchMovieCommand request, CancellationToken cancellationToken) {
            var movie = await _context.Movies.FirstOrDefaultAsync(m=>m.Id==request.Id);
            if (movie == null) return false;

            if (!string.IsNullOrEmpty(request.dto.Title))
                movie.Title = request.dto.Title;

            if (!string.IsNullOrEmpty(request.dto.Description))
                movie.Description = request.dto.Description;

            if (request.dto.ReleaseDate.HasValue)
                movie.ReleaseDate = request.dto.ReleaseDate.Value;

            if (request.dto.DurationMinutes.HasValue)
                movie.DurationMinutes = request.dto.DurationMinutes.Value;

            if (request.dto.Rating.HasValue)
                movie.Rating = request.dto.Rating.Value;

            if (request.dto.ActorIds != null && request.dto.ActorIds.Any())
            {
                var actors = await _context.Actors.Where(a=>request.dto.ActorIds.Contains(a.Id)).ToListAsync(cancellationToken);
                if (actors.ToList().Count != request.dto.ActorIds.Count)
                {
                    var invalidIds = request.dto.ActorIds.Except(actors.Select(a => a.Id).ToList());
                    throw new ArgumentException($"One or more actors with Ids {string.Join(", ", invalidIds)} do not exist.");
                }
                movie.Actors = actors.ToList();
            }

             _context.Movies.Update(movie);
            await _context.SaveChangesAsync(cancellationToken);

            return true;

        }
    }
}
