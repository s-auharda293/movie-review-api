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

    -- Validate sort column and direction
    IF @SortColumn NOT IN ('Title','Description','ReleaseDate','DurationMinutes','Rating','CreatedAt')
        SET @SortColumn = 'CreatedAt';
    IF @SortDirection NOT IN ('ASC','DESC')
        SET @SortDirection = 'ASC';

    -- Precast search term
    DECLARE @SearchDate DATE = TRY_CAST(@SearchTerm AS DATE);
    DECLARE @SearchInt INT = TRY_CAST(@SearchTerm AS INT);
    DECLARE @SearchFloat FLOAT = TRY_CAST(@SearchTerm AS FLOAT);

    -- Build WHERE clause
    DECLARE @WhereClause NVARCHAR(MAX) = N'1=1';
    IF @SearchTerm IS NOT NULL AND @SearchTerm <> ''
    BEGIN
        SET @WhereClause = N'(' +
            CASE WHEN @SearchColumn = 'Title' THEN 'Title LIKE ''%'' + @SearchTerm + ''%''' ELSE '1=0' END +
            CASE WHEN @SearchColumn = 'Description' THEN ' OR Description LIKE ''%'' + @SearchTerm + ''%''' ELSE '' END +
            CASE WHEN @SearchColumn = 'ReleaseDate' THEN ' OR ReleaseDate = @SearchDate' ELSE '' END +
            CASE WHEN @SearchColumn = 'DurationMinutes' THEN ' OR DurationMinutes = @SearchInt' ELSE '' END +
            CASE WHEN @SearchColumn = 'Rating' THEN ' OR Rating = @SearchFloat' ELSE '' END +
            N')';
    END

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    -- Return TotalCount as first result set
    DECLARE @sqlCount NVARCHAR(MAX) = N'
    SELECT COUNT(*) AS TotalCount
    FROM Movies
    WHERE ' + @WhereClause + N';';

    EXEC sp_executesql 
        @sqlCount,
        N'@SearchTerm NVARCHAR(400), @SearchDate DATE, @SearchInt INT, @SearchFloat FLOAT',
        @SearchTerm=@SearchTerm,
        @SearchDate=@SearchDate,
        @SearchInt=@SearchInt,
        @SearchFloat=@SearchFloat;

    -- Return paged data as second result set
    DECLARE @sqlData NVARCHAR(MAX) = N'
    SELECT *
    FROM Movies
    WHERE ' + @WhereClause + N'
    ORDER BY ' + QUOTENAME(@SortColumn) + ' ' + @SortDirection + N'
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;';

    EXEC sp_executesql 
        @sqlData,
        N'@SearchTerm NVARCHAR(400), @SearchDate DATE, @SearchInt INT, @SearchFloat FLOAT, @Offset INT, @PageSize INT',
        @SearchTerm=@SearchTerm,
        @SearchDate=@SearchDate,
        @SearchInt=@SearchInt,
        @SearchFloat=@SearchFloat,
        @Offset=@Offset,
        @PageSize=@PageSize;
END