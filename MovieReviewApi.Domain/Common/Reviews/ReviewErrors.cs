using MovieReviewApi.Domain.Common;

namespace MovieReviewApi.Domain.Entities
{
    public static class ReviewErrors
    {
        public static readonly Error NotFound = new(
            "Reviews.NotFound",
            "The requested review was not found.");

        public static readonly Error AlreadyReviewed = new(
            "Reviews.AlreadyReviewed",
            "This user has already reviewed the movie.");

        public static readonly Error MovieNotFound = new(
            "Reviews.MovieNotFound",
            "Cannot add review because the movie does not exist.");
    }
}
