using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using Newfactjo.Models;
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
                .Where(n => n.IsPublished && n.Placement == NewsPlacement.TopBar)
                .OrderByDescending(n => n.PublishedDate)
                .Take(4)
                .ToList();

            return View(newsList);
        }
    }
}
