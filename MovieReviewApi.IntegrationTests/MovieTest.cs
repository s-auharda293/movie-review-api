using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace MovieReviewApi.IntegrationTests
{

    public class MovieTests : IClassFixture<MovieReviewWebApplicationFactory>
    {
        private readonly IMediator _mediator;
        private readonly ITestOutputHelper _output;
        private readonly IServiceScope _scope;

        public MovieTests(MovieReviewWebApplicationFactory factory, ITestOutputHelper output)
        {
            //factory.EnsureDatabase();

            _scope = factory.Services.CreateScope();
            _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
            _output = output;
        }

        public static class MovieTestData {
            public static IEnumerable<object[]> CreateMovie()
            {
                return new List<object[]> {
        new object[]{
            new CreateMovieDto {
                Title = "The Great Adventure",
                Description = "An epic journey of heroes.",
                ReleaseDate = DateTime.Parse("2025-08-01"),
                DurationMinutes = 120,
                Rating = 8,
                ActorIds = new List<Guid> { Guid.Parse("B8CE43E2-5813-4DF9-B27D-68A302AB87FD") }
            }
        },
        new object[]{
            new CreateMovieDto {
                Title = "Mystery Island",
                Description = "A thrilling story on a deserted island.",
                ReleaseDate = DateTime.Parse("2025-05-15"),
                DurationMinutes = 95,
                Rating = 7,
                ActorIds = new List<Guid> { Guid.Parse("587D2C5F-ECE7-45B4-ADD8-7D9FC7BCF249"),
                                            Guid.Parse("42D900E3-FC0D-4CFB-AB92-B07CE2BFFE83")}
            }
        }
       };
            }


        }

        [Theory]
        [MemberData(nameof(MovieTestData.CreateMovie), MemberType = typeof(MovieTestData))]
        public async Task CreateMovie_WithValidData_ReturnsCreatedMovie(CreateMovieDto movieDto)
        {
            // arrange
            var command = new CreateMovieCommand(movieDto);

            // act
            var result = await _mediator.Send(command);

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);

            var movie = result.Value;

            Assert.Equal(movieDto.Title, movie.Title);
            Assert.Equal(movieDto.Description, movie.Description);
            Assert.Equal(movieDto.ReleaseDate, movie.ReleaseDate);
            Assert.Equal(movieDto.DurationMinutes, movie.DurationMinutes);
            Assert.Equal(movieDto.Rating, movie.Rating);
            Assert.NotNull(movie?.Id);

            _output.WriteLine($"Response: {JsonSerializer.Serialize(result)}");
        }


    }

}
