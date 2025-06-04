using Deepo.API.Constant;
using Deepo.Mediator.Query;
using Framework.Common.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deepo.API.Controller;

[Route(ControllerConstants.VINYL_CONTROLLER_NAME)]
[ApiController]
public class VinylController : ControllerBase
{
    private readonly ILogger<VinylController> _logger;
    private readonly IMediator _mediator;

    public VinylController(IMediator mediator, ILogger<VinylController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> Get(string market, CancellationToken cancellationToken)
    {
        CountAllVinylReleasesQuery query = new(market);
        OperationResult<int> result = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);

        if (result is null || result.IsFailed)
        {
            return BadRequest(result);
        }
        else if (!result.HasContent)
        {
            return NoContent();
        }
        else
        {
            return Ok(result);
        }
    }

    //vinyl?market=FR&offset=100&limit=50
    [HttpGet]
    public async Task<IActionResult> Get(string market, int offset, int limit, CancellationToken cancellationToken)
    {
        GetAllVinylReleasesWithPaginationQuery query = new(market)
        {
            Offset = offset,
            Limit = limit
        };
        OperationResultList<Dto.ReleaseVinylDto> result = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);

        if (result is null || result.IsFailed)
        {
            return BadRequest(result);
        }
        else if (!result.HasContent)
        {
            return NoContent();
        }
        else
        {
            return Ok(result);
        }
    }

    //vinyl/8b8d2254-ec00-49b9-9d45-7241fb7b937d
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetVinylReleaseQuery(id);
        OperationResult<Dto.ReleaseVinylExDto> result = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);

        if (result is null || result.IsFailed)
        {
            return BadRequest(result);
        }
        else if (!result.HasContent)
        {
            return NoContent();
        }
        else
        {
            return Ok(result);
        }
    }
}

