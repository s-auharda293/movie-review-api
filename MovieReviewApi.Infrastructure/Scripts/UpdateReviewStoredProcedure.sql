  CREATE PROCEDURE UpdateReview
                    @Id UNIQUEIDENTIFIER,
                    @UserName NVARCHAR(4000),
                    @Comment NVARCHAR(4000),
                    @Rating DECIMAL(3,2) 
                AS
                BEGIN
                    SET NOCOUNT ON;

                UPDATE Reviews SET 
                    UserName=@UserName,
                    Comment=@Comment,
                    Rating=@Rating,
                    UpdatedAt=SYSUTCDATETIME()
                WHERE Id=@Id;

                SELECT Id, MovieId, UserName, Comment, Rating, UpdatedAt
                FROM Reviews
                WHERE Id = @Id;
END