using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Review;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Domain.Entities;
using System.Data;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDbConnectionFactory _connection;

    public CreateReviewHandler(IApplicationDbContext context, IDbConnectionFactory connection)
    {
        _context = context;
        _connection = connection;
    }

    public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == request.dto.MovieId, cancellationToken);
        if (movie == null)
            return Result<ReviewDto>.Failure(ReviewErrors.MovieNotFound);

        var connection = await _connection.CreateConnectionAsync(cancellationToken);

        // Prepare parameters for stored procedure
        var parameters = new DynamicParameters();
        parameters.Add("@MovieId", request.dto.MovieId, DbType.Guid);
        parameters.Add("@UserName", request.dto.UserName, DbType.String);
        parameters.Add("@Comment", request.dto.Comment, DbType.String);
        parameters.Add("@Rating", request.dto.Rating, DbType.Decimal);

        var review = await connection.QueryFirstAsync<dynamic>(
            "CreateReview",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        // Map to DTO
        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            MovieId = review.MovieId,
            UserName = review.UserName,
            Comment = review.Comment,
            Rating = review.Rating
        };

        return Result<ReviewDto>.Success(reviewDto);
    }
}
