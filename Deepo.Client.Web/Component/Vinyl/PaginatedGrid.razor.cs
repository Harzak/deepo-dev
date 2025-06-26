using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class PaginatedGrid
{
    [Inject]
    private IVinylCatalog VinylCatalog { get; set; } = default!;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            this.VinylCatalog.OnPropertyChanged(StateHasChanged);
        }
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
