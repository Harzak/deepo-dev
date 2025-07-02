using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.LogMessage;
using Deepo.DAL.Repository.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Feature.Author;

internal sealed class AuthorRepository : IAuthorRepository
{
    private readonly ILogger _logger;
    private readonly IDbContextFactory<DEEPOContext> _contextFactory;

    public AuthorRepository(IDbContextFactory<DEEPOContext> contextFactory, ILogger<AuthorRepository> logger)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<DatabaseOperationResult> Insert(IAuthor item, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        DatabaseOperationResult result = new();

        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        Provider provider = await context.Providers
                                    .FirstAsync(x => x.Code == item.Provider_Code, cancellationToken)
                                    .ConfigureAwait(false);

        context.Authors.Add(new Models.Author
        {
            Name = item.Name ?? string.Empty,
            Code = item.Name?.Trim().Length > 10
                ? item.Name?.Trim()[..10].ToUpper(CultureInfo.CurrentCulture)
                : item.Name?.Trim().ToUpper(new CultureInfo(0x040A, false)),
            Provider = provider,
            Provider_Author_Identifier = item.Provider_Identifier
        });

        try
        {
            int rowAffected = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            result.IsSuccess = rowAffected > -1;
            result.RowAffected = rowAffected;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(context.Authors), context?.Database?.GetDbConnection()?.ConnectionString, ex);
        }
        catch (Exception)
        {
            throw;
        }

        return result;
    }

    public async Task<bool> ExistsAsync(IAuthor item, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.Authors
                        .AnyAsync(x => x.Provider.Code == item.Provider_Code &&
                                       x.Provider_Author_Identifier == item.Provider_Identifier,
                                 cancellationToken)
                        .ConfigureAwait(false);
    }

    public async Task<Models.Author?> GetByProviderIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
    {
        using DEEPOContext context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        return await context.Authors
            .FirstOrDefaultAsync(x => x.Provider_Author_Identifier == identifier, cancellationToken)
            .ConfigureAwait(false);
    }
}
