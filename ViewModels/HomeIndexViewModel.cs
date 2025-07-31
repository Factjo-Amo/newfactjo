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
        public List<CategoryWithNewsViewModel> SpecialThreeColumns { get; set; } = new();  // ✅ التصنيفات الخاصة الثلاثة (نافذة الحقيقة، مال وأعمال، وجهة نظر)

        public List<Advertisement> Advertisements { get; set; } = new();
    }

}

