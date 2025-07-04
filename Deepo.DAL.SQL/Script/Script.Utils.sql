
-- ##### Delete Release by GUID ##### --
BEGIN TRANSACTION;

DECLARE @TargetGUID NVARCHAR(68) = '99273e62-ab24-4918-b60a-8e0059b0d866';
DECLARE @ReleaseID INT;
DECLARE @ReleaseAlbumID INT;

SELECT @ReleaseID = [Release_ID]
FROM [dbo].[Release]
WHERE [GUID] = @TargetGUID;

IF @ReleaseID IS NULL
BEGIN
    PRINT 'No Release found with GUID: ' + @TargetGUID;
    ROLLBACK TRANSACTION;
    RETURN;
END

SELECT @ReleaseAlbumID = [Release_Album_ID]
FROM [dbo].[Release_Album]
WHERE [Release_ID] = @ReleaseID;

IF @ReleaseAlbumID IS NOT NULL
BEGIN
    DELETE FROM [dbo].[Tracklist_Album]
    WHERE [Release_Album_ID] = @ReleaseAlbumID;
END

IF @ReleaseAlbumID IS NOT NULL AND EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Genre_Album_Release')
BEGIN
    DELETE FROM [dbo].[Genre_Album_Release]
    WHERE [Release_Album_ID] = @ReleaseAlbumID;
END

IF @ReleaseAlbumID IS NOT NULL
BEGIN
    DELETE FROM [dbo].[Release_Album]
    WHERE [Release_Album_ID] = @ReleaseAlbumID;
END

DELETE FROM [dbo].[Author_Release]
WHERE [Release_ID] = @ReleaseID;

DELETE FROM [dbo].[Asset_Release]
WHERE [Release_ID] = @ReleaseID;

DELETE FROM [dbo].[Provider_Release]
WHERE [Release_ID] = @ReleaseID;

DELETE FROM [dbo].[Availability_Release]
WHERE [Release_ID] = @ReleaseID;

DELETE FROM [dbo].[Release_Movie]
WHERE [Release_ID] = @ReleaseID;

DELETE FROM [dbo].[Release_TVShow]
WHERE [Release_ID] = @ReleaseID;

DELETE FROM [dbo].[Release]
WHERE [Release_ID] = @ReleaseID;

ROLLBACK TRANSACTION;

-------------------------------------------------------------

-- ##### Delete All Releases #####

BEGIN TRANSACTION;

DELETE FROM Genre_Album_Release
DELETE FROM Genre_Album
DELETE FROM Asset_Release
DELETE FROM Asset
DELETE FROM Author_Release
DELETE FROM Author
DELETE FROM Tracklist_Album
DELETE FROM Provider_Release
DELETE FROM Release_Album
DELETE FROM Release

ROLLBACK TRANSACTION;