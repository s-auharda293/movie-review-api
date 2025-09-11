using FluentValidation;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;


namespace MovieReviewApi.Application.Validators.ActorValidator;
public class PatchActorValidator : AbstractValidator<PatchActorCommand>
{
    public PatchActorValidator()
    {

        RuleFor(x => x.dto.Name)
            .MinimumLength(2).WithMessage("Name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Name can't exceed 100 characters")
            .When(x => (x.dto.Name)!=null);

        RuleFor(x => x.dto.Bio)
            .MinimumLength(10).WithMessage("Bio must be at least 10 characters")
            .MaximumLength(500).WithMessage("Bio can't exceed 500 characters")
            .When(x=>(x.dto.Bio)!=null);

        RuleFor(x => x.dto.DateOfBirth)
            .Must(BeInThePast)
            .WithMessage("Date of birth must be in the past")
            .When(x => x.dto.DateOfBirth.HasValue);


    }
    private static bool BeInThePast(DateTime? dateOfBirth) {
        return !dateOfBirth.HasValue || dateOfBirth.Value < DateTime.UtcNow;
    }

}