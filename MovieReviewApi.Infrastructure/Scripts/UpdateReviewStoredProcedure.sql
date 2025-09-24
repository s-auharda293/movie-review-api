CREATE PROCEDURE UpdateReview
    @Id UNIQUEIDENTIFIER,
    @Comment NVARCHAR(4000),
    @Rating DECIMAL(3,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Reviews
    SET Comment = @Comment,
        Rating = @Rating,
        UpdatedAt = SYSUTCDATETIME()
    WHERE Id = @Id;

    SELECT Id, MovieId, Comment, Rating, UpdatedAt
    FROM Reviews
    WHERE Id = @Id;
END
