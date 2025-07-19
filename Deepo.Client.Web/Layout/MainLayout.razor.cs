using Deepo.Client.Web.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Deepo.Client.Web.Layout;

public partial class MainLayout
{
    [Inject]
    private IThemeProvider? ThemeProvider { get; set; }
}