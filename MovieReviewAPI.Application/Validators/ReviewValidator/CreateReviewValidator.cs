using FluentValidation;
using MovieReviewApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Validators.ReviewValidator
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.UserName)
                .MinimumLength(5).WithMessage("Username must be at least 5 characters")
              .MaximumLength(100).WithMessage("UserName can't exceed 100 characters")
              .When(x => (x.UserName)!=null);

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required")
                .MinimumLength(5).WithMessage("Comment must be at least 5 characters")
                .MaximumLength(1000).WithMessage("Comment can't exceed 1000 characters");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10");

            RuleFor(x => x.MovieId)
               .NotEmpty()
               .Must(IsValidGuid)
               .WithMessage("MovieId is not valid Guid");

        }

        private static bool IsValidGuid(Guid id)
        {
            return id != Guid.Empty;
        }
    }
}
