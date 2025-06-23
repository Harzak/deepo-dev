CREATE TABLE [dbo].[Tracklist_Album](
	[Tracklist_Album_ID] INT IDENTITY (1, 1) NOT NULL,
	[Release_Album_ID]	 INT				 NOT NULL,
	[Track_Album_ID]	 INT				 NOT NULL,
	CONSTRAINT [PK_Tracklist_Album] PRIMARY KEY CLUSTERED ([Tracklist_Album_ID] ASC),
	CONSTRAINT [FK_Tracklist_Album_Release_Album] FOREIGN KEY ([Release_Album_ID]) REFERENCES [dbo].[Release_Album] ([Release_Album_ID]),
	CONSTRAINT [FK_Tracklist_Album_Track_Album] FOREIGN KEY ([Track_Album_ID]) REFERENCES [dbo].[Track_Album] ([Track_Album_ID])
)