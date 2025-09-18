using Microsoft.EntityFrameworkCore;

namespace MovieReviewApi.Application.KeylessEntities;

[Keyless]
public class GetMoviesResult
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Rating { get; set; }

    public string? ActorNames { get; set; }
}
