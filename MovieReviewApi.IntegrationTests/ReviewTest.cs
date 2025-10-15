using Azure.Core;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace MovieReviewApi.IntegrationTests
{
    public class ReviewsTest : IClassFixture<MovieReviewWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public ReviewsTest(MovieReviewWebApplicationFactory factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }

        private async Task<MovieDto> SeedMovieWithActorsAsync()
        {
            var actor1Dto = new { dto = new { Name = "Actor One", DateOfBirth = DateTime.Parse("1980-01-01"), Bio = "An experienced actor." } };

            var actor2Dto = new { dto = new { Name = "Actor Two", DateOfBirth = DateTime.Parse("1990-05-05"), Bio = "A versatile actor." } };

            var actor1Response = await _client.PostAsJsonAsync("/api/actors", actor1Dto);
            actor1Response.EnsureSuccessStatusCode();
            using var doc1 = JsonDocument.Parse(await actor1Response.Content.ReadAsStringAsync());
            var actor1Json = doc1.RootElement.GetProperty("value").GetRawText();
            var actor1 = JsonSerializer.Deserialize<ActorDto>(actor1Json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(actor1);

            var actor2Response = await _client.PostAsJsonAsync("/api/actors", actor2Dto);
            actor2Response.EnsureSuccessStatusCode();
            using var doc2 = JsonDocument.Parse(await actor2Response.Content.ReadAsStringAsync());
            var actor2Json = doc2.RootElement.GetProperty("value").GetRawText();
            var actor2 = JsonSerializer.Deserialize<ActorDto>(actor2Json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(actor2);

            var form = new MultipartFormDataContent();
            form.Add(new StringContent("The Great Adventure"), "dto.Title");
            form.Add(new StringContent("An epic journey of heroes."), "dto.Description");
            form.Add(new StringContent("2025-08-01"), "dto.ReleaseDate"); // Date as string
            form.Add(new StringContent("120"), "dto.DurationMinutes");
            form.Add(new StringContent("8"), "dto.Rating");
            form.Add(new StringContent(actor1!.Id.ToString()), "dto.ActorIds[0]");
            form.Add(new StringContent(actor2!.Id.ToString()), "dto.ActorIds[1]");

            // Step 2: send POST request
            var movieResponse = await _client.PostAsync("/api/movies", form);
            movieResponse.EnsureSuccessStatusCode();

            // Step 3: read and parse the response
            var movieResponseJson = await movieResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(movieResponseJson);
            var movieJson = doc.RootElement.GetProperty("value").GetRawText();
            var movie = JsonSerializer.Deserialize<MovieDto>(movieJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(movie);
            return movie!;
        }




        private async Task<(string AccessToken, string UserId)> AuthenticateAndGetTokenAsync()
        {
            // Register user
            var email = $"review_user_{Guid.NewGuid()}@test.com";
            var password = "StrongPass123!";

            var registerRequest = new
            {
                request = new
                {
                    FirstName = "Review",
                    LastName = "Tester",
                    Email = email,
                    Password = password
                }
            };

            await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

            // Login user
            var loginRequest = new
            {
                request = new
                {
                    Email = email,
                    Password = password
                }
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var accessToken = JsonDocument.Parse(loginJson).RootElement
                .GetProperty("value").GetProperty("accessToken").GetString();
            var userId = JsonDocument.Parse(loginJson).RootElement
                .GetProperty("value").GetProperty("id").GetString();

            return (accessToken!, userId!);
        }


        private void AuthorizeClient(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task GetAllReviews_WhenCalled_ReturnsOk()
        {
            var (accessToken, userId) = await AuthenticateAndGetTokenAsync();
            AuthorizeClient(accessToken);

            var movie = await SeedMovieWithActorsAsync();

            var createReview = new
            { 
                dto = new
            {
                MovieId = movie.Id,
                UserId = userId,
                Rating = 5,
                Comment = "Great movie!"
            }
            };

            var createdResponse = await _client.PostAsJsonAsync("/api/reviews", createReview);
            var res = await createdResponse.Content.ReadAsStringAsync();
            _output.WriteLine(await createdResponse.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.Created, createdResponse.StatusCode);

            var response = await _client.GetAsync("/api/reviews");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(jsonString);

            using var doc = JsonDocument.Parse(jsonString);
            var reviewsJson = doc.RootElement.GetProperty("value").GetRawText();

            var reviews = JsonSerializer.Deserialize<List<ReviewDto>>(reviewsJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(reviews);
            Assert.IsType<List<ReviewDto>>(reviews);
        }

        [Fact]
        public async Task GetReviewsByMovieId_WithRandomGuid_ReturnsOkOrNotFound()
        {
            var movieId = Guid.NewGuid();
            var response = await _client.GetAsync($"/api/reviews/by-movie-id?id={movieId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _output.WriteLine($"Response: {response.StatusCode}");
        }

        [Fact]
        public async Task GetReviewsByMovieId_WithInsertedReview_ReturnsFound()
        {
            var (accessToken, userId) = await AuthenticateAndGetTokenAsync();
            AuthorizeClient(accessToken);

            var movie = await SeedMovieWithActorsAsync();

            var createReview = new
            {
                dto = new
                {
                    MovieId = movie.Id,
                    UserId = userId,
                    Rating = 5,
                    Comment = "Great movie!"
                }
            };

            var createdResponse = await _client.PostAsJsonAsync("/api/reviews", createReview);
            var res = await createdResponse.Content.ReadAsStringAsync();
            _output.WriteLine(await createdResponse.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.Created, createdResponse.StatusCode);

            var movieId = JsonDocument.Parse(res).RootElement
            .GetProperty("value").GetProperty("movieId").GetString();

            var response = await _client.GetAsync($"/api/reviews/by-movie-id?id={movieId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _output.WriteLine($"Response: {response.StatusCode}");
        }

        [Fact]
        public async Task CreateReview_WithValidData_ReturnsCreated()
        {
            var (accessToken, userId) = await AuthenticateAndGetTokenAsync();
            AuthorizeClient(accessToken);

            var actor1Dto = new { dto = new { Name = "Actor One", DateOfBirth = DateTime.Parse("1980-01-01"), Bio = "An experienced actor." } };

            var actor2Dto = new { dto = new { Name = "Actor Two", DateOfBirth = DateTime.Parse("1990-05-05"), Bio = "A versatile actor." } };

            var actor1Response = await _client.PostAsJsonAsync("/api/actors", actor1Dto);
            actor1Response.EnsureSuccessStatusCode();
            using var doc1 = JsonDocument.Parse(await actor1Response.Content.ReadAsStringAsync());
            var actor1Json = doc1.RootElement.GetProperty("value").GetRawText();
            var actor1 = JsonSerializer.Deserialize<ActorDto>(actor1Json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(actor1);

            var actor2Response = await _client.PostAsJsonAsync("/api/actors", actor2Dto);
            actor2Response.EnsureSuccessStatusCode();
            using var doc2 = JsonDocument.Parse(await actor2Response.Content.ReadAsStringAsync());
            var actor2Json = doc2.RootElement.GetProperty("value").GetRawText();
            var actor2 = JsonSerializer.Deserialize<ActorDto>(actor2Json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(actor2);

            var form = new MultipartFormDataContent();
            form.Add(new StringContent("The Great Adventure"), "dto.Title");
            form.Add(new StringContent("An epic journey of heroes."), "dto.Description");
            form.Add(new StringContent("2025-08-01"), "dto.ReleaseDate"); // Date as string
            form.Add(new StringContent("120"), "dto.DurationMinutes");
            form.Add(new StringContent("8"), "dto.Rating");
            form.Add(new StringContent(actor1!.Id.ToString()), "dto.ActorIds[0]");
            form.Add(new StringContent(actor2!.Id.ToString()), "dto.ActorIds[1]");

            // Step 2: send POST request
            var movieResponse = await _client.PostAsync("/api/movies", form);
            movieResponse.EnsureSuccessStatusCode();

            // Step 3: read and parse the response
            var movieResponseJson = await movieResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(movieResponseJson);
            var movieJson = doc.RootElement.GetProperty("value").GetRawText();
            var movie = JsonSerializer.Deserialize<MovieDto>(movieJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(movie);

            var createReview = new
            {
                dto = new
                {
                    MovieId = movie.Id,
                    UserId = userId,
                    Rating = 5,
                    Comment = "Great movie!"
                }
            };

            var createdResponse = await _client.PostAsJsonAsync("/api/reviews", createReview);
            var res = await createdResponse.Content.ReadAsStringAsync();
            _output.WriteLine(await createdResponse.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.Created, createdResponse.StatusCode);
        }

        [Fact]
        public async Task GetReviewById_WithRandomGuid_ReturnsOkOrNotFound()
        {
            var id = Guid.NewGuid();
            var response = await _client.GetAsync($"/api/reviews/{id}");
            _output.WriteLine($"Response: {response.StatusCode}");
        }

        [Fact]
        public async Task GetReviewsByUser_WithRandomUserId_ReturnsOkOrNotFound()
        {
            var userId = Guid.NewGuid().ToString();
            var response = await _client.GetAsync($"/api/reviews/user/{userId}");
            _output.WriteLine($"Response: {response.StatusCode}");
        }

        [Fact]
        public async Task GetReviewById_WhenReviewExists_ReturnsOk()
        {
            var (accessToken, userId) = await AuthenticateAndGetTokenAsync();
            AuthorizeClient(accessToken);

            var movie = await SeedMovieWithActorsAsync();

            // Create review first
            var createReview = new
            {
                dto = new
                {
                    MovieId = movie.Id,
                    UserId = userId,
                    Rating = 5,
                    Comment = "Great movie!"
                }
            };
            var createdResponse = await _client.PostAsJsonAsync("/api/reviews", createReview);
            createdResponse.EnsureSuccessStatusCode();
            using var createdDoc = JsonDocument.Parse(await createdResponse.Content.ReadAsStringAsync());
            var reviewJson = createdDoc.RootElement.GetProperty("value").GetRawText();
            var review = JsonSerializer.Deserialize<ReviewDto>(reviewJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            var response = await _client.GetAsync($"/api/reviews/{review!.Id}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(jsonString);
        }

        [Fact]
        public async Task SearchReviews_WithKeyword_ReturnsOk()
        {
            var (accessToken, userId) = await AuthenticateAndGetTokenAsync();
            AuthorizeClient(accessToken);

            var movie = await SeedMovieWithActorsAsync();

            var createReview = new
            {
                dto = new
                {
                    MovieId = movie.Id,
                    UserId = userId,
                    Rating = 5,
                    Comment = "Amazing adventure!"
                }
            };
            await _client.PostAsJsonAsync("/api/reviews", createReview);

            // Act - search by keyword
            var searchRequest = new
            {
                request = new
                {
                    page = 1,
                    pageSize = 5,
                    searchColumn = "Comment",
                    searchTerm = "adventure",
                    sort = new
                    {
                        field = "Rating",
                        dir = "Desc"
                    }
                }
            };



            var response = await _client.PostAsJsonAsync("/api/reviews/query", searchRequest);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(jsonString);

            // Deserialize 'value' property
            using var doc = JsonDocument.Parse(jsonString);
            var reviewsJson = doc.RootElement.GetProperty("value").GetProperty("reviews").GetRawText();
            var reviews = JsonSerializer.Deserialize<List<ReviewDto>>(reviewsJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(reviews);
            Assert.NotEmpty(reviews);
        }

        [Fact]
        public async Task UpdateReview_WhenReviewExists_ReturnsOk()
        {
            var (accessToken, userId) = await AuthenticateAndGetTokenAsync();
            AuthorizeClient(accessToken);

            var movie = await SeedMovieWithActorsAsync();

            // Create review first
            var createReview = new
            {
                dto = new
                {
                    MovieId = movie.Id,
                    UserId = userId,
                    Rating = 4,
                    Comment = "Nice movie"
                }
            };
            var createdResponse = await _client.PostAsJsonAsync("/api/reviews", createReview);
            createdResponse.EnsureSuccessStatusCode();
            using var createdDoc = JsonDocument.Parse(await createdResponse.Content.ReadAsStringAsync());
            var reviewJson = createdDoc.RootElement.GetProperty("value").GetRawText();
            var review = JsonSerializer.Deserialize<ReviewDto>(reviewJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act - update review
            var updateReview = new
            {
                id = review!.Id,
                dto = new
                {
                    Rating = 5,
                    Comment = "Updated review"
                }
            };
            AuthorizeClient(accessToken);

            var updateResponse = await _client.PutAsJsonAsync("/api/reviews", updateReview);
            var updatedJson = await updateResponse.Content.ReadAsStringAsync();
            updateResponse.EnsureSuccessStatusCode();

            _output.WriteLine(updatedJson);
        }

        [Fact]
        public async Task PatchReview_WhenReviewExists_ReturnsOk()
        {
            var (accessToken, userId) = await AuthenticateAndGetTokenAsync();
            AuthorizeClient(accessToken);

            var movie = await SeedMovieWithActorsAsync();

            // Create review first
            var createReview = new
            {
                dto = new
                {
                    MovieId = movie.Id,
                    UserId = userId,
                    Rating = 4,
                    Comment = "Nice movie"
                }
            };
            var createdResponse = await _client.PostAsJsonAsync("/api/reviews", createReview);
            createdResponse.EnsureSuccessStatusCode();
            using var createdDoc = JsonDocument.Parse(await createdResponse.Content.ReadAsStringAsync());
            var reviewJson = createdDoc.RootElement.GetProperty("value").GetRawText();
            var review = JsonSerializer.Deserialize<ReviewDto>(reviewJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act - patch review
            var patchReview = new
            {
                Id = review!.Id,
                dto = new {
                Rating = 5,
                Comment = "Patched comment"
                }
            };
            AuthorizeClient(accessToken);
            var patchResponse = await _client.PatchAsJsonAsync("/api/reviews", patchReview);
            var patchedJson = await patchResponse.Content.ReadAsStringAsync();

            patchResponse.EnsureSuccessStatusCode();

            _output.WriteLine(patchedJson);
        }

        [Fact]
        public async Task DeleteReview_WhenReviewExists_ReturnsNoContent()
        {
            var (accessToken, userId) = await AuthenticateAndGetTokenAsync();
            AuthorizeClient(accessToken);

            var movie = await SeedMovieWithActorsAsync();

            // Create review first
            var createReview = new
            {
                dto = new
                {
                    MovieId = movie.Id,
                    UserId = userId,
                    Rating = 3,
                    Comment = "To be deleted"
                }
            };
            var createdResponse = await _client.PostAsJsonAsync("/api/reviews", createReview);
            createdResponse.EnsureSuccessStatusCode();
            using var createdDoc = JsonDocument.Parse(await createdResponse.Content.ReadAsStringAsync());
            var reviewJson = createdDoc.RootElement.GetProperty("value").GetRawText();
            var review = JsonSerializer.Deserialize<ReviewDto>(reviewJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act - delete review
            var deleteBody = new { Id = review!.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/reviews")
            {
                Content = new StringContent(JsonSerializer.Serialize(deleteBody), Encoding.UTF8, "application/json")
            };
            var deleteResponse = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }




    }
}
