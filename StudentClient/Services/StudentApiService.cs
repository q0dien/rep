using System.Net;
using System.Net.Http.Json;
using StudentClient.Models;

namespace StudentClient.Services;

public class StudentApiService : IStudentApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public StudentApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        var client = _httpClientFactory.CreateClient("StudentApi");

        try
        {
            var response = await client.GetAsync("api/students");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var students = await response.Content.ReadFromJsonAsync<List<Student>>();
                return students ?? new List<Student>();
            }

            return new List<Student>();
        }
        catch
        {
            return new List<Student>();
        }
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("StudentApi");

        try
        {
            var response = await client.GetAsync($"api/students/{id}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<Student>();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(Student student)
    {
        var client = _httpClientFactory.CreateClient("StudentApi");

        try
        {
            var response = await client.PostAsJsonAsync("api/students", student);

            return response.StatusCode == HttpStatusCode.Created;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("StudentApi");

        try
        {
            var response = await client.DeleteAsync($"api/students/{id}");

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        var client = _httpClientFactory.CreateClient("StudentApi");

        try
        {
            var response = await client.PutAsJsonAsync(
                $"api/students/{student.Id}",
                student);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}