using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Movie;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MovieReviewApi.IntegrationTests
{
    public class MovieTests : IClassFixture<MovieReviewWebApplicationFactory>
    {
        private readonly IMediator _mediator;
        private readonly ITestOutputHelper _output;

        public MovieTests(MovieReviewWebApplicationFactory factory, ITestOutputHelper output)
        {
            var scope = factory.Services.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _output = output;
        }

        [Fact]
        public async Task CreateMovie_WithValidData_ReturnsCreatedMovie()
        {
            // Arrange - manually insert movie data
            var actor1Dto = new CreateActorDto
            {
                Name = "Actor One",
                DateOfBirth = DateTime.Parse("1980-01-01"),
                Bio = "An experienced actor.",
                MovieIds = null
            };

            var actor2Dto = new CreateActorDto
            {
                Name = "Actor Two",
                DateOfBirth = DateTime.Parse("1990-05-05"),
                Bio = "A versatile actor.",
                MovieIds = null
            };

            var actor1 = await _mediator.Send(new CreateActorCommand(actor1Dto));
            var actor2 = await _mediator.Send(new CreateActorCommand(actor2Dto));

            // Step 2: Create a movie using the seeded actor IDs
            var movieDto = new CreateMovieDto
            {
                Title = "The Great Adventure",
                Description = "An epic journey of heroes.",
                ReleaseDate = DateTime.Parse("2025-08-01"),
                DurationMinutes = 120,
                Rating = 8,
                ActorIds = new List<Guid> { actor1.Value!.Id, actor2.Value!.Id }
            };

            var command = new CreateMovieCommand(movieDto);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);

            var movie = result.Value;

            Assert.Equal(movieDto.Title, movie.Title);
            Assert.Equal(movieDto.Description, movie.Description);
            Assert.Equal(movieDto.ReleaseDate, movie.ReleaseDate);
            Assert.Equal(movieDto.DurationMinutes, movie.DurationMinutes);
            Assert.Equal(movieDto.Rating, movie.Rating);
            Assert.Equal(movieDto.ActorIds.Count, movie.Actors.Count);

            _output.WriteLine($"Response: {JsonSerializer.Serialize(result)}");
        }
    }
}
