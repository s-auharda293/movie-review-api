using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex_Movie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Movies_DurationMinutes",
                table: "Movies",
                column: "DurationMinutes");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Rating",
                table: "Movies",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ReleaseDate",
                table: "Movies",
                column: "ReleaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Status",
                table: "Movies",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_DurationMinutes",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_Rating",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ReleaseDate",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_Status",
                table: "Movies");
        }
    }
}
