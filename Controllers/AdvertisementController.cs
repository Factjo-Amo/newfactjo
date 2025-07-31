using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newfactjo.Data;
using Newfactjo.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Newfactjo.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly AppDbContext _context;

        public AdvertisementController(AppDbContext context)
        {
            _context = context;
        }

        // عرض قائمة الإعلانات
        public async Task<IActionResult> Index()
        {
            var ads = await _context.Advertisements
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return View(ads);
        }



        // صفحة إضافة إعلان جديد - GET
        public IActionResult Create()
        {
            return View();
        }

        // حفظ الإعلان الجديد - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Advertisement advertisement)
        {
            // تحقق: يجب رفع صورة أو إدخال رابط صورة
            if (advertisement.ImageFile == null && string.IsNullOrWhiteSpace(advertisement.ImageUrl))
            {
                ModelState.AddModelError("ImageFile", "يجب رفع صورة أو إدخال رابط صورة.");
            }

            // تحقق: يجب رفع فيديو أو إدخال رابط فيديو
            if (advertisement.VideoFile == null && string.IsNullOrWhiteSpace(advertisement.VideoUrl))
            {
                ModelState.AddModelError("VideoFile", "يجب رفع فيديو أو إدخال رابط فيديو.");
            }

            if (ModelState.IsValid)
            {
                // رفع الصورة إذا تم اختيارها
                if (advertisement.ImageFile != null && advertisement.ImageFile.Length > 0)
                {
                    var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(advertisement.ImageFile.FileName)}";
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ads", imageFileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await advertisement.ImageFile.CopyToAsync(stream);
                    }

                    advertisement.ImageUrl = "/uploads/ads/" + imageFileName; // حفظ المسار النسبي
                }

                // رفع الفيديو إذا تم اختيارها
                if (advertisement.VideoFile != null && advertisement.VideoFile.Length > 0)
                {
                    var videoFileName = $"{Guid.NewGuid()}{Path.GetExtension(advertisement.VideoFile.FileName)}";
                    var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ads", videoFileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(videoPath)!);

                    using (var stream = new FileStream(videoPath, FileMode.Create))
                    {
                        await advertisement.VideoFile.CopyToAsync(stream);
                    }

                    advertisement.VideoUrl = "/uploads/ads/" + videoFileName; // حفظ المسار النسبي
                }

                advertisement.CreatedAt = DateTime.Now;

                _context.Add(advertisement);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(advertisement);
        }

        // صفحة تعديل إعلان - GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement == null) return NotFound();

            return View(advertisement);
        }

        // حفظ التعديلات - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Advertisement advertisement, IFormFile ImageFile, IFormFile VideoFile)
        {
            if (id != advertisement.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // رفع الصورة إذا تم اختيارها
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ads", imageFileName);

                        Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        advertisement.ImageUrl = "/uploads/ads/" + imageFileName; // تحديث مسار الصورة
                    }

                    // رفع الفيديو إذا تم اختيارها
                    if (VideoFile != null && VideoFile.Length > 0)
                    {
                        var videoFileName = $"{Guid.NewGuid()}{Path.GetExtension(VideoFile.FileName)}";
                        var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ads", videoFileName);

                        Directory.CreateDirectory(Path.GetDirectoryName(videoPath)!);

                        using (var stream = new FileStream(videoPath, FileMode.Create))
                        {
                            await VideoFile.CopyToAsync(stream);
                        }

                        advertisement.VideoUrl = "/uploads/ads/" + videoFileName; // تحديث مسار الفيديو
                    }

                    _context.Update(advertisement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementExists(advertisement.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(advertisement);
        }


        // صفحة حذف إعلان - GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var advertisement = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null) return NotFound();

            return View(advertisement);
        }

        // تأكيد الحذف - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement != null)
            {
                _context.Advertisements.Remove(advertisement);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertisementExists(int id)
        {
            return _context.Advertisements.Any(e => e.Id == id);
        }
    }
}

