CREATE PROCEDURE SearchMovies
    @SearchTerm NVARCHAR(400) = NULL,
    @SearchColumn NVARCHAR(50) = 'Title',
    @Page INT = 1,
    @PageSize INT = 20,
    @SortColumn NVARCHAR(50) = 'CreatedAt',
    @SortDirection NVARCHAR(4) = 'ASC'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SearchDate DATE = TRY_CAST(@SearchTerm AS DATE);
    DECLARE @SearchInt INT = TRY_CAST(@SearchTerm AS INT);
    DECLARE @SearchFloat FLOAT = TRY_CAST(@SearchTerm AS FLOAT);

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

     SELECT COUNT(1) as TotalCount
        FROM Movies
        WHERE
        (
            @SearchTerm IS NULL OR @SearchTerm = '' OR (
                (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%') OR
                (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%') OR
                (@SearchColumn = 'ReleaseDate' AND ReleaseDate = @SearchDate) OR
                (@SearchColumn = 'DurationMinutes' AND DurationMinutes = @SearchInt) OR
                (@SearchColumn = 'Rating' AND Rating = @SearchFloat)
            )
        )

    ;WITH FilteredMovies AS (
        SELECT *
        FROM Movies
        WHERE
        (
            @SearchTerm IS NULL OR @SearchTerm = '' OR (
                (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%') OR
                (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%') OR
                (@SearchColumn = 'ReleaseDate' AND ReleaseDate = @SearchDate) OR
                (@SearchColumn = 'DurationMinutes' AND DurationMinutes = @SearchInt) OR
                (@SearchColumn = 'Rating' AND Rating = @SearchFloat)
            )
        )
    )
    SELECT *
    FROM FilteredMovies
    ORDER BY
        CASE WHEN @SortColumn = 'Title' AND @SortDirection = 'ASC' THEN Title END ASC,
        CASE WHEN @SortColumn = 'Title' AND @SortDirection = 'DESC' THEN Title END DESC,
        CASE WHEN @SortColumn = 'Description' AND @SortDirection = 'ASC' THEN Description END ASC,
        CASE WHEN @SortColumn = 'Description' AND @SortDirection = 'DESC' THEN Description END DESC,
        CASE WHEN @SortColumn = 'ReleaseDate' AND @SortDirection = 'ASC' THEN ReleaseDate END ASC,
        CASE WHEN @SortColumn = 'ReleaseDate' AND @SortDirection = 'DESC' THEN ReleaseDate END DESC,
        CASE WHEN @SortColumn = 'DurationMinutes' AND @SortDirection = 'ASC' THEN DurationMinutes END ASC,
        CASE WHEN @SortColumn = 'DurationMinutes' AND @SortDirection = 'DESC' THEN DurationMinutes END DESC,
        CASE WHEN @SortColumn = 'Rating' AND @SortDirection = 'ASC' THEN Rating END ASC,
        CASE WHEN @SortColumn = 'Rating' AND @SortDirection = 'DESC' THEN Rating END DESC,
        CASE WHEN @SortColumn = 'CreatedAt' AND @SortDirection = 'ASC' THEN CreatedAt END ASC,
        CASE WHEN @SortColumn = 'CreatedAt' AND @SortDirection = 'DESC' THEN CreatedAt END DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

END;
