using Deepo.API.Constant;
using Deepo.Framework.Results;
using Deepo.Mediator.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deepo.API.Controller;

/// <summary>
/// API controller for managing vinyl release operations.
/// </summary>
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

    /// <summary>
    /// Gets the total count of vinyl releases for a specified market.
    /// </summary>
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

    /// <summary>
    /// Gets vinyl releases with pagination for a specified market.
    /// </summary>
    /// <remarks>
    /// Example: /vinyl?market=FR&offset=100&limit=50
    /// </remarks>
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

    /// <summary>
    /// Gets a specific vinyl release by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Example: /vinyl/8b8d2254-ec00-49b9-9d45-7241fb7b937d
    /// </remarks>
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

    /// <summary>
    /// Gets all vinyl genres, optionally filtered by a search term.
    /// </summary>
    /// <remarks>
    /// Example: /vinyl/genres
    /// </remarks>
    [HttpGet]
    [Route("genres")]
    public async Task<IActionResult> GetGenres(CancellationToken cancellationToken, string? filter = null)
    {
        var query = new GetAllVinylGenresQuery(filter);
        OperationResultList<Dto.GenreDto> result = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
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

