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

        public List<ActorMovieDto> Movies { get; set; } = new();
    }

    public class ActorMovieDto { 
        public Guid Id { get; set; }
        public String Title { get; set; } = null!;
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

    public class ActorRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? Sort { get; set; } // JSON string or a list of objects
        public string? SearchColumn { get; set; }
        public string? SearchTerm { get; set; }
    }

    public class ActorResponseDto
    {
        public IEnumerable<ActorDto> Actors { get; set; } = new List<ActorDto>();
        public int TotalCount { get; set; }
    }

}
