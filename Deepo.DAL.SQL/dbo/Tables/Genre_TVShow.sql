CREATE TABLE [dbo].[Genre_TVShow] (
    [Genre_TVShow_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (5)  NOT NULL,
    [Name]            NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Genre_TVShow_ID] PRIMARY KEY CLUSTERED ([Genre_TVShow_ID] ASC)
);

