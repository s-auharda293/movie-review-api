using MediatR;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Handlers.Auth
{
    public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenCommand, Result<RevokeRefreshTokenResponse>>
    {
        private readonly IUserService _userService;

        public RevokeRefreshTokenHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<RevokeRefreshTokenResponse>> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _userService.RevokeRefreshToken(new RevokeRefreshToken
            {
                RefreshToken = request.RefreshToken
            });
        }
    }
}
