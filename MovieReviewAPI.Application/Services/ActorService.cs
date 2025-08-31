using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Repositories;

namespace MovieReviewApi.Application.Services;

public class ActorService: IActorService
{
    private readonly IActorRepository _repository;
    public ActorService(IActorRepository repository) {
        _repository = repository;
    }

    public async Task<IEnumerable<ActorDto>> GetAllActorsAsync() { 
        var actors = await _repository.GetAllAsync();
        return actors.Select(a => new ActorDto
        {
            Id = a.Id,
            Name = a.Name,
            Bio = a.Bio,
            DateOfBirth = a.DateOfBirth,
            //MovieIds = a.Movies.Select(m => m.Id).ToList()
        }).ToList();
    }

    public async Task<ActorDto?> GetActorByIdAsync(int id) {
        var a = await _repository.GetByIdAsync(id);
        if (a == null) return null;

        return new ActorDto
        {
            Id = a.Id,
            Name = a.Name,
            Bio = a.Bio,
            DateOfBirth = a.DateOfBirth,
            //MovieIds = a.Movies.Select(m => m.Id).ToList()
        };

    }

    public async Task<ActorDto> CreateActorAsync(CreateActorDto dto)
    {
        var actor = new Actor
        {
            Name = dto.Name,
            Bio = dto.Bio,
            DateOfBirth = dto.DateOfBirth,
        };

        await _repository.AddAsync(actor);
        await _repository.SaveChangesAsync();

        return new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name,
            Bio = actor.Bio,
            DateOfBirth = actor.DateOfBirth,
            //MovieIds = actor.Movies.Select(m => m.Id).ToList()
        };
    }

    public async Task<bool> UpdateActorAsync(int id, UpdateActorDto dto)
    {
        var actor = await _repository.GetByIdAsync(id);
        if (actor == null) return false;

        actor.Name = dto.Name;
        actor.Bio = dto.Bio;
        actor.DateOfBirth = dto.DateOfBirth;

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PatchActorAsync(int id, PatchActorDto dto)
    {
        var actor = await _repository.GetByIdAsync(id);
        if (actor == null) return false;

        if (!string.IsNullOrEmpty(dto.Name)) actor.Name = dto.Name;
        if (dto.DateOfBirth.HasValue) actor.DateOfBirth = dto.DateOfBirth;
        if (!string.IsNullOrEmpty(dto.Bio)) actor.Bio = dto.Bio;

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteActorAsync(int id)
    {
        var actor = await _repository.GetByIdAsync(id);
        if (actor == null) return false;

        await _repository.DeleteAsync(actor);
        await _repository.SaveChangesAsync();
        return true;
    }
}
