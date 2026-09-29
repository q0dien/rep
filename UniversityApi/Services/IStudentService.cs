
using UniversityApi.Common;
using UniversityApi.DTO.Courses;
using UniversityApi.DTO.Students;

namespace UniversityApi.Services;

public interface IStudentService
{
    Task<ReturnResult<List<StudentDto>>> GetAllAsync();

    Task<ReturnResult<StudentDto>> GetByIdAsync(int id);

    Task<ReturnResult<StudentDto>> CreateAsync(
        StudentCreateDto dto);

    Task<ReturnResult<StudentDto>> UpdateAsync(
        int id,
        StudentUpdateDto dto);

    Task<ReturnResult<bool>> DeleteAsync(int id);

    Task<ReturnResult<List<CourseDto>>> GetCoursesAsync(int id);
}

