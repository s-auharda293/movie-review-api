using Microsoft.AspNetCore.Http;
using MovieReviewApi.Application.Interfaces.Identity;
using System.Security.Claims;

namespace MovieReviewApi.Infrastructure.Services.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? GetUserId()
        {
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return userId?.Value;
        }
    }
}
