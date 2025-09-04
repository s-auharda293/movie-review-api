using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Persistence;

namespace MovieReviewApi.Infrastructure.Repositories;

public class ActorRepository : IActorRepository
{
    private readonly MovieReviewDbContext _dbcontext;
    public ActorRepository(MovieReviewDbContext context)
    {
        _dbcontext = context;
    }
    public async Task<IEnumerable<Actor>> GetAllAsync() =>
      await _dbcontext.Actors.Include(a=>a.Movies).ToListAsync();

    public async Task<Actor?> GetByIdAsync(Guid id) =>
        await _dbcontext.Actors.Include(a => a.Movies).FirstOrDefaultAsync(a => a.Id == id);

    public async Task AddAsync(Actor actor) => await _dbcontext.Actors.AddAsync(actor);
    public Task UpdateAsync(Actor actor)
    {
        _dbcontext.Actors.Update(actor);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Actor actor)
    {
        _dbcontext.Actors.Remove(actor);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await _dbcontext.SaveChangesAsync();

    public async Task<IEnumerable<Actor>> GetActorsByIdsAsync(List<Guid> ids)
    {
        return await _dbcontext.Actors.Where(a => ids.Contains(a.Id)).ToListAsync();
    }
}
