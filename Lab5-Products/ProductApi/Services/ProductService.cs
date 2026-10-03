using ProductApi.Models;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly List<Product> products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "Ноутбук для учебы и работы",
            Price = 350000,
            Quantity = 10
        },
        new Product
        {
            Id = 2,
            Name = "Mouse",
            Description = "Беспроводная компьютерная мышь",
            Price = 12000,
            Quantity = 20
        },
        new Product
        {
            Id = 3,
            Name = "Keyboard",
            Description = "Механическая клавиатура",
            Price = 25000,
            Quantity = 15
        }
    };

    public IEnumerable<Product> GetAll()
    {
        return products;
    }

    public Product? GetById(int id)
    {
        return products.FirstOrDefault(p => p.Id == id);
    }

    public Product Create(Product product)
    {
        product.Id = products.Count == 0
            ? 1
            : products.Max(p => p.Id) + 1;

        products.Add(product);

        return product;
    }

    public Product? Update(int id, Product product)
    {
        var existingProduct = products.FirstOrDefault(p => p.Id == id);

        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Quantity = product.Quantity;

        return existingProduct;
    }

    public bool Delete(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        products.Remove(product);

        return true;
    }
}