using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace MovieReviewApi.Application.Services;

public class ActorService: IActorService
{
    private readonly IActorRepository _repository;
    private readonly IMovieRepository _movieRepository;
    public ActorService(IActorRepository repository, IMovieRepository movieRepository/*, IValidator<CreateActorDto> createValidator, IValidator<UpdateActorDto> updateValidator, IValidator<PatchActorDto> patchValidator*/) {
        _repository = repository;
        _movieRepository = movieRepository;
    }

    public async Task<IEnumerable<ActorDto>> GetAllActorsAsync()
    {
        var actors = await _repository.GetAllAsync();
        return actors.Select(a => new ActorDto
        {
            Id = a.Id,
            Name = a.Name,
            Bio = a.Bio,
            DateOfBirth = a.DateOfBirth,
            Movies = a.Movies?.Select(m=>m.Title).ToList()??new List<String>(),
        }).ToList();
    }


    public async Task<ActorDto?> GetActorByIdAsync(Guid id) {
        if (id == Guid.Empty)
            throw new ArgumentException("Actor Id cannot be empty.", nameof(id));

        var a = await _repository.GetByIdAsync(id);
        if (a == null) return null;

        return new ActorDto
        {
            Id = a.Id,
            Name = a.Name,
            Bio = a.Bio,
            DateOfBirth = a.DateOfBirth,
            Movies = a.Movies?.Select(m =>m.Title).ToList() ?? new List<string>()
        };

    }

    public async Task<ActorDto> CreateActorAsync(CreateActorDto dto)
    {

        //ValidationResult validationResult = await _createValidator.ValidateAsync(dto);
        //if (!validationResult.IsValid)
        //    throw new ValidationException(validationResult.Errors);

        var actor = new Actor
        {
            Name = dto.Name,
            Bio = dto.Bio,
            DateOfBirth = dto.DateOfBirth,
        };

        if (dto.MovieIds != null && dto.MovieIds.Any()) {
            var movies = await _movieRepository.GetMoviesByIdsAsync(dto.MovieIds);
            if (movies.ToList().Count != dto.MovieIds.Count) {
                var invalidIds = dto.MovieIds.Except((movies.Select(m => m.Id).ToList())).ToList();
                throw new ArgumentException($"One or more movies with Ids {string.Join(", ", invalidIds)} do not exist.");
            }
            actor.Movies = movies.ToList();
        }

        await _repository.AddAsync(actor);
        await _repository.SaveChangesAsync();

        return new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name,
            Bio = actor.Bio,
            DateOfBirth = actor.DateOfBirth,
            Movies = actor.Movies?.Select(m=>m.Title).ToList()??new List<string>()
        };
    }

    public async Task<bool> UpdateActorAsync(Guid id, UpdateActorDto dto)
    {
        //ValidationResult validationResult = await _updateValidator.ValidateAsync(dto);
        //if (!validationResult.IsValid)
        //    throw new ValidationException(validationResult.Errors);

        var actor = await _repository.GetByIdAsync(id);
        if (actor == null) return false;

        actor.Name = dto.Name;
        actor.Bio = dto.Bio;
        actor.DateOfBirth = dto.DateOfBirth;

        if (dto.MovieIds != null && dto.MovieIds.Any())
        {
            var movies = await _movieRepository.GetMoviesByIdsAsync(dto.MovieIds);
            if (movies.ToList().Count != dto.MovieIds.Count)
            {
                var invalidIds = dto.MovieIds.Except((movies.Select(m => m.Id).ToList())).ToList();
                throw new ArgumentException($"One or more movies with Ids {string.Join(", ", invalidIds)} do not exist.");
            }
            actor.Movies = movies.ToList();
        }

        await _repository.UpdateAsync(actor);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PatchActorAsync(Guid id, PatchActorDto dto)
    {
        //ValidationResult validationResult = await _patchValidator.ValidateAsync(dto);
        //if (!validationResult.IsValid)
        //    throw new ValidationException(validationResult.Errors);

        var actor = await _repository.GetByIdAsync(id);
        if (actor == null) return false;

        if (!string.IsNullOrEmpty(dto.Name)) actor.Name = dto.Name;
        if (dto.DateOfBirth.HasValue) actor.DateOfBirth = dto.DateOfBirth;
        if (!string.IsNullOrEmpty(dto.Bio)) actor.Bio = dto.Bio;

        if (dto.MovieIds != null && dto.MovieIds.Any())
        {
            var movies = await _movieRepository.GetMoviesByIdsAsync(dto.MovieIds);
            if (movies.ToList().Count != dto.MovieIds.Count)
            {
                var invalidIds = dto.MovieIds.Except((movies.Select(m => m.Id).ToList())).ToList();
                throw new ArgumentException($"One or more movies with Ids {string.Join(", ", invalidIds)} do not exist.");
            }
            actor.Movies = movies.ToList();
        }
        await _repository.UpdateAsync(actor);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteActorAsync(Guid id)
    {
        var actor = await _repository.GetByIdAsync(id);
        if (actor == null) return false;

        if (actor.Movies != null && actor.Movies.Any())
        {
            actor.Movies.Clear(); //remove tracked list actor.Movies
        }

        await _repository.DeleteAsync(actor);
        await _repository.SaveChangesAsync(); //when we do this EF Core deletes the rows in the join table not the movies
        return true;
    }
}
