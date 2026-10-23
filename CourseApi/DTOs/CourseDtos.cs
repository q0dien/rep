
namespace CourseApi.DTOs;

public record CourseV1Dto(
    int Id,
    string Name,
    string Teacher,
    int Credits);

public record CourseV2Dto(
    int Id,
    string Name,
    string Teacher,
    int Credits,
    string? TeacherName,
    int EnrollmentCount);
