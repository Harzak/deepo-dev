using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Result;
using System.Collections.ObjectModel;

namespace Deepo.DAL.Repository.Interfaces;

public interface IGenreAlbumRepository
{
    ReadOnlyCollection<Genre_Album> GetAll();
    Task<DatabaseOperationResult> Insert(string genreName, CancellationToken cancellationToken);
    bool TryFindGenre(string genreSearched, out Genre_Album genreFind);
}