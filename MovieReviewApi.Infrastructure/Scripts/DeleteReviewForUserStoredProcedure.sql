CREATE PROCEDURE DeleteReviewForUser
    @Id UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MovieId UNIQUEIDENTIFIER;

    SELECT @MovieId = MovieId
    FROM Reviews
    WHERE Id = @Id;

    -- Delete review only if it belongs to the user
    DELETE FROM Reviews
    WHERE Id = @Id AND UserId = @UserId;

    UPDATE Movies
    SET Rating = (
        SELECT AVG(Rating)
        FROM Reviews
        WHERE MovieId = @MovieId
          AND Rating IS NOT NULL
    )
    WHERE Id = @MovieId;

    -- Return the number of affected rows
    SELECT @@ROWCOUNT AS AffectedRows;
END
