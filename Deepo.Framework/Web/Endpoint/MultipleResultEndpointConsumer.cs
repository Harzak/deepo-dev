using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Framework.Web.Endpoint;

public abstract class MultipleResultEndpointConsumer<TModel> : EndpointConsumerBase<TModel>, IEndpointListQuery
{
    protected MultipleResultEndpointConsumer(ILogger logger) : base(logger) { }

    public abstract string Get(string query = "");

    public abstract string Options();

    public abstract string Trace();
}
