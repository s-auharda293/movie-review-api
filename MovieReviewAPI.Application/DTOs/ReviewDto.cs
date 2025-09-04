using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieReviewApi.Application.DTOs
{
    public abstract class ReviewBaseDto
    {

        [StringLength(100, ErrorMessage = "UserName can't exceed 100 characters")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [StringLength(1000, ErrorMessage = "Comment can't exceed 1000 characters")]
        public string Comment { get; set; } = null!;

        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
        public decimal Rating { get; set; }
    }

    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public string? UserName { get; set; }
        public string Comment { get; set; } = null!;
        public decimal Rating { get; set; }
    }

    public class CreateReviewDto : ReviewBaseDto
    {
        [Required(ErrorMessage = "MovieId is required to create reviews")]
        public Guid MovieId { get; set; }
    }

    public class UpdateReviewDto : ReviewBaseDto
    {
    }

    public class PatchReviewDto
    {
        public string? UserName { get; set; }
        public string? Comment { get; set; }
        public decimal? Rating { get; set; }
    }
}
