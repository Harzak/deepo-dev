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

    private DateTime? _selectedDate;
    private DtoResult<List<GenreDto>>? _genreFetchResult;
    private List<GenreDto> _selectedGenres = [];

    protected override async Task OnInitializedAsync()
    {
        _selectedDate = DateTime.Now;
        await this.LoadGenresAsync().ConfigureAwait(false);
        this.SelectAllGenres();
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

    private void SelectAllGenres()
    {
        _selectedGenres = _genreFetchResult?.Content?.ToList() ?? [];
    }

    private void InvokeFilterChanged()
    {
        VinylEventBus.FilterChanged(new VinylFilterEventArgs
        {
            SelectedDate = _selectedDate,
            SelectedGenres = _selectedGenres
        });
    }
}