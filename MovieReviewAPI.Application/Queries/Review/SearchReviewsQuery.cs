using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Review
{
    public record SearchReviewsQuery(ReviewRequestDto request) : IRequest<Result<ReviewResponseDto>>;
}
