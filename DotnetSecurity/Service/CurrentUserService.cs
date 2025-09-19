
using System.Security.Claims;

namespace DotnetSecurity.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor) //get user id from token
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? GetUserId()
        {
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier); //NameIdentifier is contains the current id of user who's logged in
            return userId?.Value;
        }
    }
}
