using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using System.Data;
using System.Security.Claims;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDbConnectionFactory _connection;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateReviewHandler(
        IApplicationDbContext context,
        IDbConnectionFactory connection,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _connection = connection;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == request.dto.MovieId, cancellationToken);
        if (movie == null)
            return Result<ReviewDto>.Failure(ReviewErrors.MovieNotFound);


        var httpUser = _httpContextAccessor.HttpContext?.User;
        var userId = httpUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = httpUser?.FindFirst(ClaimTypes.Name)?.Value;

        var alreadyReviewed = await _context.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == request.dto.MovieId);
        if (alreadyReviewed!=null) {
            return Result<ReviewDto>.Failure(ReviewErrors.AlreadyReviewed);
        }

        if (string.IsNullOrEmpty(userId))
            return Result<ReviewDto>.Failure(ReviewErrors.UserNotAuthenticated);

        var connection = await _connection.CreateConnectionAsync(cancellationToken);

        var parameters = new DynamicParameters();
        parameters.Add("@MovieId", request.dto.MovieId, DbType.Guid);
        parameters.Add("@UserId", Guid.Parse(userId), DbType.Guid); 
        parameters.Add("@UserName", userName, DbType.String);
        parameters.Add("@Comment", request.dto.Comment, DbType.String);
        parameters.Add("@Rating", request.dto.Rating, DbType.Decimal);

        var review = await connection.QueryFirstAsync<dynamic>(
            "CreateReview",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            MovieId = review.MovieId,
            UserId = review.UserId,
            UserName = userName,
            Comment = review.Comment,
            Rating = review.Rating
        };

        return Result<ReviewDto>.Success(reviewDto);
    }
}
