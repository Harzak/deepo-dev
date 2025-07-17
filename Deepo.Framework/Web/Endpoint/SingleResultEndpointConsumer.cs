using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Framework.Web.Endpoint;

public abstract class SingleResultEndpointConsumer<TModel> : EndpointConsumerBase<TModel>, IEndpointItemQuery
{
    protected SingleResultEndpointConsumer(ILogger logger) : base(logger) { }

    public abstract string Get(string id);

    public virtual string Post(string id, string content)
    {
        throw new NotSupportedException("POST method is not supported for this endpoint.");
    }

    public  virtual string Put(string id, string content)
    {
        throw new NotSupportedException("PUT method is not supported for this endpoint.");
    }

    public virtual string Patch(string id, string content)
    {
        throw new NotSupportedException("PATCH method is not supported for this endpoint.");
    }

    public virtual string Delete(string id)
    {
        throw new NotSupportedException("DELETE method is not supported for this endpoint.");
    }

    public virtual string Options()
    {
        throw new NotSupportedException("OPTIONS method is not supported for this endpoint.");
    }

    public virtual string Trace()
    {
        throw new NotSupportedException("TRACE method is not supported for this endpoint.");
    }
}

