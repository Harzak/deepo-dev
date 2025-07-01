CREATE VIEW [dbo].[V_Spotify_Vinyl_Fetch_History] AS 
SELECT
	[dbo].Release_Fetch_History.Date_UTC,
	[dbo].Release_Fetch_History.Identifier
FROM [dbo].[Release_Fetch_History]
INNER JOIN [dbo].Type_Release ON [dbo].Type_Release.Type_Release_ID = [dbo].Release_Fetch_History.Type_Release_ID
INNER JOIN [dbo].Provider ON [dbo].Provider.Provider_ID = [dbo].Release_Fetch_History.Provider_ID
WHERE [dbo].Type_Release.Code = 'VINYL'
AND [dbo].Provider.Code = 'SPOTIFY'
