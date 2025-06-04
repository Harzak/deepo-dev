CREATE TABLE [dbo].[Author_Release] (
    [Author_Release_ID] INT IDENTITY (1, 1) NOT NULL,
    [Release_ID]        INT NOT NULL,
    [Author_ID]         INT NOT NULL,
    CONSTRAINT [PK_Author_Release] PRIMARY KEY CLUSTERED ([Author_Release_ID] ASC),
    CONSTRAINT [FK_Author_Release_Author] FOREIGN KEY ([Author_ID]) REFERENCES [dbo].[Author] ([Author_ID]),
    CONSTRAINT [FK_Author_Release_Release] FOREIGN KEY ([Release_ID]) REFERENCES [dbo].[Release] ([Release_ID])
);

