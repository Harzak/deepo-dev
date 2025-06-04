CREATE VIEW [dbo].[V_Plannification_Fetcher]
	AS 
	SELECT Fetcher.Name AS 'Name',
		Fetcher.Fetcher_GUID,
		PlanificationType.Name 'PlanificationTypeName',
		PlanificationType.Code,
		Planning.HourStart,
		Planning.MinuteStart,
		Planning.HourEnd,
		Planning.MinuteEnd
FROM dbo.Planification
INNER JOIN dbo.Fetcher ON dbo.Fetcher.Fetcher_ID = dbo.Planification.Fetcher_ID
INNER JOIN dbo.PlanificationType ON dbo.PlanificationType.PlanificationType_ID = dbo.Planification.PlanificationType_ID
LEFT JOIN dbo.Planning ON dbo.Planning.Planing_ID = dbo.Planification.Planning_ID
