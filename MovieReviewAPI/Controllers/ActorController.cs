using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetActors() {
            var actors = await _mediator.Send(new GetActorsQuery());
            return Ok(actors);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetActor(Guid id) {
            var actor = await _mediator.Send(new GetActorByIdQuery(id));
            return actor.IsSuccess ? Ok(actor):NotFound(actor);
        }

        [HttpPost]
        [Authorize(Roles=UserRoles.Admin)]
        public async Task<ActionResult<ActorDto>> PostActor(CreateActorCommand createActorCommand)
        {
            var actor = await _mediator.Send(createActorCommand);
            return actor.IsSuccess ? CreatedAtAction(nameof(GetActor),
                                                     new { id = actor?.Value?.Id },
                                                     actor) : BadRequest(actor);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<Actor>> PutActor(UpdateActorCommand updateActorCommand) {

            var updated = await _mediator.Send(updateActorCommand);
            return updated.IsSuccess ? Ok(updated) : NotFound(updated);
           
        }

        [HttpPatch]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<Actor>> PatchActor( PatchActorCommand patchActorCommand) {
            
            var patched = await _mediator.Send(patchActorCommand);
            return patched.IsSuccess ? Ok(patched) : BadRequest(patched);
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteActor(DeleteActorCommand deleteActorCommand) {    
            var deleted = await _mediator.Send(deleteActorCommand);
            return deleted.IsSuccess ? NoContent() : NotFound(deleted);
        }
    }
}
