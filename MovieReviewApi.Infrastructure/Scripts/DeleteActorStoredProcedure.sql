CREATE PROCEDURE DeleteActor
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete actor-movie links first
    DELETE FROM ActorMovie
    WHERE ActorId = @Id;

    -- Update ActorNamesCache in all movies that had actors removed
    UPDATE m
    SET m.ActorNamesCache = agg.ActorNames
    FROM Movies m
    JOIN (
        SELECT am.MovieId,
               STRING_AGG(a2.Name, ', ') AS ActorNames
        FROM ActorMovie am
        JOIN Actors a2 ON a2.Id = am.ActorId
        GROUP BY am.MovieId
    ) agg ON m.Id = agg.MovieId;

    -- Delete the actor
    DELETE FROM Actors
    WHERE Id = @Id;
END
