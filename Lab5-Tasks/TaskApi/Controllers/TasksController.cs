using Microsoft.AspNetCore.Mvc;
using TaskApi.Models;
using TaskApi.Services;

namespace TaskApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService service;

    public TasksController(ITaskService service)
    {
        this.service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(service.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var task = service.GetById(id);

        if (task == null)
        {
            return NotFound("Задача не найдена.");
        }

        return Ok(task);
    }

    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdTask = service.Create(task);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdTask.Id },
            createdTask);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TaskItem task)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedTask = service.Update(id, task);

        if (updatedTask == null)
        {
            return NotFound("Задача не найдена.");
        }

        return Ok(updatedTask);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = service.Delete(id);

        if (!deleted)
        {
            return NotFound("Задача не найдена.");
        }

        return NoContent();
    }
}