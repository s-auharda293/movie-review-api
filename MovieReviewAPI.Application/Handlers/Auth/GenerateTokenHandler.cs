using MediatR;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces.Identity;

namespace MovieReviewApi.Application.Handlers.Auth
{
    public class RefreshTokenHandler : IRequestHandler<GenerateTokenCommand, Result<CurrentUserResponse>>
    {
        private readonly IUserService _userService;

        public RefreshTokenHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<CurrentUserResponse>> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            return await _userService.GenerateNewAccessTokenAsync();
        }
    }

}
