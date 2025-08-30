using MovieReviewApi.Domain.Common;

namespace MovieReviewApi.Domain.Entities;

public class Movie:BaseEntity
{
    public string Title { get; set; } = null!;

   public string Description { get; set; } = null!;

   public DateTime ReleaseDate { get; set; }

   public int DurationMinutes { get; set; }

   public decimal Rating { get; set; }
    
    //one movie multiple genres
    public List<Genre> Genres { get; set; } = new();

    //one movie multiple actors
   public List<Actor> Actors { get; set; } = new();

   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

   public DateTime UpdatedAt { get; set; }


}
