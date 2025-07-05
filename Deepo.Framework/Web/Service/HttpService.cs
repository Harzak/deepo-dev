using Deepo.Framework.Interfaces;
using Deepo.Framework.LogMessages;
using Deepo.Framework.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Framework.Web.Service;

public class HttpService : IHttpService, IDisposable
{
    public string Name { get; }

    private bool _disposedValue;
    private readonly ILogger<HttpService> _logger;

    private readonly ITimeProvider _timeProvider;
    private readonly string _user_agent;
    private readonly HttpClient _httpClient;

    private HttpService(HttpClient httpClient, ITimeProvider timeProvider, IHttpClientOption option, ILogger<HttpService> logger)
    {
        ArgumentNullException.ThrowIfNull(option, nameof(option));

        Name = option.Name;
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger;
        _timeProvider = timeProvider;
        _user_agent = option.UserAgent;

        _httpClient.BaseAddress = option.BaseAddress;
        _httpClient.DefaultRequestHeaders.Add(HttpClientConstants.HEADER_NAME_TASKID, option.TaskID);
        Initialize();
    }

    public HttpService(IHttpClientFactory httpClientFactory,
        ITimeProvider timeProvider,
        IHttpClientOption option,
        ILogger<HttpService> logger)

        : this(httpClientFactory?.CreateClient(option?.Name ?? "") ?? throw new ArgumentNullException(nameof(httpClientFactory)),
              timeProvider,
              option ?? throw new ArgumentNullException(nameof(option)),
              logger)
    {

    }

    private void Initialize()
    {
        SetHeader();
    }

    #region GET Methods
    public virtual async Task<OperationResult<string>> GetAsync(string endpoint, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(endpoint);

        if (!ToValidUri(endpoint, out Uri? relativeUri) || relativeUri == null)
        {
            return new OperationResult<string>().WithError("Invalid URI");
        }

        return await GetAsync(relativeUri, cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<OperationResult<string>> GetAsync(Uri endpoint, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(endpoint);

        HttpResponseMessage? response = await SendGetRequestAsync(endpoint, cancellationToken).ConfigureAwait(false);

        if (response is null)
        {
            return new OperationResult<string>().WithError("Unknown error");
        }
        return await Read(response, cancellationToken).ConfigureAwait(false);
    }

    public virtual async IAsyncEnumerable<string> GetAsync(string endpoint, IPaginableEndpointQuery paginableQuery, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentNullException.ThrowIfNull(paginableQuery);  

        string? query = endpoint;

        while (!string.IsNullOrEmpty(query?.Trim()))
        {
            string jsonResponse = (await GetAsync(query, cancellationToken).ConfigureAwait(false)).Content;
            if (string.IsNullOrEmpty(jsonResponse?.Trim()))
            {
                yield break;
            }

            yield return jsonResponse;

            query = paginableQuery.Next(jsonResponse);
        }
    }

    public virtual async IAsyncEnumerable<string> GetAsync(Uri endpoint, IPaginableEndpointQuery paginableQuery, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (endpoint != null)
        {
            await foreach (var item in GetAsync(endpoint.AbsoluteUri, paginableQuery, cancellationToken).ConfigureAwait(false))
            {
                yield return item;
            }
        }
    }

    private async Task<HttpResponseMessage?> SendGetRequestAsync(Uri relativeUri, CancellationToken cancellationToken)
    {
        SetHeader();

        try
        {
            return await _httpClient.GetAsync(relativeUri, cancellationToken).ConfigureAwait(false);
        }
        catch (InvalidOperationException ex)
        {
            HttpClientLogs.InvalidRequest(_logger, relativeUri.ToString(), ex);
        }
        catch (HttpRequestException ex)
        {
            HttpClientLogs.NetworkIssue(_logger, relativeUri.ToString(), ex);
            //retry
        }
        catch (TaskCanceledException ex)
        {
            HttpClientLogs.RequestCanceled(_logger, relativeUri.ToString(), ex);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }
    #endregion

    #region POST Methods
    public virtual async Task<OperationResult<string>> PostAsync(string endpoint, HttpContent content, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(endpoint);

        if (!ToValidUri(endpoint, out Uri? relativeUri) ||relativeUri == null)
        {
            return new OperationResult<string>().WithError("Invalid URI");
        }

        HttpResponseMessage? response = await SendPostRequestAsync(relativeUri, content, cancellationToken).ConfigureAwait(false);

        if (response is null)
        {
            return new OperationResult<string>().WithError("Unknown error");
        }

        return await Read(response, cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<OperationResult<string>> PostAsync(Uri endpoint, HttpContent content, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(endpoint);

        HttpResponseMessage? response = await SendPostRequestAsync(endpoint, content, cancellationToken).ConfigureAwait(false);

        if (response is null)
        {
            return new OperationResult<string>().WithError("Unknown error");
        }

        return await Read(response, cancellationToken).ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage?> SendPostRequestAsync(Uri absoluteUri, HttpContent content, CancellationToken cancellationToken)
    {
        SetHeader();

        try
        {
            return await _httpClient.PostAsync(absoluteUri, content, cancellationToken).ConfigureAwait(false);
        }
        catch (InvalidOperationException ex)
        {
            HttpClientLogs.InvalidRequest(_logger, absoluteUri.AbsoluteUri, ex);
        }
        catch (HttpRequestException ex)
        {
            HttpClientLogs.NetworkIssue(_logger, absoluteUri.AbsoluteUri, ex);
            //retry
        }
        catch (TaskCanceledException ex)
        {
            HttpClientLogs.RequestCanceled(_logger, absoluteUri.AbsoluteUri, ex);
        }
        catch (System.Exception)
        {
            throw;
        }

        return null;
    }
    #endregion

    #region HTTP Response
    private async Task<OperationResult<string>> Read(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return new OperationResult<string>()
        {
            IsSuccess = response.IsSuccessStatusCode,
            Content = response.Content != null
                        ? await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)
                        : string.Empty,
            ErrorMessage = response.StatusCode.ToString(),
            ErrorCode = ((int)response.StatusCode).ToString(CultureInfo.CurrentCulture)
        };
    }
    #endregion

    #region HTTP Request header
    protected virtual void SetHeader()
    {
        SetUserAgent(_user_agent);
        SetDate(_timeProvider.DateTimeUTCNow());
    }

    public virtual void SetAuthorization(string scheme, string parameter)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, parameter);
    }

    private void SetUserAgent(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            if (_httpClient.DefaultRequestHeaders.Contains(HeaderNames.UserAgent))
            {
                _httpClient.DefaultRequestHeaders.Remove(HeaderNames.UserAgent);
            }
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, name);
        }
    }

    private void SetDate(DateTime date)
    {
        _httpClient.DefaultRequestHeaders.Date = date;
    }
    #endregion

    private bool ToValidUri(string endpoint, out Uri? relativeUri)
    {
        if (!string.IsNullOrEmpty(endpoint?.Trim()))
        {
            if (Uri.IsWellFormedUriString(endpoint, UriKind.RelativeOrAbsolute))
            {
                return Uri.TryCreate(endpoint, UriKind.RelativeOrAbsolute, out relativeUri);
            }
        }
        relativeUri = null;
        return false;
    }

    #region Dispose
    ~HttpService() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {

            }

            _httpClient?.Dispose();
            _disposedValue = true;
        }
    }
    #endregion
}