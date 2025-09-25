using FluentValidation;
using MovieReviewApi.Application.Commands.Auth;

namespace MovieReviewApi.Application.Validators.ApplicationUserValidator
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Request.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters long")
                .MaximumLength(100).WithMessage("First name can't exceed 100 characters");

            RuleFor(x => x.Request.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters long")
                .MaximumLength(100).WithMessage("Last name can't exceed 100 characters");

            RuleFor(x => x.Request.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Request.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
