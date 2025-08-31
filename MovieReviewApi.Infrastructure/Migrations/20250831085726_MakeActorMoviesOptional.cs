using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeActorMoviesOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorMovie_Actors_ActorsId",
                table: "ActorMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorMovie_Movies_MoviesId",
                table: "ActorMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreMovie_Genres_GenresId",
                table: "GenreMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreMovie_Movies_MoviesId",
                table: "GenreMovie");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "BaseEntity");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "BaseEntity",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BaseEntity",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "BaseEntity",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "BaseEntity",
                type: "decimal(2,1)",
                precision: 2,
                scale: 1,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldPrecision: 2,
                oldScale: 1);

            migrationBuilder.AlterColumn<int>(
                name: "DurationMinutes",
                table: "BaseEntity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BaseEntity",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "BaseEntity",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "BaseEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "BaseEntity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BaseEntity",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genre_Name",
                table: "BaseEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BaseEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaseEntity",
                table: "BaseEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorMovie_BaseEntity_ActorsId",
                table: "ActorMovie",
                column: "ActorsId",
                principalTable: "BaseEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorMovie_BaseEntity_MoviesId",
                table: "ActorMovie",
                column: "MoviesId",
                principalTable: "BaseEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenreMovie_BaseEntity_GenresId",
                table: "GenreMovie",
                column: "GenresId",
                principalTable: "BaseEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GenreMovie_BaseEntity_MoviesId",
                table: "GenreMovie",
                column: "MoviesId",
                principalTable: "BaseEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorMovie_BaseEntity_ActorsId",
                table: "ActorMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorMovie_BaseEntity_MoviesId",
                table: "ActorMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreMovie_BaseEntity_GenresId",
                table: "GenreMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreMovie_BaseEntity_MoviesId",
                table: "GenreMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaseEntity",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Genre_Name",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BaseEntity");

            migrationBuilder.RenameTable(
                name: "BaseEntity",
                newName: "Movies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Movies",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Movies",
                type: "decimal(2,1)",
                precision: 2,
                scale: 1,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldPrecision: 2,
                oldScale: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DurationMinutes",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Movies",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ActorMovie_Actors_ActorsId",
                table: "ActorMovie",
                column: "ActorsId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorMovie_Movies_MoviesId",
                table: "ActorMovie",
                column: "MoviesId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenreMovie_Genres_GenresId",
                table: "GenreMovie",
                column: "GenresId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenreMovie_Movies_MoviesId",
                table: "GenreMovie",
                column: "MoviesId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
