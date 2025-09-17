using MovieReviewApi.Application.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;

namespace MovieReviewApi.IntegrationTests
{
    public class MovieTests : IClassFixture<MovieReviewWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public MovieTests(MovieReviewWebApplicationFactory factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
            
        }

        public class Result<T>
        {
            public bool IsSuccess { get; set; }
            public bool IsFailure { get; set; }
            public T? Value { get; set; }
            public List<string> Errors { get; set; } = new();
        }

        [Fact]
        public async Task GetAllActors_ReturnsAllActors()
        {
            var response = await _client.GetAsync("/api/Actors");
            var content = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Response: {content}");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostActor_ReturnsPostedActor() {
            //arrange
            var actorDto = new ActorDto
            {
                Name = "Test Doe",
                DateOfBirth = DateTime.Parse("1990-05-15T00:00:00Z"),
                Bio = "An actor known for action movies."
                // MovieIds is omitted
            };

            //act
            var response = await _client.PostAsJsonAsync("/api/Actors", actorDto);
            //var createdActor = await response.Content.ReadFromJsonAsync<Result<ActorDto>>();
            ////assert
            //response.EnsureSuccessStatusCode();
            //_output.WriteLine($"Status Code: {response.StatusCode}");

            //Assert.NotNull(createdActor);

            //Assert.Equal(actorDto.Name, createdActor?.Value?.Name);
            //Assert.Equal(actorDto.DateOfBirth, createdActor?.Value?.DateOfBirth);
            //Assert.Equal(actorDto.Bio, createdActor?.Value?.Bio);

            //Assert.NotEqual(Guid.Empty, createdActor?.Value?.Id ?? Guid.Empty);

            //_output.WriteLine($"Response: {JsonSerializer.Serialize(createdActor)}");

            response.EnsureSuccessStatusCode();
        }
    }
}
