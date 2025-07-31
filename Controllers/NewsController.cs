using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newfactjo.Data;
using Newfactjo.Models;
using Newfactjo.Services;

namespace Newfactjo.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly PermissionService _permissionService;

        public NewsController(AppDbContext context, IWebHostEnvironment hostEnvironment, PermissionService permissionService)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _permissionService = permissionService;
        }

        // ✅ دالة ديناميكية لفحص الصلاحية
        private bool HasPermission(string permissionName)
        {
            var role = HttpContext.Session.GetString("AdminRole");
            return _permissionService.HasPermission(role, permissionName);
        }

        public async Task<IActionResult> Index(int? searchId)
        {
            if (!HasPermission("ManageNews")) return RedirectToAction("Unauthorized", "Home");

            var query = _context.NewsItems
                .Include(n => n.Category)
                .Include(n => n.AdminUser)
                .OrderByDescending(n => n.PublishedDate)
                .AsQueryable();

            if (searchId.HasValue)
                query = query.Where(n => n.Id == searchId.Value);
            else
                query = query.Where(n => n.IsPublished);

            var newsWithCategories = await query.ToListAsync();
            return View(newsWithCategories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var role = HttpContext.Session.GetString("AdminRole");
            News news;

            if (_permissionService.HasPermission(role, "ManageNews"))
            {
                news = await _context.NewsItems
                    .Include(n => n.Category)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            else
            {
                news = await _context.NewsItems
                    .Include(n => n.Category)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(m => m.Id == id && m.IsPublished);
            }

            if (news == null) return NotFound();
            
            // 🟡 جلب الأخبار المشابهة من نفس التصنيف (ما عدا هذا الخبر)
            var similarNews = await _context.NewsItems
                .Where(n => n.CategoryId == news.CategoryId && n.Id != news.Id && n.IsPublished)
                .OrderByDescending(n => n.PublishedDate)
                .Take(4)
                .ToListAsync();

            ViewBag.SimilarNews = similarNews;
            return View(news);
        }

        public IActionResult Create()
        {
            if (!HasPermission("ManageNews")) return RedirectToAction("Unauthorized", "Home");

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news, IFormFile? imageFile, List<IFormFile>? imageFiles, IFormFile? videoFile)
        {
            if (!HasPermission("ManageNews")) return RedirectToAction("Unauthorized", "Home");

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "/images", imageFile.FileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await imageFile.CopyToAsync(stream);
                    news.ImageUrl = "/images/" + imageFile.FileName;
                }

                if (videoFile != null && videoFile.Length > 0)
                {
                    string videoDir = Path.Combine(_hostEnvironment.WebRootPath, "videos");
                    Directory.CreateDirectory(videoDir);

                    string videoPath = Path.Combine(videoDir, videoFile.FileName);
                    using var stream = new FileStream(videoPath, FileMode.Create);
                    await videoFile.CopyToAsync(stream);

                    news.VideoFilePath = "/videos/" + videoFile.FileName;
                }

                news.IsPublished = false;
                int? adminId = HttpContext.Session.GetInt32("AdminId");
                if (adminId != null) news.AdminUserId = adminId.Value;

                _context.Add(news);
                await _context.SaveChangesAsync();

                if (imageFiles != null && imageFiles.Any())
                {
                    foreach (var file in imageFiles)
                    {
                        string path = Path.Combine(_hostEnvironment.WebRootPath + "/images", file.FileName);
                        using var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);

                        _context.NewsImages.Add(new NewsImage
                        {
                            NewsId = news.Id,
                            ImagePath = "/images/" + file.FileName
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                if (adminId != null)
                {
                    _context.NewsLogs.Add(new NewsLog
                    {
                        NewsId = news.Id,
                        NewsTitle = news.Title,
                        ActionType = "إنشاء",
                        AdminUserId = adminId.Value,
                        ActionDate = DateTime.Now
                    });
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!HasPermission("ManageNews")) return RedirectToAction("Unauthorized", "Home");

            if (id == null) return NotFound();
            var news = await _context.NewsItems.FindAsync(id);
            if (news == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News news, IFormFile? imageFile, IFormFile? videoFile)
        {
            if (!HasPermission("ManageNews")) return RedirectToAction("Unauthorized", "Home");

            if (id != news.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.NewsItems.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
                    if (existing == null) return NotFound();

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string path = Path.Combine(_hostEnvironment.WebRootPath + "/images", imageFile.FileName);
                        using var stream = new FileStream(path, FileMode.Create);
                        await imageFile.CopyToAsync(stream);
                        news.ImageUrl = "/images/" + imageFile.FileName;
                    }
                    else
                    {
                        news.ImageUrl = existing.ImageUrl;
                    }

                    if (videoFile != null && videoFile.Length > 0)
                    {
                        string dir = Path.Combine(_hostEnvironment.WebRootPath, "videos");
                        Directory.CreateDirectory(dir);
                        string path = Path.Combine(dir, videoFile.FileName);
                        using var stream = new FileStream(path, FileMode.Create);
                        await videoFile.CopyToAsync(stream);
                        news.VideoFilePath = "/videos/" + videoFile.FileName;
                    }
                    else
                    {
                        news.VideoFilePath = existing.VideoFilePath;
                    }

                    news.AdminUserId = existing.AdminUserId;
                    news.IsPublished = existing.IsPublished;

                    _context.Update(news);
                    await _context.SaveChangesAsync();

                    int? adminId = HttpContext.Session.GetInt32("AdminId");
                    if (adminId != null)
                    {
                        _context.NewsLogs.Add(new NewsLog
                        {
                            NewsId = news.Id,
                            NewsTitle = news.Title,
                            ActionType = "تعديل",
                            AdminUserId = adminId.Value,
                            ActionDate = DateTime.Now
                        });
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id)) return NotFound();
                    else throw;
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        public IActionResult Delete(int? id)
        {
            TempData["Message"] = "❌ حذف الأخبار غير مسموح.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            TempData["Message"] = "❌ حذف الأخبار غير مسموح.";
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.NewsItems.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublish(int id)
        {
            if (!HasPermission("ManageNews")) return RedirectToAction("Unauthorized", "Home");

            var news = await _context.NewsItems.FindAsync(id);
            if (news == null) return NotFound();

            news.IsPublished = !news.IsPublished;
            await _context.SaveChangesAsync();

            TempData["Message"] = news.IsPublished ? "✅ تم تفعيل الخبر بنجاح." : "✅ تم إلغاء تفعيل الخبر.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ByCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var newsInCategory = await _context.NewsItems
                .Where(n => n.CategoryId == id && n.IsPublished)
                .Include(n => n.Category)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            ViewBag.CategoryName = category.Name;
            ViewBag.IsTrendCategory = (id == 16);
            return View(newsInCategory);
        }

        public async Task<IActionResult> Pending()
        {
            if (!HasPermission("ManageNews")) return RedirectToAction("Unauthorized", "Home");

            var pendingNews = await _context.NewsItems
                .Where(n => !n.IsPublished)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return View(pendingNews);
        }

        public async Task<IActionResult> All()
        {
            var allNews = await _context.NewsItems
                .Include(n => n.Category)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return View(allNews);
        }
    }
}
