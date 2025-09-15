using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteMovieStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE DeleteMovie
                @Id UNIQUEIDENTIFIER
            AS
            BEGIN
                SET NOCOUNT ON;

                DELETE FROM ActorMovie WHERE MovieId = @Id;

                DELETE FROM Movies WHERE Id = @Id;
            END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS DeleteMovie");
        }
    }
}
