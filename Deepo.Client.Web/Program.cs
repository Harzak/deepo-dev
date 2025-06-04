using Deepo.Client.Web.Configuration;
using Deepo.Client.Web.Theme;
using Framework.Common.Utils.Time.Provider;
using Framework.Web.Http.Client.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System.Globalization;
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

        builder.Services.AddTransient<ITimeProvider, Framework.Common.Utils.Time.Provider.TimeProvider>();
        builder.Services.AddSingleton<IThemeProvider, ThemeProvider>();
        builder.Services.AddTransient<IHttpService, HttpService>();

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

