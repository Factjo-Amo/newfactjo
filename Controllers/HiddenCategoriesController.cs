using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using Newfactjo.Models;
using Newfactjo.ViewModels;

using System.Linq;

namespace Newfactjo.Controllers
{
    public class HiddenCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public HiddenCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ عرض التصنيفات مع إمكانية الإخفاء أو الإظهار
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var hiddenCategoryIds = _context.HiddenCategories.Select(h => h.CategoryId).ToList();

            var model = categories.Select(cat => new HiddenCategoryViewModel
            {
                CategoryId = cat.Id,
                CategoryName = cat.Name,
                IsHidden = hiddenCategoryIds.Contains(cat.Id)
            }).ToList();

            return View(model);
        }

        // ✅ إخفاء تصنيف
        [HttpPost]
        public IActionResult Hide(int id)
        {
            if (!_context.HiddenCategories.Any(h => h.CategoryId == id))
            {
                _context.HiddenCategories.Add(new HiddenCategory { CategoryId = id });
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ✅ إظهار تصنيف
        [HttpPost]
        public IActionResult Show(int id)
        {
            var hidden = _context.HiddenCategories.FirstOrDefault(h => h.CategoryId == id);
            if (hidden != null)
            {
                _context.HiddenCategories.Remove(hidden);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
