using MudBlazor;

namespace Mediporta.Client.App.Config;

public static class ThemeBuilder
{
    public static MudTheme Build()
    {
        return new MudTheme
        {
            LayoutProperties = new LayoutProperties()
            {
                AppbarHeight = "50px"
            },
            PaletteLight = BuildLightTheme(),
        };
    }

    private static PaletteLight BuildLightTheme()
    {
        var palette = new PaletteLight
        {
            Primary = "#60519b",
            TextPrimary = "#bfc0d1",
            Background = "#1e202e",
            BackgroundGray = "#31323e",
            Surface = "#31323e", // tło okna dialogowego, tabeli, card itd
        };

        return palette;
    }
}