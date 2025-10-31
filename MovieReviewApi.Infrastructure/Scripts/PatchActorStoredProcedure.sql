CREATE PROCEDURE PatchActor
    @Id UNIQUEIDENTIFIER, 
    @Name NVARCHAR(100) = NULL,
    @Bio NVARCHAR(4000) = NULL,
    @DateOfBirth DATETIME2 = NULL,
    @MovieIds NVARCHAR(MAX) = NULL -- comma-separated movie GUIDs
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

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
        DELETE FROM ActorMovie WHERE ActorId = @Id;

        -- Insert new links if any
        IF LEN(@MovieIds) > 0
        BEGIN
            INSERT INTO ActorMovie (ActorId, MovieId)
            SELECT @Id, CAST(value AS UNIQUEIDENTIFIER)
            FROM STRING_SPLIT(@MovieIds, ',');
        END
    END

     UPDATE a
    SET a.MovieTitlesCache = agg.MovieTitles
    FROM Actors a
    JOIN (
        SELECT am.ActorId,
               STRING_AGG(m.Title, ', ') AS MovieTitles
        FROM ActorMovie am
        JOIN Movies m ON m.Id = am.MovieId
        WHERE am.ActorId = @Id
        GROUP BY am.ActorId
    ) agg ON a.Id = agg.ActorId;

    UPDATE m
    SET m.ActorNamesCache = agg.ActorNames
    FROM Movies m
    JOIN (
        SELECT am.MovieId,
               STRING_AGG(a2.Name, ', ') AS ActorNames
        FROM ActorMovie am
        JOIN Actors a2 ON a2.Id = am.ActorId
        WHERE am.ActorId = @Id
        GROUP BY am.MovieId
    ) agg ON m.Id = agg.MovieId;

    SELECT Id, Name, Bio, DateOfBirth, CreatedAt, UpdatedAt, MovieTitlesCache,
    Status, ProposedBy, ProposedAt, StatusChangedBy, StatusChangedAt
    FROM Actors
    WHERE Id = @Id;
END
