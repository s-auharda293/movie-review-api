using DotnetSecurity.Domain.Contracts;

namespace DotnetSecurity.Service
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);

        string GenerateRefreshToken();
    }
}
