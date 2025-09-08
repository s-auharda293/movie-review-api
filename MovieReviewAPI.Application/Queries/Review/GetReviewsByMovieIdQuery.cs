using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Queries.Review
{
   public record GetReviewsByMovieIdQuery(Guid Id):IRequest<IEnumerable<ReviewDto>>;
}
