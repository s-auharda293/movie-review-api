CREATE PROCEDURE SearchActors
    @SearchTerm NVARCHAR(200) = NULL,
    @SearchColumn NVARCHAR(50) = NULL,
    @Page INT = 1,
    @PageSize INT = 20,
    @SortColumn NVARCHAR(50) = 'CreatedAt',
    @SortDirection NVARCHAR(4) = 'ASC'
AS
BEGIN
    SET NOCOUNT ON;

    -- Filter actors directly from the Actor table
    SELECT COUNT(1) AS TotalCount
    FROM Actors
    WHERE
        (@SearchTerm IS NULL OR @SearchTerm = '')
        OR (@SearchColumn = 'Name' AND Name LIKE '%' + @SearchTerm + '%')
        OR (@SearchColumn = 'Bio' AND Bio LIKE '%' + @SearchTerm + '%')
        OR (@SearchColumn = 'DateOfBirth' AND DateOfBirth = TRY_CAST(@SearchTerm AS DATE))
        OR (@SearchColumn = 'Status' AND Status LIKE '%' + @SearchTerm + '%');
    SELECT Id,Name,Bio,DateOfBirth,MovieTitlesCache,Status,ProposedAt,StatusChangedAt
    FROM Actors
    WHERE
        (@SearchTerm IS NULL OR @SearchTerm = '')
        OR (@SearchColumn = 'Name' AND Name LIKE '%' + @SearchTerm + '%')
        OR (@SearchColumn = 'Bio' AND Bio LIKE '%' + @SearchTerm + '%')
        OR (@SearchColumn = 'DateOfBirth' AND DateOfBirth = TRY_CAST(@SearchTerm AS DATE))
        OR (@SearchColumn = 'Status' AND Status LIKE '%' + @SearchTerm + '%')
    ORDER BY
        CASE 
            WHEN @SortColumn = 'Name' AND @SortDirection = 'ASC' THEN Name
            WHEN @SortColumn = 'Name' AND @SortDirection = 'DESC' THEN Name
            WHEN @SortColumn = 'Bio' AND @SortDirection = 'ASC' THEN Bio
            WHEN @SortColumn = 'Bio' AND @SortDirection = 'DESC' THEN Bio
            WHEN @SortColumn = 'DateOfBirth' AND @SortDirection = 'ASC' THEN DateOfBirth
            WHEN @SortColumn = 'DateOfBirth' AND @SortDirection = 'DESC' THEN DateOfBirth
            ELSE CreatedAt
        END
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
