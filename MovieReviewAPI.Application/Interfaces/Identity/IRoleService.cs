namespace MovieReviewApi.Application.Interfaces.Identity
{
    public interface IRoleService
    {
        Task<Result<bool>> AssignRoleAsync(string userId, string role);
    }

}
