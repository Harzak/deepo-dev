CREATE TABLE [dbo].[Release_TVShow] (
    [Release_TVShow_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Release_ID]        INT           NULL,
    [Season]            NVARCHAR (50) NULL,
    [Genre_TVShow_ID]   INT           NULL,
    CONSTRAINT [PK_Release_Serie] PRIMARY KEY CLUSTERED ([Release_TVShow_ID] ASC),
    CONSTRAINT [FK_Release_Serie_Release] FOREIGN KEY ([Release_ID]) REFERENCES [dbo].[Release] ([Release_ID]),
    CONSTRAINT [FK_Release_TVShow_Genre_TVShow] FOREIGN KEY ([Genre_TVShow_ID]) REFERENCES [dbo].[Genre_TVShow] ([Genre_TVShow_ID])
);

