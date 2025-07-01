CREATE TABLE [dbo].[Release_Fetch_History]
(
	[Release_Fetch_History_ID]	INT IDENTITY (1, 1)	NOT NULL,
	[Date_UTC]					DATETIME			NOT NULL,
	[Identifier]				NVARCHAR (500)      NOT NULL,
	[Identifier_Desc] 			NVARCHAR (100)      NULL,
	[Type_Release_ID]			INT                 NOT NULL,
	[Provider_ID]				INT                 NOT NULL,
	CONSTRAINT [PK_[Release_Fetch_History] PRIMARY KEY CLUSTERED ([Release_Fetch_History_ID] ASC),
	CONSTRAINT [FK_Release_Fetch_History_Release_Type] FOREIGN KEY ([Type_Release_ID]) REFERENCES [dbo].[Type_Release] ([Type_Release_ID]),
	CONSTRAINT [FK_Release_Fetch_History_Provider] FOREIGN KEY ([Provider_ID]) REFERENCES [dbo].[Provider] ([Provider_ID])
)