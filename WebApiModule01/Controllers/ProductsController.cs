using Microsoft.AspNetCore.Mvc;
using WebApiModule01.Models;

namespace WebApiModule01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static List<Product> p = new List<Product>
    {
        new Product
        {
            Id = 1,
            Name = "Ноутбук Lenovo",
            Category = "Ноутбуки",
            Price = 450000,
            Stock = 10
        },
        new Product
        {
            Id = 2,
            Name = "Мышь Logitech",
            Category = "Аксессуары",
            Price = 15000,
            Stock = 25
        }
    };

    [HttpGet]
    public ActionResult<List<Product>> Get()
    {
        return Ok(p);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> Get(int id)
    {
        Product? x = p.FirstOrDefault(a => a.Id == id);

        if (x == null)
        {
            return NotFound();
        }

        return Ok(x);
    }

    [HttpPost]
    public ActionResult<Product> Post(Product x)
    {
        x.Id = p.Count + 1;
        p.Add(x);

        return CreatedAtAction(nameof(Get), new { id = x.Id }, x);
    }
}