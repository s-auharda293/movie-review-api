using MediatR;
using MovieReviewApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Queries.Actor
{
    public record GetActorByIdQuery(Guid Id): IRequest<ActorDto>;
}
