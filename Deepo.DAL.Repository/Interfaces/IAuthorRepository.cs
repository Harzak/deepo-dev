using Deepo.DAL.Repository.Result;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Interfaces;

/// <summary>
/// Repository interface for managing author data operations including insertion, existence checks, and retrieval.
/// </summary>
public interface IAuthorRepository
{
    /// <summary>
    /// Inserts a new author into the database with provider-specific information.
    /// </summary>
    Task<DatabaseOperationResult> Insert(IAuthor item, CancellationToken cancellationToken);
    
    /// <summary>
    /// Checks if an author already exists in the database based on provider code and identifier.
    /// </summary>
    Task<bool> ExistsAsync(IAuthor item, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves an author by their provider-specific identifier.
    /// </summary>
    Task<Models.Author?> GetByProviderIdentifierAsync(string identifier, CancellationToken cancellationToken = default);
}
