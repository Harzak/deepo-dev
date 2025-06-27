using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Interfaces;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Filtering.Vinyl;

public sealed class VinylFiltersStore : IVinylFiltersStore
{
    public event EventHandler<FilterEventArgs>? FilterChanged;

    private readonly IHttpService _httpService;

    private readonly Collection<GenreDto> _searchedGenres;
    private readonly Collection<string> _searchedMarkets;
    private Collection<GenreDto> _availableGenres;
    private Collection<string> _availableMarkets;
    private DateTime _searchedDate;

    public DateTime SearchDate => _searchedDate;
    public IEnumerable<GenreDto> SearchedGenres => _searchedGenres;
    public IEnumerable<GenreDto> AvailableGenres => _availableGenres;
    public IEnumerable<string> SearchedMarkets => _searchedMarkets;
    public IEnumerable<string> AvailableMarkets => _availableMarkets;

    public VinylFiltersStore(IHttpService httpService)
    {
        _httpService = httpService;

        _searchedGenres = [];
        _availableGenres = [];
        _searchedMarkets = [];
        _availableMarkets = [];
        _searchedDate = DateTime.MinValue;
    }

    public async Task InitializeAsync()
    {
        _availableMarkets = [.. Constants.AVAILABLE_MARKETS];

        OperationResult<string> httpResult = await _httpService.GetAsync(HttpRoute.VINYL_GENRE_ROUTE, CancellationToken.None).ConfigureAwait(false);
        if (httpResult.IsSuccess && httpResult.HasContent)
        {
            DtoResult<List<GenreDto>>? result = JsonConvert.DeserializeObject<DtoResult<List<GenreDto>>>(httpResult.Content);
            if (result?.IsSuccess == true && result.Content is not null)
            {
                _availableGenres = [.. result.Content];
            }
        }

        this.SetSearchedGenres(_availableGenres);
        this.SetSearchedMarkets(_availableMarkets);
        this.SetSearchedDate(DateTime.Now);
        this.InvokeFilterChanged();
    }

    public void SetSearchedGenres(IEnumerable<GenreDto> genres)
    {
        GenreDto[] genreArray = genres as GenreDto[] ?? genres.ToArray();

        if (_searchedGenres.Count != genreArray.Length || !_searchedGenres.SequenceEqual(genreArray))
        {
            _searchedGenres.Clear();

            if (genreArray.Length > 0)
            {
                foreach (GenreDto genre in genreArray)
                {
                    _searchedGenres.Add(genre);
                }
            }
            else
            {
                foreach (GenreDto genre in _availableGenres)
                {
                    _searchedGenres.Add(genre);
                }
            }
            this.InvokeFilterChanged();
        }
    }

    public void SetSearchedMarkets(IEnumerable<string> markets)
    {
        string[] marketsArray = markets as string[] ?? markets.ToArray();

        if (_searchedMarkets.Count != marketsArray.Length || !_searchedMarkets.SequenceEqual(marketsArray))
        {
            _searchedMarkets.Clear();

            if (marketsArray.Length > 0)
            {
                foreach (string market in marketsArray)
                {
                    _searchedMarkets.Add(market);
                }
            }
            else
            {
                foreach (string market in _availableMarkets)
                {
                    _searchedMarkets.Add(market);
                }
            }
            this.InvokeFilterChanged();
        }
    }

    public void SetSearchedDate(DateTime date)
    {
        if (_searchedDate != date)
        {
            _searchedDate = date;
            this.InvokeFilterChanged();
        }
    }

    private void InvokeFilterChanged()
    {
        FilterChanged?.Invoke(this, new FilterEventArgs(this.SearchDate, this.SearchedGenres, this.SearchedMarkets));
    }
}