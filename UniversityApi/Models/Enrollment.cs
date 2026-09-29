using System.ComponentModel.DataAnnotations;

namespace UniversityApi.Models;

public class Enrollment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    [Range(0, 100)]
    public decimal? Grade { get; set; }

    public Student? Student { get; set; }

    public Course? Course { get; set; }
}