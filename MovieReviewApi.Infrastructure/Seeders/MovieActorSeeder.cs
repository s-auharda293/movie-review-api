using Bogus;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Infrastructure.Seeders
{
    public class MovieActorSeeder
    {
        public static async Task SeedAsync(IApplicationDbContext context, CancellationToken cancellationToken)
        {
            if (context.Actors.Any() || context.Movies.Any())
            {
                return;
            }

            var faker = new Faker();

            var actors = new List<Actor>();

            for (int i = 0; i < 200; i++)
            {
                actors.Add(new Actor
                {
                    Name = faker.Name.FullName(),
                    DateOfBirth = faker.Date.Past(50, DateTime.Now.AddYears(-20)),
                    Bio = faker.Lorem.Paragraph()
                });
            }

            await context.Actors.AddRangeAsync(actors);
            await context.SaveChangesAsync(cancellationToken);

            var batchSize = 500;
            for (int i = 0; i < 10000; i += batchSize)
            {
                var batch = new List<Movie>();
                for (int j = 0; j < batchSize && i + j < 10000; j++)
                {
                    var movie = new Movie
                    {
                        Title = faker.Lorem.Sentence(10),
                        Description = faker.Lorem.Paragraph(),
                        ReleaseDate = faker.Date.Past(30),
                        DurationMinutes = faker.Random.Int(80, 180),
                        Rating = Math.Round((decimal)faker.Random.Double(0, 10), 1),
                        Actors = actors.OrderBy(x => Guid.NewGuid()).Take(faker.Random.Int(1, 10)).ToList()
                    };
                    batch.Add(movie);
                }

                await context.Movies.AddRangeAsync(batch, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }


        }
    }
}
