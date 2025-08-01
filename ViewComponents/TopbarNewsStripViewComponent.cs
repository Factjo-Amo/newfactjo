using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using System.Linq;

namespace Newfactjo.ViewComponents
{
    public class TopbarNewsStripViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public TopbarNewsStripViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var newsList = _context.NewsItems
                .Where(n => n.CategoryId == 11 && n.IsPublished)
                .OrderByDescending(n => n.PublishedDate)
                .Take(5)
                .ToList();

            return View(newsList);
        }
    }
}
