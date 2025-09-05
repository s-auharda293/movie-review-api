using System.ComponentModel.DataAnnotations;
namespace MovieReviewApi.Application.DTOs;


public abstract class MovieBaseDto
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? ReleaseDate { get; set; }

    public int DurationMinutes { get; set; }

    public decimal Rating { get; set; }

    public List<Guid>? ActorIds { get; set; }

}
public class MovieDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Rating { get; set; }
    public List<string> Actors { get; set; } = new();
}

public class CreateMovieDto: MovieBaseDto
{
}

public class UpdateMovieDto: MovieBaseDto
{
}

public class PatchMovieDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public int? DurationMinutes { get; set; }

    public decimal? Rating { get; set; }

    public List<Guid>? ActorIds { get; set; }
}
