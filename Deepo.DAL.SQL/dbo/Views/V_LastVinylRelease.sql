CREATE VIEW [dbo].[V_LastVinylRelease]
	AS 
	SELECT
	  [dbo].Release.Release_ID,
	  [dbo].Release.GUID AS 'ReleasGUID',
	  [dbo].Release.Name AS 'AlbumName',
	  STRING_AGG([dbo].Author.Name, ', ') AS 'ArtistsNames',
	  [dbo].Release.Creation_Date,
	  [dbo].Asset.Content_Min_URL as 'Thumb_URL',
	  [dbo].Asset.Content_URL as 'Cover_URL'
	FROM
	  [dbo].Release
	  LEFT JOIN [dbo].Release_Album ON [dbo].Release_Album.Release_ID = [dbo].Release.Release_ID
	  FULL OUTER JOIN [dbo].Author_Release ON [dbo].Author_Release.Release_ID = [dbo].Release.Release_ID
	  FULL OUTER JOIN [dbo].Author ON [dbo].Author.Author_ID = [dbo].Author_Release.Author_ID
	  LEFT JOIN [dbo].Asset_Release ON [dbo].Release.Release_ID = [dbo].Asset_Release.Release_ID
	  LEFT JOIN [dbo].Asset ON [dbo].Asset.Asset_ID = [dbo].Asset_Release.Asset_ID
	GROUP BY
	  [dbo].Release.Release_ID,
	  [dbo].Release.GUID,
	  [dbo].Release.Name,
	  [dbo].Release.Creation_Date,
	  [dbo].Asset.Content_Min_URL,
	  [dbo].Asset.Content_URL