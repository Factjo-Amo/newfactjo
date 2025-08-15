using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;           
using System.Linq;
// الاخبار العلوية الاربعة 
namespace Newfactjo.ViewComponents
{
    public class FlashLineViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public FlashLineViewComponent(AppDbContext context) => _context = context;

        // مبدئيًا: 4 عناصر من تصنيف أخبار أعلى الموقع (11) — غيّر كما تريد
        public IViewComponentResult Invoke(int take = 4, int? categoryId = 11)
        {
            var q = _context.NewsItems.Where(n => n.IsPublished);
            if (categoryId.HasValue) q = q.Where(n => n.CategoryId == categoryId.Value);

            var items = q
                .OrderByDescending(n => n.PublishedDate)   // إن لم تكن موجودة لديك، غيّرها لـ Id
                .Take(take)
                .Select(n => new { n.Id, n.Title, n.ImageUrl })
                .ToList();

            return View(items);
        }
    }
}
