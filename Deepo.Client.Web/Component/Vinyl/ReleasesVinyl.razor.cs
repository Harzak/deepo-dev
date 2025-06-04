using Deepo.Client.Web.Dto;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class ReleasesVinyl
{
    [Inject]
    private IHttpService? HttpService { get; set; }

    private const int MAX_ITEM_PERPAGE = 20;

    private int _maxItem;
    private int _selectedPage = 1;
    private int SelectedPage
    {
        get => _selectedPage;
        set
        {
            if (_selectedPage != value)
            {
                _selectedPage = value;
                StateHasChanged();
            }
        }
    }
    private int MaxPage => (_maxItem + MAX_ITEM_PERPAGE - 1) / MAX_ITEM_PERPAGE;

    protected async override Task OnInitializedAsync()
    {
        await GetMaxResultCountAsync().ConfigureAwait(false);
    }

    private async Task GetMaxResultCountAsync()
    {
        ArgumentNullException.ThrowIfNull(HttpService);

        OperationResult<string> httpResult = await HttpService.GetAsync("vinyl/count?market=FR", CancellationToken.None).ConfigureAwait(false);
        if (httpResult.IsSuccess && httpResult.HasContent)
        {
            DtoResult<int>? countResult = JsonConvert.DeserializeObject<DtoResult<int>>(httpResult.Content);
            if (countResult != null && countResult.IsSuccess && countResult.HasContent)
            {
                _maxItem = countResult.Content;
                return;
            }
        }
    }
}

