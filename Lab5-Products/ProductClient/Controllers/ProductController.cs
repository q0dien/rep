using Microsoft.AspNetCore.Mvc;
using ProductClient.Models;
using ProductClient.Services;

namespace ProductClient.Controllers;

public class ProductController : Controller
{
    private readonly IProductApiService _productApiService;

    public ProductController(IProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productApiService.GetAllAsync();

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productApiService.GetByIdAsync(id);

        if (product == null)
        {
            ViewBag.ErrorMessage = "Товар не найден. Сервер вернул 404.";
            return View();
        }

        return View(product);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMessage =
                "Некорректные данные. Сервер вернул 400 Bad Request.";

            return View(product);
        }

        var success = await _productApiService.CreateAsync(product);

        if (!success)
        {
            ModelState.AddModelError(
                "",
                "Не удалось добавить товар. Возможна ошибка 400 или 500.");

            return View(product);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productApiService.GetByIdAsync(id);

        if (product == null)
        {
            TempData["ErrorMessage"] =
                "Товар не найден. Сервер вернул 404.";

            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMessage =
                "Некорректные данные. Сервер вернул 400 Bad Request.";

            return View(product);
        }

        var success = await _productApiService.UpdateAsync(product);

        if (!success)
        {
            ModelState.AddModelError(
                "",
                "Не удалось изменить товар. Возможна ошибка 400, 404 или 500.");

            return View(product);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _productApiService.DeleteAsync(id);

        if (!success)
        {
            TempData["ErrorMessage"] =
                "Не удалось удалить товар. Возможна ошибка 404 или 500.";

            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }
}