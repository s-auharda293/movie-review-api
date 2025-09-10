using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Commands.Movie
{
    public record DeleteMovieCommand(Guid Id):IRequest<Result<bool>>;
}
