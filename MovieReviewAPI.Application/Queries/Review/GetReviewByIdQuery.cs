using MediatR;
using MovieReviewApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Queries.Review
{
    public record GetReviewByIdQuery(Guid Id):IRequest<ReviewDto>;
}
