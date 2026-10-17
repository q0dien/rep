using System.Security.Claims;
using ApiSecurity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiSecurity.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private static readonly List<Product> Products = new()
    {
        new Product { Id = 1, Name = "оутбук", Price = 350000m, Category = "лектроника" },
        new Product { Id = 2, Name = "ышь", Price = 8000m, Category = "ксессуары" },
        new Product { Id = 3, Name = "лавиатура", Price = 15000m, Category = "ксессуары" }
    };

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetProducts()
    {
        return Ok(Products);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public IActionResult GetProductById(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new { message = "Товар не найден" });
        }

        return Ok(product);
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        product.Id = Products.Count == 0
            ? 1
            : Products.Max(p => p.Id) + 1;

        Products.Add(product);

        var username = HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = product.Id },
            new { product, createdBy = username });
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public IActionResult DeleteProduct(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new { message = "Товар не найден" });
        }

        Products.Remove(product);

        return Ok(new { message = "Товар удалён", id = id });
    }
}
