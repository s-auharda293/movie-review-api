CREATE PROCEDURE PatchMovie
    @Id UNIQUEIDENTIFIER,
    @Title NVARCHAR(4000) = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @ReleaseDate DATETIME2 = NULL,
    @DurationMinutes INT = NULL,
    @Rating DECIMAL(3,1) = NULL,
    @ActorIds NVARCHAR(MAX) = NULL, -- comma-separated actor GUIDs
    @Url NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MovieId UNIQUEIDENTIFIER = @Id;

    -- Update only fields provided
    UPDATE Movies
    SET Title = COALESCE(@Title, Title),
        Description = COALESCE(@Description, Description),
        ReleaseDate = COALESCE(@ReleaseDate, ReleaseDate),
        DurationMinutes = COALESCE(@DurationMinutes, DurationMinutes),
        Rating = COALESCE(@Rating, Rating),
        UpdatedAt = SYSUTCDATETIME(),
        Url = COALESCE(@Url, Url)
    WHERE Id = @MovieId;

    -- If ActorIds are passed, refresh links
    IF @ActorIds IS NOT NULL
    BEGIN
        -- Clear existing links first
        DELETE FROM ActorMovie WHERE MovieId = @MovieId;

        -- Insert new ones only if not empty string
        IF LEN(@ActorIds) > 0
        BEGIN
            INSERT INTO ActorMovie (MovieId, ActorId)
            SELECT @MovieId, CAST(value AS UNIQUEIDENTIFIER)
            FROM STRING_SPLIT(@ActorIds, ',');
        END
    END

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

    -- Refresh Actor.MovieTitlesCache for actors linked to this movie
    UPDATE a
    SET a.MovieTitlesCache = agg.MovieTitles
    FROM Actors a
    JOIN (
        SELECT am.ActorId,
               STRING_AGG(m2.Title, ', ') AS MovieTitles
        FROM ActorMovie am
        JOIN Movies m2 ON m2.Id = am.MovieId
        WHERE am.MovieId = @MovieId
        GROUP BY am.ActorId
    ) agg ON a.Id = agg.ActorId;

    -- Return the updated row
    SELECT Id, Title, Description, ReleaseDate, DurationMinutes, Rating, CreatedAt, UpdatedAt
    FROM Movies
    WHERE Id = @MovieId;
END
