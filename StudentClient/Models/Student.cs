using System.ComponentModel.DataAnnotations;

namespace StudentClient.Models;

public class Student
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите ФИО")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите группу")]
    public string Group { get; set; } = string.Empty;

    [Range(1, 6, ErrorMessage = "Курс должен быть от 1 до 6")]
    public int Course { get; set; }

    [Required(ErrorMessage = "Введите Email")]
    [EmailAddress(ErrorMessage = "Введите корректный Email")]
    public string Email { get; set; } = string.Empty;
}