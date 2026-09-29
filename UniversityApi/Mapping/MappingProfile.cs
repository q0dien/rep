using AutoMapper;
using UniversityApi.DTO.Courses;
using UniversityApi.DTO.Enrollments;
using UniversityApi.DTO.Students;
using UniversityApi.DTO.Teachers;
using UniversityApi.Models;

namespace UniversityApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<StudentCreateDto, Student>();
        CreateMap<StudentUpdateDto, Student>();

        CreateMap<Teacher, TeacherDto>();
        CreateMap<TeacherCreateDto, Teacher>();
        CreateMap<TeacherUpdateDto, Teacher>();

        CreateMap<Course, CourseDto>()
            .ForMember(
                destination => destination.TeacherName,
                options => options.MapFrom(
                    source => source.Teacher == null
                        ? null
                        : $"{source.Teacher.FirstName} {source.Teacher.LastName}"));

        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseUpdateDto, Course>();

        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(
                destination => destination.StudentName,
                options => options.MapFrom(
                    source => source.Student == null
                        ? null
                        : $"{source.Student.FirstName} {source.Student.LastName}"))
            .ForMember(
                destination => destination.CourseName,
                options => options.MapFrom(
                    source => source.Course == null
                        ? null
                        : source.Course.Name));

        CreateMap<EnrollmentCreateDto, Enrollment>();
    }
}