using MediatR;
using MovieReviewApi.Application.DTOs;


namespace MovieReviewApi.Application.Queries.Auth
{
        public record GetCurrentUserQuery() : IRequest<Result<CurrentUserResponse>>;
}
