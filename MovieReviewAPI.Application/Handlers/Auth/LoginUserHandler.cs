using MediatR;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces.Identity;

namespace MovieReviewApi.Application.Handlers.Auth
{

    public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<UserResponse>>
    {
        private readonly IUserService _userService;

        public LoginUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<UserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.LoginAsync(request.Request);
        }
    }
}
