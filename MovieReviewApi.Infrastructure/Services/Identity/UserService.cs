using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces.Identity;
using MovieReviewApi.Domain.Common.Identity;
using MovieReviewApi.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace MovieReviewApi.Infrastructure.Services.Identity
{
    public class UserService:IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(ITokenService tokenService, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<UserService> logger)
        {
            _tokenService = tokenService;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Result<UserResponse>> RegisterAsync(UserRegisterRequest request)
        {
            _logger.LogInformation("Registering user");

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogError("Email already exists");
                return Result<UserResponse>.Failure(IdentityErrors.EmailAlreadyExists);
            }

            var newUser = _mapper.Map<ApplicationUser>(request);
            newUser.UserName = GenerateUserName(request?.FirstName,request?.LastName);

            var result = await _userManager.CreateAsync(newUser, request?.Password!);
            if (!result.Succeeded)
            {
                var errorDetails = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user: {errors}", errorDetails);

                return Result<UserResponse>.Failure(IdentityErrors.UserCreationFailedWithDetails(errorDetails));
            }

            _logger.LogInformation("User created successfully");
            await _tokenService.GenerateToken(newUser);

            newUser.CreatedAt = DateTime.Now;
            newUser.UpdatedAt = DateTime.Now;

            var registeredUser = _mapper.Map<UserResponse>(newUser);
            return Result<UserResponse>.Success(registeredUser);
        }


        private string GenerateUserName(string firstName,string lastName)
        {
            var baseUsername = $"{firstName}{lastName}";
            baseUsername = new string(baseUsername.Where(char.IsLetterOrDigit).ToArray());
            var username = baseUsername;
            var count = 1;
            while (_userManager.Users.Any(u => u.UserName == username)) {
                username = $"{baseUsername}{count}";
                count++;
            }
            return username;
        }

        public async Task<Result<UserResponse>> LoginAsync(UserLoginRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Login request is null");
                return Result<UserResponse>.Failure(IdentityErrors.LoginRequestNull);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                _logger.LogError("Invalid email or password");
                return Result<UserResponse>.Failure(IdentityErrors.InvalidCredentials);
            }

            // Generate access token
            var token = await _tokenService.GenerateToken(user);

            // Generate refresh token
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Hash the refresh token and store it in the database or override the existing refresh token
            using var sha256 = SHA256.Create();
            var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
            user.RefreshToken = Convert.ToBase64String(refreshTokenHash);
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(2);

            user.CreatedAt = DateTime.Now;

            // Update user information in database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorDetails = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user: {errors}", errorDetails);

                return Result<UserResponse>.Failure(IdentityErrors.UserCreationFailedWithDetails(errorDetails));
            }

            var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);
            userResponse.AccessToken = token;
            userResponse.RefreshToken = refreshToken;

            return Result<UserResponse>.Success(userResponse);
        }


        //public Task<UserResponse> GetByIdAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}



        public async Task<Result<CurrentUserResponse>> GetCurrentUserAsync()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.GetUserId());
            if (user == null) {
                _logger.LogError("User not found");
                return Result<CurrentUserResponse>.Failure(IdentityErrors.UserNotFound);
            }
            var currentUser = _mapper.Map<CurrentUserResponse>(user);
            return Result<CurrentUserResponse>.Success(currentUser);
        }


        //public Task<CurrentUserResponse> RefreshTokenAsync(RefreshTokenRequest request)
        //{
        //    throw new NotImplementedException();
        //}



        //public Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest refreshTokenRemoveRequest)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
