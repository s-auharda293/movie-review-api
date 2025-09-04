using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Infrastructure.Repositories;

public interface IActorRepository
{
    Task<IEnumerable<Actor>> GetAllAsync();
    Task<Actor?> GetByIdAsync(Guid id);
    Task AddAsync(Actor actor);
    Task UpdateAsync(Actor actor);
    Task DeleteAsync(Actor actor);
    Task SaveChangesAsync();

    Task<IEnumerable<Actor>> GetActorsByIdsAsync(List<Guid> ids);
}