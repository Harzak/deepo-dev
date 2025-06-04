CREATE TABLE [fetcher].[Planification] (
    [Planification_ID]     INT IDENTITY (1, 1)  NOT NULL,
    [PlanificationType_ID] INT                  NULL,
    [Planning_ID]          INT                  NULL,
    [Fetcher_ID]           INT                  NOT NULL,
    [DateNextStart]        DATETIME2            NULL,
    CONSTRAINT [PK_Planification] PRIMARY KEY CLUSTERED ([Planification_ID] ASC),
    CONSTRAINT [FK_Planification_Fetcher] FOREIGN KEY ([Fetcher_ID]) REFERENCES [fetcher].[Fetcher] ([Fetcher_ID]),
    CONSTRAINT [FK_Planification_PlanificationType] FOREIGN KEY ([PlanificationType_ID]) REFERENCES [fetcher].[PlanificationType] ([PlanificationType_ID]),
    CONSTRAINT [FK_Planification_Planning] FOREIGN KEY ([Planning_ID]) REFERENCES [fetcher].[Planning] ([Planing_ID])
);
