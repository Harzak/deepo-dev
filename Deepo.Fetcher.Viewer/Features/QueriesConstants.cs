namespace Deepo.Fetcher.Viewer.Features;

internal static class QueriesConstants
{
    public const string MovieSubscriptionQuery = "SELECT [Release_Movie_ID] FROM [dbo].[Release_Movie]";
    public const string VinyleSubscriptionQuery = "SELECT [Release_Album_ID] FROM [dbo].[Release_Album]";
    public const string HttpRequestSubscriptionQuery = "SELECT [HttpRequest_ID] FROM [fetcher].[HttpRequest]";
}