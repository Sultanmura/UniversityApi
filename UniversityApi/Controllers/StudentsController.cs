using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Students;
using UniversityApi.Models;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IRepository<Student> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentsController> _logger;

        // Внедрение зависимостей через конструктор (Dependency Injection)
        public StudentsController(IRepository<Student> repository, IMapper mapper, ILogger<StudentsController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/students (Получение всех студентов)
        [HttpGet]
        public async Task<ActionResult<ReturnResult<IEnumerable<StudentDto>>>> GetAll()
        {
            var students = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<StudentDto>>(students);
            return Ok(ReturnResult<IEnumerable<StudentDto>>.Success(dtos));
        }

        // GET: api/students/{id} (Получение студента по ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnResult<StudentDto>>> GetById(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                // Логирование попытки получить несуществующий объект (Требование ТЗ)
                _logger.LogWarning("Попытка получить несуществующего студента с ID: {Id}", id);
                return NotFound(ReturnResult<StudentDto>.Failure("STUDENT_NOT_FOUND", "Студент не найден.", 404));
            }
            return Ok(ReturnResult<StudentDto>.Success(_mapper.Map<StudentDto>(student)));
        }

        // POST: api/students (Создание студента)
        [HttpPost]
        public async Task<ActionResult<ReturnResult<StudentDto>>> Create(StudentCreateDto dto)
        {
            var student = _mapper.Map<Student>(dto);
            await _repository.AddAsync(student);
            await _repository.SaveChangesAsync();

            // Логирование создания объекта (Требование ТЗ)
            _logger.LogInformation("Создан новый студент с ID: {Id}", student.Id);

            var resultDto = _mapper.Map<StudentDto>(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, ReturnResult<StudentDto>.Success(resultDto, 201));
        }

        // PUT: api/students/{id} (Изменение студента)
        [HttpPut("{id}")]
        public async Task<ActionResult<ReturnResult<object>>> Update(int id, StudentUpdateDto dto)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Попытка изменить несуществующего студента с ID: {Id}", id);
                return NotFound(ReturnResult<object>.Failure("STUDENT_NOT_FOUND", "Студент не найден.", 404));
            }

            _mapper.Map(dto, student);
            _repository.Update(student);
            await _repository.SaveChangesAsync();

            // Логирование изменения объекта (Требование ТЗ)
            _logger.LogInformation("Данные студента с ID: {Id} успешно обновлены", id);
            return Ok(ReturnResult<object>.Success(new { Message = "Данные успешно обновлены" }));
        }

        // DELETE: api/students/{id} (Удаление студента)
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReturnResult<object>>> Delete(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Попытка удалить несуществующего студента с ID: {Id}", id);
                return NotFound(ReturnResult<object>.Failure("STUDENT_NOT_FOUND", "Студент не найден.", 404));
            }

            _repository.Delete(student);
            await _repository.SaveChangesAsync();

            // Логирование удаления объекта (Требование ТЗ)
            _logger.LogInformation("Студент с ID: {Id} успешно удален", id);
            return Ok(ReturnResult<object>.Success(new { Message = "Студент успешно удален" }));
        }
    }
}