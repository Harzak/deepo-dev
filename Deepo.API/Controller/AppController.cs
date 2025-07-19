using Deepo.API.Constant;
using Microsoft.AspNetCore.Mvc;

namespace Deepo.API.Controller;

/// <summary>
/// API controller for application-level operations such as health checks.
/// </summary>
[Route(ControllerConstants.APP_CONTROLLER_NAME)]
[ApiController]
public class AppController : ControllerBase
{
    private readonly ILogger<AppController> _logger;

    public AppController(ILogger<AppController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Health check endpoint that returns a simple pong response.
    /// </summary>
    [HttpGet]
    [Route("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}

