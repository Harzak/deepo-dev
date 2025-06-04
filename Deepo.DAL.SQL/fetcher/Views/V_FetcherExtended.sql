CREATE VIEW [fetcher].[V_FetcherExtended] AS
SELECT
  [fetcher].Fetcher.Fetcher_GUID,
  [fetcher].Fetcher.Name AS 'FetcherName',
  [fetcher].Planification.DateNextStart,
  [fetcher].PlanificationType.Name AS 'PlanificationTypeName',
  [fetcher].PlanificationType.Code,
  [fetcher].Planning.HourStart,
  [fetcher].Planning.HourEnd,
  [fetcher].Planning.MinuteStart,
  [fetcher].Planning.MinuteEnd,
  Max([fetcher].Execution.StartedAt) AS 'LastStart',
  Max([fetcher].Execution.EndedAt) AS 'LastEnd'
FROM
  [fetcher].Fetcher
  LEFT JOIN [fetcher].Planification ON [fetcher].Planification.Fetcher_ID = [fetcher].Fetcher.Fetcher_ID
  LEFT JOIN [fetcher].PlanificationType ON [fetcher].PlanificationType.PlanificationType_ID = [fetcher].Planification.PlanificationType_ID
  LEFT JOIN [fetcher].Planning ON [fetcher].Planning.Planing_ID = [fetcher].Planification.Planning_ID
  LEFT JOIN [fetcher].Execution ON [fetcher].Execution.Fetcher_ID = [fetcher].Fetcher.Fetcher_ID
  group by [fetcher].Fetcher.Fetcher_GUID,
  [fetcher].Fetcher.Name,
  [fetcher].Planification.DateNextStart,
  [fetcher].PlanificationType.Name,
  [fetcher].PlanificationType.Code,
  [fetcher].Planning.HourStart,
  [fetcher].Planning.HourEnd,
  [fetcher].Planning.MinuteStart,
  [fetcher].Planning.MinuteEnd