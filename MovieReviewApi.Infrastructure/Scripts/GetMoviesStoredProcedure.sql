CREATE PROCEDURE GetMovies
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  
        m.Id,
        m.Title,
        m.Description,
        m.ReleaseDate,
        m.DurationMinutes,
        m.Rating,
        ISNULL(STRING_AGG(CAST(a.Id AS VARCHAR(36)), ','), '') AS ActorIds
    FROM Movies m
    LEFT JOIN ActorMovie am on m.Id = am.MovieId
    LEFT JOIN Actors a on am.ActorId = a.Id
    GROUP BY 
        m.Id,
        m.Title,
        m.Description,
        m.ReleaseDate,
        m.DurationMinutes,
        m.Rating
END
