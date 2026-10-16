namespace UniversityApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Связь: один студент может иметь много записей на курсы
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}