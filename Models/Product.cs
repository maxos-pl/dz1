using System.ComponentModel.DataAnnotations;

namespace RazorPagesApp.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название обязательно для заполнения")]
    [StringLength(100, ErrorMessage = "Длина названия не должна превышать 100 символов")]
    [Display(Name = "Название")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Описание")]
    [StringLength(500, ErrorMessage = "Длина описания не должна превышать 500 символов")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Укажите цену")]
    [Range(0.01, 1000000.00, ErrorMessage = "Цена должна быть больше нуля")]
    [Display(Name = "Цена (руб.)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Укажите категорию")]
    [Display(Name = "Категория")]
    public string Category { get; set; } = "Общее";

    [Display(Name = "В наличии")]
    public bool IsAvailable { get; set; } = true;

    [Display(Name = "Дата добавления")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
