using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using System.Linq;

namespace Newfactjo.ViewComponents
{
    public class SuggestedNewsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public SuggestedNewsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var suggestedNews = _context.NewsItems
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.ViewsCount)
                .Take(5)
                .ToList();

            return View(suggestedNews);
        }
    }
}
