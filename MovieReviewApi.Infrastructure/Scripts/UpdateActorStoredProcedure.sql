     CREATE PROCEDURE UpdateActor
    @Id UNIQUEIDENTIFIER, 
    @Name NVARCHAR(4000),
    @Bio NVARCHAR(4000) = NULL,
    @DateOfBirth DATETIME2 = NULL,
    @MovieIds NVARCHAR(MAX) = NULL -- comma-separated movie GUIDs
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

     UPDATE Actors
            SET Name = @Name,
                Bio = @Bio,
                DateOfBirth = @DateOfBirth,
                UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @Id;

     DELETE FROM ActorMovie WHERE ActorId = @Id;

    -- Insert actor-movie links if MovieIds provided
    IF @MovieIds IS NOT NULL AND LEN(@MovieIds) > 0
    BEGIN

        INSERT INTO ActorMovie (ActorId, MovieId)
        SELECT @Id, CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@MovieIds, ',');

    END

    -- Return actor info
    SELECT Id, Name, Bio,DateOfBirth,
           @UpdatedAt AS UpdatedAt 
            FROM Actors
            WHERE Id=@Id;
END