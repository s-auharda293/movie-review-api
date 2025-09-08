using MediatR;
using MovieReviewApi.Application.DTOs;

namespace MovieReviewApi.Application.Commands.Review
{
    public record PatchReviewCommand(Guid Id, PatchReviewDto dto):IRequest<bool>;
}
