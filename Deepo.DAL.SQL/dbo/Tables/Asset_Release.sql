CREATE TABLE [dbo].[Asset_Release] (
    [Asset_Release_ID] INT IDENTITY (1, 1)  NOT NULL,
    [Release_ID]       INT                  NOT NULL,
    [Asset_ID]         INT                  NOT NULL,
    CONSTRAINT [PK_Asset_Release] PRIMARY KEY CLUSTERED ([Asset_Release_ID] ASC),
    CONSTRAINT [FK_Asset_Release_Asset] FOREIGN KEY ([Asset_ID]) REFERENCES [dbo].[Asset] ([Asset_ID]),
    CONSTRAINT [FK_Asset_Release_Release] FOREIGN KEY ([Release_ID]) REFERENCES [dbo].[Release] ([Release_ID])
);