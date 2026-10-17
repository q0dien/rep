
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private static readonly List<TaskItem> Tasks = new()
    {
        new TaskItem
        {
            Id = 1,
            Title = "Изучить ASP.NET Core",
            Description = "Разобраться с созданием Web API",
            IsCompleted = false
        },
        new TaskItem
        {
            Id = 2,
            Title = "Настроить JWT",
            Description = "Добавить аутентификацию",
            IsCompleted = true
        },
        new TaskItem
        {
            Id = 3,
            Title = "Протестировать API",
            Description = "Проверить защищённые методы",
            IsCompleted = false
        }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Tasks);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        var username = HttpContext.User.Claims
            .FirstOrDefault(c =>
                c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;

        task.Id = Tasks.Count == 0 ? 1 : Tasks.Max(t => t.Id) + 1;
        Tasks.Add(task);

        return Ok(new
        {
            message = "Задача успешно создана",
            username,
            task
        });
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound();

        Tasks.Remove(task);

        return Ok(new
        {
            message = "Задача успешно удалена",
            taskId = id
        });
    }
}
