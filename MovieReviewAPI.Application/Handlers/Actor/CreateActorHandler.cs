using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Interfaces.Identity;
using MovieReviewApi.Domain.Common.Actors;
using MovieReviewApi.Domain.Entities;
using MovieReviewApi.Domain.Enums;
using System.Data;
using System.Security.Claims;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class CreateActorHandler : IRequestHandler<CreateActorCommand, Result<ActorWithMoviesDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDbConnectionFactory _connection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

        public CreateActorHandler(IApplicationDbContext context, IDbConnectionFactory connection, IHttpContextAccessor httpContextAccessor, ICurrentUserService currentUserService)
        {
            _context = context;
            _connection = connection;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
        }

        public async Task<Result<ActorWithMoviesDto>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
        {

            var connection = await _connection.CreateConnectionAsync(cancellationToken);

            var userId = Guid.Parse(_currentUserService.GetUserId()!);
            var role = _currentUserService.GetUserRole();

            var proposedAt = DateTime.UtcNow;
            Guid? statusChangedBy = null;
            DateTime? statusChangedAt= null;
            ProposalStatus status = ProposalStatus.Pending;

            if (role is "Admin" or "Moderator")
            {
                status = ProposalStatus.Approved;
                statusChangedBy = userId;
                statusChangedAt = DateTime.UtcNow;
            }


            //     public ProposalStatus Status { get; set; } = ProposalStatus.Pending;
            //public Guid? RejectedBy { get; set; }
            //public DateTime? RejectedAt { get; set; }


            List<string> movieTitles = new();

            string? movieIdsCsv = null;

            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
            {
                var movies = await _context.Movies.Where(m => request.dto.MovieIds.Contains(m.Id)).ToListAsync(cancellationToken);
                if (movies.Count != request.dto.MovieIds.Count)
                {
                    var invalidIds = request.dto.MovieIds.Except(movies.Select(m => m.Id)).ToList();
                    return Result<ActorWithMoviesDto>.Failure(ActorErrors.MoviesNotFound(invalidIds));
                }
                movieIdsCsv = string.Join(",", request.dto.MovieIds);
            }

            // Prepare parameters
            var parameters = new DynamicParameters();
            parameters.Add("@Name", request.dto.Name, DbType.String);
            parameters.Add("@Bio", request.dto.Bio, DbType.String);
            parameters.Add("@DateOfBirth", request.dto.DateOfBirth, DbType.DateTime2);
            parameters.Add("@MovieIds", movieIdsCsv, DbType.String);
            parameters.Add("@Status", status.ToString(), DbType.String);
            parameters.Add("@ProposedBy", userId, DbType.Guid);
            parameters.Add("@ProposedAt", proposedAt, DbType.DateTime2);
            parameters.Add("@StatusChangedBy", statusChangedBy ?? null, DbType.Guid);
            parameters.Add("@StatusChangedAt", statusChangedAt ?? null, DbType.DateTime2);

            // Execute stored procedure and get the first row
            var actor = await connection.QueryFirstAsync<dynamic>(
                "CreateActor",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (request.dto.MovieIds != null && request.dto.MovieIds.Any())
            {
                movieTitles = await _context.Movies
                    .Where(m => request.dto.MovieIds.Contains(m.Id))
                    .Select(m => m.Title)
                    .ToListAsync(cancellationToken);
            }


            var actorDto = new ActorWithMoviesDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                DateOfBirth = actor.DateOfBirth,
                MovieTitles = movieTitles,
                ProposedAt = actor.ProposedAt,
                Status = actor.Status,
                StatusChangedAt = actor.StatusChangedAt
            };

            return Result<ActorWithMoviesDto>.Success(actorDto);
        }
    }
}