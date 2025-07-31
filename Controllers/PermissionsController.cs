using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using Newfactjo.Models;
using Newfactjo.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Newfactjo.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly AppDbContext _context;

        // ✅ جميع الأدوار الموجودة في الموقع
       
        // ✅ جميع الصلاحيات الممكنة
        private readonly List<string> permissions = new List<string>
        {
            "ManageNews",
            "ManageArticles",
            "ManageCategories",
            "ManageUsers",
            "ManageAds",
            "ManageLogs",
            "ManageHiddenCategories",
             "ManagePermissions",
            "ManageRoles"

        };

        public PermissionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var roles = _context.Roles.Select(r => r.Name).ToList();

            // ✅ بناء جدول الصلاحيات
            var model = new List<PermissionMatrixViewModel>();

            foreach (var role in roles)
            {
                var rolePermissions = _context.RolePermissions
                    .Where(r => r.RoleName == role)
                    .ToDictionary(r => r.PermissionName, r => r.IsGranted);

                var permissionDict = new Dictionary<string, bool>();

                foreach (var perm in permissions)
                {
                    permissionDict[perm] = rolePermissions.ContainsKey(perm) && rolePermissions[perm];
                }

                model.Add(new PermissionMatrixViewModel
                {
                    RoleName = role,
                    Permissions = permissionDict
                });
            }

            ViewBag.AllPermissions = permissions;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(List<PermissionMatrixViewModel> model)
        {
            // حذف كل الصلاحيات القديمة
            _context.RolePermissions.RemoveRange(_context.RolePermissions);
            _context.SaveChanges();

            // إعادة حفظ الصلاحيات الجديدة مع حماية من null
            foreach (var roleEntry in model)
            {
                if (roleEntry.Permissions != null)
                {
                    foreach (var perm in roleEntry.Permissions)
                    {
                        _context.RolePermissions.Add(new RolePermission
                        {
                            RoleName = roleEntry.RoleName,
                            PermissionName = perm.Key,
                            IsGranted = perm.Value
                        });
                    }
                }
            }

            _context.SaveChanges();

            TempData["Success"] = "✅ تم حفظ الصلاحيات بنجاح.";
            return RedirectToAction("Index");
        }

    }
}
