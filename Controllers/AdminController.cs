using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using Newfactjo.Models;
using System.Linq;
using Newfactjo.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Newfactjo.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // صفحة تسجيل الدخول (GET)
        public IActionResult Login()
        {
            return View();
        }

        // معالجة تسجيل الدخول (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _context.AdminUsers
                    .FirstOrDefault(a => a.Username == model.Username && a.Password == model.Password);

                if (admin != null)
                {
                    // ✅ نضيف AdminId إلى الجلسة لتتبع من قام بإدخال الأخبار
                    HttpContext.Session.SetInt32("AdminId", admin.Id);
                    HttpContext.Session.SetString("AdminUsername", admin.Username);
                    HttpContext.Session.SetString("AdminRole", admin.Role ?? ""); // ✅ ← نحفظ الدور مباشرة لأنه string

                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError("", "اسم المستخدم أو كلمة المرور غير صحيحة.");
            }

            return View(model);
        }

        // تسجيل الخروج
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ✅ عرض واجهة إدارة التصنيفات المخفية
        [HttpGet]
        public IActionResult ManageHiddenCategories()
        {
            var allCategories = _context.Categories.ToList();

            var hiddenIds = _context.HiddenCategories.Select(h => h.CategoryId).ToList();

            var model = allCategories.Select(cat => new HiddenCategoryViewModel
            {
                CategoryId = cat.Id,
                CategoryName = cat.Name,
                IsHidden = hiddenIds.Contains(cat.Id)
            }).ToList();

            return View(model);
        }

        // ✅ حفظ التعديلات على التصنيفات المخفية
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageHiddenCategories(List<int> selectedCategoryIds)
        {
            // حذف كل الإعدادات السابقة
            var allHidden = _context.HiddenCategories.ToList();
            _context.HiddenCategories.RemoveRange(allHidden);
            _context.SaveChanges();

            // إضافة التصنيفات المحددة حاليًا
            if (selectedCategoryIds != null)
            {
                foreach (var id in selectedCategoryIds)
                {
                    _context.HiddenCategories.Add(new HiddenCategory { CategoryId = id });
                }
                _context.SaveChanges();
            }

            TempData["SuccessMessage"] = "تم حفظ التصنيفات المخفية بنجاح.";
            return RedirectToAction("ManageHiddenCategories");
        }
    }
}
