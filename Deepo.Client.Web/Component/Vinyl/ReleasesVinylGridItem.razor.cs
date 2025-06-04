using Deepo.Client.Web.Dto;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class ReleasesVinylGridItem
{
    [Inject]
    public IDialogService? DialogService { get; set; }

    [Parameter]
    public ReleaseVinylDto? Item { get; set; }

    private const string TRUNCATION_TEXT = "...";

    private async Task OpenDialogAsync()
    {
        ArgumentNullException.ThrowIfNull(DialogService);
        DialogParameters<Guid> parameters = new()
        {
            { "Vinyl_ID", Item?.Id ?? Guid.Empty}
        };
        DialogOptions options = new()
        {
            CloseButton = true,
            BackdropClick = true,
            CloseOnEscapeKey = true,
            FullWidth = true
        };
        await DialogService.ShowAsync<ReleaseVinylDetailDialog>(string.Empty, parameters, options).ConfigureAwait(false);
    }
}