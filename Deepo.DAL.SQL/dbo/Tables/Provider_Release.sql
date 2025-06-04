CREATE TABLE [dbo].[Provider_Release] (
    [Provider_Release_ID] INT IDENTITY (1, 1) NOT NULL,
    [Provider_ID]         INT NOT NULL,
    [Release_ID]          INT NOT NULL,
    [Provider_Release_Identifier] [nvarchar](400) NULL,
    CONSTRAINT [PK_Source_Release] PRIMARY KEY CLUSTERED ([Provider_Release_ID] ASC),
    CONSTRAINT [FK_Provider_Release_Provider] FOREIGN KEY ([Provider_ID]) REFERENCES [dbo].[Provider] ([Provider_ID]),
    CONSTRAINT [FK_Provider_Release_Release] FOREIGN KEY ([Release_ID]) REFERENCES [dbo].[Release] ([Release_ID])
);

