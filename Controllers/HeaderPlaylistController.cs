using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using Newfactjo.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Newfactjo.Controllers
{
    public class HeaderPlaylistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HeaderPlaylistController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ عرض جميع العناصر
        public IActionResult Index()
        {
            var list = _context.HeaderPlaylists
                .OrderBy(p => p.DisplayOrder)
                .ToList();

            return View(list);
        }

        // ✅ عرض صفحة إضافة عنصر جديد
        public IActionResult Create()
        {
            return View("Create");
        }

        // ✅ تنفيذ إضافة عنصر جديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile mediaFile, string linkUrl, int displayOrder, string mediaType)
        {
            if (mediaFile == null || mediaFile.Length == 0 || string.IsNullOrEmpty(mediaType))
            {
                ModelState.AddModelError("", "يرجى رفع صورة أو فيديو وتحديد النوع.");
                return View();
            }

            // حفظ الملف
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "header");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(mediaFile.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await mediaFile.CopyToAsync(stream);
            }

            var item = new HeaderPlaylist
            {
                MediaUrl = "/uploads/header/" + fileName,
                LinkUrl = linkUrl,
                DisplayOrder = displayOrder,
                MediaType = mediaType,
                IsActive = true
            };

            _context.HeaderPlaylists.Add(item);
            await _context.SaveChangesAsync();

            ViewBag.Message = "تمت الإضافة بنجاح.";
            return RedirectToAction("Create");
        }

        // ✅✅ الصق الكود التالي هنا (Edit)

        // GET: HeaderPlaylist/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _context.HeaderPlaylists.Find(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HeaderPlaylist model, IFormFile? newMediaFile)
        {
            if (id != model.Id)
                return NotFound();

            var existing = _context.HeaderPlaylists.Find(id);
            if (existing == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existing.LinkUrl = model.LinkUrl;
                existing.DisplayOrder = model.DisplayOrder;
                existing.IsActive = model.IsActive;

                if (newMediaFile != null && newMediaFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "header");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(newMediaFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await newMediaFile.CopyToAsync(stream);
                    }

                    existing.MediaUrl = "/uploads/header/" + fileName;

                    // تحديث نوع الوسائط تلقائيًا
                    if (newMediaFile.ContentType.StartsWith("image"))
                        existing.MediaType = "image";
                    else if (newMediaFile.ContentType.StartsWith("video"))
                        existing.MediaType = "video";
                }
                else
                {
                    // إذا لم يتم رفع ملف جديد، نستخدم النوع الحالي
                    existing.MediaType = model.MediaType;
                }

                _context.Update(existing);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            // ⚠️ مهم جدًا: نعيد View بالكائن الذي يحتوي على MediaUrl و MediaType
            return View(existing);
        }
        // GET: HeaderPlaylist/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var item = _context.HeaderPlaylists.FirstOrDefault(p => p.Id == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // POST: HeaderPlaylist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.HeaderPlaylists.FindAsync(id);
            if (item == null)
                return NotFound();

            // حذف الملف من السيرفر (اختياري)
            if (!string.IsNullOrEmpty(item.MediaUrl))
            {
                var physicalPath = Path.Combine(_env.WebRootPath, item.MediaUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }

            _context.HeaderPlaylists.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
