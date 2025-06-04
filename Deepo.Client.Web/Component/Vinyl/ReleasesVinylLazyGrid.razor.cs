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
    private DtoResult<List<ReleaseVinylDto>>? _releasesResult;

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.IsVisible && !_isLoaded)
        {
            await LoadReleasesAsync().ConfigureAwait(false);
            StateHasChanged();
        }
    }

    private async Task LoadReleasesAsync()
    {
        string query = string.Format(CultureInfo.InvariantCulture, HttpRoute.VINYL_RELEASE_ROUTE, (this.Position * this.MaxItem), this.MaxItem);
        OperationResult<string> httpResult = await HttpService.GetAsync(query, CancellationToken.None).ConfigureAwait(false);

        if (httpResult.IsSuccess && httpResult.HasContent)
        {
            _releasesResult = JsonConvert.DeserializeObject<DtoResult<List<ReleaseVinylDto>>>(httpResult.Content);
            _isLoaded = true;
        }
    }
}

