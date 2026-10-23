
namespace CourseApi.Models;

public class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Teacher { get; set; } = "";

    public int Credits { get; set; }

    // Преподаватель, ответственный за курс
    public int? TeacherId { get; set; }
    public Teacher? TeacherProfile { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();
}
