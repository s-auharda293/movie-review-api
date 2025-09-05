using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Services;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Infrastructure.Persistence;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        public readonly IActorService _service;

        public ActorsController(IActorService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetActors() {
            var actors = await _service.GetAllActorsAsync();
            return Ok(actors);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetActor([FromRoute] Guid id) {
            var actor = await _service.GetActorByIdAsync(id);
            return actor == null ? NotFound() : Ok(actor);
        }

        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor([FromBody] CreateActorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var actor = await _service.CreateActorAsync(dto);
                return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor);
            }
            catch (ArgumentException ex) {
                return BadRequest(new { error = ex.Message});
            
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Actor>> PutActor([FromRoute] Guid id, [FromBody] UpdateActorDto dto) {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            try { 
            var updated = await _service.UpdateActorAsync(id, dto);
            return updated ? Ok("Actor updated") : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<Actor>> PatchActor([FromRoute] Guid id, [FromBody] PatchActorDto dto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try { 
            var patched = await _service.PatchActorAsync(id, dto);
            return patched ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });

            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteActor(Guid id) {    
                var deleted = await _service.DeleteActorAsync(id);
            return deleted ? NoContent() : NotFound(); 
        }

    }
}
