namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for endpoint queries that support pagination operations and result metadata extraction.
/// </summary>
public interface IPaginableEndpointQuery
{
    /// <summary>
    /// Extracts the total number of items from the response content.
    /// </summary>
    public int Total(string content);
    
    /// <summary>
    /// Extracts the maximum number of items per page from the response content.
    /// </summary>
    public int Limit(string content);
    
    /// <summary>
    /// Extracts the offset position in the result set from the response content.
    /// </summary>
    public int Offset(string content);
    
    /// <summary>
    /// Extracts the URL for the next page of results from the response content.
    /// </summary>
    public string? Next(string content);
    
    /// <summary>
    /// Extracts the URL for the last page of results from the response content.
    /// </summary>
    public string Last(string content);
}

