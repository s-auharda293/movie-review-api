using FluentValidation;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Validators.MovieValidator
{
    public class UpdateMovieValidator : AbstractValidator<UpdateMovieDto>
    {
        public UpdateMovieValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Movie title is required")
                .MinimumLength(5).WithMessage("Movie title must be at least 5 characters")
                .MaximumLength(300).WithMessage("Movie title can't exceed 300 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.ReleaseDate)
               .LessThan(DateTime.UtcNow).WithMessage("Release date must be in the past")
               .When(x => x.ReleaseDate.HasValue);

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(1, 600)
                .WithMessage("Duration must be between 1 and 600 minutes");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10)
                .WithMessage("Rating must be between 0 and 10");

            RuleFor(x => x.ActorIds)
           .Must(HaveValidGuids)
           .WithMessage("All movie IDs must be valid GUIDs")
           .When(x => x.ActorIds != null && x.ActorIds.Any());
        }

        private static bool HaveValidGuids(List<Guid>? movieIds)
        {
            return movieIds?.All(id => id != Guid.Empty) ?? true;
        }
    }
}
