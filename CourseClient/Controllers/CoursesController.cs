using System.Net.Http.Json;
using CourseClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseClient.Controllers;

public class CoursesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CoursesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CourseApi");
            var courses = await client.GetFromJsonAsync<List<CourseViewModel>>(
                "api/v1/courses");

            return View(courses ?? new List<CourseViewModel>());
        }
        catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is TaskCanceledException)
        {
            return View("ApiUnavailable");
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CourseApi");
            var course = await client.GetFromJsonAsync<CourseViewModel>(
                $"api/v1/courses/{id}");

            if (course == null)
                return NotFound();

            return View(course);
        }
        catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is TaskCanceledException)
        {
            return View("ApiUnavailable");
        }
    }
}
