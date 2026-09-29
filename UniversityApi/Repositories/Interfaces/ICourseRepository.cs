using UniversityApi.Models;

namespace UniversityApi.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync(string? search = null);
    Task<Course?> GetByIdAsync(int id);
    Task<Course> AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(Course course);
}