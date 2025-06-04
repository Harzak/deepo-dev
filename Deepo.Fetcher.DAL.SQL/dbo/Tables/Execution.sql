CREATE TABLE [dbo].[Execution] (
    [Execution_ID]    INT  IDENTITY (1, 1) NOT NULL,
    [Fetcher_ID]  INT  NOT NULL,
    [StartTime] DATE NULL,
    [EndTime] DATE NOT NULL,
    CONSTRAINT [PK_Execution] PRIMARY KEY CLUSTERED ([Execution_ID] ASC),
    CONSTRAINT [FK_Execution_Fetchers] FOREIGN KEY ([Fetcher_ID]) REFERENCES [dbo].[Fetcher] ([Fetcher_ID])
);

