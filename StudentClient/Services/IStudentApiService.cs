using StudentClient.Models;

namespace StudentClient.Services;

public interface IStudentApiService
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Student student);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(Student student);
}