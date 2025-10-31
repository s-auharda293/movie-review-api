CREATE PROCEDURE UpdateActor
    @Id UNIQUEIDENTIFIER, 
    @Name NVARCHAR(100),
    @Bio NVARCHAR(4000) = NULL,
    @DateOfBirth DATETIME2 = NULL,
    @MovieIds NVARCHAR(MAX) = NULL -- comma-separated movie GUIDs
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

    -- Update actor info
    UPDATE Actors
    SET Name = @Name,
        Bio = @Bio,
        DateOfBirth = @DateOfBirth,
        UpdatedAt = @UpdatedAt
    WHERE Id = @Id;

    -- Remove old actor-movie links
    DELETE FROM ActorMovie WHERE ActorId = @Id;

    -- Insert new actor-movie links
    IF @MovieIds IS NOT NULL AND LEN(@MovieIds) > 0
    BEGIN
        INSERT INTO ActorMovie (ActorId, MovieId)
        SELECT @Id, CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@MovieIds, ',');

        -- Update Actor's MovieTitlesCache
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

        -- Update Movies' ActorNamesCache
        UPDATE m
        SET m.ActorNamesCache = agg.ActorNames
        FROM Movies m
        JOIN (
            SELECT am.MovieId,
                   STRING_AGG(a2.Name, ', ') AS ActorNames
            FROM ActorMovie am
            JOIN Actors a2 ON a2.Id = am.ActorId
            WHERE am.MovieId IN (SELECT CAST(value AS UNIQUEIDENTIFIER) FROM STRING_SPLIT(@MovieIds, ','))
            GROUP BY am.MovieId
        ) agg ON m.Id = agg.MovieId;
    END

    -- Return updated actor info
    SELECT Id, Name, Bio, DateOfBirth, UpdatedAt,  
    Status, ProposedBy, ProposedAt, StatusChangedBy, StatusChangedAt
    FROM Actors
    WHERE Id = @Id;
END
