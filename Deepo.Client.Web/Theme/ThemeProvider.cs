using MudBlazor;

namespace Deepo.Client.Web.Theme;

public class ThemeProvider : IThemeProvider
{
    private const string YELLOW_DARK = "#d2b770";
    //private const string YELLOW_LIGHT = "#fff3bf";

    public MudTheme BuildMudTheme() => new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = Colors.Blue.Default,
            Secondary = Colors.Green.Accent4
        },
        PaletteDark = new PaletteDark()
        {
            Surface = Colors.Gray.Darken3,
            ActionDefault = YELLOW_DARK,
            TextDisabled = Colors.Gray.Default,
            TextPrimary = Colors.Gray.Lighten3,
            Primary = YELLOW_DARK,
            Background = Colors.Gray.Darken4
        },


        LayoutProperties = new LayoutProperties()
        {
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "300px"
        }
    };
}