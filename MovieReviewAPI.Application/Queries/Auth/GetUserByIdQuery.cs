using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Auth
{
    public record GetUserByIdQuery(Guid Id):IRequest<Result<UserResponse>>;
}
