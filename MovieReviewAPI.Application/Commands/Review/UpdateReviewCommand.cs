using MediatR;
using MovieReviewApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Commands.Review
{
    public record UpdateReviewCommand(Guid Id, UpdateReviewDto dto):IRequest<bool>;
}
