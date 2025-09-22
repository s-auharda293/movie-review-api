namespace MovieReviewApi.Application.DTOs
{
    public class UserRegisterRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }


    public class UserResponse { 
        public Guid Id { get; set; }
        public string? UserName{ get; set; }

        public string? Email { get; set; }



        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? AccessToken { get; set; }
        public string? RefreshToken{ get; set; }
    }

    public class UserLoginRequest { 
    
        public string? Email { get; set; }

        public string? Password { get; set; }
    }

    public class CurrentUserResponse { 
    
        public string? UserName{ get; set; }

        public string? Email { get; set; }

        public string? AccessToken { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }

    public class UpdateUserRequest {
        public string? FirstName{ get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


    }


    public class RevokeRefreshTokenResponse
    {
        public string? Message { get; set; }
    }

    public class RevokeRefreshToken {
        public string? RefreshToken { get; set; }
    }

    public class GenerateTokenRequest
    {
        public string? RefreshToken { get; set; }
    }

    public class ChangePasswordRequest { 
        public string? Email { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    
    }

    public class ChangePasswordResponse
    {
        public string? Message { get; set; }     
        public DateTime? UpdatedAt { get; set; } 
    }



}
