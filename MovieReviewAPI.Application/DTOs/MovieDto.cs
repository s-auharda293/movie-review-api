using System.ComponentModel.DataAnnotations;
namespace MovieReviewApi.Application.DTOs;


public abstract class MovieBaseDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Title must be 1-150 characters")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string Description { get; set; } = null!;

    public DateTime? ReleaseDate { get; set; }

    [Required(ErrorMessage = "DurationMinutes is required")]
    [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes")]
    public int DurationMinutes { get; set; }

    [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
    public decimal Rating { get; set; }

    public List<int>? ActorIds { get; set; }

}
public class MovieDto
{
    public int Id { get; set; }
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
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Title must be 1-150 characters")]
    public string? Title { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    public DateTime? ReleaseDate { get; set; }

    [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes")]
    public int? DurationMinutes { get; set; }

    [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
    public decimal? Rating { get; set; }

    public List<int>? ActorIds { get; set; }
}
