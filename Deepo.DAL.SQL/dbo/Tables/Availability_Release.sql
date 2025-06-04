CREATE TABLE [dbo].[Availability_Release] (
    [Availability_Release_ID] INT IDENTITY (1, 1) NOT NULL,
    [Release_ID]              INT                 NULL,
    [Availability_Date]       DATETIME            NULL,
    [Country_ID]              INT                 NULL,
    CONSTRAINT [PK_Availability_Release] PRIMARY KEY CLUSTERED ([Availability_Release_ID] ASC),
    CONSTRAINT [FK_Availability_Release_Country] FOREIGN KEY ([Country_ID]) REFERENCES [dbo].[Country] ([Country_ID]),
    CONSTRAINT [FK_Availability_Release_Release] FOREIGN KEY ([Release_ID]) REFERENCES [dbo].[Release] ([Release_ID])
);

