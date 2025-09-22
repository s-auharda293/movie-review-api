
namespace MovieReviewApi.Domain.Common.Identity
{
    public static class IdentityErrors
    {
        public static readonly Error JWTSecretKeyNotConfigured = new(
            "Identity.JWTSecretKeyNotConfigured",
            "JWT secret key is not configured."
        );

        public static readonly Error EmailAlreadyExists =
            new("Identity.EmailAlreadyExists", "A user with this email already exists.");

        public static readonly Error UserCreationFailed =
            new("Identity.UserCreationFailed", "Failed to create user.");

        public static readonly Error InvalidCredentials =
            new("Identity.InvalidCredentials", "Invalid email or password.");

        public static readonly Error UserNotFound =
            new("Identity.UserNotFound", "User not found.");

        public static readonly Error UserUpdateFailed =
            new("Identity.UserUpdateFailed", "Failed to update user.");

        public static readonly Error InvalidRefreshToken =
            new("Identity.InvalidRefreshToken", "Refresh token is invalid.");

        public static readonly Error RefreshTokenExpired =
            new("Identity.RefreshTokenExpired", "Refresh token has expired.");

        public static readonly Error RevokeRefreshTokenFailed =
            new("Identity.RevokeRefreshTokenFailed", "Failed to revoke refresh token.");

        public static readonly Error UserDeletionFailed =
            new("Identity.UserDeletionFailed", "Failed to delete user.");

        public static readonly Error LoginRequestNull = new(
          "Identity.LoginRequestNull",
          "Login request cannot be null.");

        public static Error UserCreationFailedWithDetails(string details) =>
          new("Identity.UserCreationFailed", $"Failed to create user. Details: {details}");

    }
}
