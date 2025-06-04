CREATE TABLE [dbo].[Fetcher] (
    [Fetcher_ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Fetcher_GUID] NVARCHAR (50)  NOT NULL,
    [Name]         NVARCHAR (300) NOT NULL,
    CONSTRAINT [PK_Fetchers] PRIMARY KEY CLUSTERED ([Fetcher_ID] ASC)
);

