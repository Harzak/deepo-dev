CREATE VIEW [dbo].[V_LastVinylRelease]
AS 
WITH ReleaseAuthors AS (
    SELECT
        AR.Release_ID,
        STRING_AGG(A.Name, ';') AS 'ArtistsNames'
    FROM
        [dbo].Author_Release AR
        INNER JOIN [dbo].Author A ON A.Author_ID = AR.Author_ID
    GROUP BY
        AR.Release_ID
),
ReleaseGenresIdentifier AS (
    SELECT
        RA.Release_ID,
        STRING_AGG(GA.Identifier, ';') WITHIN GROUP (ORDER BY GA.Name) AS 'GenresIdentifier'
    FROM
        [dbo].Release_Album RA
        INNER JOIN [dbo].Genre_Album_Release GAR ON GAR.Release_Album_ID = RA.Release_Album_ID
        INNER JOIN [dbo].Genre_Album GA ON GA.Genre_Album_ID = GAR.Genre_Album_ID
    GROUP BY
        RA.Release_ID
),
ReleaseAssets AS (
    SELECT
        AR.Release_ID,
        A.Content_Min_URL AS 'Thumb_URL',
        A.Content_URL AS 'Cover_URL'
    FROM
        [dbo].Asset_Release AR
        INNER JOIN [dbo].Asset A ON A.Asset_ID = AR.Asset_ID
)
SELECT
    R.Release_ID,
    R.Release_Date_UTC,
    R.GUID AS 'ReleasGUID',
    R.Name AS 'AlbumName',
    RA.ArtistsNames,
	RA1.Market,
    RG.GenresIdentifier,
    R.Creation_Date,
    AS1.Thumb_URL,
    AS1.Cover_URL
FROM
    [dbo].Release R
    LEFT JOIN [dbo].Release_Album RA1 ON RA1.Release_ID = R.Release_ID
    LEFT JOIN ReleaseAuthors RA ON RA.Release_ID = R.Release_ID
    LEFT JOIN ReleaseGenresIdentifier RG ON RG.Release_ID = R.Release_ID
    LEFT JOIN ReleaseAssets AS1 ON AS1.Release_ID = R.Release_ID