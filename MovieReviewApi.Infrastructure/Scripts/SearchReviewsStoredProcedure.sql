CREATE PROCEDURE SearchReviews
    @Page INT = 1,
    @PageSize INT = 5,
    @SortColumn NVARCHAR(100) = NULL,
    @SortDir NVARCHAR(4) = 'ASC', -- "ASC" or "DESC"
    @SearchColumn NVARCHAR(100) = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    ;WITH FilteredReviews AS (
        SELECT 
            r.Id,
            r.MovieId,
            r.UserId,
            u.UserName,
            r.Comment,
            r.Rating,
            r.CreatedAt
        FROM Reviews r
        INNER JOIN AspNetUsers u ON r.UserId = u.Id
        WHERE
            @SearchTerm IS NULL OR @SearchTerm = '' OR
            (
                (@SearchColumn = 'Comment' AND r.Comment LIKE '%' + @SearchTerm + '%') OR
                (@SearchColumn = 'UserName' AND u.UserName LIKE '%' + @SearchTerm + '%') OR
                (@SearchColumn = 'Rating' AND CAST(r.Rating AS NVARCHAR(50)) LIKE '%' + @SearchTerm + '%')
            )
    )
    SELECT *
    INTO #TempReviews
    FROM FilteredReviews;

    SELECT @TotalCount = COUNT(1) FROM #TempReviews;

    SELECT *
    FROM #TempReviews
    ORDER BY
        CASE WHEN @SortColumn = 'Comment' AND @SortDir = 'ASC' THEN Comment END ASC,
        CASE WHEN @SortColumn = 'Comment' AND @SortDir = 'DESC' THEN Comment END DESC,
        CASE WHEN @SortColumn = 'UserName' AND @SortDir = 'ASC' THEN UserName END ASC,
        CASE WHEN @SortColumn = 'UserName' AND @SortDir = 'DESC' THEN UserName END DESC,
        CASE WHEN @SortColumn = 'Rating' AND @SortDir = 'ASC' THEN Rating END ASC,
        CASE WHEN @SortColumn = 'Rating' AND @SortDir = 'DESC' THEN Rating END DESC,
        CreatedAt
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Clean up temp table
    DROP TABLE #TempReviews;
END
GO
