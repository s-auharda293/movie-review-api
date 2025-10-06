using MediatR;
using MovieReviewApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Queries.Movie
{
    public record GetMovieByIdQuery(Guid Id) : IRequest<Result<MovieDto>>;
}
