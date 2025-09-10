using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Review
{
    public record CreateReviewCommand(CreateReviewDto dto) : IRequest<Result<ReviewDto>>;
}
