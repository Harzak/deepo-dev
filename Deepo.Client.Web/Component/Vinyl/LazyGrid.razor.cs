using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.EventBus.Vinyl;
using Deepo.Client.Web.Resources;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class LazyGrid : IVinylEventBusSubscriber
{
    [Inject]
    private IHttpService HttpService { get; set; } = default!;

    [Inject]
    private IStringLocalizer<Languages> Localizer { get; set; } = default!;

    [Inject]
    private IVinylEventBus VinylEventBus { get; set; } = default!;

    [Parameter]
    public int MaxItem { get; set; }

    [Parameter]
    public int Position { get; set; }

    [Parameter]
    public bool IsVisible { get; set; }

    private bool _isLoaded;
    private DtoResult<List<ReleaseVinylDto>>? _releasesFetchResult;
    private List<ReleaseVinylDto> _releasesFiltered = [];

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.IsVisible && !_isLoaded)
        {
            VinylEventBus.Subscribe(this);
            await this.LoadReleasesAsync().ConfigureAwait(false);
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
            _releasesFiltered = _releasesFetchResult?.Content ?? [];
        }
    }

    public void OnFilterChanged(VinylFilterEventArgs args)
    {
        _releasesFiltered = _releasesFetchResult?.Content?.Where(release => args.SelectedGenres.Any(genre => release.Genres.Any(x => x.Identifier == genre.Identifier))).ToList() ?? [];
        StateHasChanged();
    }

    public void Dispose()
    {
        VinylEventBus.Unsubscribe(this);
    }
}

