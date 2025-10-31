CREATE PROCEDURE DeleteReview
                    @Id UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DELETE FROM Reviews
                    WHERE Id = @Id;
END