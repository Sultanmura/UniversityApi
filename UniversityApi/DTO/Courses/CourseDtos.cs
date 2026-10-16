using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Courses
{
    public record CourseDto(int Id, string Name, string Description, int Credits, int TeacherId, DateTime CreatedAt);

    public record CourseCreateDto(
        [Required][MaxLength(100)] string Name,
        string Description,
        [Range(1, 10, ErrorMessage = "Количество кредитов должно быть от 1 до 10")] int Credits,
        [Required] int TeacherId
    );
}