using MovieReviewApi.Domain.Common;

namespace MovieReviewApi.Domain.Entities
{
    public static class ReviewErrors
    {
        public static readonly Error NotFound = new(
            "Review.NotFound",
            "The requested review was not found.");

        public static readonly Error UserNotAuthenticated = new(
           "Reviw.UserNotAuthenticated",
           "You must be logged in to create a review."
       );

        public static Error UserNotAuthorized = new(
            "UserNotAuthorized", 
            "You are not authorized to modify this review."
         );


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

        public static readonly Error ReviewsForNonExistentUser = new(
            "Review.UserDoesNotExist",
            "Cannot get reviews because the user hasn't posted any."
        );

    }
}
