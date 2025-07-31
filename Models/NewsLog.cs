using System;

namespace Newfactjo.Models
{
    public class NewsLog
    {
        public int Id { get; set; }

        public int? NewsId { get; set; } // ✅ اختياري
        public string? NewsTitle { get; set; }
        public string ActionType { get; set; } // "إنشاء", "تعديل", "حذف"

        public int AdminUserId { get; set; }
        public DateTime ActionDate { get; set; }

        // ✅ العلاقات
        public News? News { get; set; } // ✅ العلاقة اختيارية
        public AdminUser AdminUser { get; set; }
    }
}
