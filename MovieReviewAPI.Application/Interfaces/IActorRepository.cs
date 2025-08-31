using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Infrastructure.Repositories;

public interface IActorRepository
{
    Task<IEnumerable<ActorDto>> GetAllAsync();
    Task<Actor?> GetByIdAsync(int id);
    Task AddAsync(Actor actor);
    Task UpdateAsync(Actor actor);
    Task DeleteAsync(Actor actor);
    Task SaveChangesAsync();
}