using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Infrastructure.Persistence;
using MovieReviewApi.Domain.Entities;
using Azure;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        public readonly MovieReviewDbContext _context;

        public ActorsController(MovieReviewDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetActors() {
            var actors = await _context.Actors.Include(a => a.Movies).ToListAsync();
            if (actors.Count == 0) {
                return Ok("There are no actors");
            }
            return Ok(actors);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetActor([FromRoute] int id) {
            var actor = await _context.Actors.Include(a => a.Movies).FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null) {
                return NotFound("The specified actor could not be found!");

            }

            return Ok(actor);

        }

        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor([FromBody] Actor actor) {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Actor>> PutActor([FromRoute] int id, [FromBody] Actor updatedActor) { 

            var actor = await _context.Actors.Include(a => a.Movies).FirstOrDefaultAsync(a => a.Id == id); //EF Core attaches actor object to the DbContext and starts tracking it 

            if (actor == null) {
                return NotFound($"Actor with id {updatedActor.Id} doesn't exist");
            };
            actor.Name = updatedActor.Name;
            actor.DateOfBirth = updatedActor.DateOfBirth;
            actor.Bio = updatedActor.Bio;
            actor.Movies = updatedActor.Movies;

            actor.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(); //don't need to call _context.Update(actor), because EF Core knows actor came from db and is being tracked

            return Ok(actor);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<Actor>> PatchActor([FromRoute] int id, [FromBody] Actor updatedActor) {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();

            if (!string.IsNullOrEmpty(updatedActor.Name)) actor.Name = updatedActor.Name;
            if (updatedActor.DateOfBirth.HasValue) actor.DateOfBirth = updatedActor.DateOfBirth;
            if(!string.IsNullOrEmpty(updatedActor.Bio)) actor.Bio = updatedActor.Bio;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteActor(int id) {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) { 
                return NotFound($"Actor with id {id} doesn't exist!");
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
