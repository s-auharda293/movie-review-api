 CREATE PROCEDURE DeleteMovie
                @Id UNIQUEIDENTIFIER
            AS
            BEGIN
                SET NOCOUNT ON;

                DELETE FROM ActorMovie WHERE MovieId = @Id;

                DELETE FROM Movies WHERE Id = @Id;
            END