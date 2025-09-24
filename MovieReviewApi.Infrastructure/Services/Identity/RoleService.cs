using Microsoft.AspNetCore.Identity;
using MovieReviewApi.Application.Interfaces.Identity;
using MovieReviewApi.Domain.Common.Identity;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Infrastructure.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> AssignRoleAsync(string userId, string role)
        {

            if (!UserRoles.AllRoles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
                return Result<bool>.Failure(IdentityErrors.InvalidRole);


            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result<bool>.Failure(IdentityErrors.UserNotFound);

            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
                return Result<bool>.Failure(IdentityErrors.RoleAssignmentFailed);

            return Result<bool>.Success(true);
        }
    }
}
