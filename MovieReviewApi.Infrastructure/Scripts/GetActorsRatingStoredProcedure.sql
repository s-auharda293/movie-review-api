CREATE PROCEDURE GetActorsRating
    @ActorIds NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH FilteredActors AS (
        SELECT CAST(value AS UNIQUEIDENTIFIER) AS ActorId
        FROM STRING_SPLIT(@ActorIds, ',')
    ),
    ActorAvg AS (
        SELECT am.ActorId, CAST(ROUND(AVG(m.Rating), 1) AS DECIMAL(3,1)) AS AvgRating
        FROM ActorMovie am
        INNER JOIN Movies m ON am.MovieId = m.Id
        GROUP BY am.ActorId
    )
    SELECT 
        a.Name AS ActorName,
        COALESCE(aa.AvgRating, 0) AS AverageRating
    FROM Actors a
    INNER JOIN FilteredActors fa ON a.Id = fa.ActorId   
    LEFT JOIN ActorAvg aa ON a.Id = aa.ActorId
    ORDER BY a.Name;
END
