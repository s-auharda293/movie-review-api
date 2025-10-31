using Azure.Core;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2021.PowerPoint.Comment;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Review;
using MovieReviewApi.Domain.Entities;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace MovieReviewApi.IntegrationTests
{
    public class ReviewsTest : IClassFixture<MovieReviewWebApplicationFactory>
    {
        private readonly ITestOutputHelper _output;
        private readonly IMediator _mediator;
        private readonly WebApplicationFactory<Program> _factory;

        public ReviewsTest(MovieReviewWebApplicationFactory factory, ITestOutputHelper output)
        {
            var scope = factory.Services.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _output = output;
            _factory = factory;
        }

        public static class TestAuthHelper
        {
            public static string SetupFakeUser(IServiceProvider serviceProvider, Guid userId, string role = UserRoles.Admin)
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

                var testUserId = userId.ToString();

                var claims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, testUserId),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name,"AdminUser")
        };

                var identity = new ClaimsIdentity(claims, "TestAuthType");
                httpContextAccessor.HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                };

                return testUserId;
            }
        }

        private async Task<MovieWithActorsDto> SeedMovieWithActorsAsync()
        {
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

            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userId = TestAuthHelper.SetupFakeUser(services, Guid.NewGuid());

            var actor1 = await _mediator.Send(new CreateActorCommand(actor1Dto));
            var actor2 = await _mediator.Send(new CreateActorCommand(actor2Dto));
            Assert.NotNull(actor1);
            Assert.NotNull(actor2);

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
            Assert.Equal(movieDto.ActorIds.Count, movie.ActorNames.Count);

            return movie!;
        }


        [Fact]
        public async Task GetAllReviews_WhenCalled_ReturnsOk()
        {

            var movie = await SeedMovieWithActorsAsync();

            var loginRequest = new UserLoginRequest { Email = "admin@movies.com", Password = "Admin@123" };

            var loginUserCommand = new LoginUserCommand(loginRequest);

            var user = await _mediator.Send(loginUserCommand);

            var createReview = new CreateReviewDto
            {
                Comment = "Great movie!",
                Rating = 5.5m,
                MovieId = movie.Id
            };


            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userId = TestAuthHelper.SetupFakeUser(services, user.Value!.Id);

            var result = await _mediator.Send(new CreateReviewCommand(createReview));
            _output.WriteLine($"Response: {JsonSerializer.Serialize(result)}");

            Assert.NotNull(result.Value);

            Assert.IsType<ReviewDto>(result.Value);
        }

        [Fact]
        public async Task GetReviewsByMovieId_WithRandomGuid_ReturnsOkOrNotFound()
        {
            var movieId = Guid.NewGuid();
            var response = await _mediator.Send(new GetReviewsByMovieIdQuery(Guid.NewGuid()));
            // Assert
            Assert.True(response.IsFailure);
            Assert.False(response.IsSuccess);
            Assert.Null(response.Value);
            Assert.NotEmpty(response.Errors);

            var error = response.Errors.First();
            Assert.Equal("Review.MovieDoesNotExist", error.Code);
            Assert.Equal("Cannot get reviews because the movie does not exist.", error.Description);
        }

        [Fact]
        public async Task GetReviewsByMovieId_WithInsertedReview_ReturnsFound()
        {
            // Arrange
            var movie = await SeedMovieWithActorsAsync();

            // Act: Create a review using MediatR
            var createReviewDto = new CreateReviewDto
            {
                MovieId = movie.Id,
                Comment = "Great movie!",
                Rating = 5.0m
            };

            var loginRequest = new UserLoginRequest { Email = "admin@movies.com", Password = "Admin@123" };

            var user = await _mediator.Send(new LoginUserCommand(loginRequest));

            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userId = TestAuthHelper.SetupFakeUser(services, user.Value!.Id);

            var createReviewResult = await _mediator.Send(new CreateReviewCommand(createReviewDto));

            _output.WriteLine($"Create review response: {JsonSerializer.Serialize(createReviewResult)}");

            Assert.NotNull(createReviewResult.Value);
            Assert.True(createReviewResult.IsSuccess);

            // Act: Get reviews for the movie
            var getReviewsResult = await _mediator.Send(new GetReviewsByMovieIdQuery(movie.Id));

            _output.WriteLine($"Get reviews response: {JsonSerializer.Serialize(getReviewsResult)}");

            // Assert
            Assert.NotNull(getReviewsResult.Value);
            Assert.True(getReviewsResult.IsSuccess);
            Assert.NotEmpty(getReviewsResult.Value);
            Assert.Contains(getReviewsResult.Value, r => r.Comment == "Great movie!" && r.MovieId == movie.Id);
        }


        [Fact]
        public async Task CreateReview_WithValidData_ReturnsCreated()
        {

            var movie = await SeedMovieWithActorsAsync();
            Assert.NotNull(movie);

            var createReviewCommand = new CreateReviewCommand(new CreateReviewDto
            {
                MovieId = movie.Id,
                Rating = 5,
                Comment = "Great movie!"
            });

            var user = await _mediator.Send(new LoginUserCommand(new UserLoginRequest { Email = "admin@movies.com", Password = "Admin@123" }));

            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userId = TestAuthHelper.SetupFakeUser(services, user.Value!.Id);

            var result = await _mediator.Send(createReviewCommand);

            _output.WriteLine($"Get reviews response: {JsonSerializer.Serialize(result)}");

            Assert.Equal("Great movie!", result.Value!.Comment);
            Assert.Equal(5.0m, result.Value.Rating);

        }

        [Fact]
        public async Task GetReviewById_WithRandomGuid_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var response = await _mediator.Send(new GetReviewByIdQuery(id));
            _output.WriteLine($"Response: {JsonSerializer.Serialize(response)}");
            Assert.False(response.IsSuccess);
            Assert.True(response.IsFailure);
            Assert.Null(response.Value);
            Assert.Single(response.Errors);
            Assert.Equal("Review.NotFound", response.Errors[0].Code);
            Assert.Equal("The requested review was not found.", response.Errors[0].Description);

        }

        [Fact]
        public async Task GetReviewsByUser_WithRandomUserId_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            var response = await _mediator.Send(new GetReviewByIdQuery(userId));
            _output.WriteLine($"Response: {JsonSerializer.Serialize(response)}");
            Assert.False(response.IsSuccess);
            Assert.True(response.IsFailure);
            Assert.Null(response.Value);
            Assert.Single(response.Errors);
            Assert.Equal("Review.NotFound", response.Errors[0].Code);
            Assert.Equal("The requested review was not found.", response.Errors[0].Description);
        }

        [Fact]
        public async Task GetReviewById_WhenReviewExists_ReturnsOk()
        {
            //Arrange
            var movie = await SeedMovieWithActorsAsync();
            Assert.NotNull(movie);

            var createReviewCommand = new CreateReviewCommand(new CreateReviewDto
            {
                MovieId = movie.Id,
                Rating = 5,
                Comment = "Great movie!"
            });

            var user = await _mediator.Send(new LoginUserCommand(new UserLoginRequest { Email = "admin@movies.com", Password = "Admin@123" }));

            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userId = TestAuthHelper.SetupFakeUser(services, user.Value!.Id);

            var result = await _mediator.Send(createReviewCommand);
            var id = result.Value!.Id;

            //Act
            var response = await _mediator.Send(new GetReviewByIdQuery(id));
            _output.WriteLine($"Response: {JsonSerializer.Serialize(response)}");


            //Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Value);
            Assert.NotEqual(Guid.Empty, result.Value.Id);
            Assert.Equal(movie.Id, result.Value.MovieId);
            Assert.Equal("AdminUser", result.Value.UserName);
            Assert.Equal("Great movie!", result.Value.Comment);
            Assert.Equal(5.0m, result.Value.Rating); 
        }

        [Fact]
        public async Task SearchReviews_WithKeyword_ReturnsOk()
        {
            var movie = await SeedMovieWithActorsAsync();
            Assert.NotNull(movie);

            var createReviewCommand = new CreateReviewCommand(new CreateReviewDto
            {
                MovieId = movie.Id,
                Rating = 5,
                Comment = "Great movie!"
            });

            var user = await _mediator.Send(new LoginUserCommand(new UserLoginRequest { Email = "admin@movies.com", Password = "Admin@123" }));

            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userId = TestAuthHelper.SetupFakeUser(services, user.Value!.Id);

            var result = await _mediator.Send(createReviewCommand);

            // Act - search by keyword
            var searchResponse = await _mediator.Send(new SearchReviewsQuery(new ReviewRequestDto { 
                Page=1,
                PageSize=5,
                SearchColumn = "Comment",
                SearchTerm = "movie",
                SortColumn = "Rating",
                SortDirection = "Desc"
            }));

            _output.WriteLine(JsonSerializer.Serialize(searchResponse));

            Assert.True(searchResponse.IsSuccess);
            Assert.False(searchResponse.IsFailure);
            Assert.Empty(searchResponse.Errors);
            Assert.NotNull(searchResponse.Value);
            ReviewDto review = searchResponse.Value.Reviews.ElementAt(0);
            Assert.NotEqual(Guid.Empty, review.Id);
            Assert.Equal(movie.Id, review.MovieId);
            Assert.Equal(Guid.Parse(userId), review.UserId);
            Assert.Equal("AdminUser", review.UserName);
            Assert.Equal("Great movie!", review.Comment);
            Assert.Equal(5.0m, review.Rating);
        }

        [Fact]
        public async Task UpdateReview_WhenReviewExists_ReturnsOk()
        {
            //Arrange
            var movie = await SeedMovieWithActorsAsync();
            Assert.NotNull(movie);

            var createReviewCommand = new CreateReviewCommand(new CreateReviewDto
            {
                MovieId = movie.Id,
                Rating = 5,
                Comment = "Great movie!"
            });

            var user = await _mediator.Send(new LoginUserCommand(new UserLoginRequest { Email = "admin@movies.com", Password = "Admin@123" }));

            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userId = TestAuthHelper.SetupFakeUser(services, user.Value!.Id);

            var result = await _mediator.Send(createReviewCommand);
            Guid reviewId = result.Value!.Id;

            // Act - update review
            var updateReviewResponse = await _mediator.Send(new UpdateReviewCommand(reviewId, new UpdateReviewDto {
                Comment = "Satisfactory",
                Rating = 8.0m
            }));

            var review = updateReviewResponse.Value;

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Value);
            Assert.NotEqual(Guid.Empty, review!.Id);        
            Assert.Equal(movie.Id, review.MovieId);
            Assert.Equal(Guid.Parse(userId), review.UserId);
            Assert.Equal("AdminUser", review.UserName);
            Assert.Equal("Satisfactory", review.Comment);
            Assert.Equal(8.0m, review.Rating);



            _output.WriteLine("Updated Review: "+ JsonSerializer.Serialize(updateReviewResponse));

        }

        [Fact]
        public async Task PatchReview_WhenReviewExists_ReturnsOk()
        {
            //Arrange
            var movie = await SeedMovieWithActorsAsync();
            Assert.NotNull(movie);

            var createReviewCommand = new CreateReviewCommand(new CreateReviewDto
            {
                MovieId = movie.Id,
                Rating = 5,
                Comment = "Great movie!"
            });

            var user = await _mediator.Send(new LoginUserCommand(new UserLoginRequest { Email = "admin@movies.com", Password = "Admin@123" }));

            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userId = TestAuthHelper.SetupFakeUser(services, user.Value!.Id);

            var result = await _mediator.Send(createReviewCommand);
            Guid reviewId = result.Value!.Id;

            // Act - patch review
            var patchResponse = await _mediator.Send(
                new PatchReviewCommand(
                    reviewId,
                    new PatchReviewDto
                    {
                        Comment = "woweee"
                    }
                )
            );

            _output.WriteLine("Patch review response: " + JsonSerializer.Serialize(patchResponse));

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Value);
            var review = patchResponse.Value;
            Assert.Equal("woweee", review!.Comment);
            Assert.Equal(5.0m, review!.Rating);
            Assert.Equal(movie.Id, review!.MovieId);
            Assert.Equal(Guid.Parse(userId), review.UserId);
            Assert.Equal("AdminUser", review.UserName);
        }


        [Fact]
        public async Task DeleteReview_WhenReviewExists_ReturnsNoContent()
        {
            //Arrange
            var movie = await SeedMovieWithActorsAsync();
            Assert.NotNull(movie);

            var createReviewCommand = new CreateReviewCommand(new CreateReviewDto
            {
                MovieId = movie.Id,
                Rating = 5,
                Comment = "Great movie!"
            });

            var user = await _mediator.Send(new LoginUserCommand(new UserLoginRequest { Email = "admin@movies.com", Password = "Admin@123" }));

            var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var userId = TestAuthHelper.SetupFakeUser(services, user.Value!.Id);

            var result = await _mediator.Send(createReviewCommand);
            Guid reviewId = result.Value!.Id;

            // Act - delete review
            var deleteResponse = await _mediator.Send(new DeleteReviewCommand(reviewId));

            _output.WriteLine("Patch review response: " + JsonSerializer.Serialize(deleteResponse));

            Assert.True(deleteResponse.IsSuccess);
            Assert.False(deleteResponse.IsFailure);

            Assert.Empty(result.Errors);
        }
    }
}

