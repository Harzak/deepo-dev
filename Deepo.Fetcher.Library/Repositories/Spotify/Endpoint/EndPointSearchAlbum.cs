using Deepo.Fetcher.Library.Configuration;
using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Framework.Web.Endpoint;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;
using Deepo.Framework.Extensions;
using Deepo.Framework.Interfaces;

namespace Deepo.Fetcher.Library.Repositories.Spotify.Endpoint;

internal class EndPointSearchAlbum : MultipleResultEndpointConsumer<IEnumerable<Dto.Spotify.DtoSpotifyAlbum>?>, IPaginableEndpointQuery
{
    private const int OFFSET_MAX_RANGE = 1000;
    private const int LIMIT_MAX_RANGE = 50;

    public string Market { get; set; }

    private readonly HttpServiceOption _options;

    internal EndPointSearchAlbum(HttpServiceOption options, ILogger logger) : base(logger)
    {
        _options = options;
        Market = Constants.DEFAULT_MARKET;
    }

    #region HTTP Methods
    public override string Get(string query = "")
    {
        return $"v1/search?market={Market}&type=album&limit={LIMIT_MAX_RANGE}&q={query}";
    }
    public override string Options()
    {
        throw new NotImplementedException();
    }

    public override string Trace()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Pagination
    public int Total(string content)
    {
        return JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Total ?? -1;
    }

    public int Limit(string content)
    {
        return JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Limit ?? -1;
    }

    public int Offset(string content)
    {
        return JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Offset ?? -1;
    }

    public string? Next(string content)
    {
        string? nextQuery = JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Next;

        if (string.IsNullOrEmpty(nextQuery))
        {
            Dto.Spotify.ResultSpotify<Dto.Spotify.DtoSpotifyAlbum>? result = JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result;
            if (result?.Href != null && result?.Offset != null && result.Offset < OFFSET_MAX_RANGE)
            {
                Uri nextUri = new(result.Href);
                nextUri = nextUri.SetParameter("offset", (result.Offset + LIMIT_MAX_RANGE).ToString(CultureInfo.InvariantCulture));
                nextQuery = nextUri.PathAndQuery;  
            }
        }
        return nextQuery ?? string.Empty;
    }
    public string Last(string content)
    {
        throw new NotImplementedException();
    }
    #endregion

    protected override IEnumerable<Dto.Spotify.DtoSpotifyAlbum>? Parse(string content)
    {
        return JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Items;
    }
}
