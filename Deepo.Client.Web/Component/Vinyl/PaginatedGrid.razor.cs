using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Interfaces;
using Deepo.Client.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.ComponentModel;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class PaginatedGrid
{
    [Inject]
    private IVinylCatalog VinylCatalog { get; set; } = default!;

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

    public int CurrentPageIndex
    {
        get => this.VinylCatalog.CurrentPageIndex;
        set
        {
            if (value < this.VinylCatalog.CurrentPageIndex)
            {
                _ = this.VinylCatalog.PreviousAsync().ConfigureAwait(false);
            }
            else if (value > this.VinylCatalog.CurrentPageIndex)
            {
                _ = this.VinylCatalog.NextAsync().ConfigureAwait(false);
            }
        }
    }
}
