using Deepo.Framework.Results;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for HTTP service operations providing asynchronous GET and POST functionality.
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// Asynchronously performs an HTTP GET request to the specified endpoint.
    /// </summary>
    public Task<OperationResult<string>> GetAsync(string endpoint, CancellationToken cancellationToken);
    
    /// <summary>
    /// Asynchronously performs an HTTP POST request to the specified endpoint with the provided content.
    /// </summary>
    public Task<OperationResult<string>> PostAsync(string endpoint, HttpContent content, CancellationToken cancellationToken);
}