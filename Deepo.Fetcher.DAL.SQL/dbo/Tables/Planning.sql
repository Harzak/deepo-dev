CREATE TABLE [dbo].[Planning] (
    [Planing_ID]  INT  IDENTITY (1, 1) NOT NULL,
    [HourStart]   INT  NULL,
    [MinuteStart] INT  NULL,
    [HourEnd]     INT  NULL,
    [MinuteEnd]   INT  NULL,
    CONSTRAINT [PK_Planning] PRIMARY KEY CLUSTERED ([Planing_ID] ASC)
);

