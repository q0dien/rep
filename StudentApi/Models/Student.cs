using System.ComponentModel.DataAnnotations;

namespace StudentApi.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Group { get; set; } = string.Empty;

    [Range(1, 6)]
    public int Course { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}