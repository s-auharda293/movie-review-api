using MediatR;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces.Identity;

namespace MovieReviewApi.Application.Handlers.Auth
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserResponse>>
    {
        private readonly IUserService _userService;

        public UpdateUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.UpdateAsync(request.Id, request.Request);
        }
    }
}
