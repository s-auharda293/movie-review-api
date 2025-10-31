using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieReviewApi.Application.Commands;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Infrastructure.Extensions
{
    public static class IdentityRoleSeeder
    {
        /// <summary>
        /// Seed default roles if they don't exist.
        /// </summary>
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await roleManager.RoleExistsAsync(UserRoles.Moderator))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));

            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
        }

        public static async Task<string> SeedAdminUserAsync(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            return await SeedUserAsync(userManager, mediator, "admin@movies.com", "Admin", "User", UserRoles.Admin, "Admin@123", true);
        }

        public static async Task<string> SeedModeratorUserAsync(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            return await SeedUserAsync(userManager, mediator, "moderator@movies.com", "Moderator", "User", UserRoles.Moderator, "Moderator@123", true);
        }

        //public static async Task<string> SeedRegularUserAsync(UserManager<ApplicationUser> userManager, IMediator mediator)
        //{
        //    return await SeedUserAsync(userManager, mediator, "user@movies.com", "Regular", "User", UserRoles.User, "User@123");
        //}

        public static async Task<string> SeedUserAsync(
          UserManager<ApplicationUser> userManager,
          IMediator mediator,
          string email,
          string firstName,
          string lastName,
          string role,
          string password,
          bool SkipDefaultRole=false
          )
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null) return existingUser.Id;

            var registerRequest = new UserRegisterRequest
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                SkipDefaultRole = SkipDefaultRole
            };

            var registerCommand = new RegisterUserCommand(registerRequest);
            var result = await mediator.Send(registerCommand);

            if (result.IsSuccess)
            {
                var user = await userManager.FindByEmailAsync(email);
                await userManager.AddToRoleAsync(user!, role);
                return user!.Id;
            }

            throw new Exception($"Failed to seed user '{email}'");
        }
    }
}
