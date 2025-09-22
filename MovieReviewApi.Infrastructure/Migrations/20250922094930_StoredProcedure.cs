using Microsoft.EntityFrameworkCore.Migrations;
using MovieReviewApi.Infrastructure.Helper;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.CreateActorStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.CreateMovieStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.CreateReviewStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.DeleteActorStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.DeleteMovieStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.DeleteReviewStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.GetMoviesStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.PatchActorStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.PatchMovieStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.PatchReviewStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.UpdateActorStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.UpdateMovieStoredProcedure.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "MovieReviewApi.Infrastructure.Scripts.UpdateReviewStoredProcedure.sql");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateActor");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateActor");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteActor");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS PatchActor");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateMovie");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateMovie");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteMovie");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS PatchMovie");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetMovies");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateReview");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateReview");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteReview");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS PatchReview");

        }
    }
}
