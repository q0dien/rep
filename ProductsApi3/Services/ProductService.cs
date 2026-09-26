using ProductsApi3.Models;

namespace ProductsApi3.Services;

public class ProductService : IProductService
{
    private static readonly List<Product> _products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 450000
        },
        new Product
        {
            Id = 2,
            Name = "Mouse",
            Price = 15000
        },
        new Product
        {
            Id = 3,
            Name = "Keyboard",
            Price = 25000
        }
    };

    public IEnumerable<Product> GetAll()
    {
        return _products;
    }

    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public Product Add(Product product)
    {
        product.Id = _products.Count == 0
            ? 1
            : _products.Max(p => p.Id) + 1;

        _products.Add(product);

        return product;
    }

    public bool Delete(int id)
    {
        var product = GetById(id);

        if (product == null)
        {
            return false;
        }

        _products.Remove(product);

        return true;
    }
}