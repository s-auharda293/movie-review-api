using MediatR;
using MovieReviewApi.Application.Commands.Auth.MovieReviewApi.Application.Features.Roles.Commands.AssignRole;
using MovieReviewApi.Application.Interfaces.Identity;
using MovieReviewApi.Domain.Common.Identity;

namespace MovieReviewApi.Application.Handlers.Auth
{
    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Result<bool>>
    {
        private readonly IRoleService _roleService;

        public AssignRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<Result<bool>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.dto.UserId) || string.IsNullOrWhiteSpace(request.dto.Role))
                return Result<bool>.Failure(IdentityErrors.RoleAssignmentInputIncorrect);

            var success = await _roleService.AssignRoleAsync(request.dto.UserId, request.dto.Role);
            if (success.IsFailure)
                return Result<bool>.Failure(success.Errors);

            return Result<bool>.Success(true);
        }
    }
}
