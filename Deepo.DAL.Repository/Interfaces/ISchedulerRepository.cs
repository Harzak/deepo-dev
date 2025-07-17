using Deepo.DAL.EF.Models;
using Deepo.Framework.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

public interface ISchedulerRepository
{
    Task<string?> GetCronExpressionForFectherAsync(string fectherIdentifier, CancellationToken cancellationToken);
    Task<Dictionary<string, string>> GetAllFetcherCronExpressionAsync(CancellationToken cancellationToken);
}