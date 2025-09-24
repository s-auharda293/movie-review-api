using System;

namespace MovieReviewApi.Application.DTOs
{
    public abstract class ReviewBaseDto
    {
        public string Comment { get; set; } = null!;
        public decimal Rating { get; set; }
    }

    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string Comment { get; set; } = null!;
        public decimal Rating { get; set; }
    }

    public class CreateReviewDto : ReviewBaseDto
    {
        public Guid MovieId { get; set; }
    }

    public class UpdateReviewDto : ReviewBaseDto
    {
    }

    public class PatchReviewDto
    {
        public string? Comment { get; set; }
        public decimal? Rating { get; set; }
    }
}
