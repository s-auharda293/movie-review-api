using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReviewStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE UpdateReview
                    @Id UNIQUEIDENTIFIER,
                    @UserName NVARCHAR(4000),
                    @Comment NVARCHAR(4000),
                    @Rating DECIMAL(3,2) 
                AS
                BEGIN
                    SET NOCOUNT ON;

                UPDATE Reviews SET 
                    UserName=@UserName,
                    Comment=@Comment,
                    Rating=@Rating,
                    UpdatedAt=SYSUTCDATETIME()
                WHERE Id=@Id;

                SELECT Id, MovieId, UserName, Comment, Rating, UpdatedAt
                FROM Reviews
                WHERE Id = @Id;
END

            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP PROCEDURE IF EXISTS UpdateReview
        ");
        }
    }
}
