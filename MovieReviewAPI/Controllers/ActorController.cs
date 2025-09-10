using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Actor;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActorsController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetActors(CancellationToken cancellationToken) {
            var actors = await _mediator.Send(new GetActorsQuery(), cancellationToken);
            return Ok(actors);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetActor(Guid id, CancellationToken cancellationToken) {
            var actor = await _mediator.Send(new GetActorByIdQuery(id), cancellationToken);
            return actor == null ? NotFound() : Ok(actor);
        }

        [HttpPost]
        public async Task<ActionResult<ActorDto>> PostActor(CreateActorDto dto,CancellationToken cancellationToken)
        {
            var actor = await _mediator.Send( new CreateActorCommand(dto),cancellationToken);
            if (actor.IsSuccess)
            {
                return CreatedAtAction(nameof(GetActor), new { id = actor?.Value?.Id }, actor);
            }
            else { 
                return BadRequest(actor);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Actor>> PutActor(Guid id, UpdateActorDto dto, CancellationToken cancellationToken) {

            var updated = await _mediator.Send(new UpdateActorCommand(id, dto),cancellationToken);
            if (updated.IsSuccess)
            {
                return Ok(updated);
            }
            else { 
                return NotFound(updated.Error);
            }
           
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<Actor>> PatchActor( Guid id, PatchActorDto dto, CancellationToken cancellationToken) {
            
            var patched = await _mediator.Send(new PatchActorCommand(id, dto), cancellationToken);
            if (patched.IsSuccess)
            {
                return Ok(patched);
            }
            else { 
                return BadRequest(patched);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteActor(Guid id, CancellationToken cancellationToken) {    
            var deleted = await _mediator.Send(new DeleteActorCommand(id),cancellationToken);
            if (deleted.IsSuccess) {
                return NoContent();
            }
            else {
                return NotFound(deleted);
            }
        }

    }
}
