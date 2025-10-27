CREATE PROCEDURE DeleteMovie
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete movie-actor links first
    DELETE FROM ActorMovie 
    WHERE MovieId = @Id;

    -- Update MovieTitlesCache for all actors
    UPDATE a
    SET a.MovieTitlesCache = agg.MovieTitles
    FROM Actors a
    JOIN (
        SELECT am.ActorId,
               STRING_AGG(m.Title, ', ') AS MovieTitles
        FROM ActorMovie am
        JOIN Movies m ON m.Id = am.MovieId
        GROUP BY am.ActorId
    ) agg ON a.Id = agg.ActorId;

    -- Delete the movie itself
    DELETE FROM Movies 
    WHERE Id = @Id;
END
