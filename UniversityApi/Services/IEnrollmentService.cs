using UniversityApi.Common;
using UniversityApi.DTO.Enrollments;

namespace UniversityApi.Services;

public interface IEnrollmentService
{
    Task<ReturnResult<List<EnrollmentDto>>> GetAllAsync();
    Task<ReturnResult<EnrollmentDto>> GetByIdAsync(int id);
    Task<ReturnResult<EnrollmentDto>> CreateAsync(
        EnrollmentCreateDto dto);
    Task<ReturnResult<EnrollmentDto>> UpdateAsync(
        int id,
        EnrollmentUpdateDto dto);
    Task<ReturnResult<bool>> DeleteAsync(int id);
}