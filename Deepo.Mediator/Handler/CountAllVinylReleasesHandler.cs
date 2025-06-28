using Deepo.DAL.Repository.Interfaces;
using Deepo.Mediator.Query;
using Framework.Common.Utils.Result;
using MediatR;

namespace Deepo.Mediator.Handler;

internal sealed class CountAllVinylReleasesHandler : IRequestHandler<CountAllVinylReleasesQuery, OperationResult<int>>
{
    private readonly IReleaseAlbumRepository _repository;

    public CountAllVinylReleasesHandler(IReleaseAlbumRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<int>> Handle(CountAllVinylReleasesQuery request, CancellationToken cancellationToken)
    {
        OperationResult<int> result = new()
        {
            Content =  _repository.Count(request.Market)
        };

        return await Task.FromResult(result.WithSuccess()).ConfigureAwait(false);
    }
}

