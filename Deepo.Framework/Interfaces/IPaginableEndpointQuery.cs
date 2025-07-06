namespace Deepo.Framework.Interfaces;

public interface IPaginableEndpointQuery
{
    public int Total(string content);
    public int Limit(string content);
    public int Offset(string content);
    public string? Next(string content);
    public string Last(string content);
}

