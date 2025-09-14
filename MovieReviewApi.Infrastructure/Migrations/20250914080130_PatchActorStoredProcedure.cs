using Microsoft.EntityFrameworkCore.Migrations;


namespace MovieReviewApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PatchActorStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
           CREATE PROCEDURE PatchActor
    @Id UNIQUEIDENTIFIER, 
    @Name NVARCHAR(4000) = NULL,
    @Bio NVARCHAR(4000) = NULL,
    @DateOfBirth DATETIME2 = NULL,
    @MovieIds NVARCHAR(MAX) = NULL -- comma-separated movie GUIDs
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

    -- Update only the fields provided (PATCH behavior)
    UPDATE Actors
    SET 
        Name = COALESCE(@Name, Name),
        Bio = COALESCE(@Bio, Bio),
        DateOfBirth = COALESCE(@DateOfBirth, DateOfBirth),
        UpdatedAt = @UpdatedAt
    WHERE Id = @Id;

    -- Handle movies only if provided
    IF @MovieIds IS NOT NULL
    BEGIN
        -- Clear existing links first
        DELETE FROM ActorMovie WHERE ActorId = @Id;

        -- If @MovieIds is empty, it means remove all links
        -- If not empty, insert the new links
        IF LEN(@MovieIds) > 0
        BEGIN
            INSERT INTO ActorMovie (ActorId, MovieId)
            SELECT @Id, CAST(value AS UNIQUEIDENTIFIER)
            FROM STRING_SPLIT(@MovieIds, ',');
        END
    END

    -- Return the updated actor row
    SELECT Id, Name, Bio, DateOfBirth, CreatedAt, UpdatedAt
    FROM Actors
    WHERE Id = @Id;
END
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS PatchActor");
        }
    }
}
