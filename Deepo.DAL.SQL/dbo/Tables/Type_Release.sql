CREATE TABLE [dbo].[Type_Release] (
    [Type_Release_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (10) NOT NULL,
    [Name]            NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Release-Type] PRIMARY KEY CLUSTERED ([Type_Release_ID] ASC)
);

