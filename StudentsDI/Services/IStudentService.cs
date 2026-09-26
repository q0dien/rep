using StudentsDI.Models;

namespace StudentsDI.Services;

public interface IStudentService
{
    IEnumerable<Student> GetAll();

    Student? GetById(int id);
}