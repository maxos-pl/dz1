using Microsoft.EntityFrameworkCore;
using RazorPagesApp.Models;

namespace RazorPagesApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Начальные тестовые данные (Seed data)
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Title = "Ноутбук Pro 15",
                Description = "Производительный рабочий ноутбук с диагональю 15.6 дюймов",
                Price = 75000.00m,
                Category = "Электроника",
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 2,
                Title = "Беспроводная мышь",
                Description = "Эргономичная оптическая мышь с подключением по Bluetooth",
                Price = 2500.00m,
                Category = "Аксессуары",
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 2, 20, 14, 30, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 3,
                Title = "Механическая клавиатура",
                Description = "Клавиатура с подсветкой и переключателями Red",
                Price = 6500.00m,
                Category = "Аксессуары",
                IsAvailable = false,
                CreatedAt = new DateTime(2026, 3, 5, 9, 15, 0, DateTimeKind.Utc)
            }
        );
    }
}
