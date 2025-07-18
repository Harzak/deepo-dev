using Deepo.DAL.EF.Models;
using Deepo.Framework.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

/// <summary>
/// Repository interface for managing scheduled fetcher operations and cron expressions.
/// </summary>
public interface ISchedulerRepository
{
    /// <summary>
    /// Retrieves the cron expression for a specific fetcher.
    /// </summary>
    Task<string?> GetCronExpressionForFectherAsync(string fectherIdentifier, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves all fetcher identifiers mapped to their corresponding cron expressions.
    /// </summary>
    Task<Dictionary<string, string>> GetAllFetcherCronExpressionAsync(CancellationToken cancellationToken);
}