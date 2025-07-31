using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using System.Linq;
using Newfactjo.Filters;  // استدعاء الفلتر

namespace Newfactjo.Controllers
{
    [AdminAuthorize]  // تطبيق الفلتر على كل الأكشنات هنا
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // تم إزالة شرط التحقق لأن الفلتر سيغطي ذلك

            ViewBag.NewsCount = _context.NewsItems.Count();
            ViewBag.ArticlesCount = _context.Articles.Count();
            ViewBag.CategoriesCount = _context.Categories.Count();

            return View();
        }
    }
}
