using Bogus;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Domain.Enums;
using MovieReviewApi.Infrastructure.Extensions;
using PuppeteerSharp.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Infrastructure.Seeders
{
    public class MovieActorSeeder
    {
        public static async Task SeedAsync(IApplicationDbContext context, UserManager<ApplicationUser> userManager, IMediator mediator, CancellationToken cancellationToken)
        {

            if (context.Actors.Any() || context.Movies.Any())
            {
                return;
            }

            //string adminId = await IdentityRoleSeeder.SeedUserAsync(userManager, mediator, "seedadmin@movies.com", "SeedAdmin", "User", UserRoles.Admin, "Admin@123");

            string moderatorId = await IdentityRoleSeeder.SeedUserAsync(userManager, mediator, "seedmoderator@movies.com", "SeedModerator", "User", UserRoles.Moderator, "Moderator@123");

            string userId = await IdentityRoleSeeder.SeedUserAsync(userManager, mediator, "seeduser@movies.com", "SeedUser", "User", UserRoles.User, "user@123");

            var faker = new Faker();

            Guid makerId = Guid.Parse(userId); 

            var actors = new List<Actor>();

            for (int i = 0; i < 200; i++)
            {
                var actor = new Actor
                {
                    Name = faker.Name.FullName(),
                    DateOfBirth = faker.Date.Past(50, DateTime.Now.AddYears(-20)),
                    Bio = faker.Lorem.Paragraph(),

                    // Maker-checker fields
                    Status = ProposalStatus.Approved.ToString(),  
                    ProposedBy = makerId,
                    StatusChangedBy = Guid.Parse(moderatorId),
                    ProposedAt = DateTime.UtcNow,
                    StatusChangedAt = DateTime.UtcNow
                };

                actors.Add(actor);
            }

            await context.Actors.AddRangeAsync(actors);
            await context.SaveChangesAsync(cancellationToken);

            // Movies
            var batchSize = 500;
            for (int i = 0; i < 10000; i += batchSize)
            {
                var batch = new List<Movie>();
                for (int j = 0; j < batchSize && i + j < 10000; j++)
                {
                    var movieActors = actors.OrderBy(x => Guid.NewGuid()).Take(faker.Random.Int(1, 10)).ToList();

                    var movie = new Movie
                    {
                        Title = faker.Lorem.Sentence(10).TrimEnd('.'),
                        Description = faker.Lorem.Paragraph(),
                        ReleaseDate = faker.Date.Past(30),
                        DurationMinutes = faker.Random.Int(80, 180),
                        Rating = Math.Round((decimal)faker.Random.Double(0, 10), 1),
                        Actors = movieActors,

                        // Maker-checker fields
                        //Status = ProposalStatus.Approved,
                        //ProposedBy = makerId,
                        //ApprovedBy = Guid.Parse(moderatorId),
                        //ProposedAt = DateTime.UtcNow,
                        //ApprovedAt = DateTime.UtcNow
                    };

                   
                    foreach (var actor in movieActors)
                    {
                        actor.Movies ??= new List<Movie>();
                        actor.Movies.Add(movie);
                        actor.MovieTitlesCache = string.Join(",", actor.Movies.Select(m => m.Title));
                    }

                    movie.ActorNamesCache = string.Join(",", movieActors.Select(a => a.Name));

                    batch.Add(movie);
                }

                await context.Movies.AddRangeAsync(batch, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}