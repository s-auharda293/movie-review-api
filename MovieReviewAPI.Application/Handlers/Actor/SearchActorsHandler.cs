using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Actor;
using System.Text.Json;

namespace MovieReviewApi.Application.Handlers.Movie
{
    public class SearchActorsHandler : IRequestHandler<SearchActorsQuery, Result<ActorResponseDto>>
    {
        private readonly IApplicationDbContext _context;

        public SearchActorsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ActorResponseDto>> Handle(SearchActorsQuery request, CancellationToken cancellationToken)
        {
            // Keep as IQueryable to allow EF to translate to SQL
            var actorsQuery = _context.Actors.Include(a => a.Movies).AsQueryable();

            var searchTerm = request.request.SearchTerm;
            var searchColumn = request.request.SearchColumn;

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm) && !string.IsNullOrWhiteSpace(searchColumn))
            {
                switch (searchColumn.ToLower())
                {
                    case "id":
                        if (Guid.TryParse(searchTerm, out var id)) //rename the parsed guid searcTerm to id
                            actorsQuery = actorsQuery.Where(a => a.Id == id);
                        break;

                    case "name":
                        actorsQuery = actorsQuery.Where(a => a.Name.Contains(searchTerm));
                        break;

                    case "bio":
                        actorsQuery = actorsQuery.Where(a => a.Bio!.Contains(searchTerm));
                        break;

                    case "dateofbirth":
                        if (DateTime.TryParse(searchTerm, out var date))
                            actorsQuery = actorsQuery.Where(a => a.DateOfBirth.HasValue && a.DateOfBirth.Value.Date == date.Date);
                        break;
                }
            }

            // Sorting (single-column)
            if (!string.IsNullOrWhiteSpace(request.request.Sort))
            {
                var sortList = JsonSerializer.Deserialize<List<SortDto>>(request.request.Sort);
                var sort = sortList?.LastOrDefault();

                if (sort != null)
                {
                    bool ascending = sort.Dir.Equals("asc", StringComparison.OrdinalIgnoreCase);
                    switch (sort.Field.ToLower())
                    {
                        case "name":
                            actorsQuery = ascending 
                                ? actorsQuery.OrderBy(a => a.Name) 
                                : actorsQuery.OrderByDescending(a => a.Name);
                            break;

                        case "bio":
                            actorsQuery = ascending 
                                ? actorsQuery.OrderBy(a => a.Bio) 
                                : actorsQuery.OrderByDescending(a => a.Bio);
                            break;

                        case "dateofbirth":
                            actorsQuery = ascending 
                                ? actorsQuery.OrderBy(a => a.DateOfBirth) 
                                : actorsQuery.OrderByDescending(a => a.DateOfBirth);
                            break;
                    }
                }
            }

            // Total count before pagination
            var totalCount = await actorsQuery.CountAsync(cancellationToken);

            // Pagination
            var pagedActors = await actorsQuery
                .Skip((request.request.Page - 1) * request.request.PageSize)
                .Take(request.request.PageSize)
                .ToListAsync(cancellationToken);

            // Map to DTOs
            var actorDtos = pagedActors.Select(a => new ActorDto
            {
                Id = a.Id,
                Name = a.Name,
                Bio = a.Bio,
                DateOfBirth = a.DateOfBirth,
                Movies = a.Movies?
                .Select(m => new ActorMovieDto { Id = m.Id, Title = m.Title })
                .ToList() ?? new List<ActorMovieDto>()
            }).ToList();


            // Response
            var response = new ActorResponseDto
            {
                Actors = actorDtos,
                TotalCount = totalCount
            };

            return Result<ActorResponseDto>.Success(response);
        }
    }
}
