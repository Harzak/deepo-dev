using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class InfiniteScrollGid
{
    [Inject]
    private IStringLocalizer<Languages> Localizer { get; set; } = default!;

    [Inject]
    private IVinylCatalog VinylCatalog { get; set; } = default!;

    private bool _loadingMore;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            this.VinylCatalog.OnPropertyChanged(StateHasChanged);
        }
    }

    private void OnExpandMoreClick(MouseEventArgs args)
    {
        _loadingMore = true;
        _ = this.LoadMoreAsync();
    }

    private async Task LoadMoreAsync()
    {
        await this.VinylCatalog.NextAsync().ConfigureAwait(false);
        _loadingMore = false;
        StateHasChanged();
    }
}

