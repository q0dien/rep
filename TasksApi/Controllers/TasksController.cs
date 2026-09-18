using Microsoft.AspNetCore.Mvc;
using TasksApi.Models;

namespace TasksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private static List<TaskItem> tasks = new()
    {
        new TaskItem
        {
            Id = 1,
            Title = "Сделать лабораторную",
            Description = "Выполнить практическую работу по ASP.NET Core",
            IsCompleted = false
        },
        new TaskItem
        {
            Id = 2,
            Title = "Подготовить отчет",
            Description = "Оформить отчет по работе",
            IsCompleted = false
        },
        new TaskItem
        {
            Id = 3,
            Title = "Сдать работу",
            Description = "Загрузить готовую работу",
            IsCompleted = false
        }
    };

    [HttpGet]
    public ActionResult<List<TaskItem>> GetTasks()
    {
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public ActionResult<TaskItem> GetTask(int id)
    {
        var task = tasks.FirstOrDefault(x => x.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost]
    public ActionResult<TaskItem> AddTask(TaskItem task)
    {
        tasks.Add(task);

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public ActionResult<TaskItem> UpdateTask(int id, TaskItem updatedTask)
    {
        var task = tasks.FirstOrDefault(x => x.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.IsCompleted = updatedTask.IsCompleted;

        return Ok(task);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTask(int id)
    {
        var task = tasks.FirstOrDefault(x => x.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        tasks.Remove(task);

        return Ok();
    }
}