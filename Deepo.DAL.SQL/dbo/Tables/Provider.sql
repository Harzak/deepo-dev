CREATE TABLE [dbo].[Provider] (
    [Provider_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Code]        VARCHAR (10) NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Provider] PRIMARY KEY CLUSTERED ([Provider_ID] ASC)
);

