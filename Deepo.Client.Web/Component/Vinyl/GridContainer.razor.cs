using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Navigation;
using Deepo.Client.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class GridContainer
{
    [Inject]
    private IStringLocalizer<Languages> Localizer { get; set; } = default!;

    [Inject]
    private IVinylCatalog VinylCatalog { get; set; } = default!;

    private EGridScrollMode _scrollMode = EGridScrollMode.InfiniteScroll;

    private bool _scrollModePopupIsOpen;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            this.VinylCatalog.OnPropertyChanged(StateHasChanged);
            await this.VinylCatalog.NextAsync().ConfigureAwait(false);
        }
    }
}

