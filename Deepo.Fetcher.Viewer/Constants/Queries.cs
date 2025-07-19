namespace Deepo.Fetcher.Viewer.Constants;

/// <summary>
/// Contains SQL query constants used for database subscriptions and monitoring.
/// </summary>
internal static class Queries
{
    /// <summary>
    /// SQL query to select movie release identifiers from the Release_Movie table.
    /// </summary>
    public const string MovieSubscriptionQuery = "SELECT [Release_Movie_ID] FROM [dbo].[Release_Movie]";
    
    /// <summary>
    /// SQL query to select vinyl release identifiers from the Release_Album table.
    /// </summary>
    public const string VinyleSubscriptionQuery = "SELECT [Release_Album_ID] FROM [dbo].[Release_Album]";
    
    /// <summary>
    /// SQL query to select HTTP request identifiers from the fetcher HttpRequest table.
    /// </summary>
    public const string HttpRequestSubscriptionQuery = "SELECT [HttpRequest_ID] FROM [fetcher].[HttpRequest]";
    
    /// <summary>
    /// SQL query to select fetcher execution identifiers from the fetcher Execution table.
    /// </summary>
    public const string FectherExecutionSubscriptionQuery = "SELECT [Execution_ID] FROM [fetcher].[Execution]";
}