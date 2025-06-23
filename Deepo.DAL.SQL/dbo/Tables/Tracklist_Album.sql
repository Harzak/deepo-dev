CREATE TABLE [dbo].[Track_Album](
	[Track_Album_ID] INT IDENTITY (1,1) NOT NULL, 
    [Position]           INT                NOT NULL, 
    [Title]              NVARCHAR(1000)     NOT NULL, 
    [Duration]           NVARCHAR(50)       NOT NULL,
    CONSTRAINT [PK_Track_Album] PRIMARY KEY CLUSTERED ([Track_Album_ID] ASC)
)
