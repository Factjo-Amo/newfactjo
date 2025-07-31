using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newfactjo.Data;

namespace Newfactjo.ViewComponents
{
    public class AllNewsListViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public AllNewsListViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = await _context.NewsItems
                .OrderByDescending(n => n.PublishedDate)
                .Take(50)
                .ToListAsync();

            return View(news);
        }
    }
}
