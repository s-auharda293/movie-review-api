using MovieReviewApi.Domain.Common;
using MovieReviewApi.Domain.Enums;

namespace MovieReviewApi.Domain.Entities;

public class Actor: BaseEntity
{

    public string Name { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }
    public string? Bio { get; set; }

    public string? MovieTitlesCache { get; set; }

    public ICollection<Movie>? Movies { get; set; } = new List<Movie>();


    //maker-checker fields
    public String Status { get; set; } = ProposalStatus.Pending.ToString(); 
    public Guid? ProposedBy { get; set; }
    public DateTime? ProposedAt { get; set; } = DateTime.UtcNow;

    public Guid? StatusChangedBy { get; set; }
    public DateTime? StatusChangedAt{ get; set; }

}
