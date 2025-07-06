using Deepo.Framework.Exceptions;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Results;
using Deepo.Framework.Web.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Framework.Web.Service;

public abstract class AuthenticatedHttpService : HttpService
{
    protected IAuthenticationHttpService AuthService { get; }

    private TokenModel _token;

    protected AuthenticatedHttpService(IHttpClientFactory httpClientFactory,
        IDateTimeFacade timeProvider,
        IAuthenticationHttpService authHttpService,
        IHttpClientOption option,
        ILogger<AuthenticatedHttpService> logger)
    : base(httpClientFactory,
          timeProvider,
          option,
          logger)
    {
        AuthService = authHttpService;
        _token = new TokenModel();
    }

    public override async Task<OperationResult<string>> GetAsync(Uri endpoint, CancellationToken cancellationToken)
    {
        if (await VerificationToken(cancellationToken).ConfigureAwait(false))
        {
            return await base.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            throw new MissingTokenException();
        }
    }

    public override async Task<OperationResult<string>> PostAsync(Uri endpoint, HttpContent content, CancellationToken cancellationToken)
    {
        if (await VerificationToken(cancellationToken).ConfigureAwait(false))
        {
            return await base.PostAsync(endpoint, content, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            throw new MissingTokenException();
        }
    }

    private async Task<bool> VerificationToken(CancellationToken cancellationToken)
    {
        if (!_token.IsExpired)
        {
            return true;
        }

        TokenModel? token = await AuthService.RefreshTokenAsync(cancellationToken).ConfigureAwait(false);

        if (token == null)
        {
            return false;
        }

        SetToken(token);
        return true;
    }

    protected void SetToken(TokenModel newToken)
    {
        _token = newToken;
        SetTokenValue(_token.Value);
    }

    protected abstract void SetTokenValue(string token);
}
