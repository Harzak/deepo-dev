/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


IF (SELECT is_broker_enabled FROM sys.databases WHERE name = N'$(DatabaseName)') != 1
BEGIN
	ALTER DATABASE [$(DatabaseName)] SET ENABLE_BROKER;
END 

GO

IF NOT EXISTS (select * from [dbo].Genre_Album)
BEGIN
	SET IDENTITY_INSERT [dbo].[Genre_Album] ON ;
	INSERT [dbo].[Genre_Album] ([Genre_Album_ID], [Code], [Name]) VALUES 
	(1, N'UNKNW', N'Unknow'), 
	(2, N'ELEC', N'Electro'), 
	(3, N'HIPP', N'Hip-hop'),
	(4, N'ROCK', N'Rock'), 
	(5, N'INDR', N'Indie rock'), 
	(6, N'JAZZ', N'Jazz'),
	(7, N'METAL', N'Metal')
	SET IDENTITY_INSERT [dbo].[Genre_Album] OFF
END

GO

IF NOT EXISTS (select * from [dbo].Genre_Movie)
BEGIN
	SET IDENTITY_INSERT [dbo].[Genre_Movie] ON 
	INSERT [dbo].[Genre_Movie] ([Genre_Movie_ID], [Code], [Name]) VALUES 
	(1, N'UNKNW', N'Unknow'),
	(3, N'ACT', N'Action'), 
	(4, N'CMDY', N'Comedy'),
	(5, N'DRAMA', N'Drama'),
	(6, N'FTSY', N'Fantasy'),
	(7, N'SCIFI', N'Sci-Fi'),
	(8, N'HOR', N'Horror'), 
	(9, N'MYST', N'Mystery'), 
	(10, N'THRL', N'Thriller')
	SET IDENTITY_INSERT [dbo].[Genre_Movie] OFF
END

GO

IF NOT EXISTS (select * from [dbo].Genre_TVShow)
BEGIN
	SET IDENTITY_INSERT [dbo].[Genre_TVShow] ON 
	INSERT [dbo].[Genre_TVShow] ([Genre_TVShow_ID], [Code], [Name]) VALUES 
	(1, N'UNKNW', N'Unknow'),
	(3, N'ACT', N'Action'),
	(4, N'CMDY', N'Comedy'),
	(5, N'DRAMA', N'Drama'),
	(6, N'FTSY', N'Fantasy'),
	(7, N'SCIFI', N'Sci-Fi'),
	(8, N'HOR', N'Horror'),
	(9, N'MYST', N'Mystery'),
	(10, N'THRL', N'Thriller')
	SET IDENTITY_INSERT [dbo].[Genre_TVShow] OFF
END

GO

IF NOT EXISTS (select * from [dbo].Provider)
BEGIN
	SET IDENTITY_INSERT [dbo].[Provider] ON 
	INSERT [dbo].[Provider] ([Provider_ID], [Code], [Name]) VALUES 
	(1, N'SPOTIFY', N'Spotify'),
	(2, N'DISCOGS', N'Discogs')
	SET IDENTITY_INSERT [dbo].[Provider] OFF
END

GO

IF NOT EXISTS (select * from [dbo].Type_Asset)
BEGIN
	SET IDENTITY_INSERT [dbo].[Type_Asset] ON 
	INSERT [dbo].[Type_Asset] ([Type_Asset_ID], [Code], [Name]) VALUES 
	(1, N'UNKNW', N'Unknow'),
	(2, N'COVER', N'Cover'),
	(4, N'BKCOV', N'Back cover'),
	(5, N'POSTR', N'Poster'),
	(6, N'ILLU', N'Illustration'),
	(7, N'TRLR', N'Trailer'),
	(8, N'VIDEO', N'Video'),
	(9, N'MUSIC', N'Music')
	SET IDENTITY_INSERT [dbo].[Type_Asset] OFF
END

GO

IF NOT EXISTS (select * from [dbo].Type_Release)
BEGIN
	SET IDENTITY_INSERT [dbo].[Type_Release] ON 
	INSERT [dbo].[Type_Release] ([Type_Release_ID], [Code], [Name]) VALUES
	(1, N'UNKNW', N'Unknow'),
	(2, N'MOVIE', N'Movie'),
	(3, N'TVSHW', N'TV Show'),
	(4, N'VINYL', N'Vinyl'),
	(5, N'VGAME', N'Video game'),
	(6, N'COMIC', N'Comic')
	SET IDENTITY_INSERT [dbo].[Type_Release] OFF
END

IF NOT EXISTS (select * from [fetcher].PlanificationType)
BEGIN
	SET IDENTITY_INSERT [fetcher].PlanificationType ON 
	INSERT [fetcher].PlanificationType ([PlanificationType_ID], [Code], [Name]) VALUES
	(1, N'DAILY', N'Daily'),
	(2, N'HOURLY', N'Hourly'),
	(3, N'ONESHOT', N'One-Shot')
	SET IDENTITY_INSERT [dbo].[Type_Release] OFF
END

/* TODO: Create index */

GO