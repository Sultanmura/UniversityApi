using AutoMapper;
using UniversityApi.Models;
using UniversityApi.DTO.Students;
using UniversityApi.DTO.Teachers;
using UniversityApi.DTO.Courses;
using UniversityApi.DTO.Enrollments;

namespace UniversityApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг Студентов
            CreateMap<Student, StudentDto>();
            CreateMap<StudentCreateDto, Student>();
            CreateMap<StudentUpdateDto, Student>();

            // Маппинг Преподавателей
            CreateMap<Teacher, TeacherDto>();
            CreateMap<TeacherCreateDto, Teacher>();

            // Маппинг Курсов
            CreateMap<Course, CourseDto>();
            CreateMap<CourseCreateDto, Course>();

            // Маппинг Записей
            CreateMap<Enrollment, EnrollmentDto>();
            CreateMap<EnrollmentCreateDto, Enrollment>();
        }
    }
}