using MediatR;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces.Identity;

namespace MovieReviewApi.Application.Handlers.Auth
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly IUserService _userService;

        public DeleteUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.DeleteAsync(request.Id);
        }
    }
}
