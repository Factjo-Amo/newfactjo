using System.Collections.Generic;
using Newfactjo.Models;

namespace Newfactjo.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<News> LatestNews { get; set; } = new();
        public List<Article> LatestArticles { get; set; } = new();
        public List<News> TickerNews { get; set; } = new();
        public List<CategoryWithNewsViewModel> CategoriesWithNews { get; set; } = new();

        // التصنيفات العامة (التي لا تشمل وسط البلد ولا التصنيفات الثلاثة الخاصة)
        public News MainTopNews { get; set; }               // خبر رئيسي أول
        public List<News> MainSmallNews { get; set; } = new(); // 4 أخبار رئيسية صغيرة
        public List<News> TopBarNews { get; set; } = new();    // شريط الأخبار العلوي

        public List<CategoryWithNewsViewModel> SpecialThreeColumns { get; set; } = new();  // ✅ التصنيفات الخاصة الثلاثة (نافذة الحقيقة، مال وأعمال، وجهة نظر)

        public List<Advertisement> Advertisements { get; set; } = new();
    }

}

