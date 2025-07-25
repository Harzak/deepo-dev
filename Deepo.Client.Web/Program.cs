using Deepo.Client.Web.Catalog;
using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Dto;
using Deepo.Client.Web.Filtering.Vinyl;
using Deepo.Client.Web.Interfaces;
using Deepo.Client.Web.Theme;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Time;
using Deepo.Framework.Web.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Reflection;

namespace Deepo.Client.Web;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddHttpClient();
        builder.Services.AddLocalization();

        builder.Services.AddMudServices();

        builder.Services.AddTransient<IDateTimeFacade, DateTimeFacade>();
        builder.Services.AddSingleton<IThemeProvider, ThemeProvider>();
        builder.Services.AddTransient<IHttpService, HttpService>();
        builder.Services.AddSingleton<IVinylCatalog, VinylLazyCatalog>();


        builder.Services.AddSingleton<IVinylFiltersStore, VinylFiltersStore>();
        builder.Services.AddSingleton<IFilter<ReleaseVinylDto>, VinylFilter>();

        builder.Services.AddSingleton<IHttpClientOption>(e => new HttpClientOption()
        {
            BaseAddress = new Uri(HttpRoute.API_BASE_ADRESS),
            Name = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty,
            TaskID = Guid.NewGuid().ToString(),
            UserAgent = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty
        });

        var app = builder.Build();
        await app.RunAsync().ConfigureAwait(false);
    }
}

