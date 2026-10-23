
namespace CourseApi.Models;

public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    // Оценка может отсутствовать, пока преподаватель её не выставил
    public decimal? Grade { get; set; }
}
