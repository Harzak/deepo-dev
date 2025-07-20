using Deepo.Framework.LogMessages;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Framework.Web.Endpoint;

/// <summary>
/// Provides an abstract base class for endpoint consumers that parse text responses into strongly-typed models.
/// </summary>
public abstract class EndpointConsumerBase<TResultModel>
{
    private readonly ILogger _logger;

    protected EndpointConsumerBase(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Attempts to parse the specified text into the result model type with error handling.
    /// </summary>
    public bool TryParse(string text, out TResultModel? result)
    {
        if (!string.IsNullOrEmpty(text?.Trim()))
        {
            try
            {
                result = Parse(text);
                return true;
            }
            catch (Exception ex) when (ex is FormatException or InvalidOperationException or JsonException)
            {
                HttpClientLogs.UnableToParse(_logger, text, typeof(TResultModel), ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Parses the specified text into the result model type.
    /// </summary>
    protected abstract TResultModel Parse(string text);
}

