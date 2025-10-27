CREATE PROCEDURE CreateMovie
    @Title NVARCHAR(4000),
    @Description NVARCHAR(MAX) = NULL,
    @ReleaseDate DATETIME2 = NULL,
    @DurationMinutes INT = NULL,
    @Rating DECIMAL(3,1) = NULL,
    @ActorIds NVARCHAR(MAX) = NULL, -- comma-separated actor GUIDs
    @Url NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MovieId UNIQUEIDENTIFIER = NEWID();
    DECLARE @CreatedAt DATETIME2 = SYSUTCDATETIME();
    DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

    -- Insert movie
    INSERT INTO Movies (Id, Title, Description, ReleaseDate, DurationMinutes, Rating, CreatedAt, UpdatedAt, Url)
    VALUES (@MovieId, @Title, @Description, COALESCE(@ReleaseDate, SYSUTCDATETIME()), @DurationMinutes, @Rating, @CreatedAt, @UpdatedAt, @Url);

    -- Insert movie-actor links if ActorIds provided
   IF @ActorIds IS NOT NULL AND LEN(@ActorIds) > 0
BEGIN
    -- Insert links
    INSERT INTO ActorMovie (MovieId, ActorId)
    SELECT @MovieId, CAST(value AS UNIQUEIDENTIFIER)
    FROM STRING_SPLIT(@ActorIds, ',');

    UPDATE a
    SET a.MovieTitlesCache = agg.MovieTitles
    FROM Actors a
    JOIN (
        SELECT am.ActorId,
               STRING_AGG(m.Title, ', ') AS MovieTitles
        FROM ActorMovie am
        JOIN Movies m ON m.Id = am.MovieId
        WHERE am.ActorId IN (SELECT CAST(value AS UNIQUEIDENTIFIER) FROM STRING_SPLIT(@ActorIds, ','))
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
        WHERE am.MovieId = @MovieId
        GROUP BY am.MovieId
    ) agg ON m.Id = agg.MovieId;
END


    -- Return the created movie row
    SELECT Id, Title, Description, ReleaseDate, DurationMinutes, Rating, CreatedAt, UpdatedAt
    FROM Movies
    WHERE Id = @MovieId;
END
