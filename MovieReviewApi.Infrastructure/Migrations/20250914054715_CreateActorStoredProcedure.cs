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
    @DateOfBirth DATETIME2 = NULL,
    @MovieIds NVARCHAR(MAX) = NULL -- comma-separated movie GUIDs
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert actor
    DECLARE @ActorId UNIQUEIDENTIFIER = NEWID();
    DECLARE @CreatedAt DATETIME2 = SYSUTCDATETIME();
    DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

    INSERT INTO Actors (Id, Name, Bio, DateOfBirth, CreatedAt, UpdatedAt)
    VALUES (@ActorId, @Name, @Bio, @DateOfBirth, @CreatedAt, @UpdatedAt);

    -- Insert actor-movie links if MovieIds provided
    IF @MovieIds IS NOT NULL AND LEN(@MovieIds) > 0
    BEGIN
        INSERT INTO ActorMovie (ActorId, MovieId)
        SELECT @ActorId, CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@MovieIds, ',');
    END

    -- Return actor info
    SELECT @ActorId AS Id, @Name AS Name, @Bio AS Bio, @DateOfBirth AS DateOfBirth,
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
