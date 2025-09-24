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

public class PatchReviewHandler : IRequestHandler<PatchReviewCommand, Result<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDbConnectionFactory _connection;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PatchReviewHandler(IApplicationDbContext context, IDbConnectionFactory connection, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _connection = connection;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ReviewDto>> Handle(PatchReviewCommand request, CancellationToken cancellationToken)
    {
        var httpUser = _httpContextAccessor.HttpContext?.User;
        var userId = httpUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = httpUser?.FindFirst(ClaimTypes.Role)?.Value;
        var userName = _httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Result<ReviewDto>.Failure(ReviewErrors.UserNotAuthenticated);

        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        if (review == null)
            return Result<ReviewDto>.Failure(ReviewErrors.NotFound);

        if (review.UserId!.ToString() != userId && userRole != "Admin") //using foreign key to directly access UserId
            return Result<ReviewDto>.Failure(ReviewErrors.UserNotAuthorized);

        var connection = await _connection.CreateConnectionAsync(cancellationToken);

        var parameters = new DynamicParameters();
        parameters.Add("@Id", request.Id, DbType.Guid);
        parameters.Add("@Comment", string.IsNullOrEmpty(request.dto.Comment) ? null : request.dto.Comment, DbType.String);
        parameters.Add("@Rating", request.dto.Rating, DbType.Decimal);

        var updatedReview = await connection.QueryFirstAsync<ReviewDto>(
            "PatchReview",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        updatedReview.UserName = userName;

        return Result<ReviewDto>.Success(updatedReview);
    }
}
