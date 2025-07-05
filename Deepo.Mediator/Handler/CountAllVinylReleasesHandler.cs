using Deepo.DAL.Repository.Interfaces;
using Deepo.Framework.Results;
using Deepo.Mediator.Query;
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
            Content = await _repository.CountAsync(request.Market, cancellationToken).ConfigureAwait(false)
        };

        return await Task.FromResult(result.WithSuccess()).ConfigureAwait(false);
    }
}

