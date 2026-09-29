
using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityApi.Common;
using UniversityApi.DTO.Courses;
using UniversityApi.DTO.Students;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentService> _logger;

    public StudentService(
        IStudentRepository repository,
        IEnrollmentRepository enrollmentRepository,
        IMapper mapper,
        ILogger<StudentService> logger)
    {
        _repository = repository;
        _enrollmentRepository = enrollmentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReturnResult<List<StudentDto>>> GetAllAsync()
    {
        var students = await _repository.GetAllAsync();
        var result = _mapper.Map<List<StudentDto>>(students);

        return ReturnResult<List<StudentDto>>.Success(result);
    }

    public async Task<ReturnResult<StudentDto>> GetByIdAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
        {
            _logger.LogWarning(
                "Student with id {Id} was not found",
                id);

            return ReturnResult<StudentDto>.Error(
                404,
                "STUDENT_NOT_FOUND",
                "Student not found");
        }

        var result = _mapper.Map<StudentDto>(student);

        return ReturnResult<StudentDto>.Success(result);
    }

    public async Task<ReturnResult<StudentDto>> CreateAsync(
        StudentCreateDto dto)
    {
        var existingStudent =
            await _repository.GetByEmailAsync(dto.Email);

        if (existingStudent != null)
        {
            return ReturnResult<StudentDto>.Error(
                409,
                "EMAIL_EXISTS",
                "Student with this email already exists");
        }

        var student = _mapper.Map<Models.Student>(dto);

        var createdStudent = await _repository.AddAsync(student);

        _logger.LogInformation(
            "Student {Id} was created",
            createdStudent.Id);

        var result = _mapper.Map<StudentDto>(createdStudent);

        return ReturnResult<StudentDto>.Success(result, 201);
    }

    public async Task<ReturnResult<StudentDto>> UpdateAsync(
        int id,
        StudentUpdateDto dto)
    {
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
        {
            _logger.LogWarning(
                "Attempt to update missing student {Id}",
                id);

            return ReturnResult<StudentDto>.Error(
                404,
                "STUDENT_NOT_FOUND",
                "Student not found");
        }

        var existingStudent =
            await _repository.GetByEmailAsync(dto.Email);

        if (existingStudent != null &&
            existingStudent.Id != id)
        {
            return ReturnResult<StudentDto>.Error(
                409,
                "EMAIL_EXISTS",
                "Student with this email already exists");
        }

        _mapper.Map(dto, student);

        await _repository.UpdateAsync(student);

        _logger.LogInformation(
            "Student {Id} was updated",
            id);

        var result = _mapper.Map<StudentDto>(student);

        return ReturnResult<StudentDto>.Success(result);
    }

    public async Task<ReturnResult<bool>> DeleteAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
        {
            _logger.LogWarning(
                "Attempt to delete missing student {Id}",
                id);

            return ReturnResult<bool>.Error(
                404,
                "STUDENT_NOT_FOUND",
                "Student not found");
        }

        await _repository.DeleteAsync(student);

        _logger.LogInformation(
            "Student {Id} was deleted",
            id);

        return ReturnResult<bool>.Success(true);
    }

    public async Task<ReturnResult<List<CourseDto>>> GetCoursesAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
        {
            _logger.LogWarning(
                "Student with id {Id} was not found",
                id);

            return ReturnResult<List<CourseDto>>.Error(
                404,
                "STUDENT_NOT_FOUND",
                "Student not found");
        }

        var enrollments =
            await _enrollmentRepository.GetByStudentIdAsync(id);

        var courses = enrollments
            .Where(e => e.Course != null)
            .Select(e => e.Course!)
            .ToList();

        var result = _mapper.Map<List<CourseDto>>(courses);

        return ReturnResult<List<CourseDto>>.Success(result);
    }
}

