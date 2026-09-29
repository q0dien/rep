using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityApi.Common;
using UniversityApi.DTO.Courses;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CourseService> _logger;

    public CourseService(
        ICourseRepository repository,
        ITeacherRepository teacherRepository,
        IMapper mapper,
        ILogger<CourseService> logger)
    {
        _repository = repository;
        _teacherRepository = teacherRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReturnResult<List<CourseDto>>> GetAllAsync(
        string? search = null)
    {
        var courses = await _repository.GetAllAsync(search);
        var result = _mapper.Map<List<CourseDto>>(courses);

        return ReturnResult<List<CourseDto>>.Success(result);
    }

    public async Task<ReturnResult<CourseDto>> GetByIdAsync(int id)
    {
        var course = await _repository.GetByIdAsync(id);

        if (course == null)
        {
            _logger.LogWarning(
                "Course with id {Id} was not found",
                id);

            return ReturnResult<CourseDto>.Error(
                404,
                "COURSE_NOT_FOUND",
                "Course not found");
        }

        var result = _mapper.Map<CourseDto>(course);

        return ReturnResult<CourseDto>.Success(result);
    }

    public async Task<ReturnResult<CourseDto>> CreateAsync(
        CourseCreateDto dto)
    {
        var teacher =
            await _teacherRepository.GetByIdAsync(dto.TeacherId);

        if (teacher == null)
        {
            return ReturnResult<CourseDto>.Error(
                404,
                "TEACHER_NOT_FOUND",
                "Teacher not found");
        }

        var course = _mapper.Map<Models.Course>(dto);

        var createdCourse = await _repository.AddAsync(course);

        _logger.LogInformation(
            "Course {Id} was created",
            createdCourse.Id);

        var result = _mapper.Map<CourseDto>(createdCourse);

        return ReturnResult<CourseDto>.Success(result, 201);
    }

    public async Task<ReturnResult<CourseDto>> UpdateAsync(
        int id,
        CourseUpdateDto dto)
    {
        var course = await _repository.GetByIdAsync(id);

        if (course == null)
        {
            _logger.LogWarning(
                "Attempt to update missing course {Id}",
                id);

            return ReturnResult<CourseDto>.Error(
                404,
                "COURSE_NOT_FOUND",
                "Course not found");
        }

        var teacher =
            await _teacherRepository.GetByIdAsync(dto.TeacherId);

        if (teacher == null)
        {
            return ReturnResult<CourseDto>.Error(
                404,
                "TEACHER_NOT_FOUND",
                "Teacher not found");
        }

        _mapper.Map(dto, course);

        await _repository.UpdateAsync(course);

        _logger.LogInformation(
            "Course {Id} was updated",
            id);

        var result = _mapper.Map<CourseDto>(course);

        return ReturnResult<CourseDto>.Success(result);
    }

    public async Task<ReturnResult<bool>> DeleteAsync(int id)
    {
        var course = await _repository.GetByIdAsync(id);

        if (course == null)
        {
            _logger.LogWarning(
                "Attempt to delete missing course {Id}",
                id);

            return ReturnResult<bool>.Error(
                404,
                "COURSE_NOT_FOUND",
                "Course not found");
        }

        await _repository.DeleteAsync(course);

        _logger.LogInformation(
            "Course {Id} was deleted",
            id);

        return ReturnResult<bool>.Success(true);
    }
}