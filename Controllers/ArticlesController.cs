using Microsoft.AspNetCore.Mvc;
using Newfactjo.Models;
using Newfactjo.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Newfactjo.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        // عرض جميع المقالات
        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles.ToListAsync();
            return View(articles);
        }
        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (article == null)
                return NotFound();

            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();

            return View(article);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article, IFormFile ImageFile)
        {
            if (id != article.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // 🔽 إذا تم رفع صورة جديدة
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "articles");
                        Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        // ✅ حفظ المسار النسبي في قاعدة البيانات
                        article.ImageUrl = "/uploads/articles/" + uniqueFileName;
                    }
                    else
                    {
                        // 🟡 مهم: الحفاظ على الصورة السابقة في حال لم يتم رفع صورة جديدة
                        var existingArticle = await _context.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == article.Id);
                        if (existingArticle != null)
                        {
                            article.ImageUrl = existingArticle.ImageUrl;
                        }
                    }

                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Articles.Any(e => e.Id == article.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }


        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (article == null)
                return NotFound();

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // عرض صفحة إضافة مقال جديد
        public IActionResult Create()
        {
            return View();
        }

        // حفظ المقال الجديد في قاعدة البيانات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                // 🔽 رفع الصورة إن وُجدت
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // إنشاء مجلد الصور داخل wwwroot إن لم يكن موجودًا
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "articles");
                    Directory.CreateDirectory(uploadsFolder); // ينشئ المجلد إذا لم يكن موجودًا

                    // توليد اسم فريد للصورة مع الحفاظ على الامتداد
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // حفظ الصورة في المسار المحدد
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // تخزين المسار النسبي للصورة داخل قاعدة البيانات
                    article.ImageUrl = "/uploads/articles/" + uniqueFileName;
                }

                article.PublishedDate = DateTime.Now; //تعيين تاريخ النشر الحالي
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // بعد الحفظ ارجع لقائمة المقالات
            }

            return View(article);
        }

    }
}
