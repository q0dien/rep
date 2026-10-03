using Microsoft.AspNetCore.Mvc;
using TaskManagerClient.Models;
using TaskManagerClient.Services;

namespace TaskManagerClient.Controllers;

public class TaskController : Controller
{
    private readonly ITaskApiService taskApiService;

    public TaskController(ITaskApiService taskApiService)
    {
        this.taskApiService = taskApiService;
    }

    public async Task<IActionResult> Index()
    {
        var tasks = await taskApiService.GetAllAsync();

        if (!string.IsNullOrEmpty(taskApiService.LastErrorMessage))
        {
            ViewBag.ErrorMessage = taskApiService.LastErrorMessage;
        }

        return View(tasks);
    }

    public async Task<IActionResult> Details(int id)
    {
        var task = await taskApiService.GetByIdAsync(id);

        if (task == null)
        {
            ViewBag.ErrorMessage = taskApiService.LastErrorMessage;
            return View();
        }

        return View(task);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskItem task)
    {
        if (!ModelState.IsValid)
        {
            return View(task);
        }

        var success = await taskApiService.CreateAsync(task);

        if (!success)
        {
            ViewBag.ErrorMessage = taskApiService.LastErrorMessage;
            return View(task);
        }

        TempData["SuccessMessage"] = "Задача успешно добавлена.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var task = await taskApiService.GetByIdAsync(id);

        if (task == null)
        {
            TempData["ErrorMessage"] = taskApiService.LastErrorMessage;
            return RedirectToAction(nameof(Index));
        }

        return View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TaskItem task)
    {
        if (!ModelState.IsValid)
        {
            return View(task);
        }

        var success = await taskApiService.UpdateAsync(task);

        if (!success)
        {
            ViewBag.ErrorMessage = taskApiService.LastErrorMessage;
            return View(task);
        }

        TempData["SuccessMessage"] = "Задача успешно изменена.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await taskApiService.DeleteAsync(id);

        if (!success)
        {
            TempData["ErrorMessage"] = taskApiService.LastErrorMessage;
        }
        else
        {
            TempData["SuccessMessage"] = "Задача успешно удалена.";
        }

        return RedirectToAction(nameof(Index));
    }
}