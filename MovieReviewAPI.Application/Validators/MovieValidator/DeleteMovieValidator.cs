using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Movies;

namespace MovieReviewApi.Application.Validators.MovieValidator
{
    public class DeleteMovieValidator : AbstractValidator<DeleteMovieCommand>
    {

        private readonly IApplicationDbContext _context;
        public DeleteMovieValidator(IApplicationDbContext context)
        {

            _context = context;

            RuleFor(x => x.Id)
               .NotEmpty().WithMessage("Movie id is required")
               .MustAsync(async (command, id, ct) => await _context.Movies.AnyAsync(m => m.Id == id, ct)).WithMessage(MovieErrors.NotFound.Description);


        }
    }
}