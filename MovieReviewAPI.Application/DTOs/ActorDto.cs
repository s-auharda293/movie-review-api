using MovieReviewApi.Domain.Entities;

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

        public string MovieTitlesCache { get; set; } = null!;
        public String? Status { get; set; }
        public DateTime ProposedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }

    }

    public class CreateActorDto : ActorBaseDto
    {
    }

    public class UpdateActorDto : ActorBaseDto
    {
    }

    public class PatchActorDto : ActorBaseDto
    {
        public new string? Name { get; set; }
        public new DateTime? DateOfBirth { get; set; }
        public new string? Bio { get; set; }

    }

    public class ActorRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? SearchColumn { get; set; }
        public string? SearchTerm { get; set; }
    }


    public class ActorWithMoviesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }

        public List<string> MovieTitles { get; set; } = new();

        public String? Status { get; set; }
        public DateTime ProposedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }
    }


    public class ActorResponseDto
    {
        public IEnumerable<ActorWithMoviesDto> Actors { get; set; } = new List<ActorWithMoviesDto>();
        public int TotalCount { get; set; }
    }

    public class ActorRatingDto {
        public string ActorName { get; set; } = null!;
        public decimal AverageRating { get; set; }
    }

    public class ActorReportRequestDto {
        public List<Guid> ActorIds { get; set; } = new();
        public string Format { get; set; } = "pdf";
    }

    public class ActorReportResultDto {
        public byte[]? Content { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }

    }

    public class ActorApprovalRequest
    {
        public Guid ActorId { get; set; }
    }

    public class ActorStatusResponseDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = null!;

        public Guid? ProposedBy { get; set; }
        public DateTime? ProposedAt { get; set; }
        public Guid? StatusChangedBy { get; set; }
        public DateTime? StatusChangedAt { get; set; }
    }



}
