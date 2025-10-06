using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.KeylessEntities;
using MovieReviewApi.Application.Queries.Movie;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class SearchMoviesHandler : IRequestHandler<SearchMoviesQuery, Result<MovieResponseDto>>
    {
            private readonly IApplicationDbContext _context;
            public SearchMoviesHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Result<MovieResponseDto>> Handle(SearchMoviesQuery request, CancellationToken cancellationToken)
            {
                // Call the stored procedure into your keyless entity
                var movies = await _context.GetMoviesResult
                    .FromSqlRaw("EXEC GetMovies")
                    .ToListAsync(cancellationToken);

            //Filtering(search)
            if (!string.IsNullOrWhiteSpace(request.request.SearchTerm) &&
                !string.IsNullOrWhiteSpace(request.request.SearchColumn))
                    {
                        switch (request.request.SearchColumn.ToLower())
                        {
                            case "id":
                                movies = movies.Where(m => m.Id.ToString().Contains(request.request.SearchTerm)).ToList();
                                break;
                            case "title":
                                movies = movies.Where(m => m.Title.Contains(request.request.SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                                break;
                            case "description":
                                movies = movies.Where(m => m.Description.Contains(request.request.SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                                break;
                            case "releasedate":
                                movies = movies.Where(m => m.ReleaseDate.ToString("yyyy-MM-dd").Contains(request.request.SearchTerm)).ToList();
                                break;
                            case "durationminutes":
                                movies = movies.Where(m => m.DurationMinutes.ToString().Contains(request.request.SearchTerm)).ToList();
                                break;
                            case "rating":
                                movies = movies.Where(m => m.Rating.ToString().Contains(request.request.SearchTerm)).ToList();
                                break;
                        }
                    }

            // Sorting (single-column)
            if (!string.IsNullOrWhiteSpace(request.request.Sort))
            {
                var sortList = System.Text.Json.JsonSerializer.Deserialize<List<SortDto>>(request.request.Sort);
                var sort = sortList?.LastOrDefault(); 

                if (sort != null)
                {
                    switch (sort.Field.ToLower())
                    {
                        case "title":
                            movies = sort.Dir.ToLower() == "asc"
                                ? movies.OrderBy(m => m.Title).ToList()
                                : movies.OrderByDescending(m => m.Title).ToList();
                            break;
                        case "description":
                            movies = sort.Dir.ToLower() == "asc"
                                ? movies.OrderBy(m => m.Description).ToList()
                                : movies.OrderByDescending(m => m.Description).ToList();
                            break;
                        case "releasedate":
                            movies = sort.Dir.ToLower() == "asc"
                                ? movies.OrderBy(m => m.ReleaseDate).ToList()
                                : movies.OrderByDescending(m => m.ReleaseDate).ToList();
                            break;
                        case "durationminutes":
                            movies = sort.Dir.ToLower() == "asc"
                                ? movies.OrderBy(m => m.DurationMinutes).ToList()
                                : movies.OrderByDescending(m => m.DurationMinutes).ToList();
                            break;
                        case "rating":
                            movies = sort.Dir.ToLower() == "asc"
                                ? movies.OrderBy(m => m.Rating).ToList()
                                : movies.OrderByDescending(m => m.Rating).ToList();
                            break;
                    }
                }
            }



            //multi column sorting
            //if (!string.IsNullOrWhiteSpace(request.request.Sort))
            //{
            //    var sortList = System.Text.Json.JsonSerializer.Deserialize<List<SortDto>>(request.request.Sort);
            //    if (sortList != null && sortList.Any())
            //    {
            //        IOrderedEnumerable<GetMoviesResult> orderedMovies = null;

            //        foreach (var sort in sortList)
            //        {
            //            switch (sort.Field.ToLower())
            //            {
            //                case "title":
            //                    orderedMovies = orderedMovies == null
            //                        ? (sort.Dir.ToLower() == "asc" ? movies.OrderBy(m => m.Title) : movies.OrderByDescending(m => m.Title))
            //                        : (sort.Dir.ToLower() == "asc" ? orderedMovies.ThenBy(m => m.Title) : orderedMovies.ThenByDescending(m => m.Title));
            //                    break;
            //                case "description":
            //                    orderedMovies = orderedMovies == null
            //                        ? (sort.Dir.ToLower() == "asc" ? movies.OrderBy(m => m.Description) : movies.OrderByDescending(m => m.Description))
            //                        : (sort.Dir.ToLower() == "asc" ? orderedMovies.ThenBy(m => m.Description) : orderedMovies.ThenByDescending(m => m.Description));
            //                    break;
            //                case "releasedate":
            //                    orderedMovies = orderedMovies == null
            //                        ? (sort.Dir.ToLower() == "asc" ? movies.OrderBy(m => m.ReleaseDate) : movies.OrderByDescending(m => m.ReleaseDate))
            //                        : (sort.Dir.ToLower() == "asc" ? orderedMovies.ThenBy(m => m.ReleaseDate) : orderedMovies.ThenByDescending(m => m.ReleaseDate));
            //                    break;
            //                case "durationminutes":
            //                    orderedMovies = orderedMovies == null
            //                        ? (sort.Dir.ToLower() == "asc" ? movies.OrderBy(m => m.DurationMinutes) : movies.OrderByDescending(m => m.DurationMinutes))
            //                        : (sort.Dir.ToLower() == "asc" ? orderedMovies.ThenBy(m => m.DurationMinutes) : orderedMovies.ThenByDescending(m => m.DurationMinutes));
            //                    break;
            //                case "rating":
            //                    orderedMovies = orderedMovies == null
            //                        ? (sort.Dir.ToLower() == "asc" ? movies.OrderBy(m => m.Rating) : movies.OrderByDescending(m => m.Rating))
            //                        : (sort.Dir.ToLower() == "asc" ? orderedMovies.ThenBy(m => m.Rating) : orderedMovies.ThenByDescending(m => m.Rating));
            //                    break;
            //            }
            //        }

            //        if (orderedMovies != null)
            //            movies = orderedMovies.ToList();
            //    }
            //}

            // Pagination
            var pagedMovies = movies
                .Skip((request.request.Page - 1) * request.request.PageSize)
                .Take(request.request.PageSize)
                .ToList();

            // Map the results to DTOs
            var movieDtos = pagedMovies.Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description ?? "",
                    ReleaseDate = m.ReleaseDate,
                    DurationMinutes = m.DurationMinutes,
                    Rating = m.Rating,
                    Actors = string.IsNullOrWhiteSpace(m.ActorNames)
                    ? new List<string>()
                    : m.ActorNames.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList()
                }).ToList();

            var response = new MovieResponseDto
            {
                Movies = movieDtos,
                TotalCount = movies.Count // total before pagination
            };

            return Result<MovieResponseDto>.Success(response);
        }
        }
}



