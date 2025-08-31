using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Persistence;

namespace MovieReviewApi.Infrastructure.Repositories;

public class ActorRepository : IActorRepository
{
    private readonly MovieReviewDbContext _context;
    public ActorRepository(MovieReviewDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Actor>> GetAllAsync() =>
      await _context.Actors.Include(a=>a.Movies).ToListAsync();

    public async Task<Actor?> GetByIdAsync(int id) =>
        await _context.Actors.Include(a => a.Movies).FirstOrDefaultAsync(a => a.Id == id);

    public async Task AddAsync(Actor actor) => await _context.Actors.AddAsync(actor);
    public Task UpdateAsync(Actor actor)
    {
        _context.Actors.Update(actor);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Actor actor)
    {
        _context.Actors.Remove(actor);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<IEnumerable<Actor>> GetActorsByIdsAsync(List<int> ids)
    {
        return await _context.Actors.Where(a => ids.Contains(a.Id)).ToListAsync();
    }
}
