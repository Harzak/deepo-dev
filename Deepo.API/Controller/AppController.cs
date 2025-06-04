using Deepo.API.Constant;
using Microsoft.AspNetCore.Mvc;

namespace Deepo.API.Controller;

[Route(ControllerConstants.APP_CONTROLLER_NAME)]
[ApiController]
public class AppController : ControllerBase
{
    private readonly ILogger<AppController> _logger;

    public AppController(ILogger<AppController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}

