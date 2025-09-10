using MovieReviewApi.Domain.Common;

namespace MovieReviewApi.Domain.Entities
{
    public static class ReviewErrors
    {
        public static readonly Error NotFound = new(
            "Review.NotFound",
            "The requested review was not found.");

        public static readonly Error AlreadyReviewed = new(
            "Review.AlreadyReviewed",
            "This user has already reviewed the movie.");

        public static readonly Error MovieNotFound = new(
            "Review.MovieNotFound",
            "Cannot add review because the movie does not exist.");

        public static readonly Error ReviewsForNonExistentMovie = new(
            "Review.MovieDoesNotExist",
            "Cannot get reviews because the movie does not exist."
        );

    }
}
