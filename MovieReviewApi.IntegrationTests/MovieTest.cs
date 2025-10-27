using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Movie;
using MovieReviewApi.Domain.Entities;
using System.Text.Json;
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

        //happy path tests
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
            Assert.Equal(movieDto.ActorIds.Count, movie.ActorNames.Count);

            _output.WriteLine($"Response: {JsonSerializer.Serialize(result)}");
        }
    

        [Fact]
        public async Task CreateMovie_WithFileUpload_ReturnsCreatedMovieWithFileUrl()
        {
            // Arrange
            if (!Directory.Exists("TestFiles")) {
                Directory.CreateDirectory("TestFiles");
            }

            var filePath = "TestFiles/samplePoster.jpg";
            await File.WriteAllBytesAsync(filePath, new byte[] { 1, 2, 3, 4 });

            var stream = File.OpenRead(filePath);
            var formFile = new FormFile(stream, 0, stream.Length, "PosterFile", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var movieDto = new CreateMovieDto
            {
                Title = "Interstellar",
                Description = "A journey through space and time.",
                ReleaseDate = DateTime.Parse("2014-11-07"),
                DurationMinutes = 169,
                Rating = 9,
                File = formFile
            };

            var command = new CreateMovieCommand(movieDto);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Interstellar", result.Value!.Title);
            Assert.Equal("A journey through space and time.", result.Value.Description);
            Assert.Equal(169, result.Value.DurationMinutes);
            Assert.Equal(9, result.Value.Rating);
            Assert.Equal(DateTime.Parse("2014-11-07"), result.Value.ReleaseDate.Date);
            _output.WriteLine($"Result: {JsonSerializer.Serialize(result)}");
        }

        [Fact]
        public async Task UpdateMovie_WithValidData_UpdatesMovie()
        {
            // Arrange - first create a movie
            var actor1 = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Actor One",
                DateOfBirth = DateTime.Parse("1980-01-01"),
                Bio = "Experienced actor"
            }));

            var actor2 = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Actor Two",
                DateOfBirth = DateTime.Parse("1982-02-02"),
                Bio = " Second Experienced actor"
            }));

            Assert.True(actor1.IsSuccess, "Actor1 creation success");
            Assert.True(actor2.IsSuccess, "Actor2 creation success");
            Assert.NotNull(actor1.Value);

            var createMovieDto = new CreateMovieDto
            {
                Title = "Original Title",
                Description = "Original Description",
                ReleaseDate = DateTime.Parse("2025-08-01"),
                DurationMinutes = 120,
                Rating = 7,
                ActorIds = new List<Guid> { actor1.Value!.Id,actor2.Value!.Id }
            };

            var movie = await _mediator.Send(new CreateMovieCommand(createMovieDto));

            Assert.True(movie.IsSuccess, "Movie creation success");
            Assert.NotNull(movie.Value);

            if (!Directory.Exists("TestFiles"))
            {
                Directory.CreateDirectory("TestFiles");
            }

            var filePath = $"TestFiles/{Guid.NewGuid()}-samplePoster.jpg";
            await File.WriteAllBytesAsync(filePath, new byte[] { 1, 2, 3, 4 });

            var stream = File.OpenRead(filePath);
            var formFile = new FormFile(stream, 0, stream.Length, "PosterFile", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Prepare updated data
            var updateCommand = new UpdateMovieCommand(
                movie.Value!.Id,
                 new UpdateMovieDto
                 {
                     Title = "Updated Title",
                     Description = "Updated Description",
                     ReleaseDate = DateTime.Parse("2025-08-01"),
                     DurationMinutes = 125,
                     Rating = 9,
                     ActorIds = new List<Guid> { actor1.Value!.Id },
                     File = formFile
                 }
            );

            // Act
            var result = await _mediator.Send(updateCommand);

            // Assert
            Assert.True(result.IsSuccess);
            var updatedMovie = result.Value!;
            Assert.Equal("Updated Title", updatedMovie.Title);
            Assert.Equal("Updated Description", updatedMovie.Description);
            Assert.Equal(125, updatedMovie.DurationMinutes);
            Assert.Equal(9, updatedMovie.Rating);

            _output.WriteLine($"Result: {JsonSerializer.Serialize(result)}");
        }

        [Fact]
        public async Task PatchMovie_WithPartialData_UpdatesOnlySpecifiedFields()
        {
            // Arrange - create actors
            var actor1 = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Patch Actor One",
                DateOfBirth = DateTime.Parse("1985-05-05"),
                Bio = "Patch Actor"
            }));

            Assert.True(actor1.IsSuccess, "Actor creation success");
            Assert.NotNull(actor1.Value);

            var createMovieDto = new CreateMovieDto
            {
                Title = "Patch Original Title",
                Description = "Patch Original Description",
                ReleaseDate = DateTime.Parse("2025-08-01"),
                DurationMinutes = 110,
                Rating = 6,
                ActorIds = new List<Guid> { actor1.Value!.Id }
            };

            var movie = await _mediator.Send(new CreateMovieCommand(createMovieDto));
            Assert.True(movie.IsSuccess, "Movie creation success");
            Assert.NotNull(movie.Value);

            // Optional: create a test file for patching
            if (!Directory.Exists("TestFiles"))
                Directory.CreateDirectory("TestFiles");

            var filePath = $"TestFiles/{Guid.NewGuid()}-patchPoster.jpg";
            await File.WriteAllBytesAsync(filePath, new byte[] { 1, 2, 3, 4 });

            var stream = File.OpenRead(filePath);
            var formFile = new FormFile(stream, 0, stream.Length, "PosterFile", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Prepare patch data - only updating Title and Poster
            var patchCommand = new PatchMovieCommand(
                movie.Value!.Id,
                new PatchMovieDto
                {
                    Title = "Patch Updated Title",
                    File = formFile
                }
            );

            // Act
            var result = await _mediator.Send(patchCommand);

            // Assert
            Assert.True(result.IsSuccess, "Patch command succeeded");
            var patchedMovie = result.Value!;
            Assert.Equal("Patch Updated Title", patchedMovie.Title);
            Assert.Equal("Patch Original Description", patchedMovie.Description); 
            Assert.Equal(110, patchedMovie.DurationMinutes); 
            Assert.Equal(6, patchedMovie.Rating);
            Assert.Single(patchedMovie.ActorNames);

            _output.WriteLine($"Patched Movie Result: {JsonSerializer.Serialize(result)}");
        }

        [Fact]
        public async Task PatchMovie_WithActorIds_UpdatesOnlySpecifiedFields()
        {
            // Arrange - create actors
            var actor1 = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Patch Actor One",
                DateOfBirth = DateTime.Parse("1985-05-05"),
                Bio = "Patch Actor One"
            }));

            var actor2 = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Patch Actor Two",
                DateOfBirth = DateTime.Parse("1985-05-05"),
                Bio = "Patch Actor Two"
            }));

            Assert.True(actor1.IsSuccess, "Actor creation success");
            Assert.NotNull(actor1.Value);


            Assert.True(actor2.IsSuccess, "Actor creation success");
            Assert.NotNull(actor2.Value);

            var createMovieDto = new CreateMovieDto
            {
                Title = "Patch Original Title",
                Description = "Patch Original Description",
                ReleaseDate = DateTime.Parse("2025-08-01"),
                DurationMinutes = 110,
                Rating = 6,
                ActorIds = new List<Guid> { actor1.Value!.Id }
            };

            var movie = await _mediator.Send(new CreateMovieCommand(createMovieDto));
            Assert.True(movie.IsSuccess, "Movie creation success");
            Assert.NotNull(movie.Value);



            // Prepare patch data - only updating Title and Poster
            var patchCommand = new PatchMovieCommand(
                movie.Value!.Id,
                new PatchMovieDto
                {
                    ActorIds = new List<Guid> { actor2.Value!.Id }
                }
            );

            // Act
            var result = await _mediator.Send(patchCommand);

            // Assert
            Assert.True(result.IsSuccess, "Patch command succeeded");
            var patchedMovie = result.Value!;
            Assert.Equal("Patch Original Title", patchedMovie.Title);
            Assert.Equal("Patch Original Description", patchedMovie.Description);
            Assert.Equal(110, patchedMovie.DurationMinutes);
            Assert.Equal(6, patchedMovie.Rating);
            Assert.Single(patchedMovie.ActorNames);

            _output.WriteLine($"Patched Movie Result: {JsonSerializer.Serialize(result)}");
        }

        [Fact]
        public async Task DeleteMovie_WithValidId_DeletesMovieSuccessfully()
        {
            // Arrange 
            var actor1 = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Actor One",
                DateOfBirth = DateTime.Parse("1980-01-01"),
                Bio = "Experienced actor"
            }));

            var actor2 = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Actor Two",
                DateOfBirth = DateTime.Parse("1982-02-02"),
                Bio = "Another experienced actor"
            }));

            Assert.True(actor1.IsSuccess && actor2.IsSuccess);
            Assert.NotNull(actor1.Value);
            Assert.NotNull(actor2.Value);

            
            var createMovieDto = new CreateMovieDto
            {
                Title = "Movie To Delete",
                Description = "This movie will be deleted in the test.",
                ReleaseDate = DateTime.Parse("2025-08-01"),
                DurationMinutes = 120,
                Rating = 7,
                ActorIds = new List<Guid> { actor1.Value!.Id, actor2.Value!.Id }
            };

            var movieResult = await _mediator.Send(new CreateMovieCommand(createMovieDto));
            Assert.True(movieResult.IsSuccess);
            Assert.NotNull(movieResult.Value);

            var movieId = movieResult.Value!.Id;

            // Act - delete the movie
            var deleteResult = await _mediator.Send(new DeleteMovieCommand(movieId));

            // Assert
            Assert.True(deleteResult.IsSuccess, "Movie deletion should succeed");

            // Optional: verify movie no longer exists
            var getResult = await _mediator.Send(new GetMovieByIdQuery(movieId));
            Assert.False(getResult.IsSuccess, "Movie should no longer exist after deletion");

            _output.WriteLine($"Deleted Movie ID: {movieId}");
        }

        [Fact]
        public async Task GetMovieById_WithValidId_ReturnsMovie()
        {
            // Arrange 
            var actor = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Actor One",
                DateOfBirth = DateTime.Parse("1980-01-01"),
                Bio = "Experienced actor"
            }));
            Assert.True(actor.IsSuccess && actor.Value != null);

            if (!Directory.Exists("TestFiles"))
            {
                Directory.CreateDirectory("TestFiles");
            }

            var filePath = "TestFiles/samplePoster.jpg";
            await File.WriteAllBytesAsync(filePath, new byte[] { 1, 2, 3, 4 });

            var stream = File.OpenRead(filePath);
            var formFile = new FormFile(stream, 0, stream.Length, "PosterFile", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var createMovieDto = new CreateMovieDto
            {
                Title = "Movie By ID",
                Description = "Test movie for GetMovieById",
                ReleaseDate = DateTime.Parse("2025-08-01"),
                DurationMinutes = 120,
                Rating = 8,
                ActorIds = new List<Guid> { actor.Value!.Id },
                File = formFile
            };

            var movieResult = await _mediator.Send(new CreateMovieCommand(createMovieDto));
            Assert.True(movieResult.IsSuccess && movieResult.Value != null);

            var movieId = movieResult.Value!.Id;

            // Act
            var getResult = await _mediator.Send(new GetMovieByIdQuery(movieId));

            // Assert
            Assert.True(getResult.IsSuccess);
            var movie = getResult.Value!;
            Assert.Equal(createMovieDto.Title, movie.Title);
            Assert.Equal(createMovieDto.Description, movie.Description);
            Assert.Single(movie.ActorNames);

            _output.WriteLine($"Retrieved Movie: {JsonSerializer.Serialize(movie)}");
        }

        [Fact]
        public async Task GetAllMovies_ReturnsListOfMovies()
        {
            // Arrange 
            var actor = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Actor One",
                DateOfBirth = DateTime.Parse("1980-01-01"),
                Bio = "Experienced actor"
            }));
            Assert.True(actor.IsSuccess && actor.Value != null);

            if (!Directory.Exists("TestFiles"))
            {
                Directory.CreateDirectory("TestFiles");
            }

            var filePath = "TestFiles/samplePoster.jpg";
            await File.WriteAllBytesAsync(filePath, new byte[] { 1, 2, 3, 4 });

            var stream = File.OpenRead(filePath);
            var formFile = new FormFile(stream, 0, stream.Length, "PosterFile", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };


            var movie1Dto = new CreateMovieDto
            {
                Title = "Movie One",
                Description = "First test movie",
                ReleaseDate = DateTime.Parse("2025-08-01"),
                DurationMinutes = 100,
                Rating = 7,
                ActorIds = new List<Guid> { actor.Value!.Id },
                File = formFile
            };
            var movie2Dto = new CreateMovieDto
            {
                Title = "Movie Two",
                Description = "Second test movie",
                ReleaseDate = DateTime.Parse("2025-09-01"),
                DurationMinutes = 110,
                Rating = 8,
                ActorIds = new List<Guid> { actor.Value!.Id },
                File = formFile
            };

            await _mediator.Send(new CreateMovieCommand(movie1Dto));
            await _mediator.Send(new CreateMovieCommand(movie2Dto));

            var requestDto = new MovieRequestDto
            {
                Page = 1,
                PageSize = 2,
            };

            var query = new SearchMoviesQuery(requestDto);

            // Act
            var allMoviesResult = await _mediator.Send(query);

            // Assert query succeeded
            Assert.True(allMoviesResult.IsSuccess, "GetMoviesQuery should succeed");

            // Get the actual list
            var allMovies = allMoviesResult.Value!;
            Assert.NotNull(allMovies);
            Assert.True(allMovies.Movies.Any(), "Movies list should not be empty");
            Assert.Contains(allMovies.Movies, m => m.Title == "Movie One");
            Assert.Contains(allMovies.Movies, m => m.Title == "Movie Two");

            _output.WriteLine($"All Movies: {JsonSerializer.Serialize(allMovies)}");

        }

        [Fact]
        public async Task SearchMovies_WithPaginationAndSearch_ReturnsCorrectResults()
        {
            // Arrange: create some movies
            var actor = await _mediator.Send(new CreateActorCommand(new CreateActorDto
            {
                Name = "Search Actor",
                DateOfBirth = DateTime.Parse("1980-01-01"),
                Bio = "Actor for search test"
            }));
            Assert.True(actor.IsSuccess && actor.Value != null);

            if (!Directory.Exists("TestFiles"))
                Directory.CreateDirectory("TestFiles");

            var filePath = "TestFiles/samplePoster.jpg";
            await File.WriteAllBytesAsync(filePath, new byte[] { 1, 2, 3, 4 });
            var stream = File.OpenRead(filePath);
            var formFile = new FormFile(stream, 0, stream.Length, "PosterFile", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Create multiple movies for pagination
            var movieTitles = new[] { "Avengers", "Batman", "Cinderella", "Dune", "Eternals", "Frozen" };
            foreach (var title in movieTitles)
            {
                await _mediator.Send(new CreateMovieCommand(new CreateMovieDto
                {
                    Title = title,
                    Description = "Test movie",
                    ReleaseDate = DateTime.Parse("2025-08-01"),
                    DurationMinutes = 120,
                    Rating = 7,
                    ActorIds = new List<Guid> { actor.Value!.Id },
                    File = formFile
                }));
            }

            // Prepare Search query - paginate 2 per page, page 2, search for titles containing "e"
            var requestDto = new MovieRequestDto
            {
                Page = 2,
                PageSize = 2,
                SearchColumn = "Title",
                SearchTerm = "e",
                SortColumn="Title",
                SortDirection="asc"
            };

            var query = new SearchMoviesQuery(requestDto);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.True(result.IsSuccess);
            var movies = result.Value!;
            Assert.NotNull(movies);
            Assert.True(movies.TotalCount >= requestDto.PageSize, "Movies count should respect PageSize");

            //// All returned movies should contain "e" in the title
            Assert.All(movies.Movies, m => Assert.Contains("e", m.Title, StringComparison.OrdinalIgnoreCase));

            //// Verify sorting: titles are ascending
            var sortedTitles = movies.Movies.Select(m => m.Title).OrderBy(t => t).ToList();
            Assert.Equal(sortedTitles, movies.Movies.Select(m => m.Title).ToList());

            _output.WriteLine($"SearchMovies Result (Page {requestDto.Page}): {JsonSerializer.Serialize(movies)}");
        }



        //unhappy path tests
        [Fact]
        public async Task CreateMovie_WithInvalidData_ReturnsFailure()
        {
            // Arrange - missing title
            var movieDto = new CreateMovieDto
            {
                Title = "", 
                Description = "Description",
                ReleaseDate = DateTime.Parse("2025-08-01"),
                DurationMinutes = 100,
                Rating = 5,
                ActorIds = new List<Guid>() // no actors
            };

            var command = new CreateMovieCommand(movieDto);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Errors);

            Assert.Contains(result.Errors, e => e.Code == "dto.Title" && e.Description.Contains("required"));
            Assert.Contains(result.Errors, e => e.Code == "dto.Title" && e.Description.Contains("at least 5 characters"));

            _output.WriteLine($"CreateMovie Failure Result: {JsonSerializer.Serialize(result.Errors)}");
        }

        [Fact]
        public async Task UpdateMovie_WithInvalidId_ReturnsFailure()
        {
            // Arrange
            var updateDto = new UpdateMovieDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                DurationMinutes = 120,
                Rating = 8,
                ActorIds = new List<Guid>()
            };
            var command = new UpdateMovieCommand(Guid.NewGuid(), updateDto); 

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal("Id",result.Errors[0].Code);
            Assert.Equal("The requested movie was not found", result.Errors[0].Description);
            _output.WriteLine($"UpdateMovie Failure Result: {JsonSerializer.Serialize(result.Errors)}");
        }

        [Fact]
        public async Task PatchMovie_WithInvalidId_ReturnsFailure()
        {
            // Arrange
            var patchDto = new PatchMovieDto
            {
                Title = "Patched Title"
            };
            var command = new PatchMovieCommand(Guid.NewGuid(), patchDto); 

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal("Id",result.Errors[0].Code);
            Assert.Equal("The requested movie was not found", result.Errors[0].Description);
            _output.WriteLine($"PatchMovie Failure Result: {JsonSerializer.Serialize(result.Errors)}");
        }

        [Fact]
        public async Task DeleteMovie_WithInvalidId_ReturnsFailure()
        {
            // Arrange
            var invalidMovieId = Guid.NewGuid();
            var command = new DeleteMovieCommand(invalidMovieId);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);

            Assert.Equal("The requested movie was not found", result.Errors[0].Description);
            _output.WriteLine($"Delete Failure Result: {JsonSerializer.Serialize(result.Errors)}");

        }

        [Fact]
        public async Task CreateMovie_WithDuplicateTitle_ReturnsFailure()
        {
            // Arrange
            var originalDto = new CreateMovieDto
            {
                Title = "Original Movie",
                Description = "Original Description",
                DurationMinutes = 120,
                Rating = 7
            };
            var originalCommand = new CreateMovieCommand(originalDto);

            // First, create the original movie
            var firstResult = await _mediator.Send(originalCommand);
            Assert.True(firstResult.IsSuccess);

            // Now try to create another movie with the same title
            var duplicateDto = new CreateMovieDto
            {
                Title = "Original Movie",
                Description = "Same title as existing movie",
                DurationMinutes = 100,
                Rating = 8
            };
            var duplicateCommand = new CreateMovieCommand(duplicateDto);

            // Act
            var result = await _mediator.Send(duplicateCommand);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Contains(result.Errors, e => e.Description.Contains("already exists"));
            _output.WriteLine($"CreateMovie Failure Result: {JsonSerializer.Serialize(result)}");
        }
    }
}
