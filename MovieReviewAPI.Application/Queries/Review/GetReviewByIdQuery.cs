using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Review
{
    public record GetReviewByIdQuery(Guid Id):IRequest<Result<ReviewDto>>;
}
