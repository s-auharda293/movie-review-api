CREATE PROCEDURE DeleteMovie
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM ActorMovie 
    WHERE MovieId = @Id;

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

    DELETE FROM Movies 
    WHERE Id = @Id;
END
