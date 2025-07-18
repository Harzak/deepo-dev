CREATE TABLE [fetcher].[Scheduler] (
    [Scheduler_ID]     INT IDENTITY (1, 1)  NOT NULL,
    [Schedule_ID]          INT                  NULL,
    [Fetcher_ID]           INT                  NOT NULL
    CONSTRAINT [PK_Scheduler] PRIMARY KEY CLUSTERED ([Scheduler_ID] ASC),
    CONSTRAINT [FK_Scheduler_Fetcher] FOREIGN KEY ([Fetcher_ID]) REFERENCES [fetcher].[Fetcher] ([Fetcher_ID]),
    CONSTRAINT [FK_Scheduler_Schedule] FOREIGN KEY ([Schedule_ID]) REFERENCES [fetcher].[Schedule] ([Schedule_ID])
);