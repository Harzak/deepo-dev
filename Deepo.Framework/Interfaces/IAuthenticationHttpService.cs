using Deepo.Framework.Web.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Framework.Interfaces;

public interface IAuthenticationHttpService
{
    public Task<TokenModel?> RefreshTokenAsync(CancellationToken cancellationToken);
}