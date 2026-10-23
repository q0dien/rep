
using CourseApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public record ChangeRoleRequest(string Role);

    // Просмотр пользователей и их ролей
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var result = new List<object>();

        foreach (var user in users)
        {
            result.Add(new
            {
                user.Id,
                user.UserName,
                user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            });
        }

        return Ok(result);
    }

    // Назначение роли пользователю
    [HttpPut("users/{id}/role")]
    public async Task<IActionResult> ChangeRole(
        string id,
        ChangeRoleRequest request)
    {
        var allowedRoles = new[] { "Admin", "Teacher", "Student" };

        if (string.IsNullOrWhiteSpace(request.Role) ||
            !allowedRoles.Contains(
                request.Role,
                StringComparer.OrdinalIgnoreCase))
        {
            return BadRequest(new
            {
                message = "Допустимые роли: Admin, Teacher, Student."
            });
        }

        var role = allowedRoles.First(r =>
            r.Equals(request.Role, StringComparison.OrdinalIgnoreCase));

        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound(new { message = "Пользователь не найден." });
        }

        // Администратор не может случайно лишить себя роли
        var currentUserId = _userManager.GetUserId(User);

        if (user.Id == currentUserId && role != "Admin")
        {
            return BadRequest(new
            {
                message = "Нельзя снять роль Admin с собственной учётной записи."
            });
        }

        if (!await _roleManager.RoleExistsAsync(role))
        {
            return StatusCode(500, new
            {
                message = "Запрошенная роль не создана в системе."
            });
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        var removeResult = await _userManager.RemoveFromRolesAsync(
            user,
            currentRoles);

        if (!removeResult.Succeeded)
        {
            return StatusCode(500, new
            {
                message = "Не удалось обновить роли пользователя."
            });
        }

        var addResult = await _userManager.AddToRoleAsync(user, role);

        if (!addResult.Succeeded)
        {
            return StatusCode(500, new
            {
                message = "Не удалось назначить новую роль."
            });
        }

        return Ok(new
        {
            message = "Роль успешно изменена.",
            userId = user.Id,
            username = user.UserName,
            role
        });
    }
}
