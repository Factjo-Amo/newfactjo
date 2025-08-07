using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Newfactjo.Models
{
    public class News
    {
        public int Id { get; set; }
        public int? AdminUserId { get; set; }

        [ForeignKey("AdminUserId")]
        public AdminUser? AdminUser { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }

        public int ViewsCount { get; set; }


        [DataType(DataType.Date)]
        public DateTime? PublishedDate { get; set; }

        [StringLength(100)]
        public string? Author { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsPublished { get; set; } = false;

        [Display(Name = "التصنيف")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public List<NewsImage>? Images { get; set; }

        public NewsPlacement Placement { get; set; } = NewsPlacement.None;


        // خاصيات الفيديو
        [StringLength(300)]
        [Display(Name = "مسار ملف الفيديو")]
        public string? VideoFilePath { get; set; }  // المسار النسبي للملف المرفوع

        [StringLength(300)]
        [Display(Name = "رابط الفيديو")]
        public string? VideoUrl { get; set; }      // رابط فيديو خارجي (مثلاً يوتيوب)

        [Display(Name = "كود تضمين الفيديو")]
        public string? VideoEmbedCode { get; set; }  // كود iframe أو غيره

        [Display(Name = "إظهار في شريط الأخبار")]
        public bool ShowInTicker { get; set; } = false;
    }
}
