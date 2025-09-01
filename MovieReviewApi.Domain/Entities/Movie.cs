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
    //public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    //one movie multiple actors
   public ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
