namespace Deepo.Framework.Interfaces;

public interface IEndpointListQuery : IEndpointQuery
{
    public string Get(string query = "");
}
