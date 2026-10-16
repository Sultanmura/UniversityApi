using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Middleware;
using UniversityApi.Repositories.Implementations;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Mapping;

var builder = WebApplication.CreateBuilder(args);

// 1. Настройка подключения к базе данных MS SQL Server через EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Регистрация универсального паттерна Repository (Lifetime: Scoped)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// 3. Регистрация AutoMapper через стандартную конфигурацию
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Добавление поддержки контроллеров
builder.Services.AddControllers();

// Настройка Swagger/OpenAPI для тестирования эндпоинтов
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Подключение нашего глобального Middleware для обработки непредвиденных ошибок
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Настройка Swagger для среды разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Включаем маршрутизацию на контроллеры
app.MapControllers();
// Автоматическое создание базы данных при старте приложения
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Этой командой EF Core сам создаст базу данных и все таблицы, если их еще нет
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при автоматическом создании базы данных.");
    }
}
app.Run();