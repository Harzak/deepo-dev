CREATE TABLE [dbo].[Country] (
    [Country_ID] INT IDENTITY (1, 1) NOT NULL,
    [Code]       NVARCHAR (10)       NULL,
    [Name]       NVARCHAR (50)       NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([Country_ID] ASC)
);

