using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Validators.MovieValidator
{
    public class CreateMovieValidator: AbstractValidator<CreateMovieCommand>
    {
        private readonly IApplicationDbContext _context;
        public CreateMovieValidator(IApplicationDbContext context) {

            _context = context;

            RuleFor(x => x.dto.Title)
                .NotEmpty().WithMessage("Movie title is required")
                .MinimumLength(5).WithMessage("Movie title must be at least 5 characters")
                .MaximumLength(300).WithMessage("Movie title can't exceed 300 characters")
                .MustAsync(async (title, ct) => !await _context.Movies.AnyAsync(m => m.Title.ToLower().Trim() == title.ToLower().Trim())).WithMessage((command, title) => $"Movie with title '{title}' already exists");

            RuleFor(x=>x.dto.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
            
            RuleFor(x => x.dto.ReleaseDate)
               .LessThan(DateTime.UtcNow).WithMessage("Release date must be in the past")
               .When(x => x.dto.ReleaseDate.HasValue);

            RuleFor(x => x.dto.DurationMinutes)
                .InclusiveBetween(1, 600)
                .WithMessage("Duration must be between 1 and 600 minutes");

            RuleFor(x => x.dto.Rating)
                .InclusiveBetween(0, 10)
                .WithMessage("Rating must be between 0 and 10");

            RuleFor(x => x.dto.ActorIds)
           .Must(HaveValidGuids)
           .WithMessage("All movie IDs must be valid GUIDs")
           .When(x => x.dto.ActorIds != null && x.dto.ActorIds.Any());
        }

        private static bool HaveValidGuids(List<Guid>? movieIds)
        {
            return movieIds?.All(id => id != Guid.Empty) ?? true;
        }
    }
}
