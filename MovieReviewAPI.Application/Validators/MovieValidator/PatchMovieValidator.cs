using FluentValidation;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Validators.MovieValidator
{
    public class PatchMovieValidator : AbstractValidator<PatchMovieDto>
    {
        public PatchMovieValidator(){
              RuleFor(x => x.Title)
                .Length(5, 300).WithMessage("Title must be 5-300 characters")
                .When(x => (x.Title)!=null);

        RuleFor(x => x.Description)
                .MinimumLength(10).WithMessage("Description must be at least 10 characters")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
                .When(x => (x.Description)!=null);

        RuleFor(x => x.ReleaseDate)
                .LessThan(DateTime.UtcNow).WithMessage("Release date must be in the past")
                .When(x => x.ReleaseDate.HasValue);

        RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(1, 600)
                .WithMessage("Duration must be between 1 and 600 minutes")
                .When(x => x.DurationMinutes.HasValue);

        RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10)
                .WithMessage("Rating must be between 0 and 10")
                .When(x => x.Rating.HasValue);

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