using Deepo.Client.Web.Dto;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class GridItem
{
    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    [Parameter]
    public ReleaseVinylDto Item { get; set; } = default!;

    private const string TRUNCATION_CHARS = "...";

    private async Task OpenDialogAsync()
    {
        DialogParameters<Guid> parameters = new()
        {
            { "VinylID", Item?.Id ?? Guid.Empty}
        };
        DialogOptions options = new()
        {
            CloseButton = true,
            BackdropClick = true,
            CloseOnEscapeKey = true,
            FullWidth = true
            
        };
        await DialogService.ShowAsync<DetailDialog>(string.Empty, parameters, options).ConfigureAwait(false);
    }
}