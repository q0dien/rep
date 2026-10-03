using Microsoft.AspNetCore.Mvc;
using StudentClient.Models;
using StudentClient.Services;

namespace StudentClient.Controllers;

public class StudentController : Controller
{
    private readonly IStudentApiService _studentApiService;

    public StudentController(IStudentApiService studentApiService)
    {
        _studentApiService = studentApiService;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _studentApiService.GetAllAsync();

        return View(students);
    }

    public async Task<IActionResult> Details(int id)
    {
        var student = await _studentApiService.GetByIdAsync(id);

        if (student == null)
        {
            ViewBag.ErrorMessage = "Студент с указанным ID не найден.";
            return View();
        }

        return View(student);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        var success = await _studentApiService.CreateAsync(student);

        if (!success)
        {
            ModelState.AddModelError(
                "",
                "Не удалось добавить студента. Проверьте данные и доступность Web API.");

            return View(student);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _studentApiService.DeleteAsync(id);

        if (!success)
        {
            TempData["ErrorMessage"] =
                "Не удалось удалить студента.";

            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var student = await _studentApiService.GetByIdAsync(id);

        if (student == null)
        {
            TempData["ErrorMessage"] =
                "Студент с указанным ID не найден.";

            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Student student)
    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        var success = await _studentApiService.UpdateAsync(student);

        if (!success)
        {
            ModelState.AddModelError(
                "",
                "Не удалось изменить данные студента.");

            return View(student);
        }

        return RedirectToAction(nameof(Index));
    }
}