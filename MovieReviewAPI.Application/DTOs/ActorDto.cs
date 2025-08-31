using System.ComponentModel.DataAnnotations;
namespace MovieReviewApi.Application.DTOs
{
    public abstract class ActorBaseDto
    {
        [Required(ErrorMessage = "Actor name is required")]
        [StringLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = null!;

        [DataType(DataType.Date)]
        [CustomValidation(typeof(ActorBaseDto), nameof(ValidateDateOfBirth))]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "Bio can't exceed 500 characters")]
        public string? Bio { get; set; }

        public List<int>? MovieIds { get; set; }

        public static ValidationResult? ValidateDateOfBirth(DateTime? dob, ValidationContext context)
        {
            if (dob.HasValue && dob.Value >= DateTime.UtcNow)
                return new ValidationResult("Date of birth must be in the past");
            return ValidationResult.Success;
        }

    }

    public class ActorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }
        public string? Bio { get; set; }

        public List<int>? MovieIds { get; set; } = new();
    }

    public class CreateActorDto: ActorBaseDto
    {
    }

    public class UpdateActorDto: ActorBaseDto 
    {
    }

    public class PatchActorDto
    {
        public string? Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Bio { get; set; }
    }
}
