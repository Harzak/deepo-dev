namespace Deepo.DAL.Repository.Interfaces;

/// <summary>
/// Represents an author entity with provider-specific identification information.
/// </summary>
public interface IAuthor
{
    /// <summary>
    /// Gets or sets the author's display name.
    /// </summary>
    string? Name { get; set; }
    
    /// <summary>
    /// Gets the unique identifier for this author within the external provider's system.
    /// </summary>
    string Provider_Identifier { get; }
    
    /// <summary>
    /// Gets the code identifying the external data provider for this author.
    /// </summary>
    string Provider_Code { get; }
}
