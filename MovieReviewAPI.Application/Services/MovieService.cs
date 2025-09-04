using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Infrastructure.Repositories;

namespace MovieReviewApi.Application.Services;


public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;
    private readonly IActorRepository _actorRepository;

    public MovieService(IMovieRepository repository, IActorRepository actorRepository) {
        _repository = repository;
        _actorRepository = actorRepository; 
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        var movies = await _repository.GetAllAsync();
        return movies.Select(m => new MovieDto
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            ReleaseDate = m.ReleaseDate,
            DurationMinutes = m.DurationMinutes,
            Rating = m.Rating,
            Actors = m.Actors.Select(a=>a.Name).ToList(),
        }).ToList();
    }


    public async Task<MovieDto?> GetMovieByIdAsync(Guid id)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null) return null;
        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseDate = movie.ReleaseDate,
            DurationMinutes = movie.DurationMinutes,
            Rating = movie.Rating,
            Actors = movie.Actors.Select(a => a.Name).ToList(),
        };
    }

    public async Task<MovieDto> CreateMovieAsync(CreateMovieDto movieDto)
    {
        var movie = new Movie
        {
            Title=movieDto.Title,
            Description=movieDto.Description,  
            ReleaseDate=movieDto.ReleaseDate??DateTime.UtcNow,
            DurationMinutes = movieDto.DurationMinutes, 
            Rating = movieDto.Rating, 
        };

        if (movieDto.ActorIds != null && movieDto.ActorIds.Any()) { 
            var actors = await _actorRepository.GetActorsByIdsAsync(movieDto.ActorIds);
            if (actors.ToList().Count != movieDto.ActorIds.Count) {
                var invalidIds = movieDto.ActorIds.Except(actors.Select(a => a.Id).ToList());
                throw new ArgumentException($"One or more actors with Ids {string.Join(", ", invalidIds)} do not exist.");
            }
            movie.Actors = actors.ToList();
        }

        await _repository.AddAsync(movie);
        await _repository.SaveChangesAsync();

        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseDate = movie.ReleaseDate,
            DurationMinutes = movie.DurationMinutes,
            Rating = movie.Rating,
            Actors = movie.Actors.Select(a => a.Name).ToList() 
        };
    }

    async Task<bool> IMovieService.UpdateMovieAsync(Guid id, UpdateMovieDto movieDto)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null) return false;

        movie.Title = movieDto.Title;
        movie.Description = movieDto.Description;
        movie.ReleaseDate = movieDto.ReleaseDate ?? DateTime.UtcNow;
        movie.DurationMinutes = movieDto.DurationMinutes;
        movie.Rating = movieDto.Rating;

        if (movieDto.ActorIds != null && movieDto.ActorIds.Any()) { 
            var actors = await _actorRepository.GetActorsByIdsAsync(movieDto.ActorIds);
            if (actors.ToList().Count != movieDto.ActorIds.Count)
            {
                var invalidIds = movieDto.ActorIds.Except(actors.Select(a => a.Id).ToList());
                throw new ArgumentException($"One or more actors with Ids {string.Join(", ", invalidIds)} do not exist.");
            }
            movie.Actors = actors.ToList();
        }

        await _repository.UpdateAsync(movie);
        await _repository.SaveChangesAsync();

        return true;
    }
    public async Task<bool> PatchMovieAsync(Guid id, PatchMovieDto movieDto)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null) return false;

        if (!string.IsNullOrEmpty(movieDto.Title))
            movie.Title = movieDto.Title;

        if (!string.IsNullOrEmpty(movieDto.Description))
            movie.Description = movieDto.Description;

        if (movieDto.ReleaseDate.HasValue)
            movie.ReleaseDate = movieDto.ReleaseDate.Value;

        if (movieDto.DurationMinutes.HasValue)
            movie.DurationMinutes = movieDto.DurationMinutes.Value;

        if (movieDto.Rating.HasValue)
            movie.Rating = movieDto.Rating.Value;

        if (movieDto.ActorIds != null && movieDto.ActorIds.Any())
        {
            var actors = await _actorRepository.GetActorsByIdsAsync(movieDto.ActorIds);
            if (actors.ToList().Count != movieDto.ActorIds.Count)
            {
                var invalidIds = movieDto.ActorIds.Except(actors.Select(a => a.Id).ToList());
                throw new ArgumentException($"One or more actors with Ids {string.Join(", ", invalidIds)} do not exist.");
            }
            movie.Actors = actors.ToList();
        }

        await _repository.UpdateAsync(movie);
        await _repository.SaveChangesAsync();

        return true;
    }


    public async Task<bool> DeleteMovieAsync(Guid id)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null) return false;

        if (movie.Actors != null && movie.Actors.Any())
        {
            movie.Actors.Clear();
        }

        await _repository.DeleteAsync(movie);
        await _repository.SaveChangesAsync();

        return true;
    }



}
