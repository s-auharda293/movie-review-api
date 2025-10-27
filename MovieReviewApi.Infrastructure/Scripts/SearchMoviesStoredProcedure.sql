CREATE PROCEDURE SearchMovies
    @SearchTerm NVARCHAR(400) = NULL,
    @SearchColumn NVARCHAR(50) = NULL,
    @Page INT = 1,
    @PageSize INT = 20,
    @SortColumn NVARCHAR(50) = 'CreatedAt',
    @SortDirection NVARCHAR(4) = 'ASC'
AS
BEGIN
    SET NOCOUNT ON;

    -- Total count
    SELECT COUNT(*) AS TotalCount
    FROM Movies
    WHERE
        (@SearchTerm IS NULL OR @SearchTerm = '')
        OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
        OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
        OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
        OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
        OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%');

    -- Paged results
    IF @SortColumn = 'Title'
    BEGIN
        IF @SortDirection = 'ASC'
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY Title
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
        ELSE
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY Title DESC
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
    END
    ELSE IF @SortColumn = 'Description'
    BEGIN
        IF @SortDirection = 'ASC'
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY Description
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
        ELSE
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY Description DESC
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
    END
    ELSE IF @SortColumn = 'ReleaseDate'
    BEGIN
        IF @SortDirection = 'ASC'
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY ReleaseDate
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
        ELSE
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY ReleaseDate DESC
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
    END
    ELSE IF @SortColumn = 'DurationMinutes'
    BEGIN
        IF @SortDirection = 'ASC'
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY DurationMinutes
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
        ELSE
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY DurationMinutes DESC
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
    END
    ELSE IF @SortColumn = 'Rating'
    BEGIN
        IF @SortDirection = 'ASC'
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY Rating
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
        ELSE
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY Rating DESC
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
    END
    ELSE -- default sorting (CreatedAt)
    BEGIN
        IF @SortDirection = 'ASC'
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY CreatedAt
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
        ELSE
            SELECT *
            FROM Movies
            WHERE
                (@SearchTerm IS NULL OR @SearchTerm = '')
                OR (@SearchColumn = 'Title' AND Title LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Description' AND Description LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'ReleaseDate' AND ReleaseDate = TRY_CAST(@SearchTerm AS DATE))
                OR (@SearchColumn = 'DurationMinutes' AND CAST(DurationMinutes AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
                OR (@SearchColumn = 'Rating' AND CAST(Rating AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%')
            ORDER BY CreatedAt DESC
            OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
    END
END
GO
