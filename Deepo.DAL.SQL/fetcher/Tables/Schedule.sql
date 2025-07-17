CREATE TABLE [fetcher].[Schedule] (
    [Schedule_ID]  INT  IDENTITY (1, 1)  NOT NULL,
    [CronExpression] NVARCHAR(100) NULL
    CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED ([Schedule_ID] ASC)
);
