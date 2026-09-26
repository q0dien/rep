using Microsoft.AspNetCore.Mvc;
using ProductsApi3.Models;
using ProductsApi3.Services;

namespace ProductsApi3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        _logger.LogInformation("Getting all products");

        var products = _productService.GetAll();

        return Ok(products);
    }

    [HttpGet("{id}")]
public IActionResult GetById(int id)
{
    try
    {
        var product = _productService.GetById(id);

        if (product == null)
        {
            _logger.LogWarning(
                "Product with ID {ProductId} was not found",
                id);

            return NotFound();
        }

        _logger.LogInformation(
            "Product with ID {ProductId} was found",
            id);

        return Ok(product);
    }
    catch (Exception ex)
    {
        _logger.LogError(
            ex,
            "Error while processing product with ID {ProductId}",
            id);

        return StatusCode(500, "An error occurred while processing the request.");
    }
}

    [HttpPost]
    public IActionResult Add(Product product)
    {
        if (product == null)
        {
            return BadRequest();
        }

        var createdProduct = _productService.Add(product);

        _logger.LogInformation(
            "Product {ProductName} was created",
            product.Name);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.Id },
            createdProduct);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _productService.Delete(id);

        if (!deleted)
        {
            _logger.LogWarning(
                "Product with ID {ProductId} was not found",
                id);

            return NotFound();
        }

        _logger.LogInformation(
            "Product with ID {ProductId} was deleted",
            id);

        return Ok();
    }
}