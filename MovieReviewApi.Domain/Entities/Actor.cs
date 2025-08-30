using MovieReviewApi.Domain.Common;

namespace MovieReviewApi.Domain.Entities;

public class Actor: BaseEntity
{

    public string Name { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }
    public string? Bio { get; set; }

    //one actor multiple movies
    public List<Movie> Movies { get; set; } = new();


}
