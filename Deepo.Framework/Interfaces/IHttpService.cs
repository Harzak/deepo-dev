using Deepo.Framework.Results;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Framework.Interfaces;

public interface IHttpService
{
    public Task<OperationResult<string>> GetAsync(string endpoint, CancellationToken cancellationToken);
    public Task<OperationResult<string>> PostAsync(string endpoint, HttpContent content, CancellationToken cancellationToken);
}
