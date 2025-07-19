using System.Runtime.CompilerServices;

namespace Deepo.Fetcher.Library.Interfaces;

internal interface IFetchFactory
{
    IFetch CreateFetchVinyl(Dto.Spotify.DtoSpotifyAlbum initialData);
}
