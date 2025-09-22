using MediatR;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces.Identity;
using MovieReviewApi.Application.Queries.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Handlers.Auth
{
    public class GetUserByIdHandler:IRequestHandler<GetUserByIdQuery,Result<UserResponse>>
    {
        private readonly IUserService _userService;

        public GetUserByIdHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetByIdAsync(request.Id);
        }
    }
}
