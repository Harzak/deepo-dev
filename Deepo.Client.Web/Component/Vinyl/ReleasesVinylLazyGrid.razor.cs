using Deepo.Client.Web.Dto;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class ReleasesVinylLazyGrid
{
    [Inject]
    private IHttpService? HttpService { get; set; }

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
            await GetReleasesAsync().ConfigureAwait(false);
            StateHasChanged();
        }
    }

    private async Task GetReleasesAsync()
    {
        ArgumentNullException.ThrowIfNull(HttpService);

        string query = $"vinyl?market=FR&offset={this.Position * this.MaxItem}&limit={this.MaxItem}";
        OperationResult<string> httpResult = await HttpService.GetAsync(query, CancellationToken.None).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(httpResult?.Content))
        {
            _releasesResult = JsonConvert.DeserializeObject<DtoResult<List<ReleaseVinylDto>>>(httpResult.Content);
            _isLoaded = true;
        }
    }

}

