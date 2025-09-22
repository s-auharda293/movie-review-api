CREATE PROCEDURE CreateMovie
    @Title NVARCHAR(4000),
    @Description NVARCHAR(MAX) = NULL,
    @ReleaseDate DATETIME2 = NULL,
    @DurationMinutes INT = NULL,
    @Rating DECIMAL(3,1) = NULL,
    @ActorIds NVARCHAR(MAX) = NULL -- comma-separated actor GUIDs
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MovieId UNIQUEIDENTIFIER = NEWID();
    DECLARE @CreatedAt DATETIME2 = SYSUTCDATETIME();
    DECLARE @UpdatedAt DATETIME2 = SYSUTCDATETIME();

    -- Insert movie
    INSERT INTO Movies (Id, Title, Description, ReleaseDate, DurationMinutes, Rating, CreatedAt, UpdatedAt)
    VALUES (@MovieId, @Title, @Description, COALESCE(@ReleaseDate, SYSUTCDATETIME()), @DurationMinutes, @Rating, @CreatedAt, @UpdatedAt);

    -- Insert movie-actor links if ActorIds provided
    IF @ActorIds IS NOT NULL AND LEN(@ActorIds) > 0
    BEGIN
        INSERT INTO ActorMovie (MovieId, ActorId)
        SELECT @MovieId, CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@ActorIds, ',');
    END

    -- Return the created movie row
    SELECT Id, Title, Description, ReleaseDate, DurationMinutes, Rating, CreatedAt, UpdatedAt
    FROM Movies
    WHERE Id = @MovieId;
END
