CREATE TABLE [dbo].[Type_Asset] (
    [Type_Asset_ID] INT IDENTITY (1, 1) NOT NULL,
    [Code]          NVARCHAR (5)        NULL,
    [Name]          NVARCHAR (50)       NULL,
    CONSTRAINT [PK_Asset_Type] PRIMARY KEY CLUSTERED ([Type_Asset_ID] ASC)
);

