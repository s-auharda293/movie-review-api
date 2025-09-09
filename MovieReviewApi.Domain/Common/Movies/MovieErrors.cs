using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Domain.Common.Movies
{
    public class MovieErrors
    {
        public static readonly Error NotFound = new("Movies.NotFound", "The requested movie was not found");

        public static readonly Error DuplicateTitle = new("Movies.DuplicateTitle", "A movie with same title already exists.");

        public static readonly Error ActorAlreadyAssigned = new("Movies.ActorAlreadyAssigned", "This actor is already assigned to the movie.");
    }
}
