CREATE PROCEDURE PatchReview
    @Id UNIQUEIDENTIFIER,
    @Comment NVARCHAR(4000) = NULL,
    @Rating DECIMAL(3,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Reviews
    SET 
        Comment = COALESCE(@Comment, Comment),
        Rating = COALESCE(@Rating, Rating),
        UpdatedAt = SYSUTCDATETIME()
    WHERE Id = @Id;

    -- Return the updated row
    SELECT Id, MovieId, Comment, Rating, UpdatedAt
    FROM Reviews
    WHERE Id = @Id;
END
