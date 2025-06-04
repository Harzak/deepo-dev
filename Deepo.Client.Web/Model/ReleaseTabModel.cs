using Deepo.Client.Web.Navigation;
using MudBlazor;

namespace Deepo.Client.Web.Model;

public class ReleaseTabModel
{
    public EReleaseType Type { get; set; }
    public string Label { get; set; }
    public string ToolTip { get; set; }
    public bool Disabled { get; set; }
    public string Icon { get; set; }

    public ReleaseTabModel()
    {
        this.Type = EReleaseType.Unknown;
        this.Label = string.Empty;
        this.ToolTip = string.Empty;
        this.Icon = Icons.Material.Outlined.HelpOutline;
        this.Disabled = true;
    }
}