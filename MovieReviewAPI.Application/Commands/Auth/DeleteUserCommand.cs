using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Commands.Auth
{
    public record DeleteUserCommand(Guid Id) : IRequest<Result<bool>>;
}
