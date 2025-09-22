using MediatR;
using MovieReviewApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Commands.Auth
{
    public record UpdateUserCommand(Guid Id, UpdateUserRequest Request) : IRequest<Result<UserResponse>>;

}
