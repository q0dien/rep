
using CourseApi.Data;
using CourseApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers;

[ApiController]
[Route("api/profiles")]
[Authorize(Roles = "Admin")]
public class ProfilesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ProfilesController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public record ProfileRequest(string UserId, string FullName);

    [HttpGet("students")]
    public async Task<IActionResult> GetStudents()
    {
        return Ok(await _context.Students
            .Select(s => new { s.Id, s.UserId, s.FullName })
            .ToListAsync());
    }

    [HttpGet("teachers")]
    public async Task<IActionResult> GetTeachers()
    {
        return Ok(await _context.Teachers
            .Select(t => new { t.Id, t.UserId, t.FullName })
            .ToListAsync());
    }

    [HttpPost("students")]
    public async Task<IActionResult> CreateStudent(ProfileRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            return BadRequest(new { message = "Укажите имя студента." });

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
            return NotFound(new { message = "Учётная запись не найдена." });

        if (!await _userManager.IsInRoleAsync(user, "Student"))
            return BadRequest(new { message = "У пользователя должна быть роль Student." });

        if (await _context.Students.AnyAsync(s => s.UserId == request.UserId))
            return Conflict(new { message = "Профиль студента уже существует." });

        var student = new Student
        {
            UserId = request.UserId,
            FullName = request.FullName.Trim()
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return Created($"/api/profiles/students/{student.Id}",
            new { student.Id, student.UserId, student.FullName });
    }

    [HttpPost("teachers")]
    public async Task<IActionResult> CreateTeacher(ProfileRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            return BadRequest(new { message = "Укажите имя преподавателя." });

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
            return NotFound(new { message = "Учётная запись не найдена." });

        if (!await _userManager.IsInRoleAsync(user, "Teacher"))
            return BadRequest(new { message = "У пользователя должна быть роль Teacher." });

        if (await _context.Teachers.AnyAsync(t => t.UserId == request.UserId))
            return Conflict(new { message = "Профиль преподавателя уже существует." });

        var teacher = new Teacher
        {
            UserId = request.UserId,
            FullName = request.FullName.Trim()
        };

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return Created($"/api/profiles/teachers/{teacher.Id}",
            new { teacher.Id, teacher.UserId, teacher.FullName });
    }
}
