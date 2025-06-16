using Microsoft.AspNetCore.Mvc;
using SCPArchiveApi.Repositories;

namespace SCPArchiveApi.Controllers;

[ApiController]
[Route("api")]
public class TestController : ControllerBase
{
    private readonly IScpRepository _repository;
    private readonly ILogger<TestController> _logger;

    public TestController(IScpRepository repository, ILogger<TestController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Test la connexion à la base de données MongoDB
    /// </summary>
    /// <returns>200 OK si la connexion est établie, 503 Service Unavailable sinon</returns>
    [HttpGet("test-connection")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            await _repository.TestConnectionAsync();
            return Ok(new { status = "Connected", message = "Successfully connected to MongoDB" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to MongoDB");
            return StatusCode(503, new { status = "Error", message = "Failed to connect to MongoDB" });
        }
    }
}
