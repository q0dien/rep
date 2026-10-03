using TaskApi.Models;

namespace TaskApi.Services;

public class TaskService : ITaskService
{
    private readonly List<TaskItem> tasks = new()
    {
        new TaskItem
        {
            Id = 1,
            Title = "Изучить HttpClient",
            Description = "Изучить работу HttpClient в ASP.NET Core",
            IsCompleted = true
        },
        new TaskItem
        {
            Id = 2,
            Title = "Реализовать Web API",
            Description = "Создать Web API для управления задачами",
            IsCompleted = false
        },
        new TaskItem
        {
            Id = 3,
            Title = "Изучить PATCH",
            Description = "Познакомиться с методом PATCH",
            IsCompleted = false
        }
    };

    public IEnumerable<TaskItem> GetAll()
    {
        return tasks;
    }

    public TaskItem? GetById(int id)
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public TaskItem Create(TaskItem task)
    {
        task.Id = tasks.Count == 0
            ? 1
            : tasks.Max(t => t.Id) + 1;

        tasks.Add(task);

        return task;
    }

    public TaskItem? Update(int id, TaskItem task)
    {
        var existingTask = tasks.FirstOrDefault(t => t.Id == id);

        if (existingTask == null)
        {
            return null;
        }

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.IsCompleted = task.IsCompleted;

        return existingTask;
    }

    public bool Delete(int id)
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
        {
            return false;
        }

        tasks.Remove(task);

        return true;
    }
}