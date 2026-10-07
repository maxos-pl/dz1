using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages.Products;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    public IActionResult OnGet()
    {
        Product = new Product
        {
            Category = "Электроника",
            IsAvailable = true,
            Price = 100.00m
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Product.CreatedAt = DateTime.UtcNow;
        _context.Products.Add(Product);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}
