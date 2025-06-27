using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Interfaces;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Filtering.Vinyl;

public class VinylFiltersStore : IVinylFiltersStore
{
   public event EventHandler? FilterChanged;

    private readonly IHttpService _httpService;

    private DateTime _searchedDate;
    private Collection<GenreDto> _searchedGenres;
    private Collection<GenreDto> _availableGenres;
    private Collection<string> _searchedMarkets;
    private Collection<string> _availableMarkets;

    public DateTime SearchDate => _searchedDate;
    public IReadOnlyCollection<GenreDto> SearchedGenres => _searchedGenres.AsReadOnly();
    public IReadOnlyCollection<GenreDto> AvailableGenres => _availableGenres.AsReadOnly();
    public IReadOnlyCollection<string> SearchedMarkets => _searchedMarkets.AsReadOnly();
    public IReadOnlyCollection<string> AvailableMarkets => _availableMarkets.AsReadOnly();

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
                _availableGenres = [..result.Content];
            }
        }

        _searchedGenres = _availableGenres;
        _searchedMarkets = _availableMarkets;
        _searchedDate = DateTime.Now;
        InvokeFilterChanged();
    }

    public void SetSearchedGenres(IEnumerable<GenreDto> genres)
    {
        IEnumerable<GenreDto> genreList = [.. genres];
        if (!_searchedGenres.SequenceEqual(genreList))
        {
            if (genreList.Any())
            {
                _searchedGenres = [.. genres];
            }
            else
            {
                _searchedGenres = _availableGenres;
            }
            InvokeFilterChanged();
        }
    }

    public void SetSearchedMarkets(IEnumerable<string> markets)
    {
        IEnumerable<string> marketList = [.. markets];
        if (!_searchedMarkets.SequenceEqual(markets))
        {
            if (marketList.Any())
            {
                _searchedMarkets = [.. markets];
            }
            else
            {
                _searchedMarkets = _availableMarkets;
            }
            InvokeFilterChanged();
        }
    }

    public void SetSearchedDate(DateTime date)
    {
        if (_searchedDate != date)
        {
            _searchedDate = date;
            InvokeFilterChanged();
        }
    }

    private void InvokeFilterChanged()
    {
        FilterChanged?.Invoke(this, EventArgs.Empty);
    }
}