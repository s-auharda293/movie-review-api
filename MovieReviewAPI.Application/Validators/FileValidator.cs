using FluentValidation;
using Microsoft.AspNetCore.Http;
namespace MovieReviewApi.Application.Validators
{
    public class FileValidator: AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("File is required")
                .Must(f => f!.Length <= 20 * 1024)
                .WithMessage("File size cannot exceed 20 KB")
                .Must(f => f!.ContentType == "image/jpeg" || f!.ContentType == "image/png")
                .WithMessage("Only JPEG or PNG images are allowed");

        }
    }
}
