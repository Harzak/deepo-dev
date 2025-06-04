using Deepo.DAL.Service.Interfaces;
using Deepo.Mediator.Query;
using Framework.Common.Utils.Result;
using MediatR;

namespace Deepo.Mediator.Handler;

internal sealed class CountAllVinylReleasesHandler : IRequestHandler<CountAllVinylReleasesQuery, OperationResult<int>>
{
    private readonly IReleaseAlbumDBService _db;

    public CountAllVinylReleasesHandler(IReleaseAlbumDBService db)
    {
        _db = db;
    }

    public async Task<OperationResult<int>> Handle(CountAllVinylReleasesQuery request, CancellationToken cancellationToken)
    {
        OperationResult<int> result = new()
        {
            Content =  _db.Count(request.Market)
        };

        return await Task.FromResult(result.WithSuccess()).ConfigureAwait(false);
    }
}

