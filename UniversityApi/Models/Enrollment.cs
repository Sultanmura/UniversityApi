namespace UniversityApi.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        // Связь со студентом
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        // Связь с курсом
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public string? Grade { get; set; } // Оценка может быть пустой при регистрации
    }
}