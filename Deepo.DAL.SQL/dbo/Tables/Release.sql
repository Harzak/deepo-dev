CREATE TABLE [dbo].[Release] (
    [Release_ID]        INT IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (500)      NOT NULL,
    [Description]       NVARCHAR (MAX)      NULL,
    [Type_Release_ID]   INT                 NOT NULL,
    [Creation_Date]     DATETIME            NOT NULL,
    [Modification_Date] DATETIME            NOT NULL,
    [Creation_User]     NVARCHAR (20)       NULL,
    [Modification_User] NVARCHAR (20)       NULL,
    [GUID]              NVARCHAR (68)       NOT NULL
    CONSTRAINT [PK_Release] PRIMARY KEY CLUSTERED ([Release_ID] ASC),
    CONSTRAINT [FK_Release_Release_Type] FOREIGN KEY ([Type_Release_ID]) REFERENCES [dbo].[Type_Release] ([Type_Release_ID])
);

