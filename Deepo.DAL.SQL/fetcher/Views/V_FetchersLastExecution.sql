CREATE VIEW [fetcher].[V_FetchersLastExecution]
	AS 
	SELECT
	  [fetcher].Fetcher.Fetcher_GUID,
	  [fetcher].Fetcher.Code,
	  [fetcher].Fetcher.Name,
	  Max([fetcher].Execution.StartedAt) AS 'StartedAt',
	  Max([fetcher].Execution.EndedAt) AS 'EndedAt'
	FROM
	  [fetcher].Execution
	  RIGHT JOIN [fetcher].Fetcher ON [fetcher].Fetcher.Fetcher_ID = [fetcher].Execution.Fetcher_ID
	GROUP BY
	  [fetcher].Fetcher.Fetcher_ID,
	  [fetcher].Fetcher.Code,
	  [fetcher].Fetcher.Name,
	  [fetcher].Fetcher.Fetcher_GUID