using FluentValidation;
using MovieReviewApi.Application.DTOs;


namespace MovieReviewApi.Application.Validators.ActorValidator;
public class PatchActorValidator : AbstractValidator<PatchActorDto>
{
    public PatchActorValidator()
    {

        RuleFor(x => x.Name)
            .MinimumLength(5).WithMessage("Name must be at least 5 characters")
            .MaximumLength(100).WithMessage("Name can't exceed 100 characters")
            .When(x => (x.Name)!=null);

        RuleFor(x => x.Bio)
            .MinimumLength(10).WithMessage("Bio must be at least 10 characters")
            .MaximumLength(500).WithMessage("Bio can't exceed 500 characters")
            .When(x=>(x.Bio)!=null);

        RuleFor(x => x.DateOfBirth)
            .Must(BeInThePast)
            .WithMessage("Date of birth must be in the past")
            .When(x => x.DateOfBirth.HasValue);


    }
    private static bool BeInThePast(DateTime? dateOfBirth) {
        return !dateOfBirth.HasValue || dateOfBirth.Value < DateTime.UtcNow;
    }

}