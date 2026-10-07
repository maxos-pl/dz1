using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Product> Products { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SelectedCategory { get; set; }

    public List<string> Categories { get; set; } = new();

    public async Task OnGetAsync()
    {
        // Получение списка уникальных категорий для фильтра
        Categories = await _context.Products
            .Select(p => p.Category)
            .Distinct()
            .ToListAsync();

        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchString))
        {
            query = query.Where(p => p.Title.ToLower().Contains(SearchString.ToLower()) 
                                  || (p.Description != null && p.Description.ToLower().Contains(SearchString.ToLower())));
        }

        if (!string.IsNullOrWhiteSpace(SelectedCategory))
        {
            query = query.Where(p => p.Category == SelectedCategory);
        }

        Products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
