using TaskManagerClient.Models;

namespace TaskManagerClient.Services;

public interface ITaskApiService
{
    Task<List<TaskItem>> GetAllAsync();

    Task<TaskItem?> GetByIdAsync(int id);

    Task<bool> CreateAsync(TaskItem task);

    Task<bool> UpdateAsync(TaskItem task);

    Task<bool> DeleteAsync(int id);

    string LastErrorMessage { get; }
}