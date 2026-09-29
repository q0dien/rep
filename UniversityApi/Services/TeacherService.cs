using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityApi.Common;
using UniversityApi.DTO.Teachers;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TeacherService> _logger;

    public TeacherService(
        ITeacherRepository repository,
        IMapper mapper,
        ILogger<TeacherService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReturnResult<List<TeacherDto>>> GetAllAsync()
    {
        var teachers = await _repository.GetAllAsync();
        var result = _mapper.Map<List<TeacherDto>>(teachers);

        return ReturnResult<List<TeacherDto>>.Success(result);
    }

    public async Task<ReturnResult<TeacherDto>> GetByIdAsync(int id)
    {
        var teacher = await _repository.GetByIdAsync(id);

        if (teacher == null)
        {
            _logger.LogWarning(
                "Teacher with id {Id} was not found",
                id);

            return ReturnResult<TeacherDto>.Error(
                404,
                "TEACHER_NOT_FOUND",
                "Teacher not found");
        }

        var result = _mapper.Map<TeacherDto>(teacher);

        return ReturnResult<TeacherDto>.Success(result);
    }

    public async Task<ReturnResult<TeacherDto>> CreateAsync(
        TeacherCreateDto dto)
    {
        var existingTeacher =
            await _repository.GetByEmailAsync(dto.Email);

        if (existingTeacher != null)
        {
            return ReturnResult<TeacherDto>.Error(
                409,
                "EMAIL_EXISTS",
                "Teacher with this email already exists");
        }

        var teacher = _mapper.Map<Models.Teacher>(dto);

        var createdTeacher = await _repository.AddAsync(teacher);

        _logger.LogInformation(
            "Teacher {Id} was created",
            createdTeacher.Id);

        var result = _mapper.Map<TeacherDto>(createdTeacher);

        return ReturnResult<TeacherDto>.Success(result, 201);
    }

    public async Task<ReturnResult<TeacherDto>> UpdateAsync(
        int id,
        TeacherUpdateDto dto)
    {
        var teacher = await _repository.GetByIdAsync(id);

        if (teacher == null)
        {
            _logger.LogWarning(
                "Attempt to update missing teacher {Id}",
                id);

            return ReturnResult<TeacherDto>.Error(
                404,
                "TEACHER_NOT_FOUND",
                "Teacher not found");
        }

        var existingTeacher =
            await _repository.GetByEmailAsync(dto.Email);

        if (existingTeacher != null &&
            existingTeacher.Id != id)
        {
            return ReturnResult<TeacherDto>.Error(
                409,
                "EMAIL_EXISTS",
                "Teacher with this email already exists");
        }

        _mapper.Map(dto, teacher);

        await _repository.UpdateAsync(teacher);

        _logger.LogInformation(
            "Teacher {Id} was updated",
            id);

        var result = _mapper.Map<TeacherDto>(teacher);

        return ReturnResult<TeacherDto>.Success(result);
    }

    public async Task<ReturnResult<bool>> DeleteAsync(int id)
    {
        var teacher = await _repository.GetByIdAsync(id);

        if (teacher == null)
        {
            _logger.LogWarning(
                "Attempt to delete missing teacher {Id}",
                id);

            return ReturnResult<bool>.Error(
                404,
                "TEACHER_NOT_FOUND",
                "Teacher not found");
        }

        await _repository.DeleteAsync(teacher);

        _logger.LogInformation(
            "Teacher {Id} was deleted",
            id);

        return ReturnResult<bool>.Success(true);
    }
}