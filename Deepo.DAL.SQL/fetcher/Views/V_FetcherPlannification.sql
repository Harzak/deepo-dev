CREATE VIEW [fetcher].[V_FetcherPlannification]
	AS 
	SELECT Fetcher.Name AS 'Name',
		Fetcher.Fetcher_GUID,
		Fetcher.Code,
		PlanificationType.Name AS 'PlanificationTypeName',
		PlanificationType.Code AS 'PlanificationCode',
		Planning.HourStart,
		Planning.MinuteStart,
		Planning.HourEnd,
		Planning.MinuteEnd
FROM [fetcher].Planification
INNER JOIN [fetcher].Fetcher ON [fetcher].Fetcher.Fetcher_ID = [fetcher].Planification.Fetcher_ID
INNER JOIN [fetcher].PlanificationType ON [fetcher].PlanificationType.PlanificationType_ID = [fetcher].Planification.PlanificationType_ID
LEFT JOIN [fetcher].Planning ON [fetcher].Planning.Planing_ID = [fetcher].Planification.Planning_ID
