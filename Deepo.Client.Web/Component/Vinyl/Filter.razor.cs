using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Filtering;
using Deepo.Client.Web.Interfaces;
using Deepo.Client.Web.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class Filter
{
    [Inject]
    private IStringLocalizer<Languages> Localizer { get; set; } = default!;

    [Inject]
    private IVinylFiltersStore Filtering { get; set; } = default!;

    protected async override Task OnInitializedAsync()
    {
        await this.Filtering.InitializeAsync().ConfigureAwait(false);
    }

    private void OnSelectedGenresChanged(IEnumerable<GenreDto> args)
    {
        if (args != null)
        {
            this.Filtering.SetSearchedGenres(args);
        }
    }

    private void OnSelectedMarketChanged(IEnumerable<string> args)
    {
        if (args != null)
        {
            this.Filtering.SetSearchedMarkets(args);
        }
    }

    private void OnSelectedDateChanged(DateTime? args)
    {
        if (args.HasValue)
        {
            this.Filtering.SetSearchedDate(args.Value);
        }
    }
}