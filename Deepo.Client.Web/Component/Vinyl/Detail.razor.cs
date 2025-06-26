using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Resources;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Newtonsoft.Json;
using System.Globalization;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class Detail
{
    [Inject]
    private IHttpService HttpService { get; set; } = default!;

    [Inject]
    private IStringLocalizer<Languages> Localizer { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public Guid VinylID { get; set; }

    private DtoResult<ReleaseVinylExDto>? _releaseResult;

    protected async override void OnInitialized()
    {
        await LoadReleaseAsync().ConfigureAwait(false);
    }

    private async Task LoadReleaseAsync()
    {
        string query = string.Format(CultureInfo.InvariantCulture, HttpRoute.VINYL_RELEASE_BY_ID_ROUTE, VinylID);
        OperationResult<string> httpResult = await HttpService.GetAsync(query, CancellationToken.None).ConfigureAwait(false);

        if (httpResult.IsSuccess && httpResult.HasContent)
        {
            _releaseResult = JsonConvert.DeserializeObject<DtoResult<ReleaseVinylExDto>>(httpResult.Content);
        }

        StateHasChanged();
    }
}