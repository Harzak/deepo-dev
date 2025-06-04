CREATE TABLE [fetcher].[HttpRequest](
	[HttpRequest_ID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationName] [nvarchar](50) NULL,
	[JobID] [nvarchar](50) NULL,
	[LevelLog] [nchar](10) NULL,
	[DateLogCreation] [datetime] NULL,
	[RequestUri] [text] NULL,
	[HttpMethod] [nvarchar](10) NULL,
	[HttpResponse] [nvarchar](50) NULL,
	[Date] [text] NULL,
	[UserAgent] [nvarchar](50) NULL,
	[Token] [nvarchar](MAX) NULL,
	[ResponseMessage] [text] NULL,
	 CONSTRAINT [PK_HttpRequest] PRIMARY KEY CLUSTERED 
(
	[HttpRequest_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]