CREATE PROCEDURE SearchReviews
    @Page INT = 1,
    @PageSize INT = 5,
    @SortColumn NVARCHAR(100) = NULL,
    @SortDir NVARCHAR(4) = 'asc', -- "asc" or "desc"
    @SearchColumn NVARCHAR(100) = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    -- Base query
     SET @SQL = '
        SELECT 
        CAST(r.Id AS UNIQUEIDENTIFIER) AS Id, 
        CAST(r.MovieId AS UNIQUEIDENTIFIER) AS MovieId, 
        CAST(r.UserId AS UNIQUEIDENTIFIER) AS UserId,
        u.UserName, r.Comment, r.Rating
        FROM Reviews r
        LEFT JOIN AspNetUsers u ON r.UserId = u.Id
        WHERE 1 = 1
    ';

      -- Count total
    DECLARE @CountSQL NVARCHAR(MAX) = '
        SELECT @TotalCountOut = COUNT(*) 
        FROM Reviews
        WHERE 1 = 1
    ';

    -- Append search filter if present
    IF (@SearchColumn IS NOT NULL AND @SearchTerm IS NOT NULL)
        SET @CountSQL += ' AND ' + QUOTENAME(@SearchColumn) + ' LIKE ''%' + @SearchTerm + '%''';
        SET @SQL += ' AND ' + QUOTENAME(@SearchColumn) + ' LIKE ''%' + @SearchTerm + '%''';

    -- Optional sorting
    IF (@SortColumn IS NOT NULL)
        SET @SQL += ' ORDER BY ' + QUOTENAME(@SortColumn) + ' ' + CASE WHEN @SortDir = 'desc' THEN 'DESC' ELSE 'ASC' END;
    ELSE
        SET @SQL += ' ORDER BY r.Id'; -- required for OFFSET

    -- Pagination
    SET @SQL += '
        OFFSET ' + CAST(@Offset AS NVARCHAR) + ' ROWS 
        FETCH NEXT ' + CAST(@PageSize AS NVARCHAR) + ' ROWS ONLY;
    ';

    -- Execute total count
    EXEC sp_executesql @CountSQL, N'@TotalCountOut INT OUTPUT', @TotalCountOut = @TotalCount OUTPUT;

    -- Execute main query
    EXEC sp_executesql @SQL;
END
GO
