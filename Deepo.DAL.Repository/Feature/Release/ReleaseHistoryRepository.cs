using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.LogMessage;
using Deepo.DAL.Repository.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Feature.Release;

public class ReleaseHistoryRepository : IReleaseHistoryRepository
{
    private readonly ILogger _logger;
    private readonly IDbContextFactory<DEEPOContext> _contextFactory;

    public ReleaseHistoryRepository(IDbContextFactory<DEEPOContext> contextFactory, ILogger<ReleaseAlbumRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<DatabaseOperationResult> AddSpotifyReleaseFetchHistoryAsync(string identifier, DateTime dateUTC, CancellationToken cancellationToken = default)
    {
        DatabaseOperationResult result = new();

        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        Provider? spotifyProvider = await context.Providers
                                            .FirstOrDefaultAsync(p => p.Code == "SPOTIFY", cancellationToken)
                                            .ConfigureAwait(false)
                                            ?? throw new InvalidOperationException("Spotify provider not found");

        Type_Release vinylTypeRelease = await context.Type_Releases
                                                .FirstOrDefaultAsync(t => t.Code == "VINYL", cancellationToken)
                                                .ConfigureAwait(false)
                                                ?? throw new InvalidOperationException("Album type release not found");

        context.Release_Fetch_Histories.Add(new Release_Fetch_History()
        {
            Date_UTC = dateUTC,
            Identifier = identifier,
            Identifier_Desc = "spotify album id",
            Provider = spotifyProvider,
            Type_Release = vinylTypeRelease
        });

        try
        {
            int rowAffected = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            result.IsSuccess = rowAffected >= 0;
            result.RowAffected = rowAffected;
            return result;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Release_Fetch_History), context?.Database?.GetDbConnection()?.ConnectionString, ex);
            return result;
        }
        catch (Exception ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(Release_Fetch_History), context?.Database?.GetDbConnection()?.ConnectionString, ex);
            throw;
        }
    }

    public async Task<ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>> GetSpotifyReleaseFetchHistoryByDateAsync(DateTime minDateUTC, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        List<V_Spotify_Vinyl_Fetch_History> histories = await context.V_Spotify_Vinyl_Fetch_Histories
                                                                .Where(x => x.Date_UTC >= minDateUTC)
                                                                .OrderByDescending(x => x.Date_UTC)
                                                                .ToListAsync(cancellationToken)
                                                                .ConfigureAwait(false);

        return histories.AsReadOnly();
    }

    public async Task<ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>> GetSpotifyReleaseFetchHistoryByIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        List<V_Spotify_Vinyl_Fetch_History> histories = await context.V_Spotify_Vinyl_Fetch_Histories
                                                                .Where(x => x.Identifier == identifier)
                                                                .OrderByDescending(x => x.Date_UTC)
                                                                .ToListAsync(cancellationToken)
                                                                .ConfigureAwait(false);

        return histories.AsReadOnly();
    }
}