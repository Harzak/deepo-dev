using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class DetailDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public Guid Vinyl_ID { get; set; }
}

