using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Newfactjo.Models
{
    public class Advertisement
    {
        public int Id { get; set; }

        [Display(Name = "اسم الشريط")]
        public string AdBarName { get; set; } = "MainAdBar";

        [Display(Name = "عنوان الإعلان")]
        public string? Title { get; set; }

        [Display(Name = "وصف الإعلان")]
        public string? Description { get; set; }

        [Display(Name = "رابط الصورة")]
        public string? ImageUrl { get; set; }

        [Display(Name = "رابط الفيديو (اختياري)")]
        public string? VideoUrl { get; set; }

        [Display(Name = "هل مفعل؟")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "تاريخ الإضافة")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // لا تُخزن في قاعدة البيانات — فقط لرفع الملفات من الفورم
        [NotMapped]
        [Display(Name = "صورة من الجهاز")]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        [Display(Name = "فيديو من الجهاز")]
        public IFormFile? VideoFile { get; set; }
    }
}
