using Deepo.DAL.Service.Feature.ReleaseAlbum;
using Deepo.Fetcher.Library.Dto.Discogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Mappers;

public static class AlbumMapper
{
    public static AlbumModel MapToAlbum(this Master master)
    {
        ArgumentNullException.ThrowIfNull(master, nameof(master));
        AlbumModel model = new()
        {
            Title = master.Title,
            Artists = master.Artists,
            CoverURL = master.Images?.ElementAt(0)?.Uri,
            Country = master.Country,
            Label = master.Labels?.ElementAt(0)?.Name,  
            Genres = master.Genres ?? [],
            ThumbURL = master.ThumbsURL,
        };

        foreach (KeyValuePair<string, string> kvp in master.ProvidersIdentifier)
        {
            model.ProvidersIdentifier.Add(kvp.Key, kvp.Value);
        }

        return model;
    }
}

