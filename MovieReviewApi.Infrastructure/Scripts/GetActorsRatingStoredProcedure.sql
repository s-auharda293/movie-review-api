CREATE PROCEDURE GetActorsRating
AS
BEGIN
    SET NOCOUNT ON;

   ;WITH ActorAvg AS (
    SELECT am.ActorId, AVG(m.Rating) AS AvgRating
    FROM ActorMovie am
    INNER JOIN Movies m ON am.MovieId = m.Id
    GROUP BY am.ActorId
    )

    SELECT 
       a.Name AS ActorName,
       COALESCE(aa.AvgRating, 0) AS AverageRating
        FROM Actors a
        LEFT JOIN ActorAvg aa ON a.Id = aa.ActorId
        ORDER BY a.Name;


END
