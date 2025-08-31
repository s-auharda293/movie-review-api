using MovieReviewApi.Domain.Common;

namespace MovieReviewApi.Domain.Entities
{
    public class Genre: BaseEntity
    {
        public string Name { get; set; } = null!;

        //a genre can be in multiple movies e.g Action -> Venom, Fast X, Jumanji...
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
