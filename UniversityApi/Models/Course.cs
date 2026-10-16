namespace UniversityApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Credits { get; set; }

        // Внешний ключ на преподавателя
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Связь: на один курс может записаться много студентов
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}