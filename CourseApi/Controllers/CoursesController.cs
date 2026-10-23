
using CourseApi.Data;
using CourseApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers;

[ApiController]
[Route("api/courses")]
[Route("api/v1/courses")]
public class CoursesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CoursesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Список курсов доступен всем
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        var courses = await _context.Courses.ToListAsync();
        return Ok(courses);
    }

    // Информация об одном курсе доступна всем
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Course>> GetCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    // Создавать курсы может только Admin
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Course>> CreateCourse(Course course)
    {
        course.Id = 0;

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCourse),
            new { id = course.Id },
            course);
    }

    // Изменять курсы может только Admin
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCourse(
        int id,
        Course updatedCourse)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        course.Name = updatedCourse.Name;
        course.Teacher = updatedCourse.Teacher;
        course.Credits = updatedCourse.Credits;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Удалять курсы может только Admin
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
