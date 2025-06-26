using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.EventBus.Vinyl;
using Deepo.Client.Web.Navigation;
using Deepo.Client.Web.Resources;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class GridContainer
{
    [Inject]
    private IStringLocalizer<Languages> Localizer { get; set; } = default!;

    [Inject]
    private IVinylCatalog VinylCatalog { get; set; } = default!;

    private EGridScrollMode _scrollMode = EGridScrollMode.InfiniteScroll;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            this.VinylCatalog.OnPropertyChanged(StateHasChanged);
            await this.VinylCatalog.NextAsync().ConfigureAwait(false);
        }
    }

}

