using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PatchMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE PatchMovie
                @Id UNIQUEIDENTIFIER,
                @Title NVARCHAR(4000) = NULL,
                @Description NVARCHAR(MAX) = NULL,
                @ReleaseDate DATETIME2 = NULL,
                @DurationMinutes INT = NULL,
                @Rating DECIMAL(3,1) = NULL,
                @ActorIds NVARCHAR(MAX) = NULL -- comma-separated actor GUIDs
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @MovieId UNIQUEIDENTIFIER = @Id;

                -- Update only fields provided
                UPDATE Movies
                SET Title = COALESCE(@Title, Title),
                    Description = COALESCE(@Description, Description),
                    ReleaseDate = COALESCE(@ReleaseDate, ReleaseDate),
                    DurationMinutes = COALESCE(@DurationMinutes, DurationMinutes),
                    Rating = COALESCE(@Rating, Rating),
                    UpdatedAt = SYSUTCDATETIME()
                WHERE Id = @MovieId;

                -- If ActorIds are passed, refresh links
                IF @ActorIds IS NOT NULL
                BEGIN
                    -- Clear existing links first
                    DELETE FROM ActorMovie WHERE MovieId = @MovieId;

                    -- Insert new ones only if not empty string
                    IF LEN(@ActorIds) > 0
                    BEGIN
                        INSERT INTO ActorMovie (MovieId, ActorId)
                        SELECT @MovieId, CAST(value AS UNIQUEIDENTIFIER)
                        FROM STRING_SPLIT(@ActorIds, ',');
                    END
                END

                -- Return the updated row
                SELECT Id, Title, Description, ReleaseDate, DurationMinutes, Rating, CreatedAt, UpdatedAt
                FROM Movies
                WHERE Id = @MovieId;
            END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS PatchMovie");
        }
    }
}
