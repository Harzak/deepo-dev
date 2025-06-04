CREATE TABLE [dbo].[Release_Movie] (
    [Release_Movie_ID] INT IDENTITY (1, 1) NOT NULL,
    [Release_ID]       INT NOT NULL,
    [Genre_Movie_ID]   INT NULL,
    CONSTRAINT [PK_Release_Movie] PRIMARY KEY CLUSTERED ([Release_Movie_ID] ASC),
    CONSTRAINT [FK_Release_Movie_Genre_Movie] FOREIGN KEY ([Genre_Movie_ID]) REFERENCES [dbo].[Genre_Movie] ([Genre_Movie_ID]),
    CONSTRAINT [FK_Release_Movie_Release] FOREIGN KEY ([Release_ID]) REFERENCES [dbo].[Release] ([Release_ID])
);

