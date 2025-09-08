using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Commands.Review
{
    public record DeleteReviewCommand(Guid Id):IRequest<bool>;
}
