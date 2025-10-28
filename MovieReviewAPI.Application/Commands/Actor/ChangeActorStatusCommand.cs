using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Actor
{
        public record ChangeActorStatusCommand(Guid ActorId, string Status) : IRequest<Result<ActorStatusResponseDto>>;

}
