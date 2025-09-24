using MediatR;
using MovieReviewApi.Application.DTOs;
using System.Collections.Generic;

public record GetReviewsByUserQuery(string UserId) : IRequest<Result<IEnumerable<ReviewDto>>>;
