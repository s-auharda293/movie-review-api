CREATE PROCEDURE GetMovies
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH ActorIdsPerMovie AS
    (
        SELECT 
            am.MovieId,
            STRING_AGG(CAST(am.ActorId AS VARCHAR(36)), ',') AS ActorIds
        FROM ActorMovie am
        GROUP BY am.MovieId
    )
    SELECT  
        m.Id,
        m.Title,
        m.Description,
        m.ReleaseDate,
        m.DurationMinutes,
        m.Rating,
        ISNULL(a.ActorIds, '') AS ActorIds,
        m.Url
    FROM Movies m
    LEFT JOIN ActorIdsPerMovie a ON m.Id = a.MovieId;
END
GO
