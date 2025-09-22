using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Application.Interfaces.Identity
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);

        string GenerateRefreshToken();
    }
}
