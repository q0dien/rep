
using Microsoft.AspNetCore.Mvc;
using UniversityApi.DTO.Students;
using UniversityApi.Services;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAllAsync();

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetByIdAsync(id);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}/courses")]
    public async Task<IActionResult> GetCourses(int id)
    {
        var response = await _service.GetCoursesAsync(id);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        StudentCreateDto dto)
    {
        var response = await _service.CreateAsync(dto);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        StudentUpdateDto dto)
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

