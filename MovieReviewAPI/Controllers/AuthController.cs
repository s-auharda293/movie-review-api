using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.Queries.Auth;

namespace MovieReviewApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register( UserRegisterRequest request)
        {
            var result = await _mediator.Send(new RegisterUserCommand(request));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(Guid id){
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var result = await _mediator.Send(new LoginUserCommand(request));

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("generate-access-token")]
        //[Authorize]
        public async Task<IActionResult> RefreshToken(GenerateTokenRequest request)
        {
            var response = await _mediator.Send(new GenerateTokenCommand(request.RefreshToken!));
            return Ok(response);
        }

        [HttpPost("revoke-refresh-token")]
        //[Authorize]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshToken request)
        {
            var response = await _mediator.Send(new RevokeRefreshTokenCommand(request.RefreshToken!));
            if (response != null && response?.Value?.Message == "Refresh token revoked successfully")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _mediator.Send(new GetCurrentUserQuery());

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateUser(Guid id,  UpdateUserRequest request)
        {
            var result = await _mediator.Send(new UpdateUserCommand(id, request));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Value);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var result = await _mediator.Send(new ChangePasswordCommand(request));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Value);
        }
    }
}
