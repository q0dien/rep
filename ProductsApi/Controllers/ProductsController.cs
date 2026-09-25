using Microsoft.AspNetCore.Mvc;
using ProductsApi.Services;

namespace ProductsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService service;
    private readonly ILogger<ProductsController> logger;

    public ProductsController(IProductService service, ILogger<ProductsController> logger)
    {
        this.service = service;
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        logger.LogTrace("Trace message");
        logger.LogDebug("Debug message");
        logger.LogInformation("Information message");
        logger.LogWarning("Warning message");
        logger.LogError("Error message");
        logger.LogCritical("Critical message");

        return Ok(service.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = service.GetById(id);

        if (product == null)
        {
            logger.LogWarning("Product with ID {ProductId} was not found", id);
            return NotFound();
        }

        logger.LogInformation("Product with ID {ProductId} was found", id);

        return Ok(product);
    }
}