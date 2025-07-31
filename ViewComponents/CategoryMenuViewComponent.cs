using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newfactjo.Data;

namespace Newfactjo.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public CategoryMenuViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories
     .Where(c => c.ShowInTopBar)
     .ToListAsync();

            return View(categories);
        }
    }
}
