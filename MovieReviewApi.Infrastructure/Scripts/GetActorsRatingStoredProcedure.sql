CREATE PROCEDURE GetActorsRating
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        a.Id AS ActorId,
        a.Name AS ActorName,
        ROUND(AVG(m.Rating), 1) AS AverageRating
    FROM Actors a
    LEFT JOIN ActorMovie am ON a.Id = am.ActorId
    LEFT JOIN Movies m ON am.MovieId = m.Id
    GROUP BY a.Id, a.Name
    ORDER BY a.Name;
END
