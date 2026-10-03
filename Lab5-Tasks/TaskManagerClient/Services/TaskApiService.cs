using System.Net;
using System.Net.Http.Json;
using TaskManagerClient.Models;

namespace TaskManagerClient.Services;

public class TaskApiService : ITaskApiService
{
    private readonly HttpClient httpClient;

    public string LastErrorMessage { get; private set; } = string.Empty;

    public TaskApiService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient("TaskApi");
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        LastErrorMessage = string.Empty;

        try
        {
            var response = await httpClient.GetAsync("api/tasks");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<List<TaskItem>>()
                       ?? new List<TaskItem>();
            }

            SetErrorMessage(response.StatusCode);
        }
        catch
        {
            LastErrorMessage = "Не удалось подключиться к Web API.";
        }

        return new List<TaskItem>();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        LastErrorMessage = string.Empty;

        try
        {
            var response = await httpClient.GetAsync($"api/tasks/{id}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<TaskItem>();
            }

            SetErrorMessage(response.StatusCode);
        }
        catch
        {
            LastErrorMessage = "Не удалось подключиться к Web API.";
        }

        return null;
    }

    public async Task<bool> CreateAsync(TaskItem task)
    {
        LastErrorMessage = string.Empty;

        try
        {
            var response = await httpClient.PostAsJsonAsync("api/tasks", task);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }

            SetErrorMessage(response.StatusCode);
        }
        catch
        {
            LastErrorMessage = "Не удалось подключиться к Web API.";
        }

        return false;
    }

    public async Task<bool> UpdateAsync(TaskItem task)
    {
        LastErrorMessage = string.Empty;

        try
        {
            var response = await httpClient.PutAsJsonAsync(
                $"api/tasks/{task.Id}",
                task);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            SetErrorMessage(response.StatusCode);
        }
        catch
        {
            LastErrorMessage = "Не удалось подключиться к Web API.";
        }

        return false;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        LastErrorMessage = string.Empty;

        try
        {
            var response = await httpClient.DeleteAsync($"api/tasks/{id}");

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            SetErrorMessage(response.StatusCode);
        }
        catch
        {
            LastErrorMessage = "Не удалось подключиться к Web API.";
        }

        return false;
    }

    private void SetErrorMessage(HttpStatusCode statusCode)
    {
        LastErrorMessage = statusCode switch
        {
            HttpStatusCode.BadRequest =>
                "Ошибка запроса (400). Проверьте введённые данные.",

            HttpStatusCode.NotFound =>
                "Задача не найдена (404).",

            HttpStatusCode.InternalServerError =>
                "Ошибка сервера Web API (500).",

            _ =>
                $"Web API вернул ошибку: {(int)statusCode}."
        };
    }
}