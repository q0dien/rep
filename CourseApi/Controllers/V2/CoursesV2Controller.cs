
using CourseApi.Data;
using CourseApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers.V2;

[ApiController]
[Route("api/v2/courses")]
public class CoursesV2Controller : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CoursesV2Controller(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CourseV2Dto>>> GetCourses()
    {
        var courses = await _context.Courses
            .Include(c => c.TeacherProfile)
            .Select(c => new CourseV2Dto(
                c.Id,
                c.Name,
                c.Teacher,
                c.Credits,
                c.TeacherProfile == null
                    ? null
                    : c.TeacherProfile.FullName,
                c.Enrollments.Count))
            .ToListAsync();

        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<CourseV2Dto>> GetCourse(int id)
    {
        var course = await _context.Courses
            .Where(c => c.Id == id)
            .Select(c => new CourseV2Dto(
                c.Id,
                c.Name,
                c.Teacher,
                c.Credits,
                c.TeacherProfile == null
                    ? null
                    : c.TeacherProfile.FullName,
                c.Enrollments.Count))
            .FirstOrDefaultAsync();

        if (course == null)
            return NotFound();

        return Ok(course);
    }
}
