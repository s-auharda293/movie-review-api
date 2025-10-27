  CREATE PROCEDURE UpdateMovie
                @Id UNIQUEIDENTIFIER,
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

                DECLARE @MovieId UNIQUEIDENTIFIER = @Id;
                DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

                -- Update movie
                UPDATE Movies
                SET Title = @Title,
                    Description = @Description,
                    ReleaseDate = COALESCE(@ReleaseDate, ReleaseDate),
                    DurationMinutes = @DurationMinutes,
                    Rating = @Rating,
                    UpdatedAt = @UpdatedAt,
                    Url = @Url
                WHERE Id = @MovieId;

                -- Refresh movie-actor links
                DELETE FROM ActorMovie WHERE MovieId = @MovieId;

                   IF @ActorIds IS NOT NULL AND LEN(@ActorIds) > 0
    BEGIN
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

                -- Return updated movie
                SELECT Id, Title, Description, ReleaseDate, DurationMinutes, Rating, CreatedAt, UpdatedAt
                FROM Movies
                WHERE Id = @MovieId;
            END