using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Review
{
    public record GetReviewsQuery:IRequest<Result<IEnumerable<ReviewDto>>>;
}
