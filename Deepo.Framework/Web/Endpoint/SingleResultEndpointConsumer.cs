using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Framework.Web.Endpoint;

public abstract class SingleResultEndpointConsumer<TModel> : EndpointConsumerBase<TModel>, IEndpointItemQuery
{
    protected SingleResultEndpointConsumer(ILogger logger) : base(logger) { }

    public abstract string Get(string id);

    public abstract string Post(string id, string content);

    public abstract string Put(string id, string content);

    public abstract string Patch(string id, string content);

    public abstract string Delete(string id);

    public abstract string Options();

    public abstract string Trace();
}

