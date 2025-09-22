

using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Interfaces.Identity
{
    public interface IUserService
    {
        Task<Result<UserResponse>> RegisterAsync(UserRegisterRequest request);
        Task<Result<UserResponse>> LoginAsync(UserLoginRequest request);
        Task<Result<CurrentUserResponse>> GetCurrentUserAsync();
        Task<Result<UserResponse>> GetByIdAsync(Guid id);
        //Task<Result<UserResponse>> UpdateAsync(Guid id, UpdateUserRequest request);
        //Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<RevokeRefreshTokenResponse>> RevokeRefreshToken(RefreshTokenRequest refreshTokenRemoveRequest);
        Task<Result<CurrentUserResponse>> GenerateNewAccessTokenAsync(RefreshTokenRequest request);
    }
}
