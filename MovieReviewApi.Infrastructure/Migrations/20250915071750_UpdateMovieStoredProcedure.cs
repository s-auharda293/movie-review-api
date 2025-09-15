using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovieStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE UpdateMovie
                @Id UNIQUEIDENTIFIER,
                @Title NVARCHAR(4000),
                @Description NVARCHAR(MAX) = NULL,
                @ReleaseDate DATETIME2 = NULL,
                @DurationMinutes INT = NULL,
                @Rating DECIMAL(3,1) = NULL,
                @ActorIds NVARCHAR(MAX) = NULL -- comma-separated actor GUIDs
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @MovieId UNIQUEIDENTIFIER = @Id;
                DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

                -- Update movie
                UPDATE Movies
                SET Title = @Title,
                    Description = @Description,
                    ReleaseDate = COALESCE(@ReleaseDate, ReleaseDate),
                    DurationMinutes = @DurationMinutes,
                    Rating = @Rating,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @MovieId;

                -- Refresh movie-actor links
                DELETE FROM ActorMovie WHERE MovieId = @MovieId;

                IF @ActorIds IS NOT NULL AND LEN(@ActorIds) > 0
                BEGIN
                    INSERT INTO ActorMovie (MovieId, ActorId)
                    SELECT @MovieId, CAST(value AS UNIQUEIDENTIFIER)
                    FROM STRING_SPLIT(@ActorIds, ',');
                END

                -- Return updated movie
                SELECT Id, Title, Description, ReleaseDate, DurationMinutes, Rating, CreatedAt, UpdatedAt
                FROM Movies
                WHERE Id = @MovieId;
            END
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS UpdateMovie");
        }
    }
}
