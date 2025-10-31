CREATE PROCEDURE UpdateReview
    @Id UNIQUEIDENTIFIER,
    @Comment NVARCHAR(4000),
    @Rating DECIMAL(4,1)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MovieId UNIQUEIDENTIFIER;

    SELECT @MovieId = MovieId
    FROM Reviews
    WHERE Id = @Id;

    UPDATE Reviews
    SET Comment = @Comment,
        Rating = @Rating,
        UpdatedAt = SYSUTCDATETIME()
    WHERE Id = @Id;

     UPDATE Movies
    SET Rating = (
        SELECT AVG(Rating)
        FROM Reviews
        WHERE MovieId = @MovieId
          AND Rating IS NOT NULL
    )
    WHERE Id = @MovieId;

    SELECT Id, MovieId, Comment, Rating, UpdatedAt
    FROM Reviews
    WHERE Id = @Id;
END
