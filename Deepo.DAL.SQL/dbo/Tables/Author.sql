CREATE TABLE [dbo].[Author] (
    [Author_ID]                  INT IDENTITY (1, 1) NOT NULL,
    [Code]                       NVARCHAR (10)       NULL,
    [Name]                       NVARCHAR (500)      NULL,
    [Provider_ID]                INT                 NOT NULL,
    [Provider_Author_Identifier] NVARCHAR (50) NOT NULL
    CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED ([Author_ID] ASC),
    CONSTRAINT [FK_Author_Provider] FOREIGN KEY([Provider_ID]) REFERENCES [dbo].[Provider] ([Provider_ID])
);

