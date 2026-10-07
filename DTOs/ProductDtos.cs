using System.ComponentModel.DataAnnotations;

namespace RazorPagesApp.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "Название обязательно для заполнения")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Длина названия должна быть от 2 до 100 символов")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Длина описания не должна превышать 500 символов")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Укажите цену")]
    [Range(0.01, 1000000.00, ErrorMessage = "Цена должна быть положительной от 0.01 до 1 000 000")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Укажите категорию")]
    [StringLength(50, ErrorMessage = "Название категории не более 50 символов")]
    public string Category { get; set; } = "Общее";

    public bool IsAvailable { get; set; } = true;
}

public class UpdateProductDto
{
    [Required(ErrorMessage = "Название обязательно для заполнения")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Длина названия должна быть от 2 до 100 символов")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Длина описания не должна превышать 500 символов")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Укажите цену")]
    [Range(0.01, 1000000.00, ErrorMessage = "Цена должна быть положительной от 0.01 до 1 000 000")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Укажите категорию")]
    [StringLength(50, ErrorMessage = "Название категории не более 50 символов")]
    public string Category { get; set; } = "Общее";

    public bool IsAvailable { get; set; } = true;
}
