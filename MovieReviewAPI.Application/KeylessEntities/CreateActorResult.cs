using Microsoft.EntityFrameworkCore;


namespace MovieReviewApi.Application.KeylessEntities.CreateActorResult
{
    [Keyless]
    public class CreateActorResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

       
    }
}

