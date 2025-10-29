using MovieReviewApi.Domain.Common;
using MovieReviewApi.Domain.Enums;

namespace MovieReviewApi.Domain.Entities;

public class Movie:BaseEntity
{
    public string Title { get; set; } = null!;

   public string Description { get; set; } = null!;

   public DateTime ReleaseDate { get; set; }

   public int DurationMinutes { get; set; }

   public decimal Rating { get; set; }

    public string? Url { get; set; }

    public string? ActorNamesCache { get; set; }

    //one movie multiple genres
    //public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    //maker-checker
    public ProposalStatus Status { get; set; } = ProposalStatus.Pending;
    public Guid? ProposedBy { get; set; }
    public Guid? ApprovedBy { get; set; }
    public Guid? RejectedBy { get; set; }
    public DateTime? ProposedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }

}
