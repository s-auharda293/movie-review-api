using FluentValidation;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Validators.ReviewValidator
{
    public class PatchReviewValidator : AbstractValidator<PatchReviewDto>
    {
        public PatchReviewValidator()
        {
            RuleFor(x => x.UserName)
              .MinimumLength(5).WithMessage("Username must be at least 5 characters")
              .MaximumLength(100).WithMessage("UserName can't exceed 100 characters")
              .MaximumLength(100).WithMessage("UserName can't exceed 100 characters")
              .When(x => (x.UserName != null));

            RuleFor(x => x.Comment)
                .MinimumLength(5).WithMessage("Comment must be at least 5 characters")
                .MaximumLength(1000).WithMessage("Comment can't exceed 1000 characters")
                .When(x=>x.Comment!=null);

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10")
                .When(x=>(x.Rating).HasValue);

        }
    }
}
