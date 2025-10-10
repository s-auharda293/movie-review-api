using System;

namespace MovieReviewApi.Application.DTOs
{
    public abstract class ReviewBaseDto
    {
        public string? Comment { get; set; } = null!;
        public decimal Rating { get; set; }
    }

    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Comment { get; set; } = null!;
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

    public class ReviewRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public SortDto? Sort { get; set; } // JSON string or a list of objects
        public string? SearchColumn { get; set; }
        public string? SearchTerm { get; set; }
    }

    public class ReviewResponseDto
    {
        public IEnumerable<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
        public int TotalCount { get; set; }
    }
}
