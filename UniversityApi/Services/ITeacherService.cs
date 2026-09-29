using UniversityApi.Common;
using UniversityApi.DTO.Teachers;

namespace UniversityApi.Services;

public interface ITeacherService
{
    Task<ReturnResult<List<TeacherDto>>> GetAllAsync();
    Task<ReturnResult<TeacherDto>> GetByIdAsync(int id);
    Task<ReturnResult<TeacherDto>> CreateAsync(TeacherCreateDto dto);
    Task<ReturnResult<TeacherDto>> UpdateAsync(
        int id,
        TeacherUpdateDto dto);
    Task<ReturnResult<bool>> DeleteAsync(int id);
}