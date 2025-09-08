namespace MovieReviewApi.Application.DTOs
{
    public abstract class ActorBaseDto
    {
        public string Name { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        public string? Bio { get; set; }

        public List<Guid>? MovieIds { get; set; }

    }

    public class ActorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }
        public string? Bio { get; set; }

        public List<string>? Movies { get; set; } = new();
    }

    public class CreateActorDto: ActorBaseDto
    {
    }

    public class UpdateActorDto: ActorBaseDto 
    {
    }

    public class PatchActorDto: ActorBaseDto
    {
        public new string? Name { get; set; }
        public new DateTime? DateOfBirth { get; set; }
        public new string? Bio { get; set; }

    }
}
