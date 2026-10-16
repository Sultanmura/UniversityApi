using Microsoft.Identity.Client.NativeInterop;

namespace UniversityApi.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Department { get; set; } = null!;

        // Связь: один преподаватель может вести много курсов
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}