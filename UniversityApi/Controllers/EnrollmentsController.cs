using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Enrollments;
using UniversityApi.Models;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IRepository<Enrollment> _enrollmentRepo;
        private readonly IRepository<Student> _studentRepo;
        private readonly IRepository<Course> _courseRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(
            IRepository<Enrollment> enrollmentRepo,
            IRepository<Student> studentRepo,
            IRepository<Course> courseRepo,
            IMapper mapper,
            ILogger<EnrollmentsController> logger)
        {
            _enrollmentRepo = enrollmentRepo;
            _studentRepo = studentRepo;
            _courseRepo = courseRepo;
            _mapper = mapper;
            _logger = logger;
        }

        // POST: api/enrollments (Записать студента на курс с проверкой бизнес-правил)
        [HttpPost]
        public async Task<ActionResult<ReturnResult<EnrollmentDto>>> Enroll(EnrollmentCreateDto dto)
        {
            // Правило №1: Нельзя записать студента на несуществующий курс
            var course = await _courseRepo.GetByIdAsync(dto.CourseId);
            if (course == null)
            {
                _logger.LogWarning("Ошибка записи: курс с ID {CourseId} не существует.", dto.CourseId);
                return BadRequest(ReturnResult<EnrollmentDto>.Failure("COURSE_NOT_FOUND", "Указанный учебный курс не существует.", 400));
            }

            // Правило №2: Нельзя записать несуществующего студента на курс
            var student = await _studentRepo.GetByIdAsync(dto.StudentId);
            if (student == null)
            {
                _logger.LogWarning("Ошибка записи: студент с ID {StudentId} не существует.", dto.StudentId);
                return BadRequest(ReturnResult<EnrollmentDto>.Failure("STUDENT_NOT_FOUND", "Указанный студент не существует.", 400));
            }

            // Правило №3: Один студент не может быть дважды записан на один и тот же курс
            var allEnrollments = await _enrollmentRepo.GetAllAsync();
            if (allEnrollments.Any(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId))
            {
                _logger.LogWarning("Ошибка записи: Студент {StudentId} уже записан на курс {CourseId}.", dto.StudentId, dto.CourseId);
                return Conflict(ReturnResult<EnrollmentDto>.Failure("ALREADY_ENROLLED", "Этот студент уже записан на данный курс.", 409));
            }

            var enrollment = _mapper.Map<Enrollment>(dto);
            await _enrollmentRepo.AddAsync(enrollment);
            await _enrollmentRepo.SaveChangesAsync();

            _logger.LogInformation("Студент {StudentId} успешно записан на курс {CourseId}. ID записи: {Id}", dto.StudentId, dto.CourseId, enrollment.Id);

            var resultDto = _mapper.Map<EnrollmentDto>(enrollment);
            return Ok(ReturnResult<EnrollmentDto>.Success(resultDto, 201));
        }

        // GET: api/enrollments (Получить список всех записей)
        [HttpGet]
        public async Task<ActionResult<ReturnResult<IEnumerable<EnrollmentDto>>>> GetAll()
        {
            var enrollments = await _enrollmentRepo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
            return Ok(ReturnResult<IEnumerable<EnrollmentDto>>.Success(dtos));
        }
    }
}