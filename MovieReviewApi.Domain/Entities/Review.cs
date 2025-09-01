using MovieReviewApi.Domain.Common;

namespace MovieReviewApi.Domain.Entities;

public class Review: BaseEntity
{
    public int MovieId { get; set; }     
    public Movie Movie { get; set; } = null!; 

    public string? UserName { get; set; } 
    public string Comment { get; set; } = null!;
    public decimal Rating { get; set; }
}