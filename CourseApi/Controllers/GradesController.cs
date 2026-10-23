
using CourseApi.Data;
using CourseApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers;

[ApiController]
[Route("api/grades")]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public GradesController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Студент получает только свои оценки
    [HttpGet]
    public async Task<IActionResult> GetGrades()
    {
        if (User.IsInRole("Admin"))
        {
            return Ok(await GetGradesQuery().ToListAsync());
        }

        if (User.IsInRole("Student"))
        {
            var userId = _userManager.GetUserId(User);

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
                return Ok(Array.Empty<object>());

            return Ok(await GetGradesQuery()
                .Where(e => e.StudentId == student.Id)
                .ToListAsync());
        }

        if (User.IsInRole("Teacher"))
        {
            var userId = _userManager.GetUserId(User);

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (teacher == null)
                return Ok(Array.Empty<object>());

            return Ok(await GetGradesQuery()
                .Where(e => e.Course!.TeacherId == teacher.Id)
                .ToListAsync());
        }

        return Forbid();
    }

    // Преподаватель выставляет оценку только в своём курсе
    [HttpPut("{enrollmentId:int}")]
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> SetGrade(
        int enrollmentId,
        SetGradeRequest request)
    {
        if (request.Grade < 0 || request.Grade > 100)
        {
            return BadRequest(new
            {
                message = "Оценка должна быть от 0 до 100."
            });
        }

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        if (enrollment == null)
            return NotFound(new { message = "Запись на курс не найдена." });

        if (User.IsInRole("Teacher"))
        {
            var userId = _userManager.GetUserId(User);

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (teacher == null ||
                enrollment.Course!.TeacherId != teacher.Id)
            {
                return Forbid();
            }
        }

        enrollment.Grade = request.Grade;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            enrollment.Id,
            enrollment.StudentId,
            enrollment.CourseId,
            enrollment.Grade
        });
    }

    private IQueryable<Enrollment> GetGradesQuery()
    {
        return _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .Select(e => new Enrollment
            {
                Id = e.Id,
                StudentId = e.StudentId,
                Student = e.Student,
                CourseId = e.CourseId,
                Course = e.Course,
                Grade = e.Grade
            });
    }

    public record SetGradeRequest(decimal Grade);
}
