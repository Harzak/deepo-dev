CREATE TABLE [fetcher].[PlanificationType] (
    [PlanificationType_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Code]                 NVARCHAR (10) NULL,
    [Name]                 NVARCHAR (50) NULL,
    CONSTRAINT [PK_PlanificationType] PRIMARY KEY CLUSTERED ([PlanificationType_ID] ASC)
);