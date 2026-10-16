using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Teachers
{
    public record TeacherDto(int Id, string FirstName, string LastName, string Email, string Department);

    public record TeacherCreateDto(
        [Required][MaxLength(50)] string FirstName,
        [Required][MaxLength(50)] string LastName,
        [Required][EmailAddress] string Email,
        [Required] string Department
    );
}