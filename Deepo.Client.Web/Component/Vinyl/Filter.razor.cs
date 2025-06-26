using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.EventBus;
using Deepo.Client.Web.EventBus.Vinyl;
using Deepo.Client.Web.Pages;
using Deepo.Client.Web.Resources;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class Filter
{
    [Inject]
    private IHttpService HttpService { get; set; } = default!;

    [Inject]
    private IStringLocalizer<Languages> Localizer { get; set; } = default!;

    [Inject]
    private IVinylEventBus VinylEventBus { get; set; } = default!;

    private DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (value.HasValue)
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value.Value;
                    this.InvokeFilterChanged();
                }
            }
            else
            {
                _selectedDate = DateTime.Now;
                this.InvokeFilterChanged();
            }
        }
    }

    private DateTime _selectedDate;
    private DtoResult<List<GenreDto>>? _genreFetchResult;
    private List<GenreDto> _selectedGenres = [];
    private List<string> _selectedMarket = [];

    protected override async Task OnInitializedAsync()
    {
        await this.LoadGenresAsync().ConfigureAwait(false);
        this.SelectAllGenres();
        this.SelectAllMarkets();
        _selectedDate = DateTime.Now;
        this.InvokeFilterChanged();
    }

    private async Task LoadGenresAsync()
    {
        OperationResult<string> httpResult = await HttpService.GetAsync(HttpRoute.VINYL_GENRE_ROUTE, CancellationToken.None).ConfigureAwait(false);
        if (httpResult.IsSuccess && httpResult.HasContent)
        {
            _genreFetchResult = JsonConvert.DeserializeObject<DtoResult<List<GenreDto>>>(httpResult.Content);
        }
        else
        {
            _genreFetchResult = new DtoResult<List<GenreDto>>
            {
                IsSuccess = false,
                Content = []
            };
        }
    }

    private void OnSelectedGenresChanged(IEnumerable<GenreDto> args)
    {
        List<GenreDto> genreList = args.ToList();

        if (genreList.Count > 0)
        {
            _selectedGenres = genreList;
        }
        else
        {
            this.SelectAllGenres();
        }
        this.InvokeFilterChanged();
    }

    private void OnSelectedMarketChanged(IEnumerable<string> args)
    {
        List<string> markets = args.ToList();

        if (markets.Count > 0)
        {
            _selectedMarket = markets;
        }
        else
        {
            this.SelectAllMarkets();
        }
        this.InvokeFilterChanged();
    }

    private void SelectAllGenres()
    {
        _selectedGenres = _genreFetchResult?.Content?.ToList() ?? [];
    }

    private void SelectAllMarkets()
    {
        _selectedMarket = Constants.AVAILABLE_MARKETS.ToList();
    }

    private void InvokeFilterChanged()
    {
        VinylEventBus.FilterChanged(new VinylFilterEventArgs
        {
            Date = _selectedDate,
            Genres = _selectedGenres,
            Markets = _selectedMarket
        });
    }
}