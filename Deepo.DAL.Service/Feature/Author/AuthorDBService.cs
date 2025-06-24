using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.DAL.Service.LogMessage;
using Deepo.DAL.Service.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Service.Feature.Author;

internal class AuthorDBService : IAuthorDBService
{
    protected readonly ILogger _logger;
    protected readonly DEEPOContext _dbContext;

    public AuthorDBService(DEEPOContext dbContext, ILogger<AuthorDBService> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<DatabaseOperationResult> Insert(IAuthor item, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        DatabaseOperationResult result = new();

        try
        {
            _dbContext.Authors.Add(new Models.Author
            {
                Name = item.Name ?? string.Empty,
                Code = item.Name?.Trim().Length > 10 ? item.Name?.Trim()[..10].ToUpper(CultureInfo.CurrentCulture) : item.Name?.Trim().ToUpper(new CultureInfo(0x040A, false)),
                Provider = _dbContext.Providers.First(x => x.Code == item.Provider_Code),
                Provider_Author_Identifier = item.Provider_Identifier
            });

            int rowAffected = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            result.IsSuccess = rowAffected > -1;
            result.RowAffected = rowAffected;
        }
        catch (DbUpdateException ex)
        {
            DatabaseLogs.UnableToAdd(_logger, nameof(_dbContext.Authors), _dbContext?.Database?.GetDbConnection()?.ConnectionString, ex);
        }
        catch (Exception)
        {
            throw;
        }

        return result;
    }

    public bool Exists(IAuthor item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        return _dbContext.Authors.Any(x => x.Provider.Code == item.Provider_Code && x.Provider_Author_Identifier == item.Provider_Identifier);
    }

    public Models.Author GetByProviderIdentifier(string identifier)
    {
        return _dbContext.Authors.First(x => x.Provider_Author_Identifier == identifier);
    }
}
