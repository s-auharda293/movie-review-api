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
            STRING_AGG(a.Name,', ') AS ActorNames 
            from Movies m
            LEFT JOIN ActorMovie am on m.Id=am.MovieId
            LEFT JOIN Actors a on am.ActorId=a.Id
            GROUP BY 
            m.Id,
            m.Title,
            m.Description,
            m.ReleaseDate,
            m.DurationMinutes,
            m.Rating
            END