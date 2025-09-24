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

        /// <summary>
        /// Seed a default admin user using the registration command and assign Admin role.
        /// </summary>
        public static async Task SeedAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            IMediator mediator)
        {
            string adminEmail = "admin@movies.com";

            // Check if admin already exists
            var existingUser = await userManager.FindByEmailAsync(adminEmail);
            if (existingUser != null) return;

            var registerAdminRequest = new UserRegisterRequest
            {
                FirstName = "Admin",
                LastName = "User",
                Email = adminEmail,
                Password = "Admin@123"
            };

            // Use your registration command
            var registerCommand = new RegisterUserCommand(registerAdminRequest);

            var result = await mediator.Send(registerCommand);

            if (result.IsSuccess)
            {
                // Fetch the created user
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                // Assign Admin role
                await userManager.AddToRoleAsync(adminUser!, UserRoles.Admin);
            }
        }
    }
}
