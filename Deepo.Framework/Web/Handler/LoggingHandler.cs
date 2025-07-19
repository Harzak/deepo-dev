using Deepo.Framework.LogMessages;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Framework.Web.Handler;

/// <summary>
/// Provides HTTP message handling with comprehensive logging of request and response information.
/// </summary>
public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger) : base()
    {
        _logger = logger;
    }

    public LoggingHandler(ILogger<LoggingHandler> logger, HttpMessageHandler innerHandler) : base(innerHandler)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        HttpClientLogs.HttpRequestLogHandler(_logger,
            DateTime.UtcNow,
            request.RequestUri?.AbsoluteUri ?? string.Empty,
            request.Method?.Method ?? string.Empty,
            response.StatusCode,
            request.Headers?.UserAgent?.ToString() ?? string.Empty,
            request.Headers?.GetValues(HttpClientConstants.HEADER_NAME_TASKID).FirstOrDefault() ?? string.Empty,
            request.Headers?.Date,
            request.Headers?.Authorization?.Parameter ?? string.Empty);

        return response;
    }
}


