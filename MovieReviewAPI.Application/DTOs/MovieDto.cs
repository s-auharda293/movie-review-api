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
    public List<MovieActorDto> Actors { get; set; } = new();
}

public class MovieActorDto {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
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

public class MovieRequestDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public string? Sort { get; set; } // JSON string or a list of objects
    public string? SearchColumn { get; set; }
    public string? SearchTerm { get; set; }
}

public class MovieResponseDto {
    public IEnumerable<MovieDto> Movies { get; set; } = new List< MovieDto > ();
    public int TotalCount { get; set; }
}

public class SortDto
{
    public string Field { get; set; } = ""; // Column name
    public string Dir { get; set; } = "asc"; // "asc" or "desc"
}
