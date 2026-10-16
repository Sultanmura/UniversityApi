using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Enrollments
{
    public record EnrollmentDto(int Id, int StudentId, int CourseId, DateTime EnrollmentDate, string? Grade);

    public record EnrollmentCreateDto([Required] int StudentId, [Required] int CourseId);

    public record UpdateGradeDto([Required][MaxLength(2)] string Grade);
}