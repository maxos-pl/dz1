using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorPagesApp.Data;
using RazorPagesApp.DTOs;
using RazorPagesApp.Models;

namespace RazorPagesApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /api/products
    /// Получить список всех товаров с возможностью фильтрации по поисковой строке, категории и диапазону цен.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.Trim().ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(searchLower) 
                                  || (p.Description != null && p.Description.ToLower().Contains(searchLower)));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category == category.Trim());
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        var products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        return Ok(products);
    }

    /// <summary>
    /// GET /api/products/categories
    /// Получить список всех существующих категорий.
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        var categories = await _context.Products
            .Select(p => p.Category)
            .Distinct()
            .ToListAsync();

        return Ok(categories);
    }

    /// <summary>
    /// GET /api/products/{id}
    /// Получить товар по его числовому идентификатору ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound(new { error = $"Товар с идентификатором {id} не найден." });
        }

        return Ok(product);
    }

    /// <summary>
    /// POST /api/products
    /// Создать новый товар.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Product>> Create([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = new Product
        {
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim(),
            Price = dto.Price,
            Category = dto.Category.Trim(),
            IsAvailable = dto.IsAvailable,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// PUT /api/products/{id}
    /// Обновить существующий товар по ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Update(int id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { error = $"Товар с идентификатором {id} не найден." });
        }

        product.Title = dto.Title.Trim();
        product.Description = dto.Description?.Trim();
        product.Price = dto.Price;
        product.Category = dto.Category.Trim();
        product.IsAvailable = dto.IsAvailable;

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    /// <summary>
    /// DELETE /api/products/{id}
    /// Удалить товар по ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { error = $"Товар с идентификатором {id} не найден." });
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = $"Товар ID={id} ('{product.Title}') успешно удален." });
    }
}
