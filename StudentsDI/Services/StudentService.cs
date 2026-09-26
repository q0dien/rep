using StudentsDI.Models;

namespace StudentsDI.Services;

public class StudentService : IStudentService
{
    private readonly List<Student> _students = new()
    {
        new Student
        {
            Id = 1,
            Name = "Aruzhan",
            Group = "CS-21"
        },
        new Student
        {
            Id = 2,
            Name = "Dana",
            Group = "CS-22"
        },
        new Student
        {
            Id = 3,
            Name = "Madina",
            Group = "CS-21"
        }
    };

    public IEnumerable<Student> GetAll()
    {
        return _students;
    }

    public Student? GetById(int id)
    {
        return _students.FirstOrDefault(s => s.Id == id);
    }
}