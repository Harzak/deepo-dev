using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Result;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

public interface IGenreAlbumRepository
{
    Task<ReadOnlyCollection<Genre_Album>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Genre_Album genre, CancellationToken cancellationToken = default);
    Task<DatabaseOperationResult> InsertAsync(string genreName, CancellationToken cancellationToken = default);
    Task<(bool Found, Genre_Album Genre)> TryFindGenreAsync(string genreSearched, CancellationToken cancellationToken = default);
}