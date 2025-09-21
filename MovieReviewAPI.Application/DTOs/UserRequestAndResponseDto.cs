﻿namespace MovieReviewApi.Application.DTOs
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
        public string? FirstName{get;}

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
    
        public string? FirstName{get;}

        public string? Email { get; set; }

        public string? AccessToken { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }

    public class UpdateUserRequest {
        public string? FirstName{get;}

        public string? Email { get; set; }

        public string? Password { get; set; }


        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


    }


    public class RevokeRefreshTokenResponse
    {
        public string? Message { get; set; }
    }

    public class RevokeRefreshToken {
        public string? Token { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string? RefreshToken { get; set; }
    }


}
