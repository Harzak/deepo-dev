using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Framework.Web.Endpoint;

public abstract class MultipleResultEndpointConsumer<TModel> : EndpointConsumerBase<TModel>, IEndpointListQuery
{
    protected MultipleResultEndpointConsumer(ILogger logger) : base(logger) { }

    public abstract string Get(string query = "");

    public virtual string Options()
    {
        throw new NotSupportedException("OPTIONS method is not supported for this endpoint.");
    }

    public virtual string Trace()
    {
        throw new NotSupportedException("TRACE method is not supported for this endpoint.");
    }
}
