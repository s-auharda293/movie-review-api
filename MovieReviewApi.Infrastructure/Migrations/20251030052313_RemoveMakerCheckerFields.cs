using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMakerCheckerFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_Status",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ProposedAt",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ProposedBy",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "RejectedBy",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Movies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "Movies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProposedAt",
                table: "Movies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProposedBy",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                table: "Movies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RejectedBy",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Status",
                table: "Movies",
                column: "Status");
        }
    }
}
