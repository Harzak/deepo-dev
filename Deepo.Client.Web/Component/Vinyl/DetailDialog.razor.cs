using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Deepo.Client.Web.Component.Vinyl;

public partial class DetailDialog
{
    [Parameter]
    public Guid VinylID { get; set; }
}

