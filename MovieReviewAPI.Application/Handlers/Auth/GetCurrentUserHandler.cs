using MediatR;
using MovieReviewApi.Application.Queries.Auth;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces.Identity;

namespace MovieReviewApi.Application.Handlers.Auth
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserResponse>>
    {
        private readonly IUserService _userService;

        public GetCurrentUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<CurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetCurrentUserAsync();
        }
    }
}
