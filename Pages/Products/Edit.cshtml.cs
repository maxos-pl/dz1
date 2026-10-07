using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages.Products;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        Product = product;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        existingProduct.Title = Product.Title;
        existingProduct.Description = Product.Description;
        existingProduct.Category = Product.Category;
        existingProduct.Price = Product.Price;
        existingProduct.IsAvailable = Product.IsAvailable;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return RedirectToPage("/Index");
    }
}
