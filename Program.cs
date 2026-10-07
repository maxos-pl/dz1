using Microsoft.EntityFrameworkCore;
using RazorPagesApp.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Подключение базы данных SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=products.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// 2. Добавление сервисов Razor Pages и API Контроллеров
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// 3. Подключение Swagger для документирования и тестирования API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Product Catalog API",
        Version = "v1",
        Description = "REST API для управления каталогом товаров (ASP.NET Razor Pages & Web API)"
    });
});

var app = builder.Build();

// Автоматическое создание БД и сидирование данных при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Конфигурация HTTP pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Включение Swagger документации
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog API v1");
    c.RoutePrefix = "swagger";
});

// Маппинг Razor Pages и REST API контроллеров
app.MapRazorPages();
app.MapControllers();

app.Run();
