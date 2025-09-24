CREATE PROCEDURE DeleteReviewForUser
    @Id UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete review only if it belongs to the user
    DELETE FROM Reviews
    WHERE Id = @Id AND UserId = @UserId;

    -- Return the number of affected rows
    SELECT @@ROWCOUNT AS AffectedRows;
END
