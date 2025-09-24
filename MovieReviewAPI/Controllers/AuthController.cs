using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.Application.Commands.Auth;
using MovieReviewApi.Application.Commands.Auth.MovieReviewApi.Application.Features.Roles.Commands.AssignRole;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Auth;
using MovieReviewApi.Domain.Entities;

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
        public async Task<IActionResult> Register(RegisterUserCommand registerUserCommand)
        {
            var result = await _mediator.Send(registerUserCommand);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand loginUserCommand)
        {
            var result = await _mediator.Send(loginUserCommand);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("generate-access-token")]
        //[Authorize]
        public async Task<IActionResult> RefreshToken(GenerateTokenCommand generateTokenCommand)
        {
            var response = await _mediator.Send(generateTokenCommand);
            return Ok(response);
        }

        [HttpPost("revoke-refresh-token")]
        //[Authorize]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenCommand revokeRefreshTokenCommand)
        {
            var response = await _mediator.Send(revokeRefreshTokenCommand);
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

        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand updateUserCommand)
        {
            var result = await _mediator.Send(updateUserCommand);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Value);
        }

        [HttpDelete]
        //[Authorize]
        public async Task<IActionResult> DeleteUser(DeleteUserCommand deleteUserCommand)
        {
            var result = await _mediator.Send(deleteUserCommand);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Value);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand changePasswordCommand)
        {
            var result = await _mediator.Send(changePasswordCommand);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Value);
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return Ok("Role assigned successfully.");
        }
    }
}
