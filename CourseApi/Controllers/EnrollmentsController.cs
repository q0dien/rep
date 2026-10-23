
using CourseApi.Data;
using CourseApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers;

[ApiController]
[Route("api/enrollments")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EnrollmentsController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Студент видит только собственные записи, администратор - все
    [HttpGet]
    public async Task<IActionResult> GetEnrollments()
    {
        if (User.IsInRole("Admin"))
        {
            return Ok(await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Select(e => new
                {
                    e.Id,
                    e.StudentId,
                    StudentName = e.Student!.FullName,
                    e.CourseId,
                    CourseName = e.Course!.Name,
                    e.Grade
                })
                .ToListAsync());
        }

        if (User.IsInRole("Student"))
        {
            var userId = _userManager.GetUserId(User);

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
                return Ok(Array.Empty<object>());

            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == student.Id)
                .Select(e => new
                {
                    e.Id,
                    e.StudentId,
                    StudentName = e.Student!.FullName,
                    e.CourseId,
                    CourseName = e.Course!.Name,
                    e.Grade
                })
                .ToListAsync();

            return Ok(enrollments);
        }

        return Forbid();
    }

    // Студент может записаться только на себя
    [HttpPost]
    [Authorize(Roles = "Student,Admin")]
    public async Task<IActionResult> CreateEnrollment(
        CreateEnrollmentRequest request)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == request.StudentId);

        if (student == null)
            return NotFound(new { message = "Студент не найден." });

        if (User.IsInRole("Student"))
        {
            var userId = _userManager.GetUserId(User);

            if (student.UserId != userId)
                return Forbid();
        }

        var courseExists = await _context.Courses
            .AnyAsync(c => c.Id == request.CourseId);

        if (!courseExists)
            return NotFound(new { message = "Курс не найден." });

        var alreadyEnrolled = await _context.Enrollments
            .AnyAsync(e =>
                e.StudentId == request.StudentId &&
                e.CourseId == request.CourseId);

        if (alreadyEnrolled)
            return Conflict(new
            {
                message = "Студент уже записан на этот курс."
            });

        var enrollment = new Enrollment
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            enrollment.Id,
            enrollment.StudentId,
            enrollment.CourseId,
            enrollment.Grade
        });
    }

    public record CreateEnrollmentRequest(int StudentId, int CourseId);
}
