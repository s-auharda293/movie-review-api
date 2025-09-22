using MediatR;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces.Identity;

namespace MovieReviewApi.Application.Handlers.Auth
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result<ChangePasswordResponse>>
    {
        private readonly IUserService _userService;

        public ChangePasswordHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<ChangePasswordResponse>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await _userService.ChangePasswordAsync(request.Request);
        }
    }
}
