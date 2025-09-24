CREATE PROCEDURE CreateReview
    @MovieId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @UserName NVARCHAR(4000),
    @Comment NVARCHAR(4000) = NULL,
    @Rating DECIMAL(3,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ReviewId UNIQUEIDENTIFIER = NEWID();
    DECLARE @CreatedAt DATETIME2 = SYSUTCDATETIME();
    DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

    INSERT INTO Reviews (Id, MovieId, UserId, Comment, Rating, CreatedAt, UpdatedAt)
    VALUES (@ReviewId, @MovieId, @UserId, @Comment, @Rating, @CreatedAt, @UpdatedAt);

    -- Return inserted review
    SELECT 
        @ReviewId AS Id,
        @MovieId AS MovieId,
        @UserId AS UserId,
        @Comment AS Comment,
        @Rating AS Rating,
        @CreatedAt AS CreatedAt,
        @UpdatedAt AS UpdatedAt;
END
