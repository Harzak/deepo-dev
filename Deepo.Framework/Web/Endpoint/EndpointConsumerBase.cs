using Deepo.Framework.LogMessages;
using Microsoft.Extensions.Logging;

namespace Deepo.Framework.Web.Endpoint;

public abstract class EndpointConsumerBase<TResultModel>
{
    private readonly ILogger _logger;

    protected EndpointConsumerBase(ILogger logger)
    {
        _logger = logger;
    }

    public bool TryParse(string text, out TResultModel result)
    {
        if (!string.IsNullOrEmpty(text?.Trim()))
        {
            try
            {
                result = Parse(text);
                return true;
            }
            catch (FormatException ex)
            {
                HttpClientLogs.UnableToParse(_logger, text, typeof(TResultModel), ex);
            }
            catch (InvalidOperationException ex)
            {
                HttpClientLogs.UnableToParse(_logger, text, typeof(TResultModel), ex);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        result = default!;
        return false;
    }

    protected abstract TResultModel Parse(string text);
}

