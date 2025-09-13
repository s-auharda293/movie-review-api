using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApi.Infrastructure.Migrations
{
    public partial class CreateActorStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROCEDURE CreateActor
    @Bio NVARCHAR(4000) = NULL,
    @DateOfBirth DATETIME2 = NULL,
    @Name NVARCHAR(4000)
AS
BEGIN
    SET NOCOUNT ON;

    -- Generate a new Id
    DECLARE @Id UNIQUEIDENTIFIER = NEWID();

    BEGIN TRANSACTION;
    BEGIN TRY
        -- Insert into BaseEntity with timestamps
        INSERT INTO [BaseEntity] ([Id], [CreatedAt], [UpdatedAt])
        VALUES (@Id, GETDATE(), GETDATE());

        -- Insert into Actors table
        INSERT INTO [Actors] ([Id], [Bio], [DateOfBirth], [Name])
        VALUES (@Id, @Bio, @DateOfBirth, @Name);

        -- Return the newly created actor with timestamps
        SELECT 
            a.[Id],
            a.[Name],
            a.[Bio],
            a.[DateOfBirth],
            b.[CreatedAt],
            b.[UpdatedAt]
        FROM [Actors] a
        INNER JOIN [BaseEntity] b ON a.[Id] = b.[Id]
        WHERE a.[Id] = @Id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END

        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateActor");
        }
    }
}
