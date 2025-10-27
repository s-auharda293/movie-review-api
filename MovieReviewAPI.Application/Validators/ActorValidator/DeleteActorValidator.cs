using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Common.Actors;
using MovieReviewApi.Domain.Common.Movies;

namespace MovieReviewApi.Application.Validators.MovieValidator
{
    public class DeleteActorValidator : AbstractValidator<DeleteActorCommand>
    {

        private readonly IApplicationDbContext _context;
        public DeleteActorValidator(IApplicationDbContext context)
        {

            _context = context;

            RuleFor(x => x.Id)
               .NotEmpty().WithMessage("Actor id is required")
               .MustAsync(async (command, id, ct) => await _context.Actors.AnyAsync(a => a.Id == id, ct)).WithMessage(ActorErrors.NotFound.Description);


        }
    }
}