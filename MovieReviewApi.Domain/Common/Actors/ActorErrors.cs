
namespace MovieReviewApi.Domain.Common.Actors
{
    public static class ActorErrors
    {
        public static readonly Error DuplicateActor = new("Actor.DuplicateActor","An actor with the same name already exists.");

        public static readonly Error NotFound = new("Actor.NotFound","The requested actor was not found");
    }
}
