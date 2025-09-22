 CREATE PROCEDURE DeleteActor
                    @Id UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- Delete actor-movie links first
                    DELETE FROM ActorMovie
                    WHERE ActorId = @Id;

                    -- Delete the actor
                    DELETE FROM Actors
                    WHERE Id = @Id;
                END