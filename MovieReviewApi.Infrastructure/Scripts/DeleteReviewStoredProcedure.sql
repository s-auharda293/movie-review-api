CREATE PROCEDURE DeleteReview
                    @Id UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;

--Delete the actor
                    DELETE FROM Reviews
                    WHERE Id = @Id;
END