using Deepo.Framework.Web.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for HTTP services that provide authentication capabilities and token management.
/// </summary>
public interface IAuthenticationHttpService : IHttpService
{
    /// <summary>
    /// Asynchronously refreshes the authentication token for continued API access.
    /// </summary>
    public Task<TokenModel?> RefreshTokenAsync(CancellationToken cancellationToken);
}