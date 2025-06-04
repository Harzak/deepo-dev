CREATE VIEW [dbo].[V_CompletedFetcher]
	AS SELECT	Fetcher.Name,
		Fetcher.Fetcher_GUID,
		Execution.StartTime,
		Execution.EndTime
	FROM Fetcher
	INNER JOIN Execution ON Execution.Fetcher_ID = Fetcher.Fetcher_ID
	WHERE Execution.StartTime < GETDATE() AND Execution.EndTime < GETDATE()
