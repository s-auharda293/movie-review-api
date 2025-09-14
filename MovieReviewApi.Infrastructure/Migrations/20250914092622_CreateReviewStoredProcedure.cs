using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateReviewStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROCEDURE CreateReview
    @MovieId UNIQUEIDENTIFIER,
    @UserName NVARCHAR(4000),
    @Comment NVARCHAR(4000) = NULL,
    @Rating DECIMAL(3,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ReviewId UNIQUEIDENTIFIER = NEWID();
    DECLARE @CreatedAt DATETIME2 = SYSUTCDATETIME();
    DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

    INSERT INTO Reviews (Id, MovieId, UserName, Comment, Rating, CreatedAt, UpdatedAt)
    VALUES (@ReviewId, @MovieId, @UserName, @Comment, @Rating, @CreatedAt, @UpdatedAt);

    -- Return inserted review
    SELECT @ReviewId AS Id,
           @MovieId AS MovieId,
           @UserName AS UserName,
           @Comment AS Comment,
           @Rating AS Rating,
           @CreatedAt AS CreatedAt,
           @UpdatedAt AS UpdatedAt;
END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS CreateReview");
        }
    }
}
