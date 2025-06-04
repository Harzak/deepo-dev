CREATE TABLE [dbo].[Asset] (
    [Asset_ID]      INT IDENTITY (1, 1) NOT NULL,
    [Content_URL]       VARCHAR(MAX) NOT NULL,
    [Content_Min_URL]   VARCHAR(MAX) NOT NULL,
    [Type_Asset_ID] INT             NULL,
    CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED ([Asset_ID] ASC),
    CONSTRAINT [FK_Asset_Asset_Type] FOREIGN KEY ([Type_Asset_ID]) REFERENCES [dbo].[Type_Asset] ([Type_Asset_ID])
);

