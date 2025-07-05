namespace Deepo.Framework.Interfaces;

public interface IEndpointItemQuery : IEndpointQuery
{
    public string Get(string id);
    public string Post(string id, string content);
    public string Put(string id, string content);
    public string Patch(string id, string content);
    public string Delete(string id);
}

