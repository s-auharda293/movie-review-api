namespace MovieReviewApi.Domain.Entities
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        public const string User = "User";

    public static readonly string[] AllRoles = { Admin, Moderator, User };
    }


    }
