using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateActorStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE CreateActor
                @Name NVARCHAR(4000),
                @Bio NVARCHAR(4000) = NULL,
                @DateOfBirth DATETIME2 = NULL
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @Id UNIQUEIDENTIFIER = NEWID();
                DECLARE @CreatedAt DATETIME2 = SYSUTCDATETIME();
                DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

                INSERT INTO Actors (Id, Name, Bio, DateOfBirth, CreatedAt, UpdatedAt)
                VALUES (@Id, @Name, @Bio, @DateOfBirth, @CreatedAt, @UpdatedAt);

                SELECT @Id AS Id, @Name AS Name, @Bio AS Bio, @DateOfBirth AS DateOfBirth,
                       @CreatedAt AS CreatedAt, @UpdatedAt AS UpdatedAt;
            END
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS CreateActor");
        }
    }
}
