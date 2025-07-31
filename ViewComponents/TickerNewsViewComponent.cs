using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using System.Linq;

namespace Newfactjo.ViewComponents
{
    public class TickerNewsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public TickerNewsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var tickerNews = _context.NewsItems
                .Where(n => n.IsPublished && n.ShowInTicker)  // هنا استخدمنا ShowInTicker
                .OrderByDescending(n => n.PublishedDate)
                .Take(10)
                .ToList();

            return View(tickerNews);
        }
    }
}
