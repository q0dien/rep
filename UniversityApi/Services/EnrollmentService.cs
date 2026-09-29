using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityApi.Common;
using UniversityApi.DTO.Enrollments;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _repository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<EnrollmentService> _logger;

    public EnrollmentService(
        IEnrollmentRepository repository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        IMapper mapper,
        ILogger<EnrollmentService> logger)
    {
        _repository = repository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReturnResult<List<EnrollmentDto>>> GetAllAsync()
    {
        var enrollments = await _repository.GetAllAsync();

        var result = _mapper.Map<List<EnrollmentDto>>(enrollments);

        return ReturnResult<List<EnrollmentDto>>.Success(result);
    }

    public async Task<ReturnResult<EnrollmentDto>> GetByIdAsync(int id)
    {
        var enrollment = await _repository.GetByIdAsync(id);

        if (enrollment == null)
        {
            _logger.LogWarning(
                "Enrollment with id {Id} was not found",
                id);

            return ReturnResult<EnrollmentDto>.Error(
                404,
                "ENROLLMENT_NOT_FOUND",
                "Enrollment not found");
        }

        var result = _mapper.Map<EnrollmentDto>(enrollment);

        return ReturnResult<EnrollmentDto>.Success(result);
    }

    public async Task<ReturnResult<EnrollmentDto>> CreateAsync(
        EnrollmentCreateDto dto)
    {
        var student =
            await _studentRepository.GetByIdAsync(dto.StudentId);

        if (student == null)
        {
            return ReturnResult<EnrollmentDto>.Error(
                404,
                "STUDENT_NOT_FOUND",
                "Student not found");
        }

        var course =
            await _courseRepository.GetByIdAsync(dto.CourseId);

        if (course == null)
        {
            return ReturnResult<EnrollmentDto>.Error(
                404,
                "COURSE_NOT_FOUND",
                "Course not found");
        }

        var existingEnrollment =
            await _repository.GetByStudentAndCourseAsync(
                dto.StudentId,
                dto.CourseId);

        if (existingEnrollment != null)
        {
            return ReturnResult<EnrollmentDto>.Error(
                409,
                "ALREADY_ENROLLED",
                "Student is already enrolled in this course");
        }

        var enrollment =
            _mapper.Map<Models.Enrollment>(dto);

        var createdEnrollment =
            await _repository.AddAsync(enrollment);

        _logger.LogInformation(
            "Student {StudentId} was enrolled in course {CourseId}",
            dto.StudentId,
            dto.CourseId);

        var result =
            _mapper.Map<EnrollmentDto>(createdEnrollment);

        return ReturnResult<EnrollmentDto>.Success(result, 201);
    }

    public async Task<ReturnResult<EnrollmentDto>> UpdateAsync(
        int id,
        EnrollmentUpdateDto dto)
    {
        var enrollment = await _repository.GetByIdAsync(id);

        if (enrollment == null)
        {
            _logger.LogWarning(
                "Attempt to update missing enrollment {Id}",
                id);

            return ReturnResult<EnrollmentDto>.Error(
                404,
                "ENROLLMENT_NOT_FOUND",
                "Enrollment not found");
        }

        enrollment.Grade = dto.Grade;

        await _repository.UpdateAsync(enrollment);

        _logger.LogInformation(
            "Enrollment {Id} grade was updated",
            id);

        var result =
            _mapper.Map<EnrollmentDto>(enrollment);

        return ReturnResult<EnrollmentDto>.Success(result);
    }

    public async Task<ReturnResult<bool>> DeleteAsync(int id)
    {
        var enrollment = await _repository.GetByIdAsync(id);

        if (enrollment == null)
        {
            _logger.LogWarning(
                "Attempt to delete missing enrollment {Id}",
                id);

            return ReturnResult<bool>.Error(
                404,
                "ENROLLMENT_NOT_FOUND",
                "Enrollment not found");
        }

        await _repository.DeleteAsync(enrollment);

        _logger.LogInformation(
            "Enrollment {Id} was deleted",
            id);

        return ReturnResult<bool>.Success(true);
    }
}