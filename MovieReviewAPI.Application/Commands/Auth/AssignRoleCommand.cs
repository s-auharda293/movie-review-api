using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Auth
{

    namespace MovieReviewApi.Application.Features.Roles.Commands.AssignRole
    {
        public record AssignRoleCommand(AssignRoleDto dto) : IRequest<Result<bool>>;
    }

}
