using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using Newfactjo.Models;
using System.Linq;

namespace Newfactjo.Controllers
{
    public class RolesController : Controller
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var roles = _context.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Roles.Any(r => r.Name == role.Name))
                {
                    _context.Roles.Add(role);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", "هذا الدور موجود بالفعل.");
            }

            return View(role);
        }
    }
}
