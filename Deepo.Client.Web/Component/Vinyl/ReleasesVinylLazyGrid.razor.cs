using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Resources;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class ReleasesVinylLazyGrid
{
    [Inject]
    private IHttpService HttpService { get; set; } = default!;

    [Inject]
    private IStringLocalizer<Languages> Localizer { get; set; } = default!;

    [Parameter]
    public int MaxItem { get; set; }

    [Parameter]
    public int Position { get; set; }

    [Parameter]
    public bool IsVisible { get; set; }

    private bool _isLoaded;
    private DtoResult<List<ReleaseVinylDto>>? _releasesFetchResult;
    private DtoResult<List<GenreDto>>? _genreFetchResult;
    private List<ReleaseVinylDto> _releasesFiltered = [];
    private List<GenreDto> _selectedGenres = [];

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.IsVisible && !_isLoaded)
        {
            await this.LoadReleasesAsync().ConfigureAwait(false);
            await this.LoadGenresAsync().ConfigureAwait(false);
            this.SelectAllGenres();
            this.ApplyFilters();
            StateHasChanged();
        }
    }

    private async Task LoadReleasesAsync()
    {
        string query = string.Format(CultureInfo.InvariantCulture, HttpRoute.VINYL_RELEASE_ROUTE, (this.Position * this.MaxItem), this.MaxItem);
        OperationResult<string> httpResult = await HttpService.GetAsync(query, CancellationToken.None).ConfigureAwait(false);

        if (httpResult.IsSuccess && httpResult.HasContent)
        {
            _releasesFetchResult = JsonConvert.DeserializeObject<DtoResult<List<ReleaseVinylDto>>>(httpResult.Content);
            _isLoaded = true;
        }
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
        this.ApplyFilters();
    }

    private void SelectAllGenres()
    {
        _selectedGenres = _genreFetchResult?.Content?.ToList() ?? [];
    }

    private void ApplyFilters()
    {
        _releasesFiltered = _releasesFetchResult?.Content?.Where(release => _selectedGenres.Any(genre => release.Genres.Any(x => x.Identifier == genre.Identifier))).ToList() ?? [];
    }
}

