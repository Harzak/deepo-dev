CREATE TABLE [dbo].[Release_Album] (
    [Release_Album_ID] INT IDENTITY (1, 1)  NOT NULL,
    [Release_ID]       INT                  NULL,
    [Duration]         NVARCHAR (50)        NULL,
    [Label]            NVARCHAR (2000)      NULL,
    [Country]          NVARCHAR (2000)      NULL,
    [Market]           NVARCHAR(300)        NULL, 
    CONSTRAINT [PK_Release_Album] PRIMARY KEY CLUSTERED ([Release_Album_ID] ASC),
    CONSTRAINT [FK_Release_Album_Release] FOREIGN KEY ([Release_ID]) REFERENCES [dbo].[Release] ([Release_ID])
);

