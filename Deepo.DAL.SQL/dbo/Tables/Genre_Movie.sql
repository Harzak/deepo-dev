CREATE TABLE [dbo].[Genre_Movie] (
    [Genre_Movie_ID] INT IDENTITY (1, 1) NOT NULL,
    [Code]           NVARCHAR (5)        NOT NULL,
    [Name]           NVARCHAR (50)       NOT NULL,
    CONSTRAINT [PK_Genre_Movie] PRIMARY KEY CLUSTERED ([Genre_Movie_ID] ASC)
);

