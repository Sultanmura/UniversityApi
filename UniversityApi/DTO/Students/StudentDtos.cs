using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Students
{
    // То, что отдаем клиенту
    public record StudentDto(int Id, string FirstName, string LastName, string Email, DateTime BirthDate, DateTime CreatedAt);

    // То, что принимаем при создании
    public record StudentCreateDto(
        [Required(ErrorMessage = "Имя обязательно")][MaxLength(50)] string FirstName,
        [Required(ErrorMessage = "Фамилия обязательна")][MaxLength(50)] string LastName,
        [Required(ErrorMessage = "Email обязателен")][EmailAddress(ErrorMessage = "Некорректный формат Email")] string Email,
        [Required] DateTime BirthDate
    );

    // То, что принимаем при обновлении
    public record StudentUpdateDto(
        [Required][MaxLength(50)] string FirstName,
        [Required][MaxLength(50)] string LastName,
        [Required][EmailAddress] string Email,
        [Required] DateTime BirthDate
    );
}