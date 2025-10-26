CREATE PROCEDURE CreateActor
    @Name NVARCHAR(100),
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

        -- Update Actor's MovieTitlesCache using GROUP BY
        UPDATE a
        SET a.MovieTitlesCache = agg.MovieTitles
        FROM Actors a
        JOIN (
            SELECT am.ActorId,
                   STRING_AGG(m.Title, ', ') AS MovieTitles
            FROM ActorMovie am
            JOIN Movies m ON m.Id = am.MovieId
            WHERE am.ActorId = @ActorId
            GROUP BY am.ActorId
        ) agg ON a.Id = agg.ActorId;

        -- Update Movie's ActorNamesCache using GROUP BY
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

    -- Return actor info
    SELECT @ActorId AS Id, @Name AS Name, @Bio AS Bio, @DateOfBirth AS DateOfBirth,
           @CreatedAt AS CreatedAt, @UpdatedAt AS UpdatedAt;
END
