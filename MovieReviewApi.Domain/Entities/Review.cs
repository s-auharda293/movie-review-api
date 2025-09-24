using MovieReviewApi.Domain.Common;

namespace MovieReviewApi.Domain.Entities;

public class Review: BaseEntity
{
    public Guid MovieId { get; set; }     
    public Movie Movie { get; set; } = null!; 
    public string Comment { get; set; } = null!;
    public decimal Rating { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}