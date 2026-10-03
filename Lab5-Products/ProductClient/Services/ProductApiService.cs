using System.Net;
using System.Net.Http.Json;
using ProductClient.Models;

namespace ProductClient.Services;

public class ProductApiService : IProductApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        var client = _httpClientFactory.CreateClient("ProductApi");

        try
        {
            var response = await client.GetAsync("api/products");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var products =
                    await response.Content.ReadFromJsonAsync<List<Product>>();

                return products ?? new List<Product>();
            }

            return new List<Product>();
        }
        catch
        {
            return new List<Product>();
        }
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ProductApi");

        try
        {
            var response =
                await client.GetAsync($"api/products/{id}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content
                    .ReadFromJsonAsync<Product>();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(Product product)
    {
        var client = _httpClientFactory.CreateClient("ProductApi");

        try
        {
            var response =
                await client.PostAsJsonAsync(
                    "api/products",
                    product);

            return response.StatusCode == HttpStatusCode.Created;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var client = _httpClientFactory.CreateClient("ProductApi");

        try
        {
            var response =
                await client.PutAsJsonAsync(
                    $"api/products/{product.Id}",
                    product);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ProductApi");

        try
        {
            var response =
                await client.DeleteAsync(
                    $"api/products/{id}");

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}