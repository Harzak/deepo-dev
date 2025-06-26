CREATE TABLE [dbo].[Genre_Album] (
    [Genre_Album_ID] INT IDENTITY (1, 1) NOT NULL,
    [Identifier]     NVARCHAR (50)       NOT NULL,
    [Name]           NVARCHAR (50)       NOT NULL,
    CONSTRAINT [PK_Genre_Album] PRIMARY KEY CLUSTERED ([Genre_Album_ID] ASC)
);

