using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Enrollments;

public class EnrollmentCreateDto
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }
}