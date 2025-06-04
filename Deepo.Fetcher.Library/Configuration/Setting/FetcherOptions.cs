namespace Deepo.Fetcher.Library.Configuration.Setting;

public class FetcherOptions
{
    public const string Fetcher = "Fetcher";

    public string FetcherVinyleName { get; set; } = string.Empty;
    public string FetcherVinyleId { get; set; } = string.Empty;
    public string FetcherMovieName { get; set; } = string.Empty;
    public string FetcherMovieId { get; set; } = string.Empty;
    public string FetcherDebugName { get; set; } = string.Empty;
    public string FetcherDebugId { get; set; } = string.Empty;

    public FetcherOptions()
    {

    }
}

