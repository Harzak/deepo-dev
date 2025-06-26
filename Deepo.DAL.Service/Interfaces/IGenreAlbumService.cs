using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Result;
using System.Collections.ObjectModel;

namespace Deepo.DAL.Service.Interfaces;

public interface IGenreAlbumService
{
    ReadOnlyCollection<Genre_Album> GetAll();
    Task<DatabaseOperationResult> Insert(string genreName, CancellationToken cancellationToken);
    bool TryFindGenre(string genreSearched, out Genre_Album genreFind);
}