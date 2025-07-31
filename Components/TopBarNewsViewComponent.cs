using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newfactjo.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Newfactjo.Components
{
    public class TopBarNewsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public TopBarNewsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // جلب الأخبار من تصنيف "أخبار أعلى الموقع"
            var topNews = await _context.NewsItems
                .Include(n => n.Category)
                .Where(n => n.Category.Name == "اخبار اعلى الموقع")
                .OrderByDescending(n => n.PublishedDate)
                .Take(3)
                .ToListAsync();

            return View(topNews);
        }
    }
}
