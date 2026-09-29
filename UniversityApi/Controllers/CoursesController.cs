using Microsoft.AspNetCore.Mvc;
using UniversityApi.DTO.Courses;
using UniversityApi.Services;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null)
    {
        var response = await _service.GetAllAsync(search);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetByIdAsync(id);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CourseCreateDto dto)
    {
        var response = await _service.CreateAsync(dto);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        CourseUpdateDto dto)
    {
        var response = await _service.UpdateAsync(id, dto);

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);

        return StatusCode(response.StatusCode, response);
    }
}