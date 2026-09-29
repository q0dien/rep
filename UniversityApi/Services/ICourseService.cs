using UniversityApi.Common;
using UniversityApi.DTO.Courses;

namespace UniversityApi.Services;

public interface ICourseService
{
    Task<ReturnResult<List<CourseDto>>> GetAllAsync(string? search = null);
    Task<ReturnResult<CourseDto>> GetByIdAsync(int id);
    Task<ReturnResult<CourseDto>> CreateAsync(CourseCreateDto dto);
    Task<ReturnResult<CourseDto>> UpdateAsync(
        int id,
        CourseUpdateDto dto);
    Task<ReturnResult<bool>> DeleteAsync(int id);
}