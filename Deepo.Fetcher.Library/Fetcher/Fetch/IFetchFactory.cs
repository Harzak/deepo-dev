using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Deepo.Fetcher.Library.Fetcher.Fetch;

internal interface IFetchFactory
{
    IFetch CreateFetchVinyl(Dto.Spotify.Album initialData);
}
