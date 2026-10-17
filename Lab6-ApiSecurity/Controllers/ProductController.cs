using ApiSecurity.Filters;
using ApiSecurity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiSecurity.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private static readonly List<Product> Products = new()
    {
        new Product { Id = 1, Name = "Ноутбук", Price = 350000m },
        new Product { Id = 2, Name = "Мышь", Price = 8000m },
        new Product { Id = 3, Name = "Клавиатура", Price = 15000m }
    };

    [HttpGet]
[Authorize]
public IActionResult GetProducts()
{
    return Ok(Products);
}

    [HttpGet("apikey")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public IActionResult GetProductsWithApiKey()
    {
        return Ok(Products);
    }
}