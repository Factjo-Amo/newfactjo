using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newfactjo.Data;
using Newfactjo.Models;
using Newfactjo.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Newfactjo.Controllers
{
    [AdminAuthorize] // 🔐 الفلتر الذي يمنع الوصول دون تسجيل دخول
    public class AdminUsersController : Controller
    {
        private readonly AppDbContext _context;

        public AdminUsersController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ عرض قائمة المستخدمين
        public IActionResult Index()
        {
            var users = _context.AdminUsers.ToList();
            return View(users);
        }

        // ✅ صفحة إضافة مستخدم (GET)
        public IActionResult Create()
        {
            var roles = _context.Roles.Select(r => r.Name).ToList();
            ViewBag.RoleList = new SelectList(roles); // لا تحتاج إلى value/label لأن القيمة = النص
            return View();
        }

        // ✅ إضافة مستخدم (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdminUser user)
        {
            if (ModelState.IsValid)
            {
                _context.AdminUsers.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            var roles = _context.Roles.Select(r => r.Name).ToList();
            ViewBag.RoleList = new SelectList(roles, user.Role); // ← نربط الدور الحالي في حال وجود خطأ

            return View(user);
        }

        // ✅ صفحة تعديل مستخدم (GET)
        public IActionResult Edit(int id)
        {
            var user = _context.AdminUsers.Find(id);
            if (user == null) return NotFound();

            var roles = _context.Roles.Select(r => r.Name).ToList();
            ViewBag.RoleList = new SelectList(roles, user.Role); // ← تحديد الدور الحالي أيضًا

            return View(user);
        }

        // ✅ تعديل مستخدم (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AdminUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            var roles = _context.Roles.Select(r => r.Name).ToList();
            ViewBag.RoleList = new SelectList(roles, user.Role);

            return View(user);
        }

        // ✅ حذف مستخدم (GET)
        public IActionResult Delete(int id)
        {
            var user = _context.AdminUsers.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // ✅ تأكيد حذف مستخدم (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.AdminUsers.Find(id);
            if (user != null)
            {
                _context.AdminUsers.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
