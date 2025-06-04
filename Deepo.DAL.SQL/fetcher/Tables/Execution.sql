CREATE TABLE [fetcher].[Execution] (
    [Execution_ID]    INT  IDENTITY (1, 1) NOT NULL,
    [Fetcher_ID]  INT  NOT NULL,
    [StartedAt] DATETIME NOT NULL,
    [EndedAt] DATETIME NULL,
    CONSTRAINT [PK_Execution] PRIMARY KEY CLUSTERED ([Execution_ID] ASC),
    CONSTRAINT [FK_Execution_Fetchers] FOREIGN KEY ([Fetcher_ID]) REFERENCES [fetcher].[Fetcher] ([Fetcher_ID])
);

