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

/// <summary>
/// Provides an abstract HTTP service with automatic authentication token management and verification.
/// </summary>
public abstract class AuthenticatedHttpService : HttpService
{
    /// <summary>
    /// Gets the authentication service used for token management.
    /// </summary>
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

    /// <summary>
    /// Asynchronously performs an authenticated HTTP GET request to the specified URI endpoint.
    /// </summary>
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

    /// <summary>
    /// Asynchronously performs an authenticated HTTP POST request to the specified URI endpoint with content.
    /// </summary>
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

    /// <summary>
    /// Sets the authentication token value for subsequent HTTP requests.
    /// </summary>
    protected void SetToken(TokenModel newToken)
    {
        _token = newToken;
        SetTokenValue(_token.Value);
    }

    /// <summary>
    /// Sets the authentication token value for subsequent HTTP requests.
    /// </summary>
    protected abstract void SetTokenValue(string token);
}
