using Azure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Actor;
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
        private readonly IMediator _mediator;

        public ActorsController(IActorService service, IMediator mediator) {
            _service = service;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetActors(CancellationToken cancellationToken) {
            var actors = await _mediator.Send(new GetActorsQuery(), cancellationToken);
            return Ok(actors);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetActor([FromRoute] Guid id, CancellationToken cancellationToken) {
            var actor = await _mediator.Send(new GetActorByIdQuery(id), cancellationToken);
            return actor == null ? NotFound() : Ok(actor);
        }

        [HttpPost]
        public async Task<ActionResult<ActorDto>> PostActor([FromBody] CreateActorDto dto,CancellationToken cancellationToken)
        {
            try
            {
                var actor = await _mediator.Send( new CreateActorCommand(dto),cancellationToken);
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
