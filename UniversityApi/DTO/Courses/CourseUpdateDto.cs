using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Courses;

public class CourseUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 10)]
    public int Credits { get; set; }

    [Required]
    public int TeacherId { get; set; }
}