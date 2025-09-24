using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using System.Data;

public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, Result<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDbConnectionFactory _connection;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateReviewHandler(IApplicationDbContext context, IDbConnectionFactory connection, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _connection = connection;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ReviewDto>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        if (review == null)
            return Result<ReviewDto>.Failure(ReviewErrors.NotFound);

        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userName = _httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Result<ReviewDto>.Failure(ReviewErrors.UserNotAuthenticated);

        // Only owner or admin can update
        if (review.UserId!.ToString() != userId && userRole != "Admin")
            return Result<ReviewDto>.Failure(ReviewErrors.UserNotAuthorized);

        var connection = await _connection.CreateConnectionAsync(cancellationToken);

        var parameters = new DynamicParameters();
        parameters.Add("@Id", request.Id, DbType.Guid);
        parameters.Add("@Comment", request.dto.Comment, DbType.String);
        parameters.Add("@Rating", request.dto.Rating, DbType.Decimal);


        var updatedReview = await connection.QueryFirstAsync<ReviewDto>(
            "UpdateReview",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        updatedReview.UserName = userName;

        return Result<ReviewDto>.Success(updatedReview);
    }
}
