using FluentValidation;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Validators.ReviewValidator
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewValidator()
        {

            RuleFor(x => x.dto.Comment)
                 .MinimumLength(5).WithMessage("Comment must be at least 5 characters")
                .MaximumLength(1000).WithMessage("Comment can't exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.dto.Comment));

            RuleFor(x => x.dto.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10");


        }
    }
}
