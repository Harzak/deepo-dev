using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Interfaces;
using Deepo.Client.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using System.ComponentModel;

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
            this.VinylCatalog.Items.PropertyChanged += OnVinylCatalogChanged;
        }
    }

    private void OnVinylCatalogChanged(object? sender, PropertyChangedEventArgs e)
    {
        StateHasChanged();
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

