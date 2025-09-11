using FluentValidation;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Validators.ReviewValidator
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.dto.UserName)
                .MinimumLength(5).WithMessage("Username must be at least 5 characters")
              .MaximumLength(100).WithMessage("UserName can't exceed 100 characters")
              .When(x => (x.dto.UserName)!=null);

            RuleFor(x => x.dto.Comment)
                .NotEmpty().WithMessage("Comment is required")
                .MinimumLength(5).WithMessage("Comment must be at least 5 characters")
                .MaximumLength(1000).WithMessage("Comment can't exceed 1000 characters");

            RuleFor(x => x.dto.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10");

            RuleFor(x => x.dto.MovieId)
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
