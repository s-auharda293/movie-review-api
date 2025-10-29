IF NOT EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE name = 'ftMoviesCatalog')
BEGIN
    CREATE FULLTEXT CATALOG ftMoviesCatalog AS DEFAULT;
END
GO

CREATE FULLTEXT INDEX ON Movies
(
    Title LANGUAGE 1033,       -- 1033 = English
    Description LANGUAGE 1033
)
KEY INDEX PK_Movies
ON ftMoviesCatalog
WITH CHANGE_TRACKING AUTO;     
GO
