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

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _mediator.Send(new GetCurrentUserQuery());

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}
