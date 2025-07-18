using Deepo.DAL.Repository.Feature.Release;
using Deepo.Fetcher.Library.Dto.Discogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Mappers;

/// <summary>
/// Static mapper class for converting Discogs DTOs to album models.
/// Provides methods to transform Discogs release data into internal album representations.
/// </summary>
public static class AlbumMapper
{
    /// <summary>
    /// Maps a Discogs release DTO to an album model.
    /// Converts all relevant properties including metadata, tracklist, and provider identifiers.
    /// </summary>
    /// <param name="master">The Discogs release DTO to map.</param>
    /// <returns>An album model populated with data from the Discogs release.</returns>
    public static AlbumModel MapToAlbum(this DtoDiscogsRelease master)
    {
        ArgumentNullException.ThrowIfNull(master, nameof(master));
        AlbumModel model = new()
        {
            Title = master.Title,
            ReleaseDateUTC = TryParseToDate(master.Released),
            Artists = master.Artists,
            CoverURL = master.Images?.ElementAtOrDefault(0)?.Uri,
            Country = master.Country,
            Label = master.Labels?.ElementAtOrDefault(0)?.Name,  
            Genres = master.Genres ?? [],
            ThumbURL = master.ThumbsURL           
        };

        foreach (KeyValuePair<string, string> kvp in master.ProvidersIdentifier)
        {
            model.ProvidersIdentifier.Add(kvp.Key, kvp.Value);
        }

        if (master.Tracklist != null && master.Tracklist.Any())
        {
            foreach (var track in master.Tracklist)
            {
                TrackModel trackModel = new()
                {
                    Title = track.Title ?? string.Empty,
                    Duration = track.Duration ?? string.Empty,
                    Position = int.TryParse(track.Position, out int position) ? position : 0,
                };
                model.Tracklist = model.Tracklist.Append(trackModel);
            }
        }

        return model;
    }

    private static DateTime TryParseToDate(string? dateStr)
    {
        if (!DateTime.TryParse(dateStr, out DateTime dateParsed))
        {
            if(int.TryParse(dateStr, out int parsedYear))
            {
                dateParsed = new DateTime(parsedYear, 0, 0);    
            }
        }
        return dateParsed;
    }
}

