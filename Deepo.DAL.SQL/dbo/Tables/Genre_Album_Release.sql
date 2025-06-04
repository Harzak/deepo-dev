CREATE TABLE [dbo].[Genre_Album_Release](
    [Release_Album_ID] INT NOT NULL,
    [Genre_Album_ID]   INT NOT NULL,
    CONSTRAINT [PK_Genre_Album_Release] PRIMARY KEY ([Genre_Album_ID], [Release_Album_ID]),
    CONSTRAINT [FK_Genre_Album_Release_Genre_Album] FOREIGN KEY ([Genre_Album_ID]) REFERENCES [dbo].[Genre_Album] ([Genre_Album_ID]),
    CONSTRAINT [FK_Genre_Album_Release_Release_Album] FOREIGN KEY ([Release_Album_ID]) REFERENCES [dbo].[Release_Album] ([Release_Album_ID])
)