using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class DeleteMovieHandler:IRequestHandler<DeleteMovieCommand,bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteMovieHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteMovieCommand request, CancellationToken cancellationToken) {

            var movie = await _context.Movies.FirstOrDefaultAsync(m=>m.Id == request.Id);
            
            if (movie == null) return false;

            if (movie.Actors != null && movie.Actors.Any())
            {
                movie.Actors.Clear();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync(cancellationToken);

            return true;

        }
    }
}
