using Microsoft.AspNetCore.Mvc;
using ProductAPIVS.Container;

namespace SalesOrderAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MasterController : ControllerBase
{
    private readonly IMasterContainer _masterContainer;
    private readonly ILogger<MasterContainer> _logger;

    public MasterController(IMasterContainer masterContainer, ILogger<MasterContainer> logger)
    {
        _masterContainer = masterContainer;
        _logger = logger;
    }

    [HttpGet("GetAllVariant/{type}")]
    public async Task<List<VariantEntity>> GetAllVariant(string type)
    {
        _logger.LogInformation("||GetAllVariant...");
        return await _masterContainer.GetAll(type);
    }
}
